using Lol.Api.Bilgewater.Samples;
using Lol.Api.Static.Champion;
using Lol.Api.Static.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace Lol.Bilgewater.Models
{
    public class ViewModel
    {
        // on local development use DataPath from config
        // published use App_Data (no idea about path in azure)
        public static string DataPath
        {
            get
            {
#if DEBUG
                return WebConfigurationManager.AppSettings["DataPath"];
#else
                return HttpContext.Current.Server.MapPath("~/App_Data");
#endif
            }
        }
        public static ViewModel FromSession
        {
            get
            {
                // get model from current session
                ViewModel vm = HttpContext.Current.Session["ViewModel"] as ViewModel;
                // if no model present, create one, default region = all, default view = welcome
                if (vm == null)
                    HttpContext.Current.Session["ViewModel"] = vm = new ViewModel { Region = "all", CurrentView = "Welcome" };
                return vm;
            }
        }
        
        private static Dictionary<string, Item> _items = null;
        // local stash for items data
        public static Dictionary<string, Item> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new Dictionary<string, Item>();
                    // donwload items from riot api, if not present locally
                    var adapter = new StaticJsonAdapter(HttpContext.Current.Server.MapPath("~/Content"));
                    var result = adapter.ListItems("euw", CultureString, ApiKey);
                    _items = result.Data;
                }
                return _items;
            }
        }

        private static Dictionary<string, Item> _itemsFiltered = null;
        // local stash for items filtered for presence in samples data
        public static Dictionary<string, Item> ItemsFiltered
        {
            get
            {
                if (_itemsFiltered == null)
                {
                    _itemsFiltered = new Dictionary<string, Item>();
                    // donwload items from riot api, if not present locally
                    var adapter = new StaticJsonAdapter(HttpContext.Current.Server.MapPath("~/Content"));
                    var result = adapter.ListItems("euw", CultureString, ApiKey);
                    _itemsFiltered = result.Data;

                    List<string> toRemove = new List<string>();
                    foreach (var key in _itemsFiltered.Keys)
                    {
                        if (!BilgewaterItems.ContainsKey(int.Parse(key)))
                        {
                            toRemove.Add(key);
                        }
                    }
                    foreach (var key in toRemove)
                        _itemsFiltered.Remove(key);
                }
                return _itemsFiltered;
            }
        }

        private static Dictionary<int, ItemStats> _bilgewaterItems = null;
        // local stash for items stats
        public static Dictionary<int, ItemStats> BilgewaterItems
        {
            get
            {
                if (_bilgewaterItems == null)
                {
                    string region = ViewModel.FromSession.Region;
                    // load items stats from proto sample, if not present locally
                    var protopath_bilge = Path.Combine(DataPath, "BILGEWATER", "Samples", region, "Items.proto");                    
                    using (var s = new FileStream(protopath_bilge, FileMode.Open))
                    {
                        _bilgewaterItems = ProtoBuf.Serializer.Deserialize<Dictionary<int, ItemStats>>(s);
                    }
                }
                return _bilgewaterItems;
            }
        }

        private static Dictionary<string, Champion> _champs = null;
        // local stash for champions
        public static Dictionary<string, Champion> Champions
        {
            get
            {
                if (_champs == null)
                {
                    _champs = new Dictionary<string, Champion>();
                    // donwload items from riot api, if not present locally
                    var adapter = new StaticJsonAdapter(HttpContext.Current.Server.MapPath("~/Content"));
                    var result = adapter.ListChampions("euw", CultureString, ApiKey);
                    _champs = result.Data;
                }
                return _champs;
            }
        }

        private static Dictionary<int, Champion> _champsById = null;
        // local stash for champions
        public static Dictionary<int, Champion> ChampionsById
        {
            get
            {
                if (_champsById == null)
                {
                    _champsById = new Dictionary<int, Champion>();
                    foreach (var champ in Champions.Values)
                        _champsById.Add(int.Parse(champ.Id), champ);
                }
                return _champsById;
            }
        }
        // save current cultre
        private static string CultureString
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.Name.Replace('-', '_');
            }
        }

        // get api key from web.config
        public static string ApiKey { get { return WebConfigurationManager.AppSettings["ApiKey"]; } }
        // current region  default is "all"
        public string Region { get; set; }
        // current view default is "Welcome"
        public string CurrentView { get; set; }
    }
}