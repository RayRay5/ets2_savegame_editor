/**
* Euro Truck Simulator 2 Savegame Editor
* author: https://github.com/RayRay5
* licensed under GNU GENERAL PUBLIC LICENSE v3.0
*
* style is, if possible, functional
*/

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;
using System.Timers;
using System.Diagnostics;

namespace ets2_saveeditor
{
    public partial class ets2_saveeditor_main_form : Form
    {
        private static readonly string _versionNumber = "1.1 Beta";
        OpenFileDialog ofd = new OpenFileDialog();
        SaveFileDialog sfd = new SaveFileDialog();

        public string filename = "";

        ContextMenuStrip normalstrip = new ContextMenuStrip();
        ContextMenuStrip cloneviewstrip = new ContextMenuStrip();
        ContextMenuStrip addToVisitedCities = new ContextMenuStrip();
        ContextMenuStrip upgradeGarages = new ContextMenuStrip();

        ToolStripMenuItem openInSecondView = new ToolStripMenuItem("Open in Second View");
        ToolStripMenuItem removeFromThisView = new ToolStripMenuItem("Remove From This View");
        ToolStripMenuItem addToVisited = new ToolStripMenuItem("Add To Visited Cities");
        ToolStripMenuItem upgradeGarage = new ToolStripMenuItem("Upgrade Garage");
        ToolStripMenuItem downgradeGarage = new ToolStripMenuItem("Downgrade Garage");
        ToolStripMenuItem newHeadquarter = new ToolStripMenuItem("Make this garage your headquarter");

        ArrayList garages = new ArrayList();
        ArrayList garageSize = new ArrayList();
        ArrayList otherStuff = new ArrayList();
        ArrayList cities = new ArrayList();
        ArrayList visitedCities = new ArrayList();
        ArrayList companies = new ArrayList();
        Dictionary<string, int> uniqueCities = new Dictionary<string, int>();

        System.Timers.Timer progressBarUpdateTimer;
        Thread thread;

        static int progress = 0;
        static int visitedCitiesBeforeAnalyzation = 0;
        static int upperLimit = 0;
        static string dir;

        public static string[] lines;

        public ets2_saveeditor_main_form()
        {
            InitializeComponent();
            normalstrip.Items.AddRange(new ToolStripMenuItem[]
            {
                openInSecondView
            });

            cloneviewstrip.Items.AddRange(new ToolStripItem[]
            {
                removeFromThisView
            });

            addToVisitedCities.Items.AddRange(new ToolStripItem[]
            {
                addToVisited
            });

            upgradeGarages.Items.AddRange(new ToolStripItem[]
            {
                upgradeGarage, downgradeGarage, newHeadquarter
            });

            openInSecondView.Click += new System.EventHandler(this.striplabel1_Click);
            removeFromThisView.Click += new System.EventHandler(this.striplabel2_Click);
            //addToVisited.Click += new System.EventHandler(this.striplabel3_Click);
            addToVisited.MouseUp += new MouseEventHandler(this.striplabel3_Click);
            upgradeGarage.Click += new System.EventHandler(this.striplabel4_Click);
            downgradeGarage.Click += new System.EventHandler(this.striplabel5_Click);
            newHeadquarter.Click += new System.EventHandler(this.striplabel6_Click);

            treeView1.NodeMouseClick += (_sender, args) => treeView1.SelectedNode = args.Node;
        }

        private void striplabel6_Click(object sender, EventArgs e)
        {
            lines[upperLimit] = " hq_city: " + treeView1.SelectedNode.Text;
        }

