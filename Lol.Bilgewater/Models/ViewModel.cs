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
                    HttpContext.Current.Session["ViewModel"] = vm = new ViewModel {Region="all" };
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

        private static Dictionary<string, Item> _itemsFiltered = null;

        public static Dictionary<string, Item> ItemsFiltered
        {
            get
            {
                if (_itemsFiltered == null)
                {
                    _itemsFiltered = new Dictionary<string, Item>();
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

        public static Dictionary<int, ItemStats> BilgewaterItems
        {
            get
            {
                if (_bilgewaterItems == null)
                {
                    string region = ViewModel.FromSession.Region;
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

        private static Dictionary<int, Champion> _champsById = null;

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

        private static string CultureString
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.Name.Replace('-', '_');
            }
        }

        public static string ApiKey { get { return WebConfigurationManager.AppSettings["ApiKey"]; } }

        public string Region { get; set; }
    }
}