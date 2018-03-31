using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ets2_saveeditor
{
    static class saveEditor
    {
        static Config conf;
        public static Config Conf { get => conf; set => conf = value; }
        public static bool dev = false;
        public static UInt16 CPU_Count = 1;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if(args.Contains("dev"))
            {
                dev = true;
            }

            CPU_Count = UInt16.Parse(Environment.ProcessorCount.ToString());
            //Console.WriteLine("Count: " + Environment.ProcessorCount);

            string version = Environment.Version.ToString();
            string[] minor = version.Split('.');
            //Console.WriteLine(minor.ToString());

            if(UInt16.Parse(minor[0]) < 4)
            {
                MessageBox.Show("You need to install the latest version of .NET Framework to run this application. Versions below 4.5.x are not supported");
                return;
            }
            else if(UInt32.Parse(minor[3]) < 42000)
            {
                MessageBox.Show("You need to update to the latest version of .NET Framework to run this application. Version 4.5.x and lower are not supported");
                return;
            }

            if (File.Exists(@"config.txt"))
            {
                string lang = "";
                try
                {
                    string[] configLines = File.ReadAllLines(@"config.txt");
                    for(UInt16 i = 0; i < configLines.Length; ++i)
                    {
                        if(Regex.IsMatch(configLines[i], @"lang:"))
                        {
                            lang = configLines[i].Replace(@"lang: ", "");
                            lang = lang.Replace(" ", "");
                        }
                    }
                    conf = new Config(lang);
                    Console.WriteLine(conf.Lang);

                    string[] langTerms = File.ReadAllLines(@"./lang/lang_" + conf.Lang + ".txt");
                    UInt16 counter = 0;

                    foreach(string term in langTerms)
                    {
                        //Config.langSet.Add(term);
                        //Console.WriteLine(term);
                        //Console.WriteLine(counter);
                        //Config.lang_terms.Add(counter.ToString(), term);
                        Config.lang_terms.Add(counter, term);
                        ++counter;
                    }

                    //Config.Lang_terms.Add("");

                    
                    //Console.WriteLine(Config.lang_terms.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("EXCC");
                }
            }

            


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ets2_saveeditor_main_form());
        }
    }
}
