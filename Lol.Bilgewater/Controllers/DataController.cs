using Lol.Api.Bilgewater;
using Lol.Api.Bilgewater.Samples;
using Lol.Api.Static;
using Lol.Bilgewater.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Lol.Bilgewater.Controllers
{
    public class DataController : Controller
    {
        public string DataPath
        {
            get
            {
#if DEBUG
                return WebConfigurationManager.AppSettings["DataPath"];
#else
                return Server.MapPath("~/App_Data");
#endif
            }
        }

        public Dictionary<int, ChampionStat> ChampionsBilgewater
        {
            get
            {
                string region = ViewModel.FromSession.Region;
                var protopath_bilge = Path.Combine(DataPath, "BILGEWATER", "Samples", region, "Champion.proto");
                using (var s = new FileStream(protopath_bilge, FileMode.Open))
                {
                    return ProtoBuf.Serializer.Deserialize<Dictionary<int, ChampionStat>>(s);
                }
            }
        }

        public Dictionary<int, ChampionStat> ChampionsRanked
        {
            get
            {
                string region = ViewModel.FromSession.Region;
                var protopath_bilge = Path.Combine(DataPath, "RANKED_SOLO", "Samples", region, "Champion.proto");
                using (var s = new FileStream(protopath_bilge, FileMode.Open))
                {
                    return ProtoBuf.Serializer.Deserialize<Dictionary<int, ChampionStat>>(s);
                }
            }
        }

        public Dictionary<int, ItemStats> ItemsBilgewater
        {
            get
            {
                string region = ViewModel.FromSession.Region;
                var protopath_bilge = Path.Combine(DataPath, "BILGEWATER", "Samples", region, "Items.proto");
                using (var s = new FileStream(protopath_bilge, FileMode.Open))
                {
                    return ProtoBuf.Serializer.Deserialize<Dictionary<int, ItemStats>>(s);
                }
            }
        }

        public Dictionary<int, ItemStats> ItemsRanked
        {
            get
            {
                string region = ViewModel.FromSession.Region;
                var protopath_bilge = Path.Combine(DataPath, "RANKED_SOLO", "Samples", region, "Items.proto");
                using (var s = new FileStream(protopath_bilge, FileMode.Open))
                {
                    return ProtoBuf.Serializer.Deserialize<Dictionary<int, ItemStats>>(s);
                }
            }
        }

        public ActionResult Mercs()
        {
            string region = ViewModel.FromSession.Region;
            var protopath = Path.Combine(DataPath, "BILGEWATER", "Samples", region, "Merc.proto");
            using (var s = new FileStream(protopath, FileMode.Open))
            {                
                var data = ProtoBuf.Serializer.Deserialize<Dictionary<int, MercStats>>(s);
                var infos = ViewModel.Items.Where(x => data.Keys.Contains(x.Value.Id)).Select(x=>x.Value).ToList();
                data[-1].Id = 3070;//träne
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        Stats = data.Values.OrderBy(x => x.Pickrate).ToList(),
                        Infos = infos
                    }
                };
            }            
        }

        public ActionResult Role()
        {
            string region = ViewModel.FromSession.Region;
            var protopath_bilge = Path.Combine(DataPath, "BILGEWATER", "Samples", region, "Role.proto");
            var protopath_ranked = Path.Combine(DataPath, "RANKED_SOLO", "Samples", region, "Role.proto");
            Dictionary<string, ChampionStat> bilgewater, ranked;
            using (var s = new FileStream(protopath_bilge, FileMode.Open))
            {
                bilgewater = ProtoBuf.Serializer.Deserialize<Dictionary<string, ChampionStat>>(s);
            }
            using (var s = new FileStream(protopath_ranked, FileMode.Open))
            {
                ranked = ProtoBuf.Serializer.Deserialize<Dictionary<string, ChampionStat>>(s);
            }
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Bilgewater = bilgewater,
                    Ranked = ranked
                }
            };
        }

        public ActionResult Champion(string id)
        {
            var champ = ViewModel.Champions[id];
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Description = champ,
                    Bilgewater = ChampionsBilgewater[int.Parse(champ.Id)],
                    Ranked = ChampionsRanked[int.Parse(champ.Id)]
                }
            };
        }

        public ActionResult Item(string id)
        {
            string region = ViewModel.FromSession.Region;
            var protopath_bilge = Path.Combine(DataPath, "BILGEWATER", "Samples", region, "Items.proto");
            var protopath_ranked = Path.Combine(DataPath, "RANKED_SOLO", "Samples", region, "Items.proto");
            Dictionary<int, ItemStats> bilgewater, ranked;
            var item = ViewModel.Items[id];
            using (var s = new FileStream(protopath_bilge, FileMode.Open))
            {
                bilgewater = ProtoBuf.Serializer.Deserialize<Dictionary<int, ItemStats>>(s);
            }
            using (var s = new FileStream(protopath_ranked, FileMode.Open))
            {
                ranked = ProtoBuf.Serializer.Deserialize<Dictionary<int, ItemStats>>(s);
            }
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Description = item,
                    Bilgewater = bilgewater.ContainsKey(int.Parse(id)) ? bilgewater[int.Parse(id)] : null,
                    Ranked = ranked.ContainsKey(int.Parse(id)) ? ranked[int.Parse(id)] : null
                }
            };
        }

        public ActionResult Duration()
        {
            string region = ViewModel.FromSession.Region;
            var protopath_bilge = Path.Combine(DataPath, "BILGEWATER", "Samples", region, "Duration.proto");
            var protopath_ranked = Path.Combine(DataPath, "RANKED_SOLO", "Samples", region, "Duration.proto");
            DurationStats bilgewater, ranked;
            using (var s = new FileStream(protopath_bilge, FileMode.Open))
            {
                bilgewater = ProtoBuf.Serializer.Deserialize<DurationStats>(s);
            }
            using (var s = new FileStream(protopath_ranked, FileMode.Open))
            {
                ranked = ProtoBuf.Serializer.Deserialize<DurationStats>(s);
            }
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Bilgewater = bilgewater,
                    Ranked = ranked
                }
            };
        }

        public ActionResult ItemChart(string id)
        {
            switch (id)
            {
                case "LowestWinrate":
                    return CalculateItemLowestWinrate();
                case "TopPickrate":
                    return CalculateItemTopPickrate();
                case "LowestPickrate":
                    return CalculateItemLowestPickrate();
                default:
                    return CalculateItemTopWinrate();
            }
        }
        private ActionResult CalculateItemTopPickrate()
        {
            var ranked = ItemsRanked;
            var list = ItemsBilgewater.Values.Where(x=>x.Id > 0).OrderByDescending(x => x.Pickrate).Select(x => new CharMerit
            {
                Id = ViewModel.Items[x.Id.ToString()].Id,
                Name = ViewModel.Items[x.Id.ToString()].Name,
                ValueBilgewater = x.Pickrate,
                ValueRanked = ranked.Values.First(y => y.Id == x.Id).Pickrate
            }).Take(10).ToList();
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Items = list
                }
            };
        }

        private ActionResult CalculateItemLowestPickrate()
        {
            var ranked = ItemsRanked;
            var list = ItemsBilgewater.Values.OrderBy(x => x.Pickrate).Select(x => new CharMerit
            {
                Id = ViewModel.Items[x.Id.ToString()].Id,
                Name = ViewModel.Items[x.Id.ToString()].Name,
                ValueBilgewater = x.Pickrate,
                ValueRanked = ranked.Values.FirstOrDefault(y => y.Id == x.Id).Pickrate
            }).Take(10).ToList();
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Items = list
                }
            };
        }

        private ActionResult CalculateItemLowestWinrate()
        {
            var ranked = ItemsRanked;
            var list = ItemsBilgewater.Values.OrderBy(x => x.Winrate).Select(x => new CharMerit
            {
                Id = ViewModel.Items[x.Id.ToString()].Id,
                Name = ViewModel.Items[x.Id.ToString()].Name,
                ValueBilgewater = x.Winrate * 100,
                ValueRanked = ranked.Values.First(y => y.Id == x.Id).Winrate * 100
            }).Take(10).ToList();
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Items = list
                }
            };
        }

        private ActionResult CalculateItemTopWinrate()
        {
            var ranked = ItemsRanked;
            var list = ItemsBilgewater.Values.OrderByDescending(x => x.Winrate).Select(x => new CharMerit
            {
                Id = ViewModel.Items[x.Id.ToString()].Id,
                Name = ViewModel.Items[x.Id.ToString()].Name,
                ValueBilgewater = x.Winrate * 100,
                ValueRanked = ranked.Values.First(y => y.Id == x.Id).Winrate * 100
            }).Take(10).ToList();
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Items = list
                }
            };
        }

        public ActionResult ChampionChart(string id)
        {
            switch (id)
            {
                case "LowestWinrate":
                    return CalculateChampionLowestWinrate();
                case "TopPickrate":
                    return CalculateChampionTopPickrate();
                case "LowestPickrate":
                    return CalculateChampionLowestPickrate();
                default:
                    return CalculateChampionTopWinrate();
            }
        }

        private ActionResult CalculateChampionTopPickrate()
        {
            var ranked = ChampionsRanked;
            var list = ChampionsBilgewater.Values.OrderByDescending(x => x.Pickrate).Select(x => new CharMerit
            {
                Key = ViewModel.ChampionsById[x.Id].Key,
                Name = ViewModel.ChampionsById[x.Id].Name,
                ValueBilgewater = x.Pickrate,
                ValueRanked = ranked.Values.First(y => y.Id == x.Id).Pickrate
            }).Take(10).ToList();
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Champions = list
                }
            };
        }

        private ActionResult CalculateChampionLowestPickrate()
        {
            var ranked = ChampionsRanked;
            var list = ChampionsBilgewater.Values.OrderBy(x => x.Pickrate).Select(x => new CharMerit
            {
                Key = ViewModel.ChampionsById[x.Id].Key,
                Name = ViewModel.ChampionsById[x.Id].Name,
                ValueBilgewater = x.Pickrate,
                ValueRanked = ranked.Values.First(y => y.Id == x.Id).Pickrate
            }).Take(10).ToList();
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Champions = list
                }
            };
        }

        private ActionResult CalculateChampionLowestWinrate()
        {
            var ranked = ChampionsRanked;
            var list = ChampionsBilgewater.Values.OrderBy(x => x.Winrate).Select(x => new CharMerit
            {
                Key = ViewModel.ChampionsById[x.Id].Key,
                Name = ViewModel.ChampionsById[x.Id].Name,
                ValueBilgewater = x.Winrate*100,
                ValueRanked = ranked.Values.First(y => y.Id == x.Id).Winrate * 100
            }).Take(10).ToList();
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Champions = list
                }
            };
        }

        private ActionResult CalculateChampionTopWinrate()
        {
            var ranked = ChampionsRanked;
            var list = ChampionsBilgewater.Values.OrderByDescending(x => x.Winrate).Select(x => new CharMerit 
            { 
                Key = ViewModel.ChampionsById[x.Id].Key, 
                Name = ViewModel.ChampionsById[x.Id].Name,
                ValueBilgewater = x.Winrate * 100,
                ValueRanked = ranked.Values.First(y => y.Id == x.Id).Winrate * 100
            }).Take(10).ToList();                        
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Champions = list
                }
            };
        }

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
	}
}