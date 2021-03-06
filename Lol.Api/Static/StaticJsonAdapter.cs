﻿
using Lol.Api.Static.Champion;
using Lol.Api.Static.Match;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;

namespace Lol.Api.Static.Items
{
    public class StaticJsonAdapter
    {
        private string _cachePath;
        public static string[] SupportedCultures = new string[] { "bg_BG",
            "cs_CZ",
            "de_DE",
            "el_GR",
            "en_AU",
            "en_GB",
            "en_PH",
            "en_PL",
            "en_SG",
            "en_US",
            "es_AR",
            "es_ES",
            "es_MX",
            "fr_FR",
            "hu_HU",
            "id_ID",
            "it_IT",
            "ja_JP",
            "ko_KR" ,
            "nl_NL",
            "ms_MY",
            "pl_PL",
            "pt_BR",
            "pt_PT",
            "ro_RO",
            "ru_RU",
            "th_TH",
            "tr_TR",
            "vn_VN",
            "zh_CN",
            "zh_MY",
            "zh_TW"};

        public string SafeLocale(string locale)
        {
            // sometimes browser sends only fr or en so we look for a suiting riot locale
            if (SupportedCultures.Contains(locale))
                return locale;
            else if (SupportedCultures.Any(x => x.Substring(0, 2) == locale.Substring(0, 2)))
                return SupportedCultures.First(x => x.Substring(0, 2) == locale.Substring(0, 2));
            // if nothing found, go default en_US 
            else
                return "en_US";
        }

        // cache path, where items will be downloaded to
        public StaticJsonAdapter(string chachePath)
        {
            _cachePath = chachePath;
        }

        // matchloader for console program
        public MatchDetail FetchMatch(string region, string matchId, string apikey)
        {
            string path = Path.Combine(_cachePath, region, "matches.zip");
            lock (path)
            {
                ZipFile zip = null;
                if (File.Exists(path))
                {
                    zip = new ZipFile(path);
                    string jsonFromCache = GetJsonFromCache(zip, matchId + ".json");
                    if (jsonFromCache != null)
                    {
                        using (zip)
                        {
                            return JsonConvert.DeserializeObject<MatchDetail>(jsonFromCache);
                        }
                    }
                }
                else
                {
                    zip = ZipFile.Create(path);
                }
                using (zip)
                {
                    var wc = new WebClient();
                    wc.Encoding = Encoding.UTF8;
                    using (wc)
                    {
                        var json = wc.DownloadString(string.Format("https://euw.api.pvp.net/api/lol/{1}/v2.2/match/{2}?includeTimeline=true&api_key={0}&version=5.14.1", apikey, region, matchId));
                        path = Path.Combine(_cachePath, region, matchId + ".json");
                        zip.BeginUpdate();
                        using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                            sw.Write(json);
                        zip.Add(path, matchId + ".json");
                        zip.CommitUpdate();
                        File.Delete(path);
                        return JsonConvert.DeserializeObject<MatchDetail>(json, new JsonSerializerSettings { Culture = Thread.CurrentThread.CurrentUICulture, });
                    }
                }
            }
        }

        // load items for region/locale
        public ItemList ListItems(string region, string locale, string apikey)
        {
            // ensure region, so nothing goes wrong
            locale = SafeLocale(locale);
            // combine local cachepath
            string path = Path.Combine(_cachePath, region, locale, "items.json");
            // if json cached, no download necessary
            string jsonFromCache = GetJsonFromCache(path);
            if (jsonFromCache != null)
            {
                return JsonConvert.DeserializeObject<ItemList>(jsonFromCache);
            }
            // nothing found, go download
            lock (path)
            {
                // create a utf8 webclient
                var wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                int tires = 0;
                using (wc)
                {
                    string json ="";
                    do
                    {
                        try
                        {
                            // download it
                            json = wc.DownloadString(string.Format("https://global.api.pvp.net/api/lol/static-data/{2}/v1.2/item?locale={0}&itemListData=all&api_key={1}&version=5.14.1", locale, apikey, region));
                            break;// we did it
                        }
                        catch (Exception ex)
                        {
                            tires++;
                            // sometimes api is busy and throws 503, so we continue and try again
                            // but sleep so we dont DOS it 
                            Thread.Sleep(1000);
                            continue;
                        }
                        // max 10 tries
                    } while (tires < 10);
                    // do not safe empty json to disk, this will get us in trouble
                    if (json == "")
                        throw new ApplicationException("its broken");
                    // safe for later
                    using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                        sw.Write(json);
                    // return the objects
                    return JsonConvert.DeserializeObject<ItemList>(json, new JsonSerializerSettings { Culture = Thread.CurrentThread.CurrentUICulture, });
                }
            }
        }

        // load champions for region/locale
        public ChampionList ListChampions(string region, string locale, string apikey)
        {
            // ensure region, so nothing goes wrong
            locale = SafeLocale(locale);
            // combine local cachepath
            string path = Path.Combine(_cachePath, region, locale, "champions.json");
            // if json cached, no download necessary
            string jsonFromCache = GetJsonFromCache(path);
            if (jsonFromCache != null)
            {
                return JsonConvert.DeserializeObject<ChampionList>(jsonFromCache);
            }
            // nothing found, go download
            lock (path)
            {
                // create a utf8 webclient
                var wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                int tires = 0;
                using (wc)
                {
                    string json = "";
                    do
                    {
                        try
                        {
                            json = wc.DownloadString(string.Format("https://global.api.pvp.net/api/lol/static-data/{2}/v1.2/champion?locale={0}&champData=all&api_key={1}&version=5.14.1", locale, apikey, region));
                            break;// we did it
                        }
                        catch (Exception ex)
                        {
                            tires++;
                            // sometimes api is busy and throws 503, so we continue and try again
                            // but sleep so we dont DOS it 
                            continue;
                        }
                        // max 10 tries
                    } while (tires < 10);
                    // do not safe empty json to disk, this will get us in trouble
                    if (json == "")
                        throw new ApplicationException("its broken");
                    // safe for later
                    using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                        sw.Write(json);
                    // return the objects
                    return JsonConvert.DeserializeObject<ChampionList>(json, new JsonSerializerSettings { Culture = Thread.CurrentThread.CurrentUICulture, });

                }
            }
        }

        // helper for safe fetch from file
        private string GetJsonFromCache(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            if (File.Exists(path))
            {
                using (var sr = new StreamReader(path, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
            return null;
        }

        private string GetJsonFromCache(ZipFile zip, string p)
        {
            var entry = zip.GetEntry(p);
            if (entry == null)
                return null;
            using (var sr = new StreamReader(zip.GetInputStream(entry), Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
