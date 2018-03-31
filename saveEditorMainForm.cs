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
    enum GarageSize : UInt16 { NULL = 0, TINY = 6, SMALL = 2, BIG = 3 };
    enum GarageSlots : UInt16 { NULL = 0, TINY = 1, SMALL = 3, BIG = 5 };

    public partial class ets2_saveeditor_main_form : Form
    {
        private static readonly string _versionNumber = "1.1.0.8 (Beta - to be released as final)";
        private static readonly string _internal_version_number = "1.1.0.8";
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
        ArrayList allTrucks = new ArrayList();
        ArrayList allDrivers = new ArrayList();
        ArrayList allProfitLogs = new ArrayList();

        System.Timers.Timer progressBarUpdateTimer;
        Thread thread;

        static Int32 progress = 0;
        static Int32 visitedCitiesBeforeAnalyzation = 0;
        static Int32 upperLimit = 0;
        static Int32 arrayCounter = 0;
        static Int32 driverArrayCounter = 1; // = 1 if we satrt with custom hq instead of the default set in the code below
        static string dir;

        public static string[] lines;
        public static Boolean[] ignore_lines;
        public static string hq_city = "";

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

            if (File.Exists(@"config.txt") && saveEditor.Conf != null)
            {
                changeLanguage();
            }
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
                    if (Int16.Parse(garageSize[index].ToString()) <= 0)
                    {
                        MessageBox.Show("Garage can not be downgraded. Garage already has smallest possible size", "Garage can not be downgraded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (Int16.Parse(garageSize[index].ToString()) > 3 && Int16.Parse(garageSize[index].ToString()) != 6)
                    {
                        //MessageBox.Show("Stop messing around with the savegame. This is definitely not going to work");
                    }
                    else
                    {
                        // Downgrade Garages
                        if (Int16.Parse(garageSize[index].ToString()) == 6)
                        {
                            garageSize[index] = 0;
                        }
                        else if(Int16.Parse(garageSize[index].ToString()) == 2)
                        {
                            garageSize[index] = 6;
                        }
                        else
                        {
                            garageSize[index] = (Int16.Parse(garageSize[index].ToString()) - 1).ToString();
                            //changeForeColor(t, index);
                        }
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
            /*MessageBox.Show("77: " + (string) sender.ToString());
            /*string sending = sender.ToString();
            if (sender.GetType() == typeof(System.Windows.Forms.Button))
            {
                MessageBox.Show("It's a button");
            }*/

            if (treeView1.Nodes[1].IsExpanded && !(sender.GetType() == typeof(System.Windows.Forms.Button)))
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
                    if (Int16.Parse(garageSize[index].ToString()) >= 3 && Int16.Parse(garageSize[index].ToString()) != 6)
                    {
                        MessageBox.Show("Garage can not be upgraded. Garage already has biggest possible size", "Garage can not be upgraded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (Int16.Parse(garageSize[index].ToString()) < 0)
                    {
                        MessageBox.Show("Stop messing around with the savegame. This is definitely not going to work and will crash your game", "You are messing around with your savegame", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        // Upgrade Garages
                        if (Int16.Parse(garageSize[index].ToString()) == 0)
                        {
                            garageSize[index] = 6;
                        }
                        else if (Int16.Parse(garageSize[index].ToString()) == 6)
                        {
                            garageSize[index] = 2;
                        }
                        else
                        {
                            garageSize[index] = (Int32.Parse(garageSize[index].ToString()) + 1).ToString();
                            //changeForeColor(t, index);
                            if (t2 != null)
                            {
                                changeForeColor(t2, index);
                            }
                            int size = Int32.Parse(garageSize[index].ToString());
                        }
                        changeForeColor(t, index);
                    }
                }
            }
            else if((sender.GetType() == typeof(System.Windows.Forms.Button)))
            {
                for(int i = 0; i < garageSize.Count; ++i)
                {
                    if(Int32.Parse(garageSize[i].ToString()) < 3)
                    {
                        garageSize[i] = "3";
                    }
                    
                }

                this.Invoke(new Action(() => { MessageBox.Show("Unlocked all Garages", "Unlocked all Garages", MessageBoxButtons.OK, MessageBoxIcon.Information); }));
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
            try
            {
                progressBar1.Value += progress;
            }
            catch(ArgumentOutOfRangeException)
            {
                Console.WriteLine("Progress Out Of Bounds");
            }

            progress = 0;

            if (!thread.IsAlive)
            {
                progressBarUpdateTimer.Stop();
                this.progressBar1.Visible = false;
                this.button2.Visible = true;
                MessageBox.Show("Saved savegame modifications", "Saving completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                Console.WriteLine("Exc4");
                this.Invoke(new Action(() => { MessageBox.Show("Unexpected exception occured: " + exception.ToString(), "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Stop); }));
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
                string firstLine = "";
                try
                {
                    firstLine = File.ReadLines(textBox1.Text).First(); // check if ScsC header is set
                }
                catch (InvalidOperationException)
                {
                    this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("The file you are opening does not contain any content or might be corrupted!", "Invalid File Content", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        })
                    );
                    return true;
                }

                if (!Regex.IsMatch(firstLine, "ScsC"))
                {
                    return false;
                }

                MessageBox.Show("File is not decrypted, please decrypt it using the Savegame-Decrypter", "Input Filestream Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    hq_city = lines[i].Replace("hq_city:", "");
                    hq_city = hq_city.Replace(" ", "");
                    Console.WriteLine("hq_upper: " + hq_city + " :");
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
                MessageBox.Show("Can't create backup file. File does already exist", "File does already exist!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                case "6":
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

        private void doBackgroundStuff(string[] lines, Boolean[] ignore_lines)
        {
            string[] savelines = lines;
            string content = "";

            int useCPU_Count = saveEditor.CPU_Count - 2;
            //int useCPU_Count = 2;
            string[] contents = new string[useCPU_Count + 1];
            Thread[] stringThreads = new Thread[useCPU_Count];

            /*for (int cpu_counting = 0; cpu_counting < useCPU_Count; ++cpu_counting)
            {
                this.Invoke(new Action(() =>
                {
                    //MessageBox.Show("cpu_counting: " + cpu_counting + Environment.NewLine + "count: " + useCPU_Count);
                }));

                if(cpu_counting == useCPU_Count)
                {
                    MessageBox.Show("Gleich");
                }

                try
                {
                    ziel = ((lines.Length / useCPU_Count) * (cpu_counting + 1)) - 1;
                    start = (lines.Length / useCPU_Count) * cpu_counting;
                }
                catch(DivideByZeroException)
                {
                    start = 0;
                }

                //Console.WriteLine("Start: " + start + Environment.NewLine + "Ziel: " + ziel);

                stringThreads[cpu_counting] = new Thread(() =>
                {
                    for (int j = start; j < ziel; ++j) // TODO
                    {
                        //File.WriteAllText(ofd.FileName, (lines[i] + Environment.NewLine));
                        if (ignore_lines[j] == false)
                        {
                            try
                            {
                                contents[cpu_counting] += savelines[j] + Environment.NewLine;
                                //string cont = contents[cpu_counting];
                            }
                            catch(IndexOutOfRangeException range)
                            {
                                //Console.WriteLine(range.ToString());
                                Console.WriteLine("cpu_counting: " + cpu_counting);
                                this.Invoke(new Action(() =>
                                {
                                    MessageBox.Show("cpu_counting: " + cpu_counting + Environment.NewLine + "count: " + useCPU_Count + Environment.NewLine + "contents length: " + contents.Length
                                        + Environment.NewLine + "savelines: " + savelines.Length + Environment.NewLine + "");
                                }));
                            }
                        }

                        //++progress;
                    }
                });
                stringThreads[cpu_counting].Start();
                stringThreads[cpu_counting].Name = "WriteBackThread " + cpu_counting.ToString();
            }

            for(int i = 0; i < stringThreads.Length; ++i)
            {
                Console.WriteLine("thread ended: " + i);
                stringThreads[i].Join();
            }

            for (int i = 0; i < stringThreads.Length; ++i)
            {
                content = contents[i].ToString();
            }*/

            string content1 = "";
            string content2 = "";
            string content3 = "";
            string content4 = "";

            //int lines_per_thread = lines.Length / useCPU_Count;

            // TODO Anfang optimieren
            Thread stringThread1 = new Thread(() => 
            {
                for (int j = 0; j < lines.Length / 4; ++j)
                {
                    //File.WriteAllText(ofd.FileName, (lines[i] + Environment.NewLine));
                    if (ignore_lines[j] == false)
                    {
                        content1 += savelines[j] + Environment.NewLine;
                    }

                    ++progress;
                }
            });
            stringThread1.Start();

            Thread stringThread2 = new Thread(() =>
            {
                for (int j = ((lines.Length / 4) * 1); j < ((lines.Length / 4) * 2); ++j)
                {
                    //File.WriteAllText(ofd.FileName, (lines[i] + Environment.NewLine));
                    if (ignore_lines[j] == false)
                    {
                        content2 += savelines[j] + Environment.NewLine;
                    }

                    ++progress;
                }
            });
            stringThread2.Start();

            Thread stringThread3 = new Thread(() =>
            {
                for (int j = ((lines.Length / 4) * 2); j < ((lines.Length / 4) * 3); ++j)
                {
                    //File.WriteAllText(ofd.FileName, (lines[i] + Environment.NewLine));
                    if (ignore_lines[j] == false)
                    {
                        content3 += savelines[j] + Environment.NewLine;
                    }

                    ++progress;
                }
            });
            stringThread3.Start();

            Thread stringThread4 = new Thread(() =>
            {
                for (int j = ((lines.Length / 4) * 3); j < lines.Length; ++j)
                {
                    //File.WriteAllText(ofd.FileName, (lines[i] + Environment.NewLine));
                    if (ignore_lines[j] == false)
                    {
                        content4 += savelines[j] + Environment.NewLine;
                    }

                    ++progress;
                }
            });
            stringThread4.Start();

            stringThread1.Join();
            stringThread2.Join();
            stringThread3.Join();
            stringThread4.Join();

            content = content1 + content2 + content3 + content4;

            // Anfang Original WriteBack Code
            /*for (int j = 0; j < lines.Length; ++j)
            {
                //File.WriteAllText(ofd.FileName, (lines[i] + Environment.NewLine));
                if (ignore_lines[j] == false)
                {
                    content += savelines[j] + Environment.NewLine;
                }

                ++progress;
            }*/
            // Ende Original Writeback Code

            File.WriteAllText(ofd.FileName, content);
            ++progress;

            // TODO Ende

            //Console.WriteLine("write b");
            toggleContextMenu(true);

            this.Invoke(new Action(() =>
            {
                this.button1.Enabled = true;
                this.button2.Enabled = true;
                this.button3.Enabled = true;
                this.button4.Enabled = true;// false; hier wird gerade dran gearbeitet
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

            visitedCitiesBeforeAnalyzation = visitedCities.Count;
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
                            //Console.WriteLine("Exc1");
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
                            Console.WriteLine("Exc2");
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
            * TODO
            */
            ignore_lines = new bool[lines.Length];

            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains("garage") && !lines[i].Contains("garages") && (!(lines[i+1].Contains("}")) || !(lines[i+1].Contains("garage"))))
                {
                    string ort = Regex.Replace(lines[i], "garage : garage.", "");
                    ort = Regex.Replace(ort, " {", "");

                    if(ort.Length < 15)
                    {
                        int ind = garages.IndexOf(ort);
                        // Baustelle Anfang
                        //TODO vehicles und drivers hinzufügen
                        if (ind > -1)
                        {
                            do
                            {
                                ++i;
                            }
                            while (!lines[i].Contains("vehicles:"));
                        
                            //int arrayLooper = 0;
                            int totalSlots = garages.Count * 5;
                            string[] insert = new string[totalSlots];

                            for(int j = 0; j < allTrucks.Count; ++j)
                            {
                                //allTrucks[i] = allTrucks[i].ToString().Replace(" trucks[0]: ", "");
                                allTrucks[j] = Regex.Replace(allTrucks[j].ToString(), " trucks\\[[0-9]*\\]: ", "");
                                //Regex.IsMatch(lines[i], "driver_readiness_timer\\[[0-9]*\\]")
                                //Regex.Replace(lines[i], "garage : garage.", "");
                            }

                            switch (UInt16.Parse(garageSize[ind].ToString()))
                            {
                                case (UInt16) GarageSize.NULL:
                                    lines[i] = " vehicles: 0";
                                    break;
                                case 1:
                                    //lines[i] = " vehicles: 1\r\n vehicles[0]: null";
                                    break;
                                case (UInt16) GarageSize.SMALL:
                                    for (int arrayLooper = 0; arrayLooper < (UInt16)GarageSlots.SMALL; ++arrayLooper)
                                    {
                                        if (arrayCounter >= allTrucks.Count)
                                        {
                                            insert[arrayCounter] = "null";
                                            //Console.WriteLine("nuller");
                                        }
                                        else
                                        {
                                            insert[arrayCounter] = allTrucks[arrayCounter].ToString();
                                        }

                                        /*Console.WriteLine("arrayCounter: " + arrayCounter);
                                        Console.WriteLine(insert[arrayCounter].ToString());*/
                                        ++arrayCounter;
                                    }
                                    lines[i] = " vehicles: 3\r\n vehicles[0]: " + insert[arrayCounter - 3] + "\r\n vehicles[1]: " + insert[arrayCounter - 2] + "\r\n vehicles[2]: " + insert[arrayCounter - 1] + "";
                                    break;
                                case (UInt16) GarageSize.BIG:
                                    for (int arrayLooper = 0; arrayLooper < (UInt16)GarageSlots.BIG; ++arrayLooper)
                                    {
                                        if (arrayCounter >= allTrucks.Count)
                                        {
                                            insert[arrayCounter] = "null";
                                            //Console.WriteLine("nuller");
                                        }
                                        else
                                        {
                                            insert[arrayCounter] = allTrucks[arrayCounter].ToString();
                                        }

                                        /*Console.WriteLine("arrayCounter: " + arrayCounter);
                                        Console.WriteLine(insert[arrayCounter].ToString());*/
                                        ++arrayCounter;
                                    }
                                    lines[i] = " vehicles: 5\r\n vehicles[0]: " + insert[arrayCounter - 5] + "\r\n vehicles[1]: " + insert[arrayCounter - 4] + "\r\n vehicles[2]: " + insert[arrayCounter - 3] + "\r\n vehicles[3]: " + insert[arrayCounter - 2] + "\r\n vehicles[4]: " + insert[arrayCounter - 1] + "";
                                    break;
                                case 4:
                                    //lines[i] = " vehicles: 1\r\n vehicles[0]: null";
                                    break;
                                case 5:
                                    //lines[i] = " vehicles: 3\r\n vehicles[0]: null\r\n vehicles[1]: null\r\n vehicles[2]: null";
                                    break;
                                case (UInt16) GarageSize.TINY:
                                    for (int arrayLooper = 0; arrayLooper < (UInt16)GarageSlots.TINY; ++arrayLooper)
                                    {
                                        if (arrayCounter >= allTrucks.Count)
                                        {
                                            insert[arrayCounter] = "null";
                                            //Console.WriteLine("nuller");
                                        }
                                        else
                                        {
                                            insert[arrayCounter] = allTrucks[arrayCounter].ToString();
                                        }

                                        /*Console.WriteLine("arrayCounter: " + arrayCounter);
                                        Console.WriteLine(insert[arrayCounter].ToString());*/
                                        ++arrayCounter;
                                    }
                                    lines[i] = " vehicles: 1\r\n vehicles[0]: " + insert[arrayCounter - 1] + "";
                                    break;
                                default:
                                    break;
                            }                            

                            do
                            {
                                ++i;
                                if(lines[i].Contains("vehicles["))
                                {
                                    //lines[i] = "";
                                    ignore_lines[i] = true;
                                }
                            }
                            while (!lines[i].Contains("drivers:"));

                            totalSlots = garages.Count * 5;
                            insert = new string[totalSlots];

                            for (int j = 0; j < allDrivers.Count; ++j)
                            {
                                //allTrucks[i] = allTrucks[i].ToString().Replace(" trucks[0]: ", "");
                                allDrivers[j] = Regex.Replace(allDrivers[j].ToString(), " drivers\\[[0-9]*\\]: ", "");
                                //Regex.IsMatch(lines[i], "driver_readiness_timer\\[[0-9]*\\]")
                                //Regex.Replace(lines[i], "garage : garage.", "");
                            }

                            switch (UInt16.Parse(garageSize[ind].ToString()))
                            {
                                case (UInt16)GarageSize.NULL:
                                    lines[i] = " drivers: 0";
                                    break;
                                case 1:
                                    //lines[i] = " vehicles: 1\r\n vehicles[0]: null";
                                    break;
                                case (UInt16)GarageSize.SMALL:
                                    for (int arrayLooper = 0; arrayLooper < (UInt16)GarageSlots.SMALL; ++arrayLooper)
                                    {
                                        if (driverArrayCounter >= allDrivers.Count)
                                        {
                                            insert[driverArrayCounter] = "null";
                                            //Console.WriteLine("nuller");
                                        }
                                        else
                                        {
                                            insert[driverArrayCounter] = allDrivers[driverArrayCounter].ToString();
                                        }

                                        /*Console.WriteLine("arrayCounter: " + arrayCounter);
                                        Console.WriteLine(insert[arrayCounter].ToString());*/
                                        ++driverArrayCounter;
                                    }

                                    lines[i] = " drivers: 3\r\n";
                                    if (hq_city == garages[ind].ToString())  // TODO
                                    {
                                        lines[i] += " drivers[0]: " + allDrivers[0];
                                        Console.WriteLine("hq insert: " + allDrivers[0]);
                                    }
                                    else
                                    {
                                        lines[i] += " drivers[0]: " + insert[driverArrayCounter - 3];

                                    }
                                    lines[i] += "\r\n drivers[1]: " + insert[driverArrayCounter - 2] + "\r\n drivers[2]: " + insert[driverArrayCounter - 1] + "";

                                    break;
                                case (UInt16)GarageSize.BIG:
                                    for (int arrayLooper = 0; arrayLooper < (UInt16)GarageSlots.BIG; ++arrayLooper)
                                    {
                                        if (driverArrayCounter >= allDrivers.Count)
                                        {
                                            insert[driverArrayCounter] = "null";
                                        }
                                        else
                                        {
                                            insert[driverArrayCounter] = allDrivers[driverArrayCounter].ToString();
                                        }
                                        ++driverArrayCounter;
                                    }

                                    lines[i] = " drivers: 5\r\n";

                                    if (hq_city == garages[ind].ToString())  // TODO
                                    {
                                        lines[i] += " drivers[0]: " + allDrivers[0];
                                    }
                                    else
                                    {
                                        lines[i] += " drivers[0]: " + insert[driverArrayCounter - 5];
                                    }
                                    lines[i] += "\r\n drivers[1]: " + insert[driverArrayCounter - 4] + "\r\n drivers[2]: " + insert[driverArrayCounter - 3] + "\r\n drivers[3]: " + insert[driverArrayCounter - 2] + "\r\n drivers[4]: " + insert[driverArrayCounter - 1] + "";

                                    break;
                                case 4:
                                    //lines[i] = " vehicles: 1\r\n vehicles[0]: null";
                                    break;
                                case 5:
                                    //lines[i] = " vehicles: 3\r\n vehicles[0]: null\r\n vehicles[1]: null\r\n vehicles[2]: null";
                                    break;
                                case (UInt16)GarageSize.TINY:
                                    for (int arrayLooper = 0; arrayLooper < (UInt16)GarageSlots.TINY; ++arrayLooper)
                                    {
                                        if (driverArrayCounter >= allDrivers.Count)
                                        {
                                            insert[driverArrayCounter] = "null";
                                            //Console.WriteLine("nuller");
                                        }
                                        else
                                        {
                                            insert[driverArrayCounter] = allDrivers[driverArrayCounter].ToString();
                                        }
                                        ++driverArrayCounter;
                                    }

                                    if (hq_city == garages[ind].ToString())  // TODO
                                    {
                                        lines[i] = " drivers: 1\r\n drivers[0]: " + allDrivers[0] + "";
                                    }
                                    else
                                    {
                                        lines[i] = " drivers: 1\r\n drivers[0]: " + insert[driverArrayCounter - 1] + "";
                                    }

                                    break;
                                default:
                                    break;
                            }

                            do
                            {
                                ++i;
                                if (lines[i].Contains("drivers["))
                                {
                                    //lines[i] = "";
                                    ignore_lines[i] = true;
                                }
                            }
                            while (!lines[i].Contains("status"));

                            lines[i] = " status: " + garageSize[ind];
                        }

                        // Baustelle Ende

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

                    if (stuff != "")
                    {
                        lines[i + visitedCitiesBeforeAnalyzation] = lines[i + visitedCitiesBeforeAnalyzation] + Environment.NewLine + stuff;
                    }
                    else
                    {
                        lines[i + visitedCitiesBeforeAnalyzation] = lines[i + visitedCitiesBeforeAnalyzation];
                    }
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

                    if (stuff != "")
                    {
                        lines[i + visitedCitiesBeforeAnalyzation] = lines[i + visitedCitiesBeforeAnalyzation] + Environment.NewLine + stuff;
                    }
                    else
                    {
                        lines[i + visitedCitiesBeforeAnalyzation] = lines[i + visitedCitiesBeforeAnalyzation];
                    }
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
                    
                    if(UInt16.Parse(comboBox1.Text) > 63 || UInt16.Parse(comboBox1.Text) < 0)
                    {
                        comboBox1.Text = "0";
                    }

                    if (UInt16.Parse(comboBox2.Text) > 6 || UInt16.Parse(comboBox2.Text) < 0)
                    {
                        comboBox2.Text = "0";
                    }

                    if (UInt16.Parse(comboBox3.Text) > 6 || UInt16.Parse(comboBox3.Text) < 0)
                    {
                        comboBox3.Text = "0";
                    }

                    if (UInt16.Parse(comboBox4.Text) > 6 || UInt16.Parse(comboBox4.Text) < 0)
                    {
                        comboBox4.Text = "0";
                    }

                    if (UInt16.Parse(comboBox5.Text) > 6 || UInt16.Parse(comboBox5.Text) < 0)
                    {
                        comboBox5.Text = "0";
                    }

                    if (UInt16.Parse(comboBox6.Text) > 6 || UInt16.Parse(comboBox6.Text) < 0)
                    {
                        comboBox6.Text = "0";
                    }

                    ++i;
                    if (comboBox1.Text != "" && comboBox1.Text != "don't change")
                    {
                        lines[i] = " adr: " + comboBox1.Text;
                    }
                    ++i;

                    if(comboBox2.Text != "" && comboBox2.Text != "don't change")
                    {
                        lines[i] = " long_dist: " + comboBox2.Text;
                    }
                    ++i;

                    if (comboBox3.Text != "" && comboBox3.Text != "don't change")
                    {
                        lines[i] = " heavy: " + comboBox3.Text;
                    }
                    ++i;

                    if (comboBox4.Text != "" && comboBox4.Text != "don't change")
                    {
                        lines[i] = " fragile: " + comboBox4.Text;
                    }
                    ++i;

                    if (comboBox5.Text != "" && comboBox5.Text != "don't change")
                    {
                        lines[i] = " urgent: " + comboBox5.Text;
                    }
                    ++i;

                    if (comboBox6.Text != "" && comboBox6.Text != "don't change")
                    {
                        lines[i] = " mechanical: " + comboBox6.Text;
                    }

                    //File.WriteAllLines(ofd.FileName, lines);
                    //Cursor.Current = Cursors.Default;
                    break;
                }
            }

            /*string[] lines1 = new string[lines.Length / 2];
            string[] lines2 = new string[lines.Length / 2 + lines.Length % 2];

            bool[] iglines1 = new bool[ignore_lines.Length / 2];
            bool[] iglines2 = new bool[ignore_lines.Length / 2 + ignore_lines.Length % 2];

            Array.Copy(lines, 0, lines1, 0, (lines.Length / 2));
            Array.Copy(lines, (lines.Length / 2), lines2, 0, ((lines.Length / 2) + (lines.Length % 2)));*/

            thread = new Thread(() => doBackgroundStuff(lines, ignore_lines));
            thread.SetApartmentState(ApartmentState.MTA);
            thread.Start();
            thread.Name = "DoBackgroundStuffThread";
            /*thread1 = new Thread(() => doBackgroundStuff(lines1, iglines1));
            thread1.SetApartmentState(ApartmentState.MTA);
            thread1.Start();
            thread1.Name = "DoBackgroundStuffThread";
            thread2 = new Thread(() => doBackgroundStuff(lines2, iglines2));
            thread2.SetApartmentState(ApartmentState.MTA);
            thread2.Start();
            thread2.Name = "DoBackgroundStuffThread";*/
            Thread info = new Thread(() => backgroundCallBack());
            info.Start();
            info.Name = "InfoStuffThread";

            createProgressTimer();
        }

        private void createProgressTimer()
        {
            //progressBarUpdateTimer.Start();
            progressBarUpdateTimer = new System.Timers.Timer();
            progressBarUpdateTimer.Enabled = true;
            progressBarUpdateTimer.Elapsed += new ElapsedEventHandler(progressBarUpdateTimer_Elapsed); //TODO fix
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
        * Resets all data to the initial state
        * author: https://github.com/RayRay5
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
                Console.WriteLine("Exc1");
                MessageBox.Show("Please report this data to the developer. Error code is EXC1", "Error resetting data", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
             * Read all known drivers of the players company
             */

            for (int i = 0; i < lines.Length; ++i)
            {
                if (Regex.IsMatch(lines[i], "driver_readiness_timer\\[[0-9]*\\]"))
                {
                    do
                    {
                        --i;
                        if(lines[i].Contains("drivers:"))
                        {
                            ++i;
                            do
                            {
                                allDrivers.Add(lines[i]);
                                ++i;
                            }
                            while (!lines[i].Contains("driver_readiness_timer"));

                            break;
                        }
                    }
                    while (!lines[i].Contains("hq_city"));

                    break;
                }
            }

            /*
             * Read all known trucks of the players company
             */

            for (int i = 0; i < lines.Length; ++i)
            {
                if (Regex.IsMatch(lines[i], "driver_readiness_timer\\[[0-9]*\\]"))
                {
                    do
                    {
                        --i;
                        // Erwartet, dass dir Arrays der Trucks und der Logs direkt hintereinander stehen
                        if (lines[i].Contains("trucks:"))
                        {
                            ++i;
                            
                            while (!lines[i].Contains("profit"))
                            {
                                allTrucks.Add(lines[i]);
                                ++i;
                            }

                            ++i;
                            while (!lines[i].Contains("drivers"))
                            {
                                allProfitLogs.Add(lines[i]);
                                ++i;
                            }

                            break;
                        }
                    }
                    while (!lines[i].Contains("hq_city"));

                    break;
                }
            }

            Console.WriteLine("-------------------");
            foreach (string s in allDrivers)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("-------------------");

            foreach (string s in allTrucks)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("-------------------");

            foreach (string s in allProfitLogs)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("-------------------");

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
                    /*if (UInt16.Parse(comboBox1.Text) > 6 || UInt16.Parse(comboBox1.Text) < 0)
                    {
                        comboBox1.SelectedIndex = 1;
                    }

                    if (UInt16.Parse(comboBox2.Text) > 6 || UInt16.Parse(comboBox2.Text) < 0)
                    {
                        comboBox2.SelectedIndex = 1;
                    }

                    if (UInt16.Parse(comboBox3.Text) > 6 || UInt16.Parse(comboBox3.Text) < 0)
                    {
                        comboBox3.SelectedIndex = 1;
                    }

                    if (UInt16.Parse(comboBox4.Text) > 6 || UInt16.Parse(comboBox4.Text) < 0)
                    {
                        comboBox4.SelectedIndex = 1;
                    }

                    if (UInt16.Parse(comboBox5.Text) > 6 || UInt16.Parse(comboBox5.Text) < 0)
                    {
                        comboBox5.SelectedIndex = 1;
                    }

                    if (UInt16.Parse(comboBox6.Text) > 6 || UInt16.Parse(comboBox6.Text) < 0)
                    {
                        comboBox6.SelectedIndex = 1;
                    }*/

                    comboBox1.SelectedIndex = 1;
                    comboBox2.SelectedIndex = 1;
                    comboBox3.SelectedIndex = 1;
                    comboBox4.SelectedIndex = 1;
                    comboBox5.SelectedIndex = 1;
                    comboBox6.SelectedIndex = 1;

                    textBox3.Text = regexReplaceNonNumericChars(lines[i]);
                    ++i;
                    try
                    {
                        comboBox1.SelectedIndex = Int32.Parse(regexReplaceNonNumericChars(lines[i])) + 1;
                        ++i;
                    }
                    catch(Exception)
                    {

                    }

                    try
                    {
                        comboBox2.SelectedIndex = Int32.Parse(regexReplaceNonNumericChars(lines[i])) + 1;
                        ++i;
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {
                        comboBox3.SelectedIndex = Int32.Parse(regexReplaceNonNumericChars(lines[i])) + 1;
                        ++i;
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {
                        comboBox4.SelectedIndex = Int32.Parse(regexReplaceNonNumericChars(lines[i])) + 1;
                        ++i;
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {
                        //Console.WriteLine("index: " + comboBox5.SelectedIndex);
                        comboBox5.SelectedIndex = Int32.Parse(regexReplaceNonNumericChars(lines[i])) + 1;
                        ++i;
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine("index: " + comboBox5.SelectedIndex);
                        Console.WriteLine("invlaid value found");
                    }

                    try
                    {
                        comboBox6.SelectedIndex = Int32.Parse(regexReplaceNonNumericChars(lines[i])) + 1;
                    }
                    catch (Exception)
                    {

                    }                    
                    break;
                }
            }

            //lines[upperLimit] = " hq_city: " + treeView1.Nodes[1].Nodes[0].Text;
            lines[upperLimit] = " hq_city: " + hq_city;
            //Console.WriteLine(treeView1.Nodes[1].Nodes[0].ToString());

            if (saveEditor.dev == true)
            {
                button4.Enabled = true;  // false; // enable job dispatcher
            }
            else
            {
                button4.Enabled = false;
            }

            button2.Enabled = true;
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
                    MessageBox.Show("Saved savegame modifications", "Saving completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            Form jobEditor = new jobEditor();
            jobEditor.Show();
        }

        private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("GNU General Public License v3.0 from June 29th, 2007." + Environment.NewLine + Environment.NewLine +
                "You can access the full text over here " + Environment.NewLine + 
                "https://raw.githubusercontent.com/RayRay5/ets2_savegame_editor/master/LICENSE", "License", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void versionInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version " + _versionNumber, "Installed Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ETS2 Savegame Editor by https://github.com/RayRay5", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void systemRequirementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(".NET Framework Version 4.6 or later" + Environment.NewLine + "No other special requirements"
                + Environment.NewLine + "However a good CPU is recommended", "System Requirements", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void everyoneIWouldLikeToThankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO", "THANK YOU!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ProcessStartInfo sInfo = new ProcessStartInfo("http://google.com");
            //Process.Start(sInfo);
        }

        private void changeLanguage()
        {
            this.button3.Text = Config.lang_terms[0];
            this.label9.Text = Config.lang_terms[1];
            this.button8.Text = Config.lang_terms[2];
            this.button4.Text = Config.lang_terms[3];
            this.button5.Text = Config.lang_terms[4];
            this.button6.Text = Config.lang_terms[5];

            this.menu1ToolStripMenuItem.Text = Config.lang_terms[6];
            this.optimizeToolStripMenuItem.Text = Config.lang_terms[7];
            this.exitToolStripMenuItem.Text = Config.lang_terms[8];

            this.menu2ToolStripMenuItem.Text = Config.lang_terms[9];
            this.helpWithToolStripMenuItem.Text = Config.lang_terms[10];
            this.commonProblemsToolStripMenuItem.Text = Config.lang_terms[11];
            this.licenseToolStripMenuItem.Text = Config.lang_terms[12];
            this.versionInfoToolStripMenuItem.Text = Config.lang_terms[13];
            this.changelogToolStripMenuItem.Text = Config.lang_terms[14];
            this.aboutToolStripMenuItem.Text = Config.lang_terms[15];
            this.systemRequirementsToolStripMenuItem.Text = Config.lang_terms[16];
            this.everyoneIWouldLikeToThankToolStripMenuItem.Text = Config.lang_terms[17];

            this.button1.Text = Config.lang_terms[18];
            this.label7.Text = Config.lang_terms[19];
            this.label8.Text = Config.lang_terms[20];
            this.label2.Text = Config.lang_terms[21];
            this.label3.Text = Config.lang_terms[22];
            this.label4.Text = Config.lang_terms[23];
            this.label5.Text = Config.lang_terms[24];
            this.label6.Text = Config.lang_terms[25];
            this.button2.Text = Config.lang_terms[26];
            this.label1.Text = Config.lang_terms[27];

            
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            /*treeView1.Nodes.Clear();
            treeView2.Nodes.Clear();
            garages.Clear();
            garageSize.Clear();
            cities.Clear();
            visitedCities.Clear();*/
            /*
             * filename = textBox1.Text;
            lines = File.ReadAllLines(filename);

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
             * */

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
            this.Invoke(new Action(() => { MessageBox.Show("Unlocked all Cities", "Unlocked all Cities", MessageBoxButtons.OK, MessageBoxIcon.Information); }));
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

            thread = new Thread(() => doBackgroundStuff(writeBack, ignore_lines));
            thread.Start();
            Thread info = new Thread(() => backgroundCallBack());
            info.Start();
        }
    }
}
