using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * SaveEditor Version V2
 * By https://github.com/RayRay5
 * Licensed under GNU General Public License v3.0
 */

namespace SaveEditorV2
{
    public static class Language
    {
        public static string LoadSelectedScript = "Load Selected Script";
        public static string OpenScriptFileButton = "Open Script";
        public static string UnitEditorButton = "Unit Editor";
        public static string LoadSavegame = "Load Savegame";
        public static string Backup = "Backup";
        public static string SaveSavegamePath = "Save Path";
        public static string AnalyzeSavegame = "Analyze Savegame";
        public static string SaveSavegame = "Save Savegame";

        public static string EditAttribute = "Edit Attribute";
        public static string DeleteAttribute = "Delete Attribute";
        public static string EditUnitHeader = "Edit Unit (Header)";
        public static string DeleteUnit = "Delete Unit";
        public static string AddAttribute = "Add Attribute";

        public static string LoadUnitTree = "Load Unit Tree";
        public static string PreviousResult = "Previous Result";
        public static string NextResult = "Next Result";
        public static string Search = "Search";
        public static string AddUnit = "Add Unit";

        public static void loadLanguage(string[] LanguageStrings)
        {
            LoadSelectedScript = LanguageStrings[0];
            OpenScriptFileButton = LanguageStrings[1];
            UnitEditorButton = LanguageStrings[2];
            LoadSavegame = LanguageStrings[3];
            Backup = LanguageStrings[4];
            SaveSavegamePath = LanguageStrings[5];
            AnalyzeSavegame = LanguageStrings[6];
            SaveSavegame = LanguageStrings[7];

            EditAttribute = LanguageStrings[8];
            DeleteAttribute = LanguageStrings[9];
            EditUnitHeader = LanguageStrings[10];
            DeleteUnit = LanguageStrings[11];
            AddAttribute = LanguageStrings[12];

            LoadUnitTree = LanguageStrings[13];
            PreviousResult = LanguageStrings[14];
            NextResult = LanguageStrings[15];
            Search = LanguageStrings[16];
            AddUnit = LanguageStrings[17];
        }
    }
}
