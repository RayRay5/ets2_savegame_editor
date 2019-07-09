using System;
using System.Collections;
using System.Collections.Generic;
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
    public class Static
    {
        //public static ArrayList getAllGarages()
        public static Dictionary<string, int> getAllGarages()
        {
            /*ArrayList results = SaveEditorAsALibrary.System.findTerm("economy : ");
            int a2 = Int32.Parse(results[0].ToString());
            foreach (var x in results)
            {
                Console.WriteLine(x.ToString());
            }*/

            ArrayList AllGarages = new ArrayList();
            ArrayList results = SaveEditorAsALibrary.System.findTerm("garage : ");
            int a2 = Int32.Parse(results[0].ToString());
            foreach (var x in results)
            {
                ArrayList unit = (ArrayList) SaveEditorAsALibrary.System.unitList[int.Parse(x.ToString())];
                string un = unit[0].ToString();
                //Console.WriteLine("X: " + x.ToString());
                //Console.WriteLine("U: " + un.ToString());
                string ort = Regex.Replace(un, "garage : garage.", "");
                ort = Regex.Replace(ort, " {", "");
                AllGarages.Add(ort);

                //Console.WriteLine("|" + ort + "|");
            }

            ArrayList StatusList = SaveEditorAsALibrary.System.findTerm("status: ");
            ArrayList sizeList = new ArrayList();

            foreach(var s in StatusList)
            {
                ArrayList unit = (ArrayList)SaveEditorAsALibrary.System.unitList[int.Parse(s.ToString())];
                /*for(int i = 0; i < unit.Count; ++i)
                {
                    Console.WriteLine("UNIT " + i + " : " + unit[i]);
                }*/
                sizeList.Add(unit[unit.Count - 4]);
                //Console.WriteLine("STRING: " + );
            }

            Dictionary<string, int> Garages = new Dictionary<string, int>();

            for(int i = 0; i < sizeList.Count; ++i)
            {
                //Console.WriteLine("LIST: " + sizeList[i]);
                //string garageSize = "";
                var garageSize = Regex.Replace(sizeList[i].ToString(), "status:", "").ToString();
                garageSize = garageSize.Replace(" ", "");
                //Console.WriteLine("GS: " + garageSize);
                Garages.Add(AllGarages[i].ToString(), Int32.Parse(garageSize));
            }


            /*for (int i = 1; i < garagesCount; ++i)
            {
                
                string line = SaveEditorAsALibrary.System.lines[line_numberGarages + i];
                line = Regex.Replace(SaveEditorAsALibrary.System.lines[line_numberGarages + i], "garage : garage.", "");
                allGarages.Add(line);
                //Console.WriteLine(SaveEditorAsALibrary.System.lines[line_numberGarages + i].ToString());
            }*/

            return Garages;
            //return AllGarages;
            //throw new NotImplementedException();
        }

        public static ArrayList getAllCities()
        {
            ArrayList cities = (ArrayList) SaveEditorAsALibrary.System.unitList[0];
            ArrayList someCities = new ArrayList();
            
            foreach (string s in cities)
            {
                if (s.Contains("companies["))
                {
                    string x = Regex.Replace(s, "companies\\[[0-9]*\\]:", "");
                    x = Regex.Replace(x, "company.volatile.[a-zA-Z_]*.", "");
                    x = x.Replace(" ", "");
                    someCities.Add(x);
                }
            }

            ArrayList visitedCities = getAllVisitedCities();

            foreach(string s in visitedCities)
            {
                someCities.Add(s);
            }

            Dictionary<string, int> uniqueCities = new Dictionary<string, int>();

            foreach(string city in someCities)
            {
                if(!uniqueCities.Keys.Contains(city))
                {
                    uniqueCities.Add(city, 0);
                }
            }

            ArrayList allCities = new ArrayList();

            foreach(string city in uniqueCities.Keys)
            {
                allCities.Add(city);
            }

            return allCities;
        }

        public static ArrayList getAllVisitedCities()
        {
            //throw new NotImplementedException();
            ArrayList viewList = (ArrayList) SaveEditorAsALibrary.System.unitList[0];
            ArrayList allVisitedCities = new ArrayList();
            foreach(string s in viewList)
            {
                if(s.Contains("visited_cities["))
                {
                    string x = Regex.Replace(s, "visited_cities\\[[0-9]*\\]:", "");
                    x = x.Replace(" ", "");
                    allVisitedCities.Add(x);
                }
            }
            return allVisitedCities;
        }

        public static int getADR()
        {
            ArrayList results2 = SaveEditorAsALibrary.System.findTerm("economy : ");
            int a2 = Int32.Parse(results2[0].ToString());

            ArrayList attrsResult2 = (ArrayList)SaveEditorAsALibrary.System.findAttr(a2, "experience_points: ");
            int res2 = Int32.Parse(attrsResult2[0].ToString());
            res2 += 2;
            ++res2;

            string str2 = SaveEditorAsALibrary.System.lines[res2].ToString();

            Regex regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");

            int adr = int.Parse(str2);

            return adr;
        }

        public static int getHighValue()
        {
            ArrayList results2 = SaveEditorAsALibrary.System.findTerm("economy : ");
            int a2 = Int32.Parse(results2[0].ToString());

            ArrayList attrsResult2 = (ArrayList)SaveEditorAsALibrary.System.findAttr(a2, "experience_points: ");
            int res2 = Int32.Parse(attrsResult2[0].ToString());
            res2 += 2;
            res2 += 3;

            string str2 = SaveEditorAsALibrary.System.lines[res2].ToString();

            Regex regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");

            int hv = int.Parse(str2);

            return hv;
        }

        public static int getUrgent()
        {
            ArrayList results2 = SaveEditorAsALibrary.System.findTerm("economy : ");
            int a2 = Int32.Parse(results2[0].ToString());

            ArrayList attrsResult2 = (ArrayList)SaveEditorAsALibrary.System.findAttr(a2, "experience_points: ");
            int res2 = Int32.Parse(attrsResult2[0].ToString());
            res2 += 2;
            res2 += 5;

            string str2 = SaveEditorAsALibrary.System.lines[res2].ToString();

            Regex regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");

            int urgent = int.Parse(str2);

            return urgent;
        }

        public static int getLongDistance()
        {
            ArrayList results2 = SaveEditorAsALibrary.System.findTerm("economy : ");

            int a2 = Int32.Parse(results2[0].ToString());

            ArrayList attrsResult2 = (ArrayList)SaveEditorAsALibrary.System.findAttr(a2, "experience_points: ");
            int res2 = Int32.Parse(attrsResult2[0].ToString());
            res2 += 2;
            res2 += 2;

            string str2 = SaveEditorAsALibrary.System.lines[res2].ToString();

            Regex regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");

            int long_distance = int.Parse(str2);

            return long_distance;
        }

        public static int getFragile()
        {
            ArrayList results2 = SaveEditorAsALibrary.System.findTerm("economy : ");

            int a2 = Int32.Parse(results2[0].ToString());

            ArrayList attrsResult2 = (ArrayList)SaveEditorAsALibrary.System.findAttr(a2, "experience_points: ");
            int res2 = Int32.Parse(attrsResult2[0].ToString());
            res2 += 2;
            res2 += 4;

            string str2 = SaveEditorAsALibrary.System.lines[res2].ToString();

            Regex regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");

            int fragile = int.Parse(str2);

            return fragile;
        }

        public static int getEco()
        {
            ArrayList results2 = SaveEditorAsALibrary.System.findTerm("economy : ");
            int a2 = Int32.Parse(results2[0].ToString());

            ArrayList attrsResult2 = (ArrayList)SaveEditorAsALibrary.System.findAttr(a2, "experience_points: ");
            int res2 = Int32.Parse(attrsResult2[0].ToString());
            res2 += 2;
            res2 += 6;

            string str2 = SaveEditorAsALibrary.System.lines[res2].ToString();

            Regex regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");

            int eco = int.Parse(str2);

            return eco;
        }

        public static long getMoney()
        {
            ArrayList results = SaveEditorAsALibrary.System.findTerm("bank : ");

            int a = Int32.Parse(results[0].ToString());
            //textBox1.Text = ((ArrayList) SaveEditorAsALibrary.System.unitList[a]).ToString();
            ArrayList attributes = (ArrayList)SaveEditorAsALibrary.System.unitList[a];
            //Console.WriteLine(attributes[1]);
            string str = attributes[1].ToString();

            Regex regex = new Regex("[^0-9]");
            str = regex.Replace(str, "");

            long money = long.Parse(str);

            return money;
        }

        public static long getExperience()
        {
            ArrayList results2 = SaveEditorAsALibrary.System.findTerm("economy : ");
            int a2 = Int32.Parse(results2[0].ToString());

            ArrayList attrsResult2 = (ArrayList)SaveEditorAsALibrary.System.findAttr(a2, "experience_points: ");
            int res2 = Int32.Parse(attrsResult2[0].ToString());
            res2 += 2;

            string str2 = SaveEditorAsALibrary.System.lines[res2].ToString();

            Regex regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");

            long exp = long.Parse(str2);

            return exp;
        }
    }
}
