﻿using Lol.Api.Bilgewater;
using Lol.Api.Bilgewater.Samples;
using Lol.Api.Static;
using Lol.Api.Static.Items;
using Lol.Api.Toolkit;
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
        // local cache for champions stats from bilgewater
        public Dictionary<int, ChampionStat> ChampionsBilgewater
        {
            get
            {
                string region = ViewModel.FromSession.Region;
                // load from proto if not present
                var protopath_bilge = Path.Combine(DataPath, "BILGEWATER", "Samples", region, "Champion.proto");
                using (var s = new FileStream(protopath_bilge, FileMode.Open))
                {
                    return ProtoBuf.Serializer.Deserialize<Dictionary<int, ChampionStat>>(s);
                }
            }
        }

        // local cache for champions stats from solo_ranked
        public Dictionary<int, ChampionStat> ChampionsRanked
        {
            get
            {
                string region = ViewModel.FromSession.Region;
                // load from proto if not present
                var protopath_bilge = Path.Combine(DataPath, "RANKED_SOLO", "Samples", region, "Champion.proto");
                using (var s = new FileStream(protopath_bilge, FileMode.Open))
                {
                    return ProtoBuf.Serializer.Deserialize<Dictionary<int, ChampionStat>>(s);
                }
            }
        }

        // local cache for item stats from bilgewater
        public Dictionary<int, ItemStats> ItemsBilgewater
        {
            get
            {
                string region = ViewModel.FromSession.Region;
                // load from proto if not present
                var protopath_bilge = Path.Combine(DataPath, "BILGEWATER", "Samples", region, "Items.proto");
                using (var s = new FileStream(protopath_bilge, FileMode.Open))
                {
                    return ProtoBuf.Serializer.Deserialize<Dictionary<int, ItemStats>>(s);
                }
            }
        }

        // local cache for item stats from solo_ranked
        public Dictionary<int, ItemStats> ItemsRanked
        {
            get
            {
                string region = ViewModel.FromSession.Region;
                // load from proto if not present
                var protopath_bilge = Path.Combine(DataPath, "RANKED_SOLO", "Samples", region, "Items.proto");
                using (var s = new FileStream(protopath_bilge, FileMode.Open))
                {
                    return ProtoBuf.Serializer.Deserialize<Dictionary<int, ItemStats>>(s);
                }
            }
        }
        [NoCache]
        public ActionResult Mercs()
        {
            string region = ViewModel.FromSession.Region;
            // get merc samples for selected region
            var protopath = Path.Combine(DataPath, "BILGEWATER", "Samples", region, "Merc.proto");
            using (var s = new FileStream(protopath, FileMode.Open))
            {
                var data = ProtoBuf.Serializer.Deserialize<Dictionary<int, MercStats>>(s);
                // load infos for description panel
                var infos = ViewModel.Items.Where(x => data.Keys.Contains(x.Value.Id)).Select(x => x.Value).ToList();
                // add the tear id for fluff
                data[-1].Id = 3070;
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        // stats order by pickrate
                        Stats = data.Values.OrderBy(x => x.Pickrate).ToList(),
                        Infos = infos
                    }
                };
            }
        }

        public ActionResult Role()
        {
            // load role stats from both sources
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
                // return combined result
                Data = new
                {
                    Bilgewater = bilgewater,
                    Ranked = ranked
                }
            };
        }
        [NoCache]
        public ActionResult Champion(string id)
        {
            // load details for champion popup
            var champ = ViewModel.Champions[id];
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    // basic description key/name/title
                    Description = champ,
                    // stats from bilgewater
                    Bilgewater = ChampionsBilgewater[int.Parse(champ.Id)],
                    // stats from ranked solo
                    Ranked = ChampionsRanked[int.Parse(champ.Id)]
                }
            };
        }
        [NoCache]
        public ActionResult Item(string id)
        {
            // load data for item popup
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
                    // basic description key/name/title/desription
                    Description = item,
                    // stats from bilgewater
                    Bilgewater = bilgewater.ContainsKey(int.Parse(id)) ? bilgewater[int.Parse(id)] : null,
                    // stats from ranked solo
                    Ranked = ranked.ContainsKey(int.Parse(id)) ? ranked[int.Parse(id)] : null
                }
            };
        }

        public ActionResult Duration()
        {
            // load duration sample data
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
                    // stats from bilgewater
                    Bilgewater = bilgewater,
                    // stats from ranked solo
                    Ranked = ranked
                }
            };
        }

        public ActionResult ItemChart(string id, bool onlyBilgewater)
        {
            // load data for item chart switch by selection
            switch (id)
            {
                case "LowestWinrate":
                    return CalculateItemLowestWinrate(onlyBilgewater);
                case "TopPickrate":
                    return CalculateItemTopPickrate(onlyBilgewater);
                case "LowestPickrate":
                    return CalculateItemLowestPickrate(onlyBilgewater);
                default:
                    return CalculateItemTopWinrate(onlyBilgewater);
            }
        }
        private ActionResult CalculateItemTopPickrate(bool onlyBilgewater)
        {
            var ranked = ItemsRanked;
            var list = ItemsBilgewater.Values.Where(x => x.Id > 0 && (!onlyBilgewater || IsBiglewater(x))).OrderByDescending(x => x.Pickrate).Select(x => new Merit
            {
                Id = ViewModel.Items[x.Id.ToString()].Id,
                Name = ViewModel.Items[x.Id.ToString()].Name,
                ValueBilgewater = x.Pickrate,
                ValueRanked = SafePickrate(ranked.Values.FirstOrDefault(y => y.Id == x.Id))
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

        private bool IsBiglewater(ItemStats x)
        {
            // helper for filetering items after bilgewater
            return ViewModel.ItemsFiltered[x.Id.ToString()].IsBilgewater;
        }

        private ActionResult CalculateItemLowestPickrate(bool onlyBilgewater)
        {
            var ranked = ItemsRanked;
            var list = ItemsBilgewater.Values.Where(x => x.Id > 0 && (!onlyBilgewater || IsBiglewater(x))).OrderBy(x => x.Pickrate).Select(x => new Merit
            {
                Id = ViewModel.Items[x.Id.ToString()].Id,
                Name = ViewModel.Items[x.Id.ToString()].Name,
                ValueBilgewater = x.Pickrate,
                ValueRanked = SafePickrate(ranked.Values.FirstOrDefault(y => y.Id == x.Id))
            }).Take(10).ToList();// take top 10
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Items = list
                }
            };
        }
        private ActionResult CalculateItemLowestWinrate(bool onlyBilgewater)
        {
            var ranked = ItemsRanked;
            var list = ItemsBilgewater.Values.Where(x => x.Id > 0 && (!onlyBilgewater || IsBiglewater(x))).OrderBy(x => x.Winrate).Select(x => new Merit
            {
                Id = ViewModel.Items[x.Id.ToString()].Id,
                Name = ViewModel.Items[x.Id.ToString()].Name,
                ValueBilgewater = x.Winrate * 100,// make winrate from 0.50 to 50
                ValueRanked = SafeWinrate(ranked.Values.FirstOrDefault(y => y.Id == x.Id)) * 100// make winrate from 0.50 to 50
            }).Take(10).ToList();// take top 10
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Items = list
                }
            };
        }

        private ActionResult CalculateItemTopWinrate(bool onlyBilgewater)
        {
            var ranked = ItemsRanked;
            var list = ItemsBilgewater.Values.Where(x => x.Id > 0 && (!onlyBilgewater || IsBiglewater(x))).OrderByDescending(x => x.Winrate).Select(x => new Merit
            {
                Id = ViewModel.Items[x.Id.ToString()].Id,
                Name = ViewModel.Items[x.Id.ToString()].Name,
                ValueBilgewater = x.Winrate * 100, // make winrate from 0.50 to 50
                ValueRanked = SafeWinrate(ranked.Values.FirstOrDefault(y => y.Id == x.Id)) * 100// make winrate from 0.50 to 50
            }).Take(10).ToList();// take top 10
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Items = list
                }
            };
        }
        /// <summary>
        /// get winrate if itemsats present 
        /// </summary>
        /// <param name="itemStats"></param>
        /// <returns></returns>
        private float SafeWinrate(ItemStats itemStats)
        {
            if (itemStats == null)
                return 0;
            else
                return itemStats.Winrate;
        }
        /// <summary>
        /// get pickrate if itemsats present 
        /// </summary>
        /// <param name="itemStats"></param>
        /// <returns></returns>
        private float SafePickrate(ItemStats itemStats)
        {
            if (itemStats == null)
                return 0;
            else
                return itemStats.Pickrate;
        }


        public ActionResult ChampionChart(string id)
        { 
            // load champion chart switch by selection
            switch (id)
            {
                case "TopAvgCS":
                    return CalculateChampionTopAvgCS();
                case "LowestAvgCS":
                    return CalculateChampionLowestAvgCS();
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

        private ActionResult CalculateChampionTopAvgCS()
        {
            var ranked = ChampionsRanked;
            var list = ChampionsBilgewater.Values.OrderByDescending(x => x.AvgMinionCount + x.AvgJungleCount).Select(x => new Merit
            {
                Key = ViewModel.ChampionsById[x.Id].Key,
                Name = ViewModel.ChampionsById[x.Id].Name,
                ValueBilgewater = x.AvgMinionCount + x.AvgJungleCount,
                ValueRanked = ranked.Values.First(y => y.Id == x.Id).AvgMinionCount + ranked.Values.First(y => y.Id == x.Id).AvgJungleCount
            }).Take(10).ToList();// take top 10
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Format = MerictFormat.Int.ToString(),
                    Champions = list
                }
            };
        }

        private ActionResult CalculateChampionLowestAvgCS()
        {
            var ranked = ChampionsRanked;
            var list = ChampionsBilgewater.Values.OrderBy(x => x.AvgMinionCount + x.AvgJungleCount).Select(x => new Merit
            {
                Key = ViewModel.ChampionsById[x.Id].Key,
                Name = ViewModel.ChampionsById[x.Id].Name,
                ValueBilgewater = x.AvgMinionCount + x.AvgJungleCount,
                ValueRanked = ranked.Values.First(y => y.Id == x.Id).AvgMinionCount + ranked.Values.First(y => y.Id == x.Id).AvgJungleCount
            }).Take(10).ToList();// take top 10
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Format = MerictFormat.Int.ToString(),
                    Champions = list
                }
            };
        }

        private ActionResult CalculateChampionTopPickrate()
        {
            var ranked = ChampionsRanked;
            var list = ChampionsBilgewater.Values.OrderByDescending(x => x.Pickrate).Select(x => new Merit
            {
                Key = ViewModel.ChampionsById[x.Id].Key,
                Name = ViewModel.ChampionsById[x.Id].Name,
                ValueBilgewater = x.Pickrate,
                ValueRanked = ranked.Values.First(y => y.Id == x.Id).Pickrate
            }).Take(10).ToList();// take top 10
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Format = MerictFormat.Float.ToString(),
                    Champions = list
                }
            };
        }

        private ActionResult CalculateChampionLowestPickrate()
        {
            var ranked = ChampionsRanked;
            var list = ChampionsBilgewater.Values.OrderBy(x => x.Pickrate).Select(x => new Merit
            {
                Key = ViewModel.ChampionsById[x.Id].Key,
                Name = ViewModel.ChampionsById[x.Id].Name,
                ValueBilgewater = x.Pickrate,
                ValueRanked = ranked.Values.First(y => y.Id == x.Id).Pickrate
            }).Take(10).ToList();// take top 10
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Format = MerictFormat.Float.ToString(),
                    Champions = list
                }
            };
        }

        private ActionResult CalculateChampionLowestWinrate()
        {
            var ranked = ChampionsRanked;
            var list = ChampionsBilgewater.Values.OrderBy(x => x.Winrate).Select(x => new Merit
            {
                Key = ViewModel.ChampionsById[x.Id].Key,
                Name = ViewModel.ChampionsById[x.Id].Name,
                ValueBilgewater = x.Winrate,
                ValueRanked = ranked.Values.First(y => y.Id == x.Id).Winrate
            }).Take(10).ToList();// take top 10
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Format = MerictFormat.Win.ToString(),
                    Champions = list
                }
            };
        }

        private ActionResult CalculateChampionTopWinrate()
        {
            var ranked = ChampionsRanked;
            var list = ChampionsBilgewater.Values.OrderByDescending(x => x.Winrate).Select(x => new Merit
            {
                Key = ViewModel.ChampionsById[x.Id].Key,
                Name = ViewModel.ChampionsById[x.Id].Name,
                ValueBilgewater = x.Winrate,
                ValueRanked = ranked.Values.First(y => y.Id == x.Id).Winrate
            }).Take(10).ToList();// take top 10
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Format = MerictFormat.Win.ToString(),
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