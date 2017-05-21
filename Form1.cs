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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        string dir;

        private void saveOldFile(string filename)
        {
            string oldfilename = "game_old.sii";
            MessageBox.Show("filename: " + filename + "\r\n" + "new path: " + Path.Combine(dir, oldfilename));
            //File.Copy(filename, Path.Combine(dir, oldfilename));
        }
        
        OpenFileDialog ofd = new OpenFileDialog();

        private void button1_Click(object sender, EventArgs e)
        {
            ofd.Filter = "ETS2 Savegames|*.sii";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                dir = Directory.GetCurrentDirectory();
                textBox1.Text = ofd.FileName;
                saveOldFile(ofd.FileName);
            }

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
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

        private void button3_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string filename = ofd.FileName;
            string[] lines = File.ReadAllLines(filename);

            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains("money_account"))
                {
                    textBox2.Text = rr(lines[i]);
                    break;
                }
            }

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
                    return;
                }
            }
            
            Cursor.Current = Cursors.Default;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
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

        private void button5_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
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
    }
}
