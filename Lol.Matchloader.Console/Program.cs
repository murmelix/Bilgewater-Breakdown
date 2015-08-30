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
        static string data_path = null;
        static string currentTarget = "BILGEWATER"; //or "SOLO_RANKED"
        static void Main(string[] args)
        {
            // get data path from config
            data_path = System.Configuration.ConfigurationSettings.AppSettings["DataPath"];
            // read command-line input
            region = args[0]; 
            currentTarget = args[1];
            apiKey = args[2];
            // create a thread for the downloads
            Thread t = new Thread(Run);
            t.Start();
            
            System.Console.ReadLine();
            // if return pressed the downloads will be stopped
            running = false;
            // wait for the processing to end
            t.Join();
        }

        static void Run()
        {
            // source path for the list of matches to download
            var src_Path = Path.Combine(data_path, currentTarget, "Source");
            // target where the match data will be stored
            var target_Path = Path.Combine(data_path, currentTarget, "Raw", region);
            try
            {
                string listdata;
                // parse the json list to a string array
                using (var sr = new StreamReader(Path.Combine(src_Path, region + ".json"), Encoding.UTF8))
                {
                    listdata = sr.ReadToEnd();
                }
                var list = JsonConvert.DeserializeObject<List<string>>(listdata);

                // look for existing zip with previous downloaded data
                string zippath = Path.Combine(target_Path, region + "_matches.zip");
                string path;
                ZipFile zip;
                // create zip if not existing yet else open
                if (System.IO.File.Exists(zippath))
                    zip = new ZipFile(zippath);
                else
                    zip = ZipFile.Create(zippath);
                int i = 0;
                using (zip)
                {
                    // file not zipped files in the zip
                    zip.BeginUpdate();
                    foreach (var f in Directory.GetFiles(target_Path, "*.json"))
                        zip.Add(f, Path.GetFileName(f));
                    zip.CommitUpdate();
                    // scan zip for downloaded files
                    for (; i < list.Count; i++)
                    {
                        System.Console.Clear();
                        System.Console.WriteLine(string.Format("[{1}] {0:0.00}% Done...", (float)i / 100.0, region));
                        if (zip.FindEntry(list[i] + ".json", true) < 0)
                            break;
                    }
                }
                // delete previously zipped files
                foreach (var f in Directory.GetFiles(target_Path, "*.json"))
                    System.IO.File.Delete(f);

                // create webclient with utf8 encoding
                var wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                int written = 1;
                // loop through list
                for (; i < list.Count;)
                {
                    try
                    {
                        // exit clause from command line
                        if (!running) break;
                        // Thread Sleep only without production api key
                        //Thread.Sleep(1300);

                        // download the data
                        var json = wc.DownloadString(string.Format("https://{1}.api.pvp.net/api/lol/{1}/v2.2/match/{2}?includeTimeline=true&api_key={0}", apiKey, region, list[i]));
                        path = Path.Combine(target_Path, list[i] + ".json");
                        // write it to file
                        using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                            sw.Write(json);
                        System.Console.Clear();
                        // output progress
                        System.Console.WriteLine(string.Format("[{1}] {0:0.00}% Done...", (float)i / 100.0, region));
                        // increment only on success
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
                // zip all found entries 
                var zf = new ZipFile(zippath);
                using (zf)
                {
                    zf.BeginUpdate();
                    foreach (var f in Directory.GetFiles(target_Path, "*.json"))
                        zf.Add(f, Path.GetFileName(f));
                    zf.CommitUpdate();
                }
                // clean up the remaining files
                foreach (var f in Directory.GetFiles(target_Path, "*.json"))
                    System.IO.File.Delete(f);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}
