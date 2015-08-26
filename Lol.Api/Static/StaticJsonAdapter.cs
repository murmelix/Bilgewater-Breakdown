
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

        public StaticJsonAdapter(string chachePath)
        {
            _cachePath = chachePath;
        }

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
                        zip.Add(path,matchId+".json");
                        zip.CommitUpdate();
                        File.Delete(path);
                        return JsonConvert.DeserializeObject<MatchDetail>(json, new JsonSerializerSettings { Culture = Thread.CurrentThread.CurrentUICulture, });
                    }
                }
            }
        }

        public ItemList ListItems(string region, string locale, string apikey)
        {
            string path = Path.Combine(_cachePath, region, locale, "items.json");
            string jsonFromCache = GetJsonFromCache(path);
            if (jsonFromCache != null)
            {
                return JsonConvert.DeserializeObject<ItemList>(jsonFromCache);
            }
            lock (path)
            {
                var wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                using (wc)
                {
                    var json = wc.DownloadString(string.Format("https://global.api.pvp.net/api/lol/static-data/{2}/v1.2/item?locale={0}&itemListData=all&api_key={1}&version=5.14.1", locale, apikey, region));
                    using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                        sw.Write(json);
                    return JsonConvert.DeserializeObject<ItemList>(json, new JsonSerializerSettings { Culture = Thread.CurrentThread.CurrentUICulture, });
                }
            }
        }

        public ChampionList ListChampions(string region, string locale, string apikey)
        {
            string path = Path.Combine(_cachePath, region, locale, "champions.json");
            string jsonFromCache = GetJsonFromCache(path);
            if (jsonFromCache != null)
            {
                return JsonConvert.DeserializeObject<ChampionList>(jsonFromCache);
            }
            lock (path)
            {
                var wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                using (wc)
                {
                    var json = wc.DownloadString(string.Format("https://global.api.pvp.net/api/lol/static-data/{2}/v1.2/champion?locale={0}&champData=all&api_key={1}&version=5.14.1", locale, apikey, region));
                    using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                        sw.Write(json);
                    return JsonConvert.DeserializeObject<ChampionList>(json, new JsonSerializerSettings { Culture = Thread.CurrentThread.CurrentUICulture, });
                }
            }
        }

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
            if(entry == null)
                return null;
            using (var sr = new StreamReader(zip.GetInputStream(entry), Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
