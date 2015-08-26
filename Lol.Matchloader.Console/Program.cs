using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;

namespace Lol.Matchloader.Console
{
    class Program
    {
        static bool running = true;
        static string apiKey = null;
        static string region = null;
        static void Main(string[] args)
        {
            region = args[0];
            apiKey = args[1];
            Thread t = new Thread(Run);
            t.Start();

            System.Console.ReadLine();
            running = false;
            t.Join();
        }

        static void Run()
        {
            try
            {
                string listdata;
                using (var sr = new StreamReader(Path.Combine("..", "..", "RANKED_SOLO", region + ".json"), Encoding.UTF8))
                {
                    listdata = sr.ReadToEnd();
                }
                string zippath = Path.Combine("..", "..", "Match", region, region + "_matches.zip");
                string path;
                ZipFile zip;
                if (System.IO.File.Exists(zippath))
                    zip = new ZipFile(zippath);
                else
                    zip = ZipFile.Create(zippath);
                var list = JsonConvert.DeserializeObject<List<string>>(listdata);
                int i = 0;
                using (zip)
                {
                    for (; i < list.Count; i++)
                    {
                        System.Console.Clear();
                        System.Console.WriteLine(string.Format("[{1}] {0:0.00}% Done...", (float)i / 100.0, region));
                        if (zip.FindEntry(list[i] + ".json", true) < 0)
                            break;
                    }
                    zip.BeginUpdate();
                    foreach (var f in Directory.GetFiles(Path.Combine("..", "..", "Match", region), "*.json"))
                        zip.Add(f, Path.GetFileName(f));
                    zip.CommitUpdate();
                }
                var wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                foreach (var f in Directory.GetFiles(Path.Combine("..", "..", "Match", region), "*.json"))
                    System.IO.File.Delete(f);

                int written = 1;
                for (; i < list.Count;)
                {
                    try
                    {
                        if (!running) break;

                        //Thread.Sleep(1300);
                        var json = wc.DownloadString(string.Format("https://{1}.api.pvp.net/api/lol/{1}/v2.2/match/{2}?includeTimeline=true&api_key={0}", apiKey, region, list[i]));
                        path = Path.Combine("..", "..", "Match", region, list[i] + ".json");
                        using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                            sw.Write(json);
                        System.Console.Clear();
                        System.Console.WriteLine(string.Format("[{1}] {0:0.00}% Done...", (float)i / 100.0, region));
                        i++;
                    }
                    catch (FileNotFoundException fnfe)
                    {
                        System.Console.WriteLine(fnfe.Message);
                    }
                    catch(Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                        continue;
                    }
                }

                var zf = new ZipFile(zippath);
                using (zf)
                {
                    zf.BeginUpdate();
                    foreach (var f in Directory.GetFiles(Path.Combine("..", "..", "Match", region), "*.json"))
                        zf.Add(f, Path.GetFileName(f));
                    zf.CommitUpdate();
                }
                foreach (var f in Directory.GetFiles(Path.Combine("..", "..", "Match", region), "*.json"))
                    System.IO.File.Delete(f);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}
