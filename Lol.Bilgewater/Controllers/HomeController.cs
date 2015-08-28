using ICSharpCode.SharpZipLib.Zip;
using Lol.Api.Static;
using Lol.Api.Static.Champion;
using Lol.Api.Static.Items;
using Lol.Api.Static.Match;
using Lol.Bilgewater.Models;
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

namespace Lol.Bilgewater.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string Region)
        {
            if(Region != null)
            {
                ViewModel.FromSession.Region = Region;
            }
            else
            {
                ViewModel.FromSession.Region = "all";            
            }
            ViewBag.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            return View(ViewModel.FromSession);
        }

        public ActionResult Champions()
        {
            return View(ViewModel.FromSession);
        }

        public ActionResult Items()
        {
            return View(ViewModel.FromSession);
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