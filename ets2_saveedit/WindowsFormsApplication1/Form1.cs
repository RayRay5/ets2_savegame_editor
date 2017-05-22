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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        OpenFileDialog ofd = new OpenFileDialog();
        SaveFileDialog sfd = new SaveFileDialog();
        ArrayList garages = new ArrayList();
        ArrayList garageSize = new ArrayList();
        ArrayList otherStuff = new ArrayList();
        string dir;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /**
        * validate if file is a .sii file and if the file is a valid file or not
        * author: https://github.com/RayRay5
        * TODO check if file is decrypted or not
        */
        private bool isInvalidFile()
        {
            if (!textBox1.Text.Contains(".sii"))
            {
                MessageBox.Show("Invalid or no file selected");
                return true;
            }
            return false;
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
        * Open the OpenFileDialog
        * author: https://github.com/RayRay5
        */
        private void button1_Click(object sender, EventArgs e)
        {
            ofd.Filter = "ETS2 Savegames|*.sii";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                //dir = Directory.GetCurrentDirectory();
                textBox1.Text = ofd.FileName;
                //saveOldFile(ofd.FileName);
            }
        }

        /**
        * Applies the settings made. (writes all stuff back to file in the hopefully correct format)
        * author: https://github.com/RayRay5
        */
        private void button2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if(isInvalidFile())
            {
                return;
            }

            dir = Directory.GetCurrentDirectory();
            saveOldFile(textBox1.Text, textBox4.Text);

            string filename = ofd.FileName;
            string[] lines = File.ReadAllLines(filename);

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

            for (int i = 0; i < 10000; ++i)
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

                    File.WriteAllLines(ofd.FileName, lines);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Saved savegame modifications");
                    return;
                }
            }
            Cursor.Current = Cursors.Default;
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
        * Analyzes the data from the savegame and sets up the corresponding gui elements
        * author: https://github.com/RayRay5
        */
        private void analyzeData()
        {
            if (isInvalidFile())
            {
                MessageBox.Show("Invalid file provided");
                return;
            }

            try
            {
                treeView1.Nodes.Clear();
                garages.Clear();
                garageSize.Clear();

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

            treeView1.Nodes.Add("other stuff");
            treeView1.Nodes.Add("Garages");

            string filename = textBox1.Text;
            string[] lines = File.ReadAllLines(filename);

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
                }
            }

            treeView1.Nodes[0].Text += "(Size: " + otherStuff.Count + ")";

            /*
            * read exp and skill data
            */
            for (int i = 0; i < 10000; ++i)
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
                    Cursor.Current = Cursors.Default;

                    return;
                }
            }
        }

        /**
        * executes the analyzationof the profile data
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
            for (int i = 0; i < 10000; ++i)
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
        private void button4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            applyPresetData("5000", "100000", "0", "0");
            analyzeData();
            Cursor.Current = Cursors.Default; 
        }

        /**
        * Applies quick start settings, see README.md
        * same as "Apply 'Slow Start'", but with different values
        * author: https://github.com/RayRay5
        */
        private void button5_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            applyPresetData("5000000", "100000000", "6", "63");
            analyzeData();
            Cursor.Current = Cursors.Default;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        /**
        * Maniuplates the data for "downgrading" a garage
        * author: https://github.com/RayRay5
        */
        private void button6_Click(object sender, EventArgs e)
        {
            TreeNode t = treeView1.SelectedNode;
            string tstring = t.ToString().Replace("TreeNode: ", "");
            int index = garages.IndexOf(tstring);

            if (Int32.Parse(garageSize[index].ToString()) > 3)
            {

            }
            else if (Int32.Parse(garageSize[index].ToString()) <= 0)
            {
                MessageBox.Show("Garage can not be downgraded. Garage already has smallest possible size");
            }
            else
            {
                garageSize[index] = (Int32.Parse(garageSize[index].ToString()) - 1).ToString();
                changeForeColor(t, index);
                int size = Int32.Parse(garageSize[index].ToString());
            }
        }

        /**
        * Maniuplates the data for "upgrading" a garage
        * author: https://github.com/RayRay5
        */
        private void button7_Click(object sender, EventArgs e)
        {
            TreeNode t = treeView1.SelectedNode;
            string tstring = t.ToString();
            tstring = tstring.Replace("TreeNode: ", "");
            int index = garages.IndexOf(tstring);
            //garages[index] = tstring;

            /*if(Int32.Parse(garageSize[index].ToString()) <= 2 && Int32.Parse(garageSize[index].ToString()) >= 0)
            {
                
            }*/
            if(Int32.Parse(garageSize[index].ToString()) >= 3)
            {
                MessageBox.Show("Garage can not be upgraded. Garage already has biggest possible size");
            }
            else if(Int32.Parse(garageSize[index].ToString()) < 0)
            {

            }
            else
            {
                garageSize[index] = (Int32.Parse(garageSize[index].ToString()) + 1).ToString();
                changeForeColor(t, index);
                int size = Int32.Parse(garageSize[index].ToString());
            }
        }

        /**
        * Select the backup file
        * author: https://github.com/RayRay5
        */
        private void button8_Click(object sender, EventArgs e)
        {
            sfd.Filter = "ETS2 Savegames|*.sii";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
            //    dir = Directory.GetCurrentDirectory();
                textBox4.Text = sfd.FileName;
            }
        }
    }
}
