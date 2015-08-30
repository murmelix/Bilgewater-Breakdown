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
        static string sample_path = null;
        static string currentSource = "RANKED_SOLO";
        public static Dictionary<string, Dictionary<long, MercStatistcs>> match_data = new Dictionary<string, Dictionary<long, MercStatistcs>>();
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
            data_path = System.Configuration.ConfigurationSettings.AppSettings["DataPath"];
            sample_path = Path.Combine(data_path, currentSource, "Samples");
            ReadMetaData();
            RefineMatchData();
            CreateSamples();
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
            foreach (var file in Directory.GetFiles(Path.Combine(data_path, currentSource, "Refined")))
            {
                string protopath = file;
                var region = Path.GetFileName(file).Split('_')[0];
                if (!File.Exists(protopath))
                    continue;
                using (var s = new FileStream(protopath, FileMode.Open))
                    match_data.Add(region, ProtoBuf.Serializer.Deserialize<Dictionary<long, MercStatistcs>>(s));
            }
            //SampleMercStats();
            SampleItemStats();
            //SampleDurationStats();
            //SampleChampionStats();
        }

        private static void SampleItemStats()
        {
            Dictionary<string, Dictionary<int, ItemStats>> stats = new Dictionary<string, Dictionary<int, ItemStats>>();
            var all = new Dictionary<int, ItemStats>();
            int allcount = 0;
            stats.Add("all", all);

            foreach (var key in match_data.Keys)
            {
                allcount += match_data[key].Count;
                var current = new Dictionary<int, ItemStats>();
                stats.Add(key, current);
                foreach (var match in match_data[key].Values)
                {
                    List<MercTeam> teams = new List<MercTeam>();
                    teams.Add(match.TeamBlue);
                    teams.Add(match.TeamRed);
                    foreach (var team in teams)
                    {
                        foreach (var part in team.Participants)
                        {
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

            foreach (var key in stats.Keys)
            {
                foreach (var item in stats[key].Keys)
                {
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

            foreach (var key in stats.Keys)
            {
                foreach (var item in stats[key])
                {
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
            if (!current.ContainsKey(item))
                current.Add(item, new ItemStats { Id=item });
            current[item].IsBiglewater = meta_data.Item.Contains((int)item);
            current[item].Pickcount++;
            if (win)
                current[item].Wincount++;
        }

        private static void SampleChampionStats()
        {
            Dictionary<string, Dictionary<int, ChampionStat>> stats = new Dictionary<string, Dictionary<int, ChampionStat>>();
            Dictionary<string, Dictionary<string, ChampionStat>> rolestats = new Dictionary<string, Dictionary<string, ChampionStat>>();
            var all = new Dictionary<int, ChampionStat>();
            int allcount = 0;
            stats.Add("all", all);
            var allrole = new Dictionary<string, ChampionStat>();
            rolestats.Add("all", allrole);
            foreach (var key in match_data.Keys)
            {
                allcount += match_data[key].Count;
                var current = new Dictionary<int, ChampionStat>();
                stats.Add(key, current);
                var currentrole = new Dictionary<string, ChampionStat>();
                rolestats.Add(key, currentrole);
                foreach (var match in match_data[key].Values)
                {
                    List<MercTeam> teams = new List<MercTeam>();
                    teams.Add(match.TeamBlue);
                    teams.Add(match.TeamRed);
                    foreach (var team in teams)
                    {
                        foreach (var champ in team.Participants)
                        {
                            ChampionStat ctl, cta;
                            if (!current.ContainsKey(champ.ChampionId))
                            {
                                ctl = new ChampionStat { Id = champ.ChampionId };
                                current.Add(champ.ChampionId, ctl);
                            }
                            else
                                ctl = current[champ.ChampionId];
                            if (!all.ContainsKey(champ.ChampionId))
                            {
                                cta = new ChampionStat { Id = champ.ChampionId };
                                all.Add(champ.ChampionId, cta);
                            }
                            else
                                cta = all[champ.ChampionId];
                            if (team.Winner)
                            {
                                ctl.Wincount++;
                                cta.Wincount++;
                            }
                            ctl.Pickcount++;
                            cta.Pickcount++;

                            ctl.KillCount += champ.Kills;
                            ctl.AssistCount += champ.Assists;
                            ctl.DeathCount += champ.Deaths;
                            ctl.MinionCount += champ.MinionsKilled;
                            ctl.JungleCount += champ.NeutralMinionsKilled;
                            ctl.HostileJungleCount += champ.NeutralMinionsKilledEnemyJungle;
                            ctl.AlliedJungleCount += champ.NeutralMinionsKilledTeamJungle;
                            ctl.PentaKillCount += champ.PentaKills;

                            cta.KillCount += champ.Kills;
                            cta.AssistCount += champ.Assists;
                            cta.DeathCount += champ.Deaths;
                            cta.MinionCount += champ.MinionsKilled;
                            cta.JungleCount += champ.NeutralMinionsKilled;
                            cta.HostileJungleCount += champ.NeutralMinionsKilledEnemyJungle;
                            cta.AlliedJungleCount += champ.NeutralMinionsKilledTeamJungle;
                            cta.PentaKillCount += champ.PentaKills;

                            var champDef = Champions.Values.First(x => x.Id == champ.ChampionId.ToString());
                            ChampionStat rtl, rta;
                            if (!currentrole.ContainsKey(champDef.Tags[0]))
                            {
                                rtl = new ChampionStat { Role = champDef.Tags[0] };
                                currentrole.Add(champDef.Tags[0], rtl);
                            }
                            else
                                rtl = currentrole[champDef.Tags[0]];
                            if (!allrole.ContainsKey(champDef.Tags[0]))
                            {
                                rta = new ChampionStat { Role = champDef.Tags[0] };
                                allrole.Add(champDef.Tags[0], rta);
                            }
                            else
                                rta = allrole[champDef.Tags[0]];
                            if (team.Winner)
                            {
                                rtl.Wincount++;
                                rta.Wincount++;
                            }
                            rtl.Pickcount++;
                            rta.Pickcount++;

                            rtl.KillCount += champ.Kills;
                            rtl.AssistCount += champ.Assists;
                            rtl.DeathCount += champ.Deaths;
                            rtl.MinionCount += champ.MinionsKilled;
                            rtl.JungleCount += champ.NeutralMinionsKilled;
                            rtl.HostileJungleCount += champ.NeutralMinionsKilledEnemyJungle;
                            rtl.AlliedJungleCount += champ.NeutralMinionsKilledTeamJungle;
                            rtl.PentaKillCount += champ.PentaKills;

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

            foreach (var key in stats.Keys)
            {
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
            foreach (var key in rolestats.Keys)
            {
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
            var picks = stats["all"].Select(x => new { Id=  x.Value.Id, Pick = x.Value.Pickrate}).OrderBy(x => x.Pick).ToList();
            var wins = stats["all"].Select(x => new { Id=  x.Value.Id, Win = x.Value.Winrate}).OrderBy(x => x.Win).ToList();
            float allPick = stats["all"].Sum(x => x.Value.Pickrate);
            float allWin = stats["all"].Sum(x => x.Value.Winrate);

            foreach (var key in stats.Keys)
            {
                string path = Path.Combine(sample_path, key, "Champion.proto");
                if (!Directory.Exists(Path.Combine(sample_path, key))) Directory.CreateDirectory(Path.Combine(sample_path, key));
                using (var s = new FileStream(path, FileMode.Create))
                    ProtoBuf.Serializer.Serialize<Dictionary<int, ChampionStat>>(s, stats[key]);
            }

            var rolepicks = rolestats["all"].Select(x => new { Role = x.Value.Role, Pick = x.Value.Pickrate }).OrderBy(x => x.Pick).ToList();
            var rolewins = rolestats["all"].Select(x => new { Role = x.Value.Role, Win = x.Value.Winrate }).OrderBy(x => x.Win).ToList();
            float roleallPick = rolestats["all"].Sum(x => x.Value.Pickrate);
            float roleallWin = rolestats["all"].Sum(x => x.Value.Winrate);

            foreach (var key in rolestats.Keys)
            {
                string path = Path.Combine(sample_path, key, "Role.proto");
                if (!Directory.Exists(Path.Combine(sample_path, key))) Directory.CreateDirectory(Path.Combine(sample_path, key));
                using (var s = new FileStream(path, FileMode.Create))
                    ProtoBuf.Serializer.Serialize<Dictionary<string, ChampionStat>>(s, rolestats[key]);
            }
        }

        private static void SampleDurationStats()
        {
            Dictionary<string, DurationStats> stats = new Dictionary<string, DurationStats>();
            var all = new DurationStats();
            int allcount = 0;
            stats.Add("all", all);
            foreach (var key in match_data.Keys)
            {
                allcount += match_data[key].Count;
                var current = new DurationStats();
                stats.Add(key, current);
                foreach (var match in match_data[key].Values)
                {
                    current.MatchDuration += match.MatchDuration;
                    all.MatchDuration += match.MatchDuration;
                    current.Firstblood += match.FirstBlood;
                    all.Firstblood += match.FirstBlood;
                    if (match.FirstDragon.HasValue)
                    {
                        current.FirstDragon += match.FirstDragon.Value;
                        current.DragonKilledMatchCount++;
                        all.FirstDragon += match.FirstDragon.Value;
                        all.DragonKilledMatchCount++;
                    }
                    if (match.FirstBaron.HasValue)
                    {
                        current.FirstBaron += match.FirstBaron.Value;
                        current.BaronKilledMatchCount++;
                        all.FirstBaron += match.FirstBaron.Value;
                        all.BaronKilledMatchCount++;
                    }
                    if (match.FirstTower.HasValue)
                    {
                        current.FirstTower += match.FirstTower.Value;
                        all.FirstTower += match.FirstTower.Value;
                    }
                    if (match.FirstInhib.HasValue)
                    {
                        current.FirstInhib += match.FirstInhib.Value;
                        all.FirstInhib += match.FirstInhib.Value;
                    }
                }
            }
            foreach (var key in stats.Keys)
            {
                var dur = stats[key];
                if (key != "all")
                {
                    dur.AvgMatchDuration = (float)dur.MatchDuration / match_data[key].Count;
                    dur.AvgFirstblood = (float)dur.Firstblood / match_data[key].Count;
                    dur.AvgFirstDragon = (float)dur.FirstDragon / dur.BaronKilledMatchCount;
                    dur.AvgFirstBaron = (float)dur.FirstBaron / dur.DragonKilledMatchCount;
                    dur.AvgFirstInhib = (float)dur.FirstInhib / match_data[key].Count;
                    dur.AvgFirstTower = (float)dur.FirstTower / match_data[key].Count;
                }
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
            foreach (var key in stats.Keys)
            {
                string path = Path.Combine(sample_path, key, "Duration.proto");
                if (!Directory.Exists(Path.Combine(sample_path, key))) Directory.CreateDirectory(Path.Combine(sample_path, key));
                using (var s = new FileStream(path, FileMode.Create))
                    ProtoBuf.Serializer.Serialize<DurationStats>(s, stats[key]);
            }
        }

        private static void SampleMercStats()
        {
            Dictionary<string, Dictionary<int, MercStats>> stats = new Dictionary<string, Dictionary<int, MercStats>>();
            var all = new Dictionary<int,MercStats>();
            int allcount = 0;
            stats.Add("all", all);
            foreach (var key in match_data.Keys)
            {
                allcount += match_data[key].Count;
                var current = new Dictionary<int, MercStats>();
                stats.Add(key, current);
                foreach (var match in match_data[key].Values)
                {
                    List<MercTeam> teams = new List<MercTeam>();
                    teams.Add(match.TeamBlue);
                    teams.Add(match.TeamRed);
                    foreach (var team in teams)
                    {
                        if (team.MercsBought != null)
                        {
                            foreach (var merc in team.MercsBought)
                            {
                                if (!current.ContainsKey(merc.Id))
                                    current.Add(merc.Id, new MercStats { Id = merc.Id });
                                if (team.Winner)
                                    current[merc.Id].Wincount++;
                                current[merc.Id].Pickcount++;
                                if (!all.ContainsKey(merc.Id))
                                    all.Add(merc.Id, new MercStats { Id = merc.Id });
                                if (team.Winner)
                                    all[merc.Id].Wincount++;
                                all[merc.Id].Pickcount++;
                            }
                            if (team.MercsBought.Count < 5)
                            {
                                if (!current.ContainsKey(-1))
                                    current.Add(-1, new MercStats { Id = -1 });
                                if (team.Winner)
                                    current[-1].Wincount += (5 - team.MercsBought.Count);
                                current[-1].Pickcount += (5 - team.MercsBought.Count);
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
            foreach(var key in stats.Keys)
            {
                foreach (var merc in stats[key])
                {
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
            float allPick = stats["all"].Sum(x => x.Value.Pickrate);
            float allWin = stats["all"].Sum(x => x.Value.Winrate);

            foreach (var key in stats.Keys)
            {
                string path = Path.Combine(sample_path, key, "Merc.proto");
                if (!Directory.Exists(Path.Combine(sample_path, key))) Directory.CreateDirectory(Path.Combine(sample_path, key));
                using (var s = new FileStream(path, FileMode.Create))
                    ProtoBuf.Serializer.Serialize<Dictionary<int, MercStats>>(s, stats[key]);
            }
        }

        public static void RefineMatchData()
        {
            string refinedPath = Path.Combine(data_path, currentSource, "Refined");
            foreach (var folder in Directory.GetDirectories(Path.Combine(data_path, currentSource, "Raw")))
            {
                var dir = Path.GetFileName(folder);
                string zippath = Path.Combine(folder, dir + "_matches.zip");
                string protopath = Path.Combine(refinedPath, dir + "_matches.proto");
                if (File.Exists(protopath))
                    continue;
                if (!File.Exists(zippath))
                    continue;
                var zip = new ZipFile(zippath);
                var data = new Dictionary<long, MercStatistcs>();
                match_data.Add(dir, data);
                long entries = zip.Count;
                long current = 0;
                using (zip)
                {
                    foreach (ZipEntry entry in zip)
                    {
                        try
                        {
                            var json = new StreamReader(zip.GetInputStream(entry)).ReadToEnd();
                            var converted = JsonConvert.DeserializeObject<MatchDetail>(json);
                            data.Add(converted.MatchId, AnalyzeMatch(converted));
                            current++;
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
                using (var s = new FileStream(protopath, FileMode.Create))
                    ProtoBuf.Serializer.Serialize<Dictionary<long, MercStatistcs>>(s, data);
            }
        }

        private static MercStatistcs AnalyzeMatch(MatchDetail converted)
        {
            var res = new MercStatistcs();
            res.MatchId = converted.MatchId;
            res.FirstBaron = converted.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.MonsterType == "BARON_NASHOR")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            res.FirstDragon = converted.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.MonsterType == "DRAGON")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            res.FirstBlood = converted.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.EventType == "CHAMPION_KILL")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            res.FirstInhib = converted.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.EventType == "BUILDING_KILL") && x.Events.Any(y => y.BuildingType == "INHIBITOR_BUILDING")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            res.FirstTower = converted.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.EventType == "BUILDING_KILL") && x.Events.Any(y => y.BuildingType == "TOWER_BUILDING")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            res.MatchDuration = converted.MatchDuration;
            res.TeamBlue = AnalyzeTeam(converted.Teams[0], converted);
            res.TeamRed = AnalyzeTeam(converted.Teams[1], converted);
            return res;
        }

        private static MercTeam AnalyzeTeam(Team team, MatchDetail match)
        {
            var teamMembers = match.Participants.Where(x => x.TeamId == team.TeamId).ToList();
            var res = new MercTeam();
            res.BaronKills = team.BaronKills;
            res.DragonKills = team.DragonKills;
            res.FirstBlood = team.FirstBlood;
            res.FirstBaron = match.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.MonsterType == "BARON_NASHOR" && teamMembers.Select(z => z.ParticipantId).Contains(y.KillerId))).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            res.FirstDragon = match.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.MonsterType == "DRAGON" && teamMembers.Select(z => z.ParticipantId).Contains(y.KillerId))).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            res.FirstInhibitorTaken = match.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.EventType == "BUILDING_KILL" && teamMembers.Select(z => z.ParticipantId).Contains(y.KillerId)) && x.Events.Any(y => y.BuildingType == "INHIBITOR_BUILDING")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            res.FirstTowerTaken = match.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.EventType == "BUILDING_KILL" && teamMembers.Select(z => z.ParticipantId).Contains(y.KillerId)) && x.Events.Any(y => y.BuildingType == "TOWER_BUILDING")).Select(x => x.Timestamp / 1000).OrderBy(x => x).FirstOrDefault();
            res.Winner = team.Winner;
            res.MercsBought = new List<MercData>();
            foreach (var m in teamMembers)
            {
                var mercId = match.Timeline.Frames.Where(x => x.Events != null && x.Events.Any(y => y.EventType == "ITEM_PURCHASED" && meta_data.Merc.Contains(y.ItemId) && y.ParticipantId == m.ParticipantId)).Select(x => x.Events.FirstOrDefault(y => y.EventType == "ITEM_PURCHASED" && meta_data.Merc.Contains(y.ItemId) && y.ParticipantId == m.ParticipantId)).FirstOrDefault();
                if (mercId != null)
                {
                    var merc = new MercData();
                    merc.Id = mercId.ItemId;
                    merc.ParticipantId = m.ParticipantId;
                    merc.DefenseLevel = match.Timeline
                        .Frames.Where(x =>
                            x.Events != null
                            && x.Events.Any(y => y.EventType == "ITEM_PURCHASED" && meta_data.Defense.Select(z => z.Id).Contains(y.ItemId) && y.ParticipantId == m.ParticipantId)
                            ).Count();
                    merc.OffenseLevel = match.Timeline
                        .Frames.Where(x =>
                            x.Events != null
                            && x.Events.Any(y => y.EventType == "ITEM_PURCHASED" && meta_data.Offense.Select(z => z.Id).Contains(y.ItemId) && y.ParticipantId == m.ParticipantId)
                            ).Count();
                    merc.UpgradeLevel = match.Timeline
                        .Frames.Where(x =>
                            x.Events != null
                            && x.Events.Any(y => y.EventType == "ITEM_PURCHASED" && meta_data.Upgrade.Select(z => z.Id).Contains(y.ItemId) && y.ParticipantId == m.ParticipantId)
                            ).Count();
                    res.MercsBought.Add(merc);
                }
            }
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
