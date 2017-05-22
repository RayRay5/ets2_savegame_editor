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

        private bool isInvalidFile()
        {
            if (!textBox1.Text.Contains(".sii"))
            {
                MessageBox.Show("Invalid or no file selected");
                return true;
            }
            return false;
        }

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

        //OpenFile
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

        //Apply settings
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

            for(int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains("garage") && !lines[i].Contains("garages") && (!(lines[i+1].Contains("}")) || !(lines[i+1].Contains("garage"))))
                {
                    string ort = Regex.Replace(lines[i], "garage : garage.", "");
                    ort = Regex.Replace(ort, " {", "");

                    if(ort.Length < 15)
                    {
                        int ind = garages.IndexOf(ort);
                        //int currGarageSize = 2 * Int32.Parse(garageSize[ind].ToString()) + 1;
                        //string ll = lines[679];
                        //string lls = lines[i].ToString();
                        //string regexedLine = Regex.Replace(lines[i].ToString(), "[^0-9]", "").ToString();

                        /*if(regexedLine != "")
                        {
                            if (Int32.Parse(regexedLine) < currGarageSize)
                            {
                                MessageBox.Show(lines[i+1]);
                                lines[i+1] = "";
                            }
                            MessageBox.Show(regexedLine);
                        }*/

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

                    //int exp = Int32.Parse(lines[i]);
                    if(textBox3.Text != "")
                    {
                        lines[i] = " experience_points: " + textBox3.Text;
                    }
                    
                    i++;
                    //int adr = Int32.Parse(lines[i]);
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

        private string rr(string line)
        {
            return Regex.Replace(line, "[^0-9.]", "");
        }

        //Analyze
        private void button3_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
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

            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains("money_account"))
                {
                    textBox2.Text = rr(lines[i]);
                    break;
                }
            }

            for(int i = 0; i < lines.Length; ++i)
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
                    if(!addString.Contains("garage"))
                    {
                        garageSize.Add(size);
                        garages.Add(addString);
                    }
                    
                }
            }
            treeView1.Nodes[1].Text += "(Size: " + garages.Count.ToString() + ")";
            //MessageBox.Show(garages.Count.ToString());

            string output = "";
            for(int i = 0; i < garages.Count; ++i)
            {
                output += garages[i] + ";";
            }
            //MessageBox.Show(output);
            //Clipboard.SetText(output);
            //MessageBox.Show(garages.Count.ToString());

            //MessageBox.Show(garages.Count.ToString());
            //MessageBox.Show(counter.ToString());

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

            for (int i = 0; i < 10000; ++i)
            {
                if (lines[i].Contains("experience_points"))
                {
                    textBox3.Text = rr(lines[i]);
                    i++;
                    comboBox1.SelectedIndex = Int32.Parse(rr(lines[i])) + 1;
                    i++;
                    comboBox2.SelectedIndex = Int32.Parse(rr(lines[i])) + 1;
                    i++;
                    comboBox3.SelectedIndex = Int32.Parse(rr(lines[i])) + 1;
                    i++;
                    comboBox4.SelectedIndex = Int32.Parse(rr(lines[i])) + 1;
                    i++;
                    comboBox5.SelectedIndex = Int32.Parse(rr(lines[i])) + 1;
                    i++;
                    comboBox6.SelectedIndex = Int32.Parse(rr(lines[i])) + 1;
                    Cursor.Current = Cursors.Default;

                    /*string output = "";
                    foreach(string g in garages)
                    {
                        output += g;
                        output += "\n";
                    }
                    MessageBox.Show(output);*/
                    return;
                }
            }
            
            Cursor.Current = Cursors.Default;
        }

        //slow start
        private void button4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (isInvalidFile())
            {
                return;
            }
            string filename = ofd.FileName;
            string[] lines = File.ReadAllLines(filename);

            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains("money_account"))
                {
                    lines[i] = " money_account: 100000";
                    break;
                }
            }

            for (int i = 0; i < 10000; ++i)
            {
                if (lines[i].Contains("experience_points"))
                {
                    lines[i] = " experience_points: 5000";
                    i++;
                    lines[i] = " adr: 0";
                    i++;
                    lines[i] = " long_dist: 0";
                    i++;
                    lines[i] = " heavy: 0";
                    i++;
                    lines[i] = " fragile: 0";
                    i++;
                    lines[i] = " urgent: 0";
                    i++;
                    lines[i] = " mechanical: 0";

                    File.WriteAllLines(ofd.FileName, lines);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Saved savegame modifications");
                    return;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        //quick start
        private void button5_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (isInvalidFile())
            {
                return;
            }

            string filename = ofd.FileName;
            string[] lines = File.ReadAllLines(filename);

            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains("money_account"))
                {
                    lines[i] = " money_account: 100000000";
                    break;
                }
            }

            for (int i = 0; i < 10000; ++i)
            {
                if (lines[i].Contains("experience_points"))
                {
                    lines[i] = " experience_points: 5000000";
                    i++;
                    lines[i] = " adr: 63";
                    i++;
                    lines[i] = " long_dist: 6";
                    i++;
                    lines[i] = " heavy: 6";
                    i++;
                    lines[i] = " fragile: 6";
                    i++;
                    lines[i] = " urgent: 6";
                    i++;
                    lines[i] = " mechanical: 6";

                    File.WriteAllLines(ofd.FileName, lines);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Saved savegame modifications");
                    return;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        //downgrade garage
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

        //upgrade garage
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