        private void striplabel5_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes[1].IsExpanded)
            {
                TreeNode t = treeView1.SelectedNode;
                TreeNode t2 = null;

                foreach (TreeNode node in treeView2.Nodes)
                {
                    for (int j = 0; j < node.Nodes.Count; ++j)
                    {
                        if (node.Nodes[j].Text.Equals(t.Text))
                        {
                            t2 = node.Nodes[j];
                        }
                    }
                }
                string tstring = t.ToString();
                tstring = tstring.Replace("TreeNode: ", "");
                int index = garages.IndexOf(tstring);

                if (index != -1)
                {
                    if (Int32.Parse(garageSize[index].ToString()) <= 0)
                    {
                        MessageBox.Show("Garage can not be downgraded. Garage already has smallest possible size");
                    }
                    else if (Int32.Parse(garageSize[index].ToString()) > 3)
                    {

                    }
                    else
                    {
                        garageSize[index] = (Int32.Parse(garageSize[index].ToString()) - 1).ToString();
                        changeForeColor(t, index);
                        if (t2 != null)
                        {
                            changeForeColor(t2, index);
                        }
                        int size = Int32.Parse(garageSize[index].ToString());
                    }
                }
            }
        }

        private void striplabel4_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes[1].IsExpanded)
            {
                TreeNode t = treeView1.SelectedNode;
                TreeNode t2 = null;

                foreach (TreeNode node in treeView2.Nodes)
                {
                    for (int j = 0; j < node.Nodes.Count; ++j)
                    {
                        if (node.Nodes[j].Text.Equals(t.Text))
                        {
                            t2 = node.Nodes[j];
                        }
                    }
                }
                string tstring = t.ToString();
                tstring = tstring.Replace("TreeNode: ", "");
                int index = garages.IndexOf(tstring);

                if (index != -1)
                {
                    if (Int32.Parse(garageSize[index].ToString()) >= 3)
                    {
                        MessageBox.Show("Garage can not be upgraded. Garage already has biggest possible size");
                    }
                    else if (Int32.Parse(garageSize[index].ToString()) < 0)
                    {

                    }
                    else
                    {
                        garageSize[index] = (Int32.Parse(garageSize[index].ToString()) + 1).ToString();
                        changeForeColor(t, index);
                        if (t2 != null)
                        {
                            changeForeColor(t2, index);
                        }
                        int size = Int32.Parse(garageSize[index].ToString());
                    }
                }
            }
            else
            {
                for(int i = 0; i < garageSize.Count; ++i)
                {
                    if(Int32.Parse(garageSize[i].ToString()) < 3)
                    {
                        garageSize[i] = "3";
                    }
                    
                }

                this.Invoke(new Action(() => { MessageBox.Show("Unlocked all Garages"); }));
                treeView1.SelectedNode = treeView1.Nodes[1];
                int ii = 0;
                foreach(TreeNode n in treeView1.Nodes[1].Nodes)
                {
                    changeForeColor(n, ii);
                    ++ii;
                }
                button5.Enabled = false;
            }
        }

        private void striplabel1_Click(object sender, EventArgs e)
        {
            TreeNode newNode = (TreeNode) treeView1.SelectedNode.Clone();
            treeView1.SelectedNode.ContextMenuStrip = null;

            treeView2.Nodes.Add(newNode);
            newNode.ContextMenuStrip = cloneviewstrip;
            foreach(TreeNode childNode in newNode.Nodes)
            {
                childNode.ContextMenuStrip = null;
            }
        }

        private void striplabel2_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < treeView1.Nodes.Count; ++i)
            {
                if(treeView1.Nodes[i].Text.Equals(treeView2.SelectedNode.Text))
                {
                    treeView1.Nodes[i].ContextMenuStrip = normalstrip;
                }
            }
            treeView2.SelectedNode.Remove();
        }

        private void striplabel3_Click(object sender, MouseEventArgs e)
        {            
            visitedCities.Add(treeView1.SelectedNode.Text);
            treeView1.SelectedNode.ContextMenuStrip = null;

            TreeNode newNode = (TreeNode)treeView1.SelectedNode.Clone();
            newNode.ForeColor = Color.Black;
            treeView1.SelectedNode.ForeColor = Color.DarkGreen;
            treeView1.Nodes[3].Nodes.Add(newNode);
            treeView1.Nodes[3].Text = "Visited Cities(Size: " + treeView1.Nodes[3].Nodes.Count + ")";

            if (treeView2.Nodes.Count > 0)
            {
                foreach (TreeNode node in treeView2.Nodes)
                {
                    foreach(TreeNode node2 in node.Nodes)
                    {
                        if(Regex.IsMatch(treeView1.SelectedNode.Text, node2.Text))
                        {
                            node2.ForeColor = Color.DarkGreen;
                        }
                    }

                    if (Regex.IsMatch(node.Text, "Visited Cities"))
                    {
                        newNode = (TreeNode)treeView1.SelectedNode.Clone();
                        newNode.ForeColor = Color.Black;
                        node.Nodes.Add(newNode);
                        node.Text = "Visited Cities(Size: " + node.Nodes.Count + ")";
                    }
                }
            }
        }

        /**
        * change progressbar state
        * author: https://github.com/RayRay5
        */

        private void reportProgress()
        {
            progressBar1.Value += progress;
            progress = 0;

            if (!thread.IsAlive)
            {
                progressBarUpdateTimer.Stop();
                this.progressBar1.Visible = false;
                this.button2.Visible = true;
                MessageBox.Show("Saved savegame modifications");
            }
        }

        /**
         * Timer for updating the progessbar
         * author: https://github.com/RayRay5
         */
        private void progressBarUpdateTimer_Elapsed(object sender, EventArgs e)
        {
            try
            {
                this.Invoke(new Action(() => reportProgress()));
            }
            catch(Exception exception)
            {
                this.Invoke(new Action(() => { MessageBox.Show("Unexpected exception occured: " + exception.ToString()); }));
            }
        }

        /**
        * validate if file is a .sii file and if the file is a valid file or not
        * author: https://github.com/RayRay5
        * TODO check if file is decrypted or not
        */
        private bool isInvalidFile()
        {
            if(textBox1.Text.Contains(".sii"))
            {
                string firstLine = File.ReadLines(textBox1.Text).First(); // check if ScsC header is set
                if (!Regex.IsMatch(firstLine, "ScsC"))
                {
                    return false;
                }
                MessageBox.Show("File is not decrypted, please decrypt it using the Savegame-Decrypter", "Input Filestream Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Application.Exit();
                return true;
            }
            MessageBox.Show("Invalid or no file selected", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return true;
        }

        /**
        * Calculates the position in the file where the processing will stop
        * author: https://github.com/RayRay5
        */
        private void calculateUpperLimit(string[] lines)
        {
            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains("hq_city"))
                {
                    upperLimit = i;
                    return;
                }
            }
        }

        /**
        * Creates a backup file, using copy function to avoid overwriting
        * author: https://github.com/RayRay5
        */
        private void saveOldFile(string filename_original, string filename_backup)
        {
            //string oldfilename = "game_old.sii";
            //MessageBox.Show("filename: " + filename + "\r\n" + "new path: " + Path.Combine(dir, oldfilename));
            if(filename_backup == "")
            {
                return;
            }

            try
            {
                //File.Copy(filename, Path.Combine(dir, oldfilename));
                File.Copy(filename_original, filename_backup);
            }
            catch(IOException)
            {
                MessageBox.Show("Can't create backup file. File does already exist");
            }
        }

        /**
        * helper function to change the color of the treenodes accordingly to the value
        * author: https://github.com/RayRay5
        */
        private void changeForeColor(TreeNode n, int index)
        {
            switch (garageSize[index].ToString())
            {
                case "0":
                    n.ForeColor = Color.DarkRed;
                    break;
                case "1":
                    n.ForeColor = Color.Red;
                    break;
                case "2":
                    n.ForeColor = Color.Orange;
                    break;
                case "3":
                    n.ForeColor = Color.Green;
                    break;
                default:
                    n.ForeColor = Color.Navy;
                    break;
            }
        }

        /**
        * Schows a MessageBox
        * author: https://github.com/RayRay5
        */

        private void backgroundCallBack()
        {
            MessageBox.Show("Writing content back to file. This might take a few minutes.",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /**
        * Writes Back the stuff back to file, running this in a seperate Thread is strongly recommended
        * author: https://github.com/RayRay5
        */

        private void doBackgroundStuff(string[] lines)
        {
            string[] savelines = lines;
            string content = "";
            for (int j = 0; j < lines.Length; ++j)
            {
                //File.WriteAllText(ofd.FileName, (lines[i] + Environment.NewLine));
                content += savelines[j] + Environment.NewLine;
                progress++;
            }
            File.WriteAllText(ofd.FileName, content);
            progress++;

            toggleContextMenu(true);

            this.Invoke(new Action(() =>
            {
                this.button1.Enabled = true;
                this.button2.Enabled = true;
                this.button3.Enabled = true;
                this.button4.Enabled = false;// true;
                this.button5.Enabled = true;
                this.button6.Enabled = true;
                this.button8.Enabled = true;

                this.comboBox1.Enabled = true;
                this.comboBox2.Enabled = true;
                this.comboBox3.Enabled = true;
                this.comboBox4.Enabled = true;
                this.comboBox5.Enabled = true;
                this.comboBox6.Enabled = true;

                this.textBox1.Enabled = true;
                this.textBox2.Enabled = true;
                this.textBox3.Enabled = true;
                this.textBox4.Enabled = true;
                Cursor.Current = Cursors.Default;
            }));
        }

        /**
        * Open the OpenFileDialog
        * author: https://github.com/RayRay5
        */
        private void button1_Click(object sender, EventArgs e)
        {
            ofd.Filter = "ETS2 Savegames (*.sii)|*.sii";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                //dir = Directory.GetCurrentDirectory();
                textBox1.Text = ofd.FileName;
                //saveOldFile(ofd.FileName);
            }
        }

        /**
         * Toggles the ContextMenuStrips
         * Intended use is to disable them during the writeback phase
         * author: https://github.com/RayRay5
         */
        private void toggleContextMenu(bool state)
        {
            try
            {
                for (int nn = 0; nn < treeView1.Nodes.Count; ++nn)
                {
                    treeView1.Nodes[nn].ContextMenuStrip.Enabled = state;
                    foreach (TreeNode child in treeView1.Nodes[nn].Nodes)
                    {
                        try
                        {
                            child.ContextMenuStrip.Enabled = state;
                        }
                        catch (Exception)
                        {
                            //Console.WriteLine("exception at: " + child.Text);
                        }
                    }
                }

                for (int nn = 0; nn < treeView2.Nodes.Count; ++nn)
                {
                    treeView2.Nodes[nn].ContextMenuStrip.Enabled = state;
                    foreach (TreeNode child in treeView2.Nodes[nn].Nodes)
                    {
                        try
                        {
                            child.ContextMenuStrip.Enabled = state;
                        }
                        catch (Exception)
                        {
                            //Console.WriteLine("exception at: " + child.Text);
                        }
                    }
                }
            }
            catch(Exception)
            {

            }
        }

        /**
        * Applies and saves the settings you made. (writes back all stuff to file in the hopefully correct format)
        * If you encounter issues with your savegame, please create an issue on this project on github
        * author: https://github.com/RayRay5
        */
        private void button2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            toggleContextMenu(false);

            if(isInvalidFile())
            {
                return;
            }

            dir = Directory.GetCurrentDirectory();
            saveOldFile(textBox1.Text, textBox4.Text);

            //string filename = ofd.FileName;
            //string[] 
            //lines = File.ReadAllLines(filename);
            //string[] backuplines = lines;

            this.button1.Enabled = false;
            this.button2.Enabled = false;
            this.button3.Enabled = false;
            this.button4.Enabled = false;
            this.button5.Enabled = false;
            this.button6.Enabled = false;
            this.button8.Enabled = false;

            this.comboBox1.Enabled = false;
            this.comboBox2.Enabled = false;
            this.comboBox3.Enabled = false;
            this.comboBox4.Enabled = false;
            this.comboBox5.Enabled = false;
            this.comboBox6.Enabled = false;

            this.textBox1.Enabled = false;
            this.textBox2.Enabled = false;
            this.textBox3.Enabled = false;
            this.textBox4.Enabled = false;

            this.button2.Visible = false;
            this.progressBar1.Visible = true;

            progressBar1.Value = 0;
            progressBar1.Maximum = lines.Length + 1;

            calculateUpperLimit(lines);

            /*
            * write back bank account money
            */
            for (int i = 0; i < lines.Length; ++i)
            {
                if (textBox2.Text != "")
                {
                    if (lines[i].Contains("money_account"))
                    {
                        lines[i] = " money_account: " + textBox2.Text;
                        break;
                    }
                }
            }

            /*
            * write back garage data
            */
            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains("garage") && !lines[i].Contains("garages") && (!(lines[i+1].Contains("}")) || !(lines[i+1].Contains("garage"))))
                {
                    string ort = Regex.Replace(lines[i], "garage : garage.", "");
                    ort = Regex.Replace(ort, " {", "");

                    if(ort.Length < 15)
                    {
                        int ind = garages.IndexOf(ort);
                        do
                        {
                            i++;
                        }
                        while (!lines[i].Contains("status"));

                        if (ind > -1)
                        {
                            lines[i] = " status: " + garageSize[ind];
                        }

                        //string size = Regex.Replace(lines[i], " status: ", "");
                        //garageSize.Add(size);

                        //addString += " (Size: " + size + ")";
                        //garages.Add(addString);
                    }
                }
            }

            for (int i = 0; i < upperLimit; ++i)
            {
                //string s = lines[i];
                if (Regex.IsMatch(lines[i], "visited_cities: "))
                {
                    lines[i] = " visited_cities: " + visitedCities.Count;
                    string stuff = "";

                    for (int j = visitedCitiesBeforeAnalyzation; j < visitedCities.Count; ++j)
                    {
                        stuff += " visited_cities[" + j + "]: " + visitedCities[j];
                        if (j < visitedCities.Count - 1)
                        {
                            stuff += Environment.NewLine;
                        }
                    }

                    lines[i + visitedCitiesBeforeAnalyzation] = lines[i + visitedCitiesBeforeAnalyzation] + Environment.NewLine + stuff;
                    break;
                }
            }

            for (int i = 0; i < upperLimit; ++i)
            {
                if (Regex.IsMatch(lines[i], "visited_cities_count: "))
                {
                    lines[i] = " visited_cities_count: " + visitedCities.Count;
                    string stuff = "";

                    for (int j = visitedCitiesBeforeAnalyzation; j < visitedCities.Count; ++j)
                    {
                        stuff += " visited_cities_count[" + j + "]: 1";
                        if (j < visitedCities.Count - 1)
                        {
                            stuff += Environment.NewLine;
                        }
                    }

                    lines[i + visitedCitiesBeforeAnalyzation] = lines[i + visitedCitiesBeforeAnalyzation] + Environment.NewLine + stuff;
                    break;
                }
            }

            for (int i = 0; i < upperLimit; ++i)
            {
                if (lines[i].Contains("experience_points"))
                {
                    if(textBox3.Text != "")
                    {
                        lines[i] = " experience_points: " + textBox3.Text;
                    }
                    
                    i++;
                    if (comboBox1.Text != "" && comboBox1.Text != "don't change")
                    {
                        lines[i] = " adr: " + comboBox1.Text;
                    }
                    i++;

                    if(comboBox2.Text != "" && comboBox2.Text != "don't change")
                    {
                        lines[i] = " long_dist: " + comboBox2.Text;
                    }                    
                    i++;

                    if (comboBox3.Text != "" && comboBox3.Text != "don't change")
                    {
                        lines[i] = " heavy: " + comboBox3.Text;
                    }
                    i++;

                    if (comboBox4.Text != "" && comboBox4.Text != "don't change")
                    {
                        lines[i] = " fragile: " + comboBox4.Text;
                    }
                    i++;

                    if (comboBox5.Text != "" && comboBox5.Text != "don't change")
                    {
                        lines[i] = " urgent: " + comboBox5.Text;
                    }
                    i++;

                    if (comboBox6.Text != "" && comboBox6.Text != "don't change")
                    {
                        lines[i] = " mechanical: " + comboBox6.Text;
                    }

                    //File.WriteAllLines(ofd.FileName, lines);
                    
                    
                    //Cursor.Current = Cursors.Default;
                    
                    break;
                }
            }

            thread = new Thread(() => doBackgroundStuff(lines));
            thread.Start();
            Thread info = new Thread(() => backgroundCallBack());
            info.Start();

            createProgressTimer();
        }

        private void createProgressTimer()
        {
            //progressBarUpdateTimer.Start();
            progressBarUpdateTimer = new System.Timers.Timer();
            progressBarUpdateTimer.Enabled = true;
            progressBarUpdateTimer.Elapsed += new ElapsedEventHandler(progressBarUpdateTimer_Elapsed);
            progressBarUpdateTimer.Interval = 1000;
            progressBarUpdateTimer.Start();
            //thread.Join();
        }

        /**
        * Helper Function to replace non-numeric chars in a string
        * author: https://github.com/RayRay5
        */
        private string regexReplaceNonNumericChars(string line)
        {
            return Regex.Replace(line, "[^0-9.]", "");
        }

        /**
        *
        *
        */
        private void resetData()
        {
            try
            {
                treeView1.Nodes.Clear();
                treeView2.Nodes.Clear();
                garages.Clear();
                garageSize.Clear();
                cities.Clear();
                visitedCities.Clear();

            }
            catch (Exception)
            {
                MessageBox.Show("some error");
            }

            textBox2.Text = "(empty)";
            textBox3.Text = "(empty)";
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
        }

        /**
        * Analyzes the data from the savegame and sets up the corresponding gui elements
        * author: https://github.com/RayRay5
        */
        private void analyzeData()
        {
            button4.Enabled = false;
            if (isInvalidFile())
            {
                return;
            }

            resetData();

            treeView1.Nodes.Add("Other Stuff");
            treeView1.Nodes.Add("Garages");
            treeView1.Nodes.Add("All Cities");
            treeView1.Nodes.Add("Visited Cities");
 
            for(int i = 0; i < treeView1.Nodes.Count; ++i)
            {
                treeView1.Nodes[i].ContextMenuStrip = normalstrip;
            }
            
            filename = textBox1.Text;
            lines = File.ReadAllLines(filename);

            calculateUpperLimit(lines);

            /*
            * read bank account money
            */
            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains("money_account"))
                {
                    textBox2.Text = regexReplaceNonNumericChars(lines[i]);
                    break;
                }
            }

            /*
            * read visited cities
            */

            foreach (string line in lines)
            {
                if (Regex.IsMatch(line, "visited_cities\\[[0-9]*\\]"))
                {
                    string cityname = Regex.Replace(line, "visited_cities\\[[0-9]*\\]: ", "");
                    cityname = Regex.Replace(cityname, " ", "");
                    visitedCities.Add(cityname);
                }
            }

            visitedCitiesBeforeAnalyzation = visitedCities.Count;

            foreach (string visitedCity in visitedCities)
            {
                treeView1.Nodes[3].Nodes.Add(visitedCity);
            }
            treeView1.Nodes[3].Text += "(Size: " + visitedCities.Count + ")";

            /*
            * read cities
            */

            for (int i = 0; i < upperLimit; ++i)
            {
                if(Regex.IsMatch(lines[i], "company.volatile"))
                {
                    string companyname = Regex.Replace(lines[i], "company.volatile.", "");
                    companyname = Regex.Replace(companyname, "companies\\[[0-9]*\\]: ", "");
                    string cityname = Regex.Replace(companyname, "[A-Za-z0-9_]*[.]", "");
                    cityname = Regex.Replace(cityname, "company : ", "");
                    cityname = Regex.Replace(cityname, "{", "");
                    cityname = Regex.Replace(cityname, " ", "");
                    cities.Add(cityname);
                    //companyname = Regex.Replace(companyname, "[.][a-zA-Z0-9_]*", "");
                    companyname = Regex.Replace(companyname, " ", "");
                    companies.Add(companyname);
                }
            }

            foreach (string city in cities)
            {
                if(!uniqueCities.Keys.Contains(city))
                {
                    uniqueCities.Add(city, 0);
                }                
            }

            foreach(string cities in uniqueCities.Keys)
            {
                treeView1.Nodes[2].Nodes.Add(cities);
                if(!visitedCities.Contains(cities))
                {
                    treeView1.Nodes[2].LastNode.ForeColor = Color.DarkRed;
                    treeView1.Nodes[2].LastNode.ContextMenuStrip = addToVisitedCities;
                }
                else
                {
                    treeView1.Nodes[2].LastNode.ForeColor = Color.DarkGreen;
                }
            }

            treeView1.Nodes[2].Text += "(Size: " + uniqueCities.Count + ")";

            /*
            * read garage data
            */
            for (int i = 0; i < lines.Length; ++i)
            {
                if (Regex.IsMatch(lines[i], "garage.") && !(lines[i].Contains("garages")))
                //if (lines[i].Contains("garage") && !lines[i].Contains("garages"))
                {
                    //MessageBox.Show(lines[i]);
                    string addString = Regex.Replace(lines[i], "garage : garage.", "");
                    addString = Regex.Replace(addString, " {", "");

                    do
                    {
                        i++;
                    }
                    while (!lines[i].Contains("status"));

                    string size = Regex.Replace(lines[i], " status: ", "");

                    //addString += " (Size: " + size + ")";
                    if (!addString.Contains("garage"))
                    {
                        garageSize.Add(size);
                        garages.Add(addString);
                    }

                }
            }
            treeView1.Nodes[1].Text += "(Size: " + garages.Count.ToString() + ")";
            //MessageBox.Show(garages.Count.ToString());

            string output = "";
            for (int i = 0; i < garages.Count; ++i)
            {
                output += garages[i] + ";";
            }

            for (int j = 0; j < garages.Count; ++j)
            {
                TreeNode n = new TreeNode();
                n.Text = garages[j].ToString();

                if (n.Text.Length > 25)
                {
                    otherStuff.Add(garages[j]);
                    treeView1.Nodes[0].Nodes.Add(n);
                }
                else
                {
                    changeForeColor(n, j);
                    treeView1.Nodes[1].Nodes.Add(n); //Nodes[1] falls es mehr Elemente im Treeview gibt
                    n.ContextMenuStrip = upgradeGarages;
                }
            }

            treeView1.Nodes[0].Text += "(Size: " + otherStuff.Count + ")";

            /*
            * read exp and skill data
            */
            for (int i = 0; i < upperLimit; ++i)
            {
                if (lines[i].Contains("experience_points"))
                {
                    textBox3.Text = regexReplaceNonNumericChars(lines[i]);
                    i++;
                    comboBox1.SelectedIndex = Int32.Parse(regexReplaceNonNumericChars(lines[i])) + 1;
                    i++;
                    comboBox2.SelectedIndex = Int32.Parse(regexReplaceNonNumericChars(lines[i])) + 1;
                    i++;
                    comboBox3.SelectedIndex = Int32.Parse(regexReplaceNonNumericChars(lines[i])) + 1;
                    i++;
                    comboBox4.SelectedIndex = Int32.Parse(regexReplaceNonNumericChars(lines[i])) + 1;
                    i++;
                    comboBox5.SelectedIndex = Int32.Parse(regexReplaceNonNumericChars(lines[i])) + 1;
                    i++;
                    comboBox6.SelectedIndex = Int32.Parse(regexReplaceNonNumericChars(lines[i])) + 1;
                    break;
                }
            }

            button4.Enabled = false;  // true; // enable job dispatcher
            button5.Enabled = true; // enable unlock buttons
            button6.Enabled = true; // enable unlock buttons
            Cursor.Current = Cursors.Default;

            //MessageBox.Show("cities[0]: " + cities[0]);
            //MessageBox.Show("companies[0]: " + companies[0]);
            return;
        }

        /**
        * executes the analyzation of the profile data
        * author: https://github.com/RayRay5
        */
        private void button3_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            analyzeData();
            Cursor.Current = Cursors.Default;
        }

        /**
        * unified version to set data for predefinded settings
        * author: https://github.com/RayRay5
        */
        private void applyPresetData(string exp, string money, string skill, string adrskill)
        {
            if (isInvalidFile())
            {
                return;
            }
            string filename = ofd.FileName;
            string[] lines = File.ReadAllLines(filename);

            calculateUpperLimit(lines);

            /*
            * Write money bank account data
            */
            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains("money_account"))
                {
                    lines[i] = " money_account: " + money;
                    break;
                }
            }

            /*
            * Write exp and skill data
            */
            for (int i = 0; i < upperLimit; ++i)
            {
                if (lines[i].Contains("experience_points"))
                {
                    lines[i] = " experience_points: " + exp;
                    i++;
                    lines[i] = " adr: " + adrskill;
                    i++;
                    lines[i] = " long_dist: " + skill;
                    i++;
                    lines[i] = " heavy: " + skill;
                    i++;
                    lines[i] = " fragile: " + skill;
                    i++;
                    lines[i] = " urgent: " + skill;
                    i++;
                    lines[i] = " mechanical: " + skill;

                    File.WriteAllLines(ofd.FileName, lines);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Saved savegame modifications");
                    return;
                }
            }
        }

        /**
        * Applies slow start settings, see README.md
        * same as apply settings, but reduced
        * author: https://github.com/RayRay5
        */
        /*private void button4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            applyPresetData("5000", "100000", "0", "0");
            analyzeData();
            Cursor.Current = Cursors.Default; 
        }*/

        /**
        * Applies quick start settings, see README.md
        * same as "Apply 'Slow Start'", but with different values
        * author: https://github.com/RayRay5
        */
        /*private void button5_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            applyPresetData("5000000", "100000000", "6", "63");
            analyzeData();
            Cursor.Current = Cursors.Default;
        }*/

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //treeView1.SelectedNode = (TreeNode) sender;
            //MessageBox.Show(e.Node.ToString());
        }

        /**
        * Select the backup file
        * author: https://github.com/RayRay5
        */
        private void button8_Click(object sender, EventArgs e)
        {
            sfd.Filter = "ETS2 Savegames (*.sii)|*.sii";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
            //    dir = Directory.GetCurrentDirectory();
                textBox4.Text = sfd.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form newForm = new newForm();
            newForm.Show();
        }

        private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("GNU General Public License v3.0 from June 29th, 2007." + Environment.NewLine + Environment.NewLine +
                "You can access the full text over here " + Environment.NewLine + 
                "https://raw.githubusercontent.com/RayRay5/ets2_savegame_editor/master/LICENSE", "License");
        }

        private void versionInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version " + _versionNumber, "Installed Version");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ETS2 Savegame Editor by https://github.com/RayRay5", "About");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void systemRequirementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(".NET Framework Version 4.6 or later" + Environment.NewLine + "No other special requirements"
                + Environment.NewLine + "However a good CPU is recommended", "System Requirements");
        }

        private void everyoneIWouldLikeToThankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO", "THANK YOU!");
        }

        private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ProcessStartInfo sInfo = new ProcessStartInfo("http://google.com");
            //Process.Start(sInfo);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            treeView1.Nodes[1].Collapse();
            striplabel4_Click(sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            visitedCities.Clear();
            string[] allKeys = uniqueCities.Keys.ToArray();
            for(int i = 0; i < uniqueCities.Count; ++i)
            {
                visitedCities.Add(allKeys[i]);
            }
            
            //TODO

            treeView1.SelectedNode = treeView1.Nodes[2];
            treeView1.Nodes[3].Nodes.Clear();

            foreach (TreeNode n in treeView1.Nodes[2].Nodes)
            {
                TreeNode copyNode = new TreeNode();
                copyNode.Text = n.Text;
                treeView1.Nodes[3].Nodes.Add(copyNode);
                n.ForeColor = Color.Green;
                //context menu strip
                n.ContextMenuStrip = null;
            }

            foreach(TreeNode n in treeView1.Nodes[3].Nodes)
            {
                n.ForeColor = Color.Green;
            }
            treeView1.Nodes[3].Text = "Visited Cities(Size: " + treeView1.Nodes[3].Nodes.Count + ")";
            button6.Enabled = false;
            this.Invoke(new Action(() => { MessageBox.Show("Unlocked all Cities"); }));
        }

        private void removeEmptyLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filename = textBox1.Text;
            string[] lines = File.ReadAllLines(filename);
            ArrayList writeBackLines = new ArrayList();

            foreach(string line in lines)
            {
                //Console.WriteLine(line);

                if(line != "\r\n" || line != " \r\n")
                {
                    writeBackLines.Add(line);
                }
                else
                {
                    Console.WriteLine("leer");
                }
            }

            string[] writeBack = (string[])writeBackLines.ToArray(typeof(string));
            progressBar1.Value = 0;
            progressBar1.Maximum = writeBack.Length + 1;
            createProgressTimer();
            this.button2.Visible = false;
            this.progressBar1.Visible = true;

            thread = new Thread(() => doBackgroundStuff(writeBack));
            thread.Start();
            Thread info = new Thread(() => backgroundCallBack());
            info.Start();
        }
    }
}
