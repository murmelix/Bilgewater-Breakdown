using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Lol.Itemsets.Controllers
{
    public class ImageController : Controller
    {
        //
        // GET: /Image/
        public ActionResult Item(string id)
        {
            var dir = Server.MapPath("/Image/Item");
            var path = Path.Combine(dir, id+".png");
            try
            {
                if (!System.IO.File.Exists(path))
                {
                    var wc = new WebClient();
                    using (wc)
                    {
                        wc.DownloadFile(string.Format("http://ddragon.leagueoflegends.com/cdn/5.2.1/img/item/{0}.png", id), path);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return base.File(path, "image/png");
        }
        //
        // GET: /Image/
        public ActionResult Champion(string id)
        {
            var dir = Server.MapPath("/Image/Champion");
            var path = Path.Combine(dir, id + ".png");
            try
            {
                if (!System.IO.File.Exists(path))
                {
                    var wc = new WebClient();
                    using (wc)
                    {
                        wc.DownloadFile(string.Format("http://ddragon.leagueoflegends.com/cdn/5.2.1/img/champion/{0}.png", id), path);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return base.File(path, "image/png");
        }
	}
}