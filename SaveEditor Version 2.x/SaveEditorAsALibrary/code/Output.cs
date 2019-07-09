using System;
using System.Collections;
using System.IO;
using System.Linq;

/**
 * SaveEditor Version V2
 * By https://github.com/RayRay5
 * Licensed under GNU General Public License v3.0
 */

namespace SaveEditorAsALibrary
{
    public class Output
    {
        public static void printText()
        {
            Console.WriteLine("Hello, World!");
        }

        public static void debugOutput(string variable)
        {
            Console.WriteLine(variable);
        }

        public static void listAttributes(ArrayList attributes, bool linenumber = false)
        {
            if(!linenumber)
            {
                foreach (string s in attributes)
                {
                    Console.WriteLine(s);
                }
            }
            else
            {
                for(int i = 0; i < attributes.Count; ++i)
                {
                    Console.WriteLine("line " + i + ": " + attributes[i].ToString());
                }
            }
        }

        public static void writeToFile()
        {
            ArrayList stringList = new ArrayList();
            foreach(ArrayList al in System.unitList)
            {
                foreach(string s in al)
                {
                    stringList.Add(s);
                }
                stringList.Add(" ");
            }
            stringList.RemoveAt(stringList.Count - 1);

            string[] stringListString = (string[]) stringList.ToArray(typeof(string));
            /*Console.WriteLine("WRITE");
            foreach(string s in stringListString)
            {
                Console.WriteLine("s: " + s);
            }*/

            File.WriteAllLines(Input.path, stringListString);
        }
    }
}
