using Lol.Api.Static.Champion;
using Lol.Api.Static.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace Lol.Bilgewater.Models
{
    public class ViewModel
    {
        private ViewModel()
        {
            CurrentMap = "SR";
            CurrentChampion = "Aatrox";
        }
        public static ViewModel FromSession
        {
            get
            {
                ViewModel vm = HttpContext.Current.Session["ViewModel"] as ViewModel;
                if (vm == null)
                    HttpContext.Current.Session["ViewModel"] = vm = new ViewModel();
                return vm;
            }
        }

        public string CurrentChampion { get; set; }
        public string CurrentMap { get; set; }

        private static Dictionary<string, Item> _items = null;

        public static Dictionary<string, Item> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new Dictionary<string, Item>();
                    var adapter = new StaticJsonAdapter(HttpContext.Current.Server.MapPath("~/Content"));
                    var result = adapter.ListItems("euw", CultureString, ApiKey);
                    _items = result.Data;
                }
                return _items;
            }
        }

        private static Dictionary<string, Champion> _champs = null;

        public static Dictionary<string, Champion> Champions
        {
            get
            {
                if (_champs == null)
                {
                    _champs = new Dictionary<string, Champion>();
                    var adapter = new StaticJsonAdapter(HttpContext.Current.Server.MapPath("~/Content"));
                    var result = adapter.ListChampions("euw", CultureString, ApiKey);
                    _champs = result.Data;
                }
                return _champs;
            }
        }

        private static string CultureString
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.Name.Replace('-', '_');
            }
        }

        public static string ApiKey { get { return WebConfigurationManager.AppSettings["ApiKey"]; } }
    }
}