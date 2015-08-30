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
            return RedirectToAction(ViewModel.FromSession.CurrentView);
        }

        public ActionResult Welcome()
        {
            ViewModel.FromSession.CurrentView = "Welcome";
            return View(ViewModel.FromSession);
        }

        public ActionResult Mercs()
        {
            ViewModel.FromSession.CurrentView = "Mercs";
            return View(ViewModel.FromSession);
        }

        public ActionResult Champions()
        {
            ViewModel.FromSession.CurrentView = "Champions";
            return View(ViewModel.FromSession);
        }

        public ActionResult Items(bool OnlyBilgewater)
        {
            ViewBag.OnlyBilgewater = OnlyBilgewater;
            ViewModel.FromSession.CurrentView = "Items";
            return View(ViewModel.FromSession);
        }

        public ActionResult About()
        {
            ViewModel.FromSession.CurrentView = "About";
            return View(ViewModel.FromSession);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}