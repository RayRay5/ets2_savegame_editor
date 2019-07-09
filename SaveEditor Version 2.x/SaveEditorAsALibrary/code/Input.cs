using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

/**
 * SaveEditor Version V2
 * By https://github.com/RayRay5
 * Licensed under GNU General Public License v3.0
 */

namespace SaveEditorAsALibrary
{
    public static class Input
    {
        public static string path = Directory.GetCurrentDirectory() + @"\game.sii";

        public static void readSavegame()
        {
            System.lines = File.ReadAllLines(path);
        }
    }
}
