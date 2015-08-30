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
            // index only is switch region all for default
            if(Region != null)
            {
                ViewModel.FromSession.Region = Region;
            }
            else
            {
                ViewModel.FromSession.Region = "all";            
            }
            ViewBag.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            // redirects to last view
            return RedirectToAction(ViewModel.FromSession.CurrentView);
        }

        public ActionResult Welcome()
        {
            // save last view
            ViewModel.FromSession.CurrentView = "Welcome";
            return View(ViewModel.FromSession);
        }

        public ActionResult Mercs()
        {
            // save last view
            ViewModel.FromSession.CurrentView = "Mercs";
            return View(ViewModel.FromSession);
        }

        public ActionResult Champions()
        {
            // save last view
            ViewModel.FromSession.CurrentView = "Champions";
            return View(ViewModel.FromSession);
        }

        public ActionResult Items(bool? OnlyBilgewater)
        {
            // save last view
            ViewBag.OnlyBilgewater = OnlyBilgewater ?? false;
            ViewModel.FromSession.CurrentView = "Items";
            return View(ViewModel.FromSession);
        }

        public ActionResult About()
        {
            // save last view
            ViewModel.FromSession.CurrentView = "About";
            return View(ViewModel.FromSession);
        }
    }
}