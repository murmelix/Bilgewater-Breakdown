using ICSharpCode.SharpZipLib.Zip;
using Lol.Api.Static;
using Lol.Api.Static.Champion;
using Lol.Api.Static.Items;
using Lol.Itemsets.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Lol.Itemsets.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            FetchAllMatches("euw");
            ViewBag.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            return View(ViewModel.FromSession);
        }

        private void FetchAllMatches(string region)
        {
            string listdata;
            using (var sr = new StreamReader(Path.Combine(Server.MapPath("~/Content"), "BILGEWATER", region+".json"), Encoding.UTF8))
            {
                listdata = sr.ReadToEnd();
            }
            string path = Path.Combine(Server.MapPath("~/Content"), region, "matches.zip");
            var zip = new ZipFile(path);
            var list = JsonConvert.DeserializeObject<List<string>>(listdata);
            var wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            using (zip)
            {
                zip.BeginUpdate();
                for (var i = 0; i < list.Count; )
                {
                    try
                    {
                        if (zip.FindEntry(list[i] + ".json", true) >= 0)
                        {
                            i++;
                            continue;
                        }
                        Thread.Sleep(1300);
                        var json = wc.DownloadString(string.Format("https://euw.api.pvp.net/api/lol/{1}/v2.2/match/{2}?includeTimeline=true&api_key={0}", WebConfigurationManager.AppSettings["ApiKey"], region, list[i]));
                        path = Path.Combine(Server.MapPath("~/Content"), region, list[i] + ".json");
                        using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                            sw.Write(json);
                        zip.Add(path, list[i] + ".json");
                        System.IO.File.Delete(path);
                        i++;
                    }
                    catch
                    {
                        continue;
                    }
                }
                zip.CommitUpdate();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}