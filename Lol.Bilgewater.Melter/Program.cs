using ICSharpCode.SharpZipLib.Zip;
using Lol.Api.Bilgewater;
using Lol.Api.Bilgewater.Samples;
using Lol.Api.Static.Champion;
using Lol.Api.Static.Items;
using Lol.Api.Static.Match;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lol.Bilgewater.Melter
{
    class Program
    {
        static string data_path = null;
        static string sample_path = null, currentSource = null;
        static string[] sources = new string[]{"RANKED_SOLO", "BILGEWATER"};
        public static Dictionary<string, Dictionary<long, MercStatistcs>> match_data = null;
        public static MetaInformations meta_data = null;

        private static Dictionary<string, Champion> _champs = null;

        public static Dictionary<string, Champion> Champions
        {
            get
            {
                if (_champs == null)
                {
                    _champs = new Dictionary<string, Champion>();
                    var adapter = new StaticJsonAdapter(Path.Combine(data_path, "metadata"));
                    var result = adapter.ListChampions("euw", "en_US", "b409f8d7-bf70-47b3-961c-ab53419e20ff");
                    _champs = result.Data;
                }
                return _champs;
            }
        }

        static void Main(string[] args)
        {
            // get data path from config
            data_path = System.Configuration.ConfigurationSettings.AppSettings["DataPath"];
            // do this for RANKED_SOLO and BILGEWATER the both datasets to compare later
            foreach (var s in sources)
            {
                match_data = new Dictionary<string, Dictionary<long, MercStatistcs>>();
                // set currentsource
                currentSource = s;
                // combine sample path
                sample_path = Path.Combine(data_path, currentSource, "Samples");                
                // read meta informations about merc ids, item ids, etc. (mercs.xml)
                ReadMetaData();
                // now read all the json files and get the infos we want
                RefineMatchData();
                // create the data samples displayed in the websites
                CreateSamples();
            }
            System.Console.ReadLine();
        }

        public static void ReadMetaData()
        {
            var xser = new XmlSerializer(typeof(MetaInformations));
            using (var sr = new StreamReader(Path.Combine(data_path, "metadata", "mercs.xml")))
            {
                meta_data = (MetaInformations)xser.Deserialize(sr);
            }
        }

        public static void CreateSamples()
        {
            // read refinded data from file
            foreach (var file in Directory.GetFiles(Path.Combine(data_path, currentSource, "Refined")))
            {
                string protopath = file;
                var region = Path.GetFileName(file).Split('_')[0];
                if (!File.Exists(protopath))
                    continue;
                // deserialize with protobuf
                using (var s = new FileStream(protopath, FileMode.Open))
                    match_data.Add(region, ProtoBuf.Serializer.Deserialize<Dictionary<long, MercStatistcs>>(s));
            }
            // create samples
            SampleMercStats();
            SampleItemStats();
            SampleDurationStats();
            SampleChampionStats();
        }

        private static void SampleItemStats()
        {
            Dictionary<string, Dictionary<int, ItemStats>> stats = new Dictionary<string, Dictionary<int, ItemStats>>();
            // create an all entry for web presentation
            var all = new Dictionary<int, ItemStats>();
            int allcount = 0;
            stats.Add("all", all);
            // each key a region
            foreach (var key in match_data.Keys)
            {
                allcount += match_data[key].Count;
                var current = new Dictionary<int, ItemStats>();
                stats.Add(key, current);
                // go through the matches
                foreach (var match in match_data[key].Values)
                {
                    List<MercTeam> teams = new List<MercTeam>();
                    teams.Add(match.TeamBlue);
                    teams.Add(match.TeamRed);
                    // count items from both teams
                    foreach (var team in teams)
                    {
                        // for each player
                        foreach (var part in team.Participants)
                        {
                            // count method helps counting
                            Count((int)part.Item0, current, team.Winner);
                            Count((int)part.Item1, current, team.Winner);
                            Count((int)part.Item2, current, team.Winner);
                            Count((int)part.Item3, current, team.Winner);
                            Count((int)part.Item4, current, team.Winner);
                            Count((int)part.Item5, current, team.Winner);
                            Count((int)part.Item6, current, team.Winner);
                        }
                    }
                }
            }
            // each key a region
            foreach (var key in stats.Keys)
            {
                // calculate averages for all items
                foreach (var item in stats[key].Keys)
                {
                    // update stats for the "all" region
                    if (!all.ContainsKey(item))
                    {
                        all.Add(item, new ItemStats
                        {
                            Id = stats[key][item].Id,
                            IsBiglewater = stats[key][item].IsBiglewater,
                            Pickcount = stats[key][item].Pickcount,
                            Wincount = stats[key][item].Wincount,
                        });
                    }
                    else
                    {
                        all[item].Pickcount += stats[key][item].Pickcount;
                        all[item].Wincount += stats[key][item].Wincount;
                    }
                }
            }
            // each key a region
            foreach (var key in stats.Keys)
            {
                // calculate averages for all items
                foreach (var item in stats[key])
                {
                    // all regions can use itemcount from dictionary, all uses allcount
                    if (key != "all")
                    {
                        item.Value.Winrate = (float)item.Value.Wincount / item.Value.Pickcount;
                        item.Value.Pickrate = (float)item.Value.Pickcount / match_data[key].Count * 10;
                    }
                    else
                    {
                        item.Value.Winrate = (float)item.Value.Wincount / item.Value.Pickcount;
                        item.Value.Pickrate = (float)item.Value.Pickcount / allcount * 10;
                    }
                }
            }
            // safe the reigon samples to seperate proto files like \na\Items.proto
            foreach (var key in stats.Keys)
            {
                string path = Path.Combine(sample_path, key, "Items.proto");
                if (!Directory.Exists(Path.Combine(sample_path, key))) Directory.CreateDirectory(Path.Combine(sample_path, key));
                using (var s = new FileStream(path, FileMode.Create))
                    ProtoBuf.Serializer.Serialize<Dictionary<int, ItemStats>>(s, stats[key]);
            }
        }

        private static void Count(int item, Dictionary<int, ItemStats> current, bool win)
        {
            // if item wasn't counted before, add it to dictionary
            if (!current.ContainsKey(item))
                current.Add(item, new ItemStats { Id=item });
            // make a bilgewater marker
            current[item].IsBiglewater = meta_data.Item.Contains((int)item);
            // increase pickcount
            current[item].Pickcount++;
            // and wincount in case...
            if (win)
                current[item].Wincount++;
        }

        private static void SampleChampionStats()
        {
            Dictionary<string, Dictionary<int, ChampionStat>> stats = new Dictionary<string, Dictionary<int, ChampionStat>>();
            Dictionary<string, Dictionary<string, ChampionStat>> rolestats = new Dictionary<string, Dictionary<string, ChampionStat>>();
            // add an all region for web presentation of championstats
            var all = new Dictionary<int, ChampionStat>();
            int allcount = 0;
            stats.Add("all", all);
            // add an all region for web presentation of rolestats
            var allrole = new Dictionary<string, ChampionStat>();
            rolestats.Add("all", allrole);
            // each key a region
            foreach (var key in match_data.Keys)
            {
                // increase allcount
                allcount += match_data[key].Count;
                var current = new Dictionary<int, ChampionStat>();
                stats.Add(key, current);
                var currentrole = new Dictionary<string, ChampionStat>();
                rolestats.Add(key, currentrole);
                // go through matches
                foreach (var match in match_data[key].Values)
                {
                    List<MercTeam> teams = new List<MercTeam>();
                    teams.Add(match.TeamBlue);
                    teams.Add(match.TeamRed);
                    // work through both teams
                    foreach (var team in teams)
                    {
                        // and all players
                        foreach (var champ in team.Participants)
                        {
                            ChampionStat ctl, cta;
                            // add champion to dictionary if not present
                            if (!current.ContainsKey(champ.ChampionId))
                            {
                                ctl = new ChampionStat { Id = champ.ChampionId };
                                current.Add(champ.ChampionId, ctl);
                            }
                            else
                                ctl = current[champ.ChampionId];
                            // add champion to dictionary if not present - for all
                            if (!all.ContainsKey(champ.ChampionId))
                            {
                                cta = new ChampionStat { Id = champ.ChampionId };
                                all.Add(champ.ChampionId, cta);
                            }
                            else
                                cta = all[champ.ChampionId];
                            // ctl = current champion
                            // cta = current champion - for all
                            if (team.Winner)
                            {
                                ctl.Wincount++;
                                cta.Wincount++;
                            }
                            ctl.Pickcount++;
                            cta.Pickcount++;
                            // count stats (kills, assists, etc)
                            ctl.KillCount += champ.Kills;
                            ctl.AssistCount += champ.Assists;
                            ctl.DeathCount += champ.Deaths;
                            ctl.MinionCount += champ.MinionsKilled;
                            ctl.JungleCount += champ.NeutralMinionsKilled;
                            ctl.HostileJungleCount += champ.NeutralMinionsKilledEnemyJungle;
                            ctl.AlliedJungleCount += champ.NeutralMinionsKilledTeamJungle;
                            ctl.PentaKillCount += champ.PentaKills;
                            // count stats (kills, assists, etc) - for all again
                            cta.KillCount += champ.Kills;
                            cta.AssistCount += champ.Assists;
                            cta.DeathCount += champ.Deaths;
                            cta.MinionCount += champ.MinionsKilled;
                            cta.JungleCount += champ.NeutralMinionsKilled;
                            cta.HostileJungleCount += champ.NeutralMinionsKilledEnemyJungle;
                            cta.AlliedJungleCount += champ.NeutralMinionsKilledTeamJungle;
                            cta.PentaKillCount += champ.PentaKills;
                            // get default role for champ
                            var champDef = Champions.Values.First(x => x.Id == champ.ChampionId.ToString());
                            ChampionStat rtl, rta;
                            // add role to dicitonary if not exists
                            if (!currentrole.ContainsKey(champDef.Tags[0]))
                            {
                                rtl = new ChampionStat { Role = champDef.Tags[0] };
                                currentrole.Add(champDef.Tags[0], rtl);
                            }
                            else
                                rtl = currentrole[champDef.Tags[0]];
                            // add role to dicitonary if not exists - for all
                            if (!allrole.ContainsKey(champDef.Tags[0]))
                            {
                                rta = new ChampionStat { Role = champDef.Tags[0] };
                                allrole.Add(champDef.Tags[0], rta);
                            }
                            else
                                rta = allrole[champDef.Tags[0]];
                            // count win and pick
                            if (team.Winner)
                            {
                                rtl.Wincount++;
                                rta.Wincount++;
                            }
                            rtl.Pickcount++;
                            rta.Pickcount++;

                            // count role stats like kill/assists/etc
                            rtl.KillCount += champ.Kills;
                            rtl.AssistCount += champ.Assists;
                            rtl.DeathCount += champ.Deaths;
                            rtl.MinionCount += champ.MinionsKilled;
                            rtl.JungleCount += champ.NeutralMinionsKilled;
                            rtl.HostileJungleCount += champ.NeutralMinionsKilledEnemyJungle;
                            rtl.AlliedJungleCount += champ.NeutralMinionsKilledTeamJungle;
                            rtl.PentaKillCount += champ.PentaKills;
                            // count role stats like kill/assists/etc - all
                            rta.KillCount += champ.Kills;
                            rta.AssistCount += champ.Assists;
                            rta.DeathCount += champ.Deaths;
                            rta.MinionCount += champ.MinionsKilled;
                            rta.JungleCount += champ.NeutralMinionsKilled;
                            rta.HostileJungleCount += champ.NeutralMinionsKilledEnemyJungle;
                            rta.AlliedJungleCount += champ.NeutralMinionsKilledTeamJungle;
                            rta.PentaKillCount += champ.PentaKills;
                        }
                    }
                }
            }
            // each key a region
            foreach (var key in stats.Keys)
            {
                // for all champs
                foreach (var champ in stats[key])
                {                    
                    champ.Value.Winrate = (float)champ.Value.Wincount / champ.Value.Pickcount;
                    if (key != "all")
                        champ.Value.Pickrate = (float)champ.Value.Pickcount / match_data[key].Count * 20;
                    else
                        champ.Value.Pickrate = (float)champ.Value.Pickcount / allcount * 20;
                    champ.Value.AvgKillCount = (float)champ.Value.KillCount / champ.Value.Pickcount;
                    champ.Value.AvgAssistCount = (float)champ.Value.AssistCount / champ.Value.Pickcount;
                    champ.Value.AvgDeathCount = (float)champ.Value.DeathCount / champ.Value.Pickcount;
                    champ.Value.AvgPentaKillCount = (float)champ.Value.PentaKillCount / champ.Value.Pickcount;
                    champ.Value.AvgMinionCount = (float)champ.Value.MinionCount / champ.Value.Pickcount;
                    champ.Value.AvgJungleCount = (float)champ.Value.JungleCount / champ.Value.Pickcount;
                    champ.Value.AvgAlliedJungleCount = (float)champ.Value.AlliedJungleCount / champ.Value.Pickcount;
                    champ.Value.AvgHostileJungleCount = (float)champ.Value.HostileJungleCount / champ.Value.Pickcount;
                }
            }
            // each key a region
            foreach (var key in rolestats.Keys)
            {
                // for all roles
                foreach (var champ in rolestats[key])
                {
                    champ.Value.Winrate = (float)champ.Value.Wincount / champ.Value.Pickcount;
                    if (key != "all")
                        champ.Value.Pickrate = (float)champ.Value.Pickcount / match_data[key].Count * 10;
                    else
                        champ.Value.Pickrate = (float)champ.Value.Pickcount / allcount * 10;
                    champ.Value.AvgKillCount = (float)champ.Value.KillCount / champ.Value.Pickcount;
                    champ.Value.AvgAssistCount = (float)champ.Value.AssistCount / champ.Value.Pickcount;
                    champ.Value.AvgDeathCount = (float)champ.Value.DeathCount / champ.Value.Pickcount;
                    champ.Value.AvgPentaKillCount = (float)champ.Value.PentaKillCount / champ.Value.Pickcount;
                    champ.Value.AvgMinionCount = (float)champ.Value.MinionCount / champ.Value.Pickcount;
                    champ.Value.AvgJungleCount = (float)champ.Value.JungleCount / champ.Value.Pickcount;
                    champ.Value.AvgAlliedJungleCount = (float)champ.Value.AlliedJungleCount / champ.Value.Pickcount;
                    champ.Value.AvgHostileJungleCount = (float)champ.Value.HostileJungleCount / champ.Value.Pickcount;
                }
            }
            // safe champion stats to file for each region
            foreach (var key in stats.Keys)
            {
                // like ko\Champion.proto
                string path = Path.Combine(sample_path, key, "Champion.proto");
                if (!Directory.Exists(Path.Combine(sample_path, key))) Directory.CreateDirectory(Path.Combine(sample_path, key));
                using (var s = new FileStream(path, FileMode.Create))
                    ProtoBuf.Serializer.Serialize<Dictionary<int, ChampionStat>>(s, stats[key]);
            }
            // safe role stats to file for each region
            foreach (var key in rolestats.Keys)
            {
                // like ko\Role.proto
                string path = Path.Combine(sample_path, key, "Role.proto");
                if (!Directory.Exists(Path.Combine(sample_path, key))) Directory.CreateDirectory(Path.Combine(sample_path, key));
                using (var s = new FileStream(path, FileMode.Create))
                    ProtoBuf.Serializer.Serialize<Dictionary<string, ChampionStat>>(s, rolestats[key]);
            }
        }

        private static void SampleDurationStats()
        {
            Dictionary<string, DurationStats> stats = new Dictionary<string, DurationStats>();
            // add all region
            var all = new DurationStats();
            int allcount = 0;
            stats.Add("all", all);
            // each key a region
            foreach (var key in match_data.Keys)
            {
                // for all count 
                allcount += match_data[key].Count;
                var current = new DurationStats();
                stats.Add(key, current);
                foreach (var match in match_data[key].Values)
                {
                    // add duration to all and current
                    current.MatchDuration += match.MatchDuration;
                    all.MatchDuration += match.MatchDuration;
                    // add firstblood time to all and current
                    current.Firstblood += match.FirstBlood;
                    all.Firstblood += match.FirstBlood;
                    // add first dragon time to all and current
                    if (match.FirstDragon.HasValue)
                    {
                        current.FirstDragon += match.FirstDragon.Value;
                        current.DragonKilledMatchCount++;
                        all.FirstDragon += match.FirstDragon.Value;
                        all.DragonKilledMatchCount++;
                    }
                    // add first baron time to all and current
                    if (match.FirstBaron.HasValue)
                    {
                        current.FirstBaron += match.FirstBaron.Value;
                        current.BaronKilledMatchCount++;
                        all.FirstBaron += match.FirstBaron.Value;
                        all.BaronKilledMatchCount++;
                    }
                    // add first tower time to all and current
                    if (match.FirstTower.HasValue)
                    {
                        current.FirstTower += match.FirstTower.Value;
                        all.FirstTower += match.FirstTower.Value;
                    }
                    // add first inhib time to all and current
                    if (match.FirstInhib.HasValue)
                    {
                        current.FirstInhib += match.FirstInhib.Value;
                        all.FirstInhib += match.FirstInhib.Value;
                    }
                }
            }
            // each key a region
            foreach (var key in stats.Keys)
            {
                var dur = stats[key];
                // calculate average duration for all merits
                if (key != "all")
                {
                    dur.AvgMatchDuration = (float)dur.MatchDuration / match_data[key].Count;
                    dur.AvgFirstblood = (float)dur.Firstblood / match_data[key].Count;
                    dur.AvgFirstDragon = (float)dur.FirstDragon / dur.BaronKilledMatchCount;
                    dur.AvgFirstBaron = (float)dur.FirstBaron / dur.DragonKilledMatchCount;
                    dur.AvgFirstInhib = (float)dur.FirstInhib / match_data[key].Count;
                    dur.AvgFirstTower = (float)dur.FirstTower / match_data[key].Count;
                }
                // calculate average duration for all merits for all
                else
                {
                    dur.AvgMatchDuration = (float)dur.MatchDuration / allcount;
                    dur.AvgFirstblood = (float)dur.Firstblood / allcount;
                    dur.AvgFirstDragon = (float)dur.FirstDragon / dur.DragonKilledMatchCount;
                    dur.AvgFirstBaron = (float)dur.FirstBaron / dur.BaronKilledMatchCount;
                    dur.AvgFirstInhib = (float)dur.FirstInhib / allcount;
                    dur.AvgFirstTower = (float)dur.FirstTower / allcount;
                }
            }
            // serialize duration stats to disk
            foreach (var key in stats.Keys)
            {
                // one file for each region like lan\Duraiton.proto
                string path = Path.Combine(sample_path, key, "Duration.proto");
                if (!Directory.Exists(Path.Combine(sample_path, key))) Directory.CreateDirectory(Path.Combine(sample_path, key));
                using (var s = new FileStream(path, FileMode.Create))
                    ProtoBuf.Serializer.Serialize<DurationStats>(s, stats[key]);
            }
        }

        private static void SampleMercStats()
        {
            Dictionary<string, Dictionary<int, MercStats>> stats = new Dictionary<string, Dictionary<int, MercStats>>();
            // add all entry for all region
            var all = new Dictionary<int,MercStats>();
            int allcount = 0;
            stats.Add("all", all);
            // every key a region
            foreach (var key in match_data.Keys)
            {
                allcount += match_data[key].Count;
                var current = new Dictionary<int, MercStats>();
                stats.Add(key, current);
                // go through matches
                foreach (var match in match_data[key].Values)
                {
                    List<MercTeam> teams = new List<MercTeam>();
                    teams.Add(match.TeamBlue);
                    teams.Add(match.TeamRed);
                    // look in both teams
                    foreach (var team in teams)
                    {
                        if (team.MercsBought != null)
                        {
                            // count merc bought
                            foreach (var merc in team.MercsBought)
                            {
                                // current region
                                if (!current.ContainsKey(merc.Id))
                                    current.Add(merc.Id, new MercStats { Id = merc.Id });
                                if (team.Winner)
                                    current[merc.Id].Wincount++;
                                current[merc.Id].Pickcount++;
                                // all region
                                if (!all.ContainsKey(merc.Id))
                                    all.Add(merc.Id, new MercStats { Id = merc.Id });
                                if (team.Winner)
                                    all[merc.Id].Wincount++;
                                all[merc.Id].Pickcount++;
                            }
                            // get the no mercs bought stats if team has less then 5 mercs bought
                            if (team.MercsBought.Count < 5)
                            {
                                // current region
                                if (!current.ContainsKey(-1))
                                    current.Add(-1, new MercStats { Id = -1 });
                                if (team.Winner)
                                    current[-1].Wincount += (5 - team.MercsBought.Count);
                                current[-1].Pickcount += (5 - team.MercsBought.Count);
                                // all region
                                if (!all.ContainsKey(-1))
                                    all.Add(-1, new MercStats { Id = -1 });
                                if (team.Winner)
                                    all[-1].Wincount += (5 - team.MercsBought.Count);
                                all[-1].Pickcount += (5 - team.MercsBought.Count);
                            }
                        }
                    }
                }
            }
            // every key a region
            foreach(var key in stats.Keys)
            {
                // go through all mercs
                foreach (var merc in stats[key])
                {
                    // calc avg win and pickrate
                    if (key != "all")
                    {
                        merc.Value.Winrate = (float)merc.Value.Wincount / match_data[key].Count * 10;
                        merc.Value.Pickrate = (float)merc.Value.Pickcount / match_data[key].Count * 10;
                    }
                    else
                    {
                        merc.Value.Winrate = (float)merc.Value.Wincount / allcount * 10;
                        merc.Value.Pickrate = (float)merc.Value.Pickcount / allcount * 10;
                    }
                }
            }
            // for each region create a file
            foreach (var key in stats.Keys)
            {
                // like euw\Merc.proto
                string path = Path.Combine(sample_path, key, "Merc.proto");
                if (!Directory.Exists(Path.Combine(sample_path, key))) Directory.CreateDirectory(Path.Combine(sample_path, key));
                using (var s = new FileStream(path, FileMode.Create))
                    ProtoBuf.Serializer.Serialize<Dictionary<int, MercStats>>(s, stats[key]);
            }
        }

        public static void RefineMatchData()
        {
            // set destination path for refined data
            string refinedPath = Path.Combine(data_path, currentSource, "Refined");
            // ensure existance of the path
            if (!Directory.Exists(refinedPath))
                Directory.CreateDirectory(refinedPath);
            // iterate through the regions
            foreach (var folder in Directory.GetDirectories(Path.Combine(data_path, currentSource, "Raw")))
            {                
                // get the region from foldername
                var region = Path.GetFileName(folder);
                // combine zippath
                string zippath = Path.Combine(folder, region + "_matches.zip");
                // combine protopath for later storage
                string protopath = Path.Combine(refinedPath, region + "_matches.proto");
                // skip when already processed
                if (File.Exists(protopath))
                    continue;
                // skip if zip is missing
                if (!File.Exists(zippath))
                    continue;
                var zip = new ZipFile(zippath);
                // create dictionary entry for region
                var data = new Dictionary<long, MercStatistcs>();
                match_data.Add(region, data);
                long entries = zip.Count;
                long current = 0;
                using (zip)
                {
                    // every entry is one match
                    foreach (ZipEntry entry in zip)
                    {
                        try
                        {
                            // extract json from zip
                            var json = new StreamReader(zip.GetInputStream(entry)).ReadToEnd();
                            // convert it into objects
                            var converted = JsonConvert.DeserializeObject<MatchDetail>(json);
                            // analyze and store
                            data.Add(converted.MatchId, AnalyzeMatch(converted));
                            current++;
                            // every 100 files update display
                            if (current % 100 == 0)
                            {
                                System.Console.Clear();
                                foreach (var list in match_data)
                                {
                                    System.Console.WriteLine(string.Format("{1}: {0:0.00}% Done...", (float)match_data[list.Key].Count / 100.0, list.Key));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error converting: " + entry.Name);
                        }
                    }
                }
                // store all the analyzed data with protobuf
                using (var s = new FileStream(protopath, FileMode.Create))
                    ProtoBuf.Serializer.Serialize<Dictionary<long, MercStatistcs>>(s, data);
            }
        }

        private static MercStatistcs AnalyzeMatch(MatchDetail converted)
        {
            var res = new MercStatistcs();
            // id is somewhat obligatory
            res.MatchId = converted.MatchId;
            // using linq we look in frames for the first event with "BARON_NASHOR" and extract the timetamp in seconds - is milliseconds in file
            res.FirstBaron = converted.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.MonsterType == "BARON_NASHOR")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            // using linq we look in frames for the first event with "BARON_DRAGON" and extract the timetamp in seconds - is milliseconds in file
            res.FirstDragon = converted.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.MonsterType == "DRAGON")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            // using linq we look in frames for the first event with "CHAMPION_KILL" and extract the timetamp in seconds - is milliseconds in file
            res.FirstBlood = converted.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.EventType == "CHAMPION_KILL")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            // using linq we look in frames for the first event with "BUILDING_KILL" and buildingtype = "INHIBITOR_BUIDLING and extract the timetamp in seconds - is milliseconds in file
            res.FirstInhib = converted.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.EventType == "BUILDING_KILL") && x.Events.Any(y => y.BuildingType == "INHIBITOR_BUILDING")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            // using linq we look in frames for the first event with "BUILDING_KILL" and buildingtype = "TOWER_BUILDING and extract the timetamp in seconds - is milliseconds in file
            res.FirstTower = converted.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.EventType == "BUILDING_KILL") && x.Events.Any(y => y.BuildingType == "TOWER_BUILDING")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            // save matchduration
            res.MatchDuration = converted.MatchDuration;
            // get data from teams
            res.TeamBlue = AnalyzeTeam(converted.Teams[0], converted);
            res.TeamRed = AnalyzeTeam(converted.Teams[1], converted);
            return res;
        }

        private static MercTeam AnalyzeTeam(Team team, MatchDetail match)
        {
            // get teammembers
            var teamMembers = match.Participants.Where(x => x.TeamId == team.TeamId).ToList();
            var res = new MercTeam();
            // safe data like baronkill
            res.BaronKills = team.BaronKills;
            // dragonkill
            res.DragonKills = team.DragonKills;
            // did they firstblood
            res.FirstBlood = team.FirstBlood;
            // when first baron
            res.FirstBaron = match.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.MonsterType == "BARON_NASHOR" && teamMembers.Select(z => z.ParticipantId).Contains(y.KillerId))).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            // when first dragon
            res.FirstDragon = match.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.MonsterType == "DRAGON" && teamMembers.Select(z => z.ParticipantId).Contains(y.KillerId))).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            // when first inhibitor
            res.FirstInhibitorTaken = match.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.EventType == "BUILDING_KILL" && teamMembers.Select(z => z.ParticipantId).Contains(y.KillerId)) && x.Events.Any(y => y.BuildingType == "INHIBITOR_BUILDING")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            // when first tower
            res.FirstTowerTaken = match.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.EventType == "BUILDING_KILL" && teamMembers.Select(z => z.ParticipantId).Contains(y.KillerId)) && x.Events.Any(y => y.BuildingType == "TOWER_BUILDING")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            // did they win?
            res.Winner = team.Winner;
            // find out wich mercaneries the members bouhgt
            res.MercsBought = new List<MercData>();
            foreach (var m in teamMembers)
            {
                // find an ITEM_PURCHASED event with one of the item ids from metadata
                var mercId = match.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.EventType == "ITEM_PURCHASED" && meta_data.Merc.Contains(y.ItemId) && y.ParticipantId == m.ParticipantId)).Select(x => x.Events.FirstOrDefault(y => y.EventType == "ITEM_PURCHASED" && meta_data.Merc.Contains(y.ItemId) && y.ParticipantId == m.ParticipantId)).FirstOrDefault();
                if (mercId != null)
                {
                    var merc = new MercData();
                    merc.Id = mercId.ItemId;
                    merc.ParticipantId = m.ParticipantId;
                    // look max defenslevel bought
                    merc.DefenseLevel = match.Timeline
                        .Frames.Where(x =>
                            x.Events != null
                            && x.Events.Any(y => y.EventType == "ITEM_PURCHASED" && meta_data.Defense.Select(z => z.Id).Contains(y.ItemId) && y.ParticipantId == m.ParticipantId)
                            ).Count();
                    // look max offenselevel bought
                    merc.OffenseLevel = match.Timeline
                        .Frames.Where(x =>
                            x.Events != null
                            && x.Events.Any(y => y.EventType == "ITEM_PURCHASED" && meta_data.Offense.Select(z => z.Id).Contains(y.ItemId) && y.ParticipantId == m.ParticipantId)
                            ).Count();
                    // look max upgradelevel bought
                    merc.UpgradeLevel = match.Timeline
                        .Frames.Where(x =>
                            x.Events != null
                            && x.Events.Any(y => y.EventType == "ITEM_PURCHASED" && meta_data.Upgrade.Select(z => z.Id).Contains(y.ItemId) && y.ParticipantId == m.ParticipantId)
                            ).Count();
                    res.MercsBought.Add(merc);
                }
            }
            // safe importent data from participant data trash the rest
            res.Participants = teamMembers.Select(x => new Lol.Api.Bilgewater.Participant
            {
                Id = x.ParticipantId,
                ChampionId = x.ChampionId,
                Assists = x.Stats.Assists,
                Deaths = x.Stats.Deaths,
                Kills = x.Stats.Kills,
                MinionsKilled = x.Stats.MinionsKilled,
                NeutralMinionsKilled = x.Stats.NeutralMinionsKilled,
                NeutralMinionsKilledEnemyJungle = x.Stats.NeutralMinionsKilledEnemyJungle,
                NeutralMinionsKilledTeamJungle = x.Stats.NeutralMinionsKilledTeamJungle,
                PentaKills = x.Stats.PentaKills,
                Item0 = x.Stats.Item0,
                Item1 = x.Stats.Item1,
                Item2 = x.Stats.Item2,
                Item3 = x.Stats.Item3,
                Item4 = x.Stats.Item4,
                Item5 = x.Stats.Item5,
                Item6 = x.Stats.Item6,
                Spell1Id = x.Spell1Id,
                Spell2Id = x.Spell2Id,
            }).ToList();
            return res;
        }
    }
}
