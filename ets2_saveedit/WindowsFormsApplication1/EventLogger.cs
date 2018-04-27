using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saveeditor
{
    public class EventLogger
    {
        public EventLogger(string filename, string[] content)
        {
            foreach(string c in content)
            {
                File.AppendAllText(@"./logs/" + filename + ".txt", c + Environment.NewLine);
            } 
        }
    }
}
