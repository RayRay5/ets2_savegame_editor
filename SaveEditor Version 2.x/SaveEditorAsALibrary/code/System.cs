using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/**
 * SaveEditor Version V2
 * By https://github.com/RayRay5
 * Licensed under GNU General Public License v3.0
 */

namespace SaveEditorAsALibrary
{
    public class System
    {
        public static readonly string __version = "1.0.0.0";
        public static readonly string __supported_net_framework_version = "4.7.1";
        public static readonly int __supported_net_framework_regkey = 461308;
        public static readonly string __supported_automation_language_version = "0.1";
        public static string[] lines;
        public static ArrayList unitList;

        /**
        * As of Nov 11th 2018 16:45 UTC
        * https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
        */
        public static bool checkNETVersion()
        {
            int key = getNETFrameworkKey();
            if(key >= __supported_net_framework_regkey)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int getNETFrameworkKey()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    return (int)ndpKey.GetValue("Release");
                }
                else
                {
                    return 0;
                }
            }
        }

        public static string getNETFrameworkVersion(int releaseKey)
        {
            if (releaseKey >= 461808)
            {
                return "4.7.2 or later";
            }

            if (releaseKey >= 461308)
            {
                return "4.7.1";
            }

            if (releaseKey >= 460798)
            {
                return "4.7";
            }

            if (releaseKey >= 394802)
            {
                return "4.6.2";
            }

            if (releaseKey >= 394254)
            {
                return "4.6.1";
            }

            if (releaseKey >= 393295)
            {
                return "4.6";
            }

            if (releaseKey >= 379893)
            {
                return "4.5.2";
            }

            if (releaseKey >= 378675)
            {
                return "4.5.1";
            }

            if (releaseKey >= 378389)
            {
                return "4.5";
            }

            return "older Version than 4.5";
        }

        public static bool checkDecryptState()
        {
            if(!System.lines[0].StartsWith("ScsC"))
            {
                return true;
            }
            return false;
        }

        public static void resetSystem()
        {
            Input.path = Directory.GetCurrentDirectory() + @"\game.sii";
            Array.Clear(System.lines, 0, System.lines.Length);
            unitList.Clear();
        }

        public static void prepareUnits()
        {
            ArrayList prepareUnitList = new ArrayList();

            for (uint i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains("{") && !lines[i].Contains("Sii") && lines[i].Length > 3)
                {
                    ArrayList attrList = new ArrayList();
                    do
                    {
                        attrList.Add(lines[i]);
                        ++i;
                    }
                    while (!(lines[i].Contains("}")));
                    attrList.Add("}");

                    prepareUnitList.Add(attrList);
                }
            }

            /*foreach(ArrayList al in prepareUnitList)
            {
                foreach(string x in al)
                {
                    Console.WriteLine(x);
                }
            }*/

        System.unitList = prepareUnitList;
            //return prepareUnitList;
        }


        public static ArrayList findTerm(string searchTerm)
        {
            ArrayList matchList = new ArrayList();
            int countUnit = 0;
            int countAttr = 0;

            foreach (ArrayList attrList in unitList)
            {
                foreach(string attr in attrList)
                {
                    Regex regex = new Regex(searchTerm, RegexOptions.IgnoreCase);
                    Match match = regex.Match(attr);
                    while(match.Success)
                    {
                        matchList.Add(countUnit);
                        match = match.NextMatch();
                    }
                    ++countAttr;
                }
                ++countUnit;
            }

            return matchList;
        }

        public static ArrayList findAttr(int unit, string searchTerm)
        {
            ArrayList matchList = new ArrayList();
            int countAttr = 0;

            ArrayList attrList = (ArrayList) unitList[unit];

            foreach(string attr in attrList)
            {
                Regex regex = new Regex(searchTerm, RegexOptions.IgnoreCase);
                Match match = regex.Match(attr);
                while (match.Success)
                {
                    matchList.Add(countAttr);
                    match = match.NextMatch();
                }
                ++countAttr;
            }

            return matchList;
        }

        public static void changeAttribute(int unitIndex, int attrIndex, string newValue)
        {
            ((ArrayList)unitList[unitIndex])[attrIndex] = newValue;
        }

        public static void addUnit(ArrayList values)
        {
            unitList.Add(values);
        }

        public static void addAttribute(int unitIndex, string attrValue)
        {
            ((ArrayList)unitList[unitIndex]).Add(attrValue);
        }

        public static void insertAttribute(int unitIndex, string attrValue, int attrIndex = 1)  // Index = 0 is the unit (name) itself
        {
            ((ArrayList)unitList[unitIndex]).Insert(attrIndex, attrValue);
        }

        public static void removeUnit(int unitIndex)
        {
            unitList.RemoveAt(unitIndex);
        }

        public static void removeAttribute(int unitIndex, int attrIndex)
        {
            ((ArrayList)unitList[unitIndex]).RemoveAt(attrIndex);
        }
    }
}
