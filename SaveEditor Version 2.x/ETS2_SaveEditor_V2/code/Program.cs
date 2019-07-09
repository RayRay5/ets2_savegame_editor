using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/**
 * SaveEditor Version V2
 * By https://github.com/RayRay5
 * Licensed under GNU General Public License v3.0
 */

namespace SaveEditorV2
{
    static class Program
    {
        public static readonly string __version = "0.2.0.0";
        public static bool debug = false;
        static string[] configData;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if(args.Length != 0)
            {
                if(args[0] == "dev" || args[0] == "public_beta")
                {
                    debug = true;
                }
            }
            try
            {
                configData = File.ReadAllLines("config.txt");
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine();
                DebugHandling.logGUIStroke(new string[] { "[INIT] [FILE] Config File Not Found!" } );
            }

            switch(configData[0])
            {
                case "de":
                    string[] strings = File.ReadAllLines("./lang/de/de.txt");
                    Language.loadLanguage(strings);
                    break;
                default:
                    // en
                    break;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
