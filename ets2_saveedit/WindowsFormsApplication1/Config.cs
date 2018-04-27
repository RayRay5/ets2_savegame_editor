using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ets2_saveeditor
{
    public class Config
    {
        private string lang;
        private string[] terms;
        public static Dictionary<UInt16, string> lang_terms = new Dictionary<UInt16, string>();
        //public static HashSet<string> langSet;

        public Config(string lang)
        {
            this.lang = null;
            this.terms = null;
            /*if (lang == "en")
            {
                lang_terms.Add(0, "Analyze Savegame");
            }*/
            this.lang = lang;
        }

        public string Lang { get => lang; set => lang = value; }
        public string[] Terms { get => terms; set => terms = value; }
        //public HashSet<string> Lang_terms { get => lang_terms; set => lang_terms = value; }
    }
}
