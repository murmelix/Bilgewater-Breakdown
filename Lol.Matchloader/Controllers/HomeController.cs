using ICSharpCode.SharpZipLib.Zip;
using Lol.Api.Toolkit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace Lol.Matchloader.Controllers
{
    public class HomeController : Controller
    {   
        public class RunParameter{
            public string ApiKey { get; set; }
            public string Region { get; set; }
            public System.Web.HttpSessionStateBase Session { get; set; }
        }


        public ActionResult Index()
        {
            return View();
        }
        [NoCache]
        public ActionResult Start(string id, string region)
        {
            var t = new Thread(Run);
            Session["Running"] = true;
            t.Start(new RunParameter() { ApiKey = id, Region = region, Session = Session });
            Session["Thread"] = t;
            return new JsonResult {JsonRequestBehavior=JsonRequestBehavior.AllowGet };
        }
        [NoCache]
        public ActionResult Stop(string id)
        {            
            Session["Running"] = false;
            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public void Run(object param)
        {
            try
            {
                var par = param as RunParameter;
                string listdata;
                using (var sr = new StreamReader(Path.Combine(Server.MapPath("~/Content"), "BILGEWATER", par.Region + ".json"), Encoding.UTF8))
                {
                    listdata = sr.ReadToEnd();
                }
                string zippath = Path.Combine(Server.MapPath("~/Content"), par.Region, par.Region+"_matches.zip");
                string path;
                ZipFile zip;
                if (System.IO.File.Exists(zippath))
                    zip = new ZipFile(zippath);
                else
                    zip = ZipFile.Create(zippath);
                var list = JsonConvert.DeserializeObject<List<string>>(listdata);
                int i=0;
                using (zip)
                {
                    for (; i < list.Count; i++)
                    {
                        if (zip.FindEntry(list[i] + ".json", true) < 0)                            
                            break;
                    }
                    zip.BeginUpdate();
                    foreach (var f in Directory.GetFiles(Path.Combine(Server.MapPath("~/Content"), par.Region), "*.json"))
                        zip.Add(f, Path.GetFileName(f));
                    zip.CommitUpdate();
                }
                var wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                foreach (var f in Directory.GetFiles(Path.Combine(Server.MapPath("~/Content"), par.Region), "*.json"))
                    System.IO.File.Delete(f);
                
                int written = 1;
                var r = par.Session["Running"] as bool?;
                for (; i < list.Count; )
                {
                    try
                   {
                        r = par.Session["Running"] as bool?;
                        if (!r.HasValue || !r.Value) break;
                        
                        Thread.Sleep(1300);
                        var json = wc.DownloadString(string.Format("https://{1}.api.pvp.net/api/lol/{1}/v2.2/match/{2}?includeTimeline=true&api_key={0}", par.ApiKey, par.Region, list[i]));
                        path = Path.Combine(Server.MapPath("~/Content"), par.Region, list[i] + ".json");
                        using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                            sw.Write(json);
                        i++;
                    }
                    catch (FileNotFoundException fnfe)
                    {

                    }
                    catch
                    {
                        continue;
                    }
                }

                var zf = new ZipFile(zippath);
                using (zf)
                {
                    zf.BeginUpdate();
                    foreach (var f in Directory.GetFiles(Path.Combine(Server.MapPath("~/Content"), par.Region), "*.json"))
                        zf.Add(f, Path.GetFileName(f));
                    zf.CommitUpdate();
                }
                foreach (var f in Directory.GetFiles(Path.Combine(Server.MapPath("~/Content"), par.Region), "*.json"))
                    System.IO.File.Delete(f);
            }            
            catch { }
        }    
    }
}
