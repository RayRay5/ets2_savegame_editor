using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

/**
 * SaveEditor Version V2
 * By https://github.com/RayRay5
 * Licensed under GNU General Public License v3.0
 */

namespace SaveEditorV2
{
    public partial class CommonUtilities : Form
    {
        public CommonUtilities()
        {
            

            InitializeComponent();
            treeView1.NodeMouseClick += (_sender, args) => treeView1.SelectedNode = args.Node;
            /*foreach(ArrayList list in SaveEditorAsALibrary.System.unitList)
            {
                foreach(var attr in list)
                {
                    Console.WriteLine(attr.ToString());
                }
            }*/
            ArrayList results = SaveEditorAsALibrary.System.findTerm("bank : ");

            int a = Int32.Parse(results[0].ToString());
            //textBox1.Text = ((ArrayList) SaveEditorAsALibrary.System.unitList[a]).ToString();
            ArrayList attributes = (ArrayList) SaveEditorAsALibrary.System.unitList[a];
            //Console.WriteLine(attributes[1]);
            string str = attributes[1].ToString();
            
            Regex regex = new Regex("[^0-9]");
            str = regex.Replace(str, "");
            textBox1.Text = str;

            ////////////////////////////////////////////////////////////////
            ////
            ////////////////////////////////////////////////////////////////


            ArrayList results2 = SaveEditorAsALibrary.System.findTerm("economy : ");

            int a2 = Int32.Parse(results2[0].ToString());
            //textBox1.Text = ((ArrayList) SaveEditorAsALibrary.System.unitList[a]).ToString();
            /*ArrayList attributes2 = (ArrayList)SaveEditorAsALibrary.System.unitList[a];
            Console.WriteLine(attributes2[1]);
            string str2 = attributes2[1].ToString();

            Regex regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");
            textBox2.Text = str2;*/

            ArrayList attrsResult2 = (ArrayList) SaveEditorAsALibrary.System.findAttr(a2, "experience_points: ");
            int res2 = Int32.Parse(attrsResult2[0].ToString());
            res2 += 2;

            string str2 = SaveEditorAsALibrary.System.lines[res2].ToString();

            Regex regex2 = new Regex("[^0-9]");
            str2 = regex.Replace(str2, "");
            textBox2.Text = str2;

            ////////////////////////////////////////////////////////////////
            ////
            ////////////////////////////////////////////////////////////////
            /// ADR
            ++res2;
            str2 = SaveEditorAsALibrary.System.lines[res2].ToString();
            regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");
            //textBox3.Text = str2;
            int val = Int32.Parse(str2);
            if(val < 0)
            {
                val = 0; 
            }
            else if(val > 63)
            {
                val = 63;
            }
            comboBox1.Text = val.ToString();

            // Long Distance
            ++res2;
            str2 = SaveEditorAsALibrary.System.lines[res2].ToString();
            regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");
            //textBox3.Text = str2;
            val = Int32.Parse(str2);
            if (val < 0)
            {
                val = 0;
            }
            else if (val > 6)
            {
                val = 6;
            }
            comboBox2.Text = val.ToString();

            // High Value
            ++res2;
            str2 = SaveEditorAsALibrary.System.lines[res2].ToString();
            regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");
            //textBox3.Text = str2;
            val = Int32.Parse(str2);
            if (val < 0)
            {
                val = 0;
            }
            else if (val > 6)
            {
                val = 6;
            }
            comboBox3.Text = val.ToString();

            // fragile
            ++res2;
            str2 = SaveEditorAsALibrary.System.lines[res2].ToString();
            regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");
            //textBox3.Text = str2;
            val = Int32.Parse(str2);
            if (val < 0)
            {
                val = 0;
            }
            else if (val > 6)
            {
                val = 6;
            }
            comboBox4.Text = val.ToString();

            // urgent
            ++res2;
            str2 = SaveEditorAsALibrary.System.lines[res2].ToString();
            regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");
            //textBox3.Text = str2;
            val = Int32.Parse(str2);
            if (val < 0)
            {
                val = 0;
            }
            else if (val > 6)
            {
                val = 6;
            }
            comboBox5.Text = val.ToString();

            // Eco
            ++res2;
            str2 = SaveEditorAsALibrary.System.lines[res2].ToString();
            regex2 = new Regex("[^0-9]");
            str2 = regex2.Replace(str2, "");
            //textBox3.Text = str2;
            val = Int32.Parse(str2);
            if (val < 0)
            {
                val = 0;
            }
            else if (val > 6)
            {
                val = 6;
            }
            comboBox6.Text = val.ToString();

            ContextMenu menu = new ContextMenu();
            MenuItem secondView = new MenuItem("OPEN IN SECOND VIEW");
            secondView.Click += SecondView_Click;
            menu.MenuItems.Add(secondView);

            ArrayList allCities = SaveEditorAsALibrary.Static.getAllCities();
            treeView1.Nodes.Add("CITIES (" + allCities.Count + ")");
            ArrayList visitedCities = SaveEditorAsALibrary.Static.getAllVisitedCities();
            treeView1.Nodes.Add("VISITED CITIES (" + visitedCities.Count + ")");

            foreach (string s in visitedCities)
            {
                //Console.WriteLine("CITY: " + s);
                treeView1.Nodes[1].Nodes.Add(s);
            }

            for(int i = 0; i < allCities.Count; ++i)
            //foreach (string s in allCities)
            {
                //Console.WriteLine("CITY: " + s);
                treeView1.Nodes[0].Nodes.Add(allCities[i].ToString());
                if(visitedCities.Contains(allCities[i]))
                {
                    treeView1.Nodes[0].Nodes[treeView1.Nodes[0].Nodes.Count - 1].ForeColor = Color.DarkGreen;
                }
                else
                {
                    treeView1.Nodes[0].Nodes[treeView1.Nodes[0].Nodes.Count - 1].ForeColor = Color.DarkRed;
                }
            }

            Dictionary<string, int> allGarages = SaveEditorAsALibrary.Static.getAllGarages();

            treeView1.Nodes.Add("GARAGES (" + allGarages.Keys.Count + ")");

            for(int i = 0; i < allGarages.Count; ++i)
            {
                //Console.WriteLine("CITY: " + s);
                treeView1.Nodes[2].Nodes.Add(allGarages.Keys.ElementAt(i));
                switch (allGarages.Values.ElementAt(i))
                {
                    case 0:
                        treeView1.Nodes[2].Nodes[treeView1.Nodes[2].Nodes.Count - 1].ForeColor = Color.DarkRed;
                        break;
                    case 1:
                        treeView1.Nodes[2].Nodes[treeView1.Nodes[2].Nodes.Count - 1].ForeColor = Color.Red;
                        break;
                    case 2:
                        treeView1.Nodes[2].Nodes[treeView1.Nodes[2].Nodes.Count - 1].ForeColor = Color.Orange;
                        break;
                    case 3:
                        treeView1.Nodes[2].Nodes[treeView1.Nodes[2].Nodes.Count - 1].ForeColor = Color.DarkGreen;
                        break;
                    default:
                        break;
                }
                
            }

            //foreach (string s in allGarages.Keys)
            //{
                //Console.WriteLine("CITY: " + s);
                //treeView1.Nodes[2].Nodes.Add(s);
                /*switch ((int)allGarages.Values)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break:
                    case 3:
                        break;
                    default:
                        break;
                }
                treeView1.Nodes[2].Nodes[treeView1.Nodes[2].Nodes.Count - 1].ForeColor = Color.DarkGreen;*/
            //}

            treeView1.Nodes[0].ContextMenu = menu;
            treeView1.Nodes[1].ContextMenu = menu;
            treeView1.Nodes[2].ContextMenu = menu;

            /*Console.WriteLine("VALUES:");
            Console.WriteLine(SaveEditorAsALibrary.Static.getExperience().ToString());
            Console.WriteLine(SaveEditorAsALibrary.Static.getMoney().ToString());
            Console.WriteLine(SaveEditorAsALibrary.Static.getADR().ToString());
            Console.WriteLine(SaveEditorAsALibrary.Static.getLongDistance().ToString());
            Console.WriteLine(SaveEditorAsALibrary.Static.getHighValue().ToString());
            Console.WriteLine(SaveEditorAsALibrary.Static.getFragile().ToString());
            Console.WriteLine(SaveEditorAsALibrary.Static.getUrgent().ToString());
            Console.WriteLine(SaveEditorAsALibrary.Static.getEco().ToString());*/

            //ArrayList list = SaveEditorAsALibrary.Static.getAllGarages();

            comboBox1.Resize += (sender, e) =>
            {
                if (!comboBox1.IsHandleCreated)
                    return;  // avoid possible exception

                comboBox1.BeginInvoke(new Action(() => comboBox1.SelectionLength = 0));
            };

            comboBox2.Resize += (sender, e) =>
            {
                if (!comboBox2.IsHandleCreated)
                    return;  // avoid possible exception

                comboBox2.BeginInvoke(new Action(() => comboBox2.SelectionLength = 0));
            };

            comboBox3.Resize += (sender, e) =>
            {
                if (!comboBox3.IsHandleCreated)
                    return;  // avoid possible exception

                comboBox3.BeginInvoke(new Action(() => comboBox3.SelectionLength = 0));
            };

            comboBox4.Resize += (sender, e) =>
            {
                if (!comboBox4.IsHandleCreated)
                    return;  // avoid possible exception

                comboBox4.BeginInvoke(new Action(() => comboBox4.SelectionLength = 0));
            };

            comboBox5.Resize += (sender, e) =>
            {
                if (!comboBox5.IsHandleCreated)
                    return;  // avoid possible exception

                comboBox5.BeginInvoke(new Action(() =>
                {
                    comboBox5.SelectionLength = 0;
                }));
            };

            comboBox6.Resize += (sender, e) =>
            {
                if (!comboBox6.IsHandleCreated)
                    return;  // avoid possible exception

                comboBox6.BeginInvoke(new Action(() => comboBox6.SelectionLength = 0));
            };

            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged += ComboBox2_SelectedIndexChanged;
            comboBox3.SelectedIndexChanged += ComboBox3_SelectedIndexChanged;
            comboBox4.SelectedIndexChanged += ComboBox4_SelectedIndexChanged;
            comboBox5.SelectedIndexChanged += ComboBox5_SelectedIndexChanged;
            comboBox6.SelectedIndexChanged += ComboBox6_SelectedIndexChanged;
            textBox1.TextChanged += TextBox1_TextChanged;
            textBox2.TextChanged += TextBox2_TextChanged;
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ComboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ComboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void SecondView_Click(object sender, EventArgs e)
        {
            treeView2.Nodes.Clear();
            TreeNode selectedNode = (TreeNode) treeView1.SelectedNode.Clone();
            treeView2.Nodes.Add(selectedNode);

            foreach(TreeNode node in treeView2.Nodes[0].Nodes)
            {
                node.ForeColor = Color.Black;
            }
        }

        private void unlock_all_cities_button_click(object sender, EventArgs e)
        {
            

        }

        private void unlock_all_garages_button_click(object sender, EventArgs e)
        {
            /*Dictionary<string, int> allGarages = SaveEditorAsALibrary.Static.getAllGarages();
            foreach (string s in allGarages.Keys)
            {
                Console.WriteLine("GARAGE: " + s);
            }

            Console.WriteLine("COUNT: " + allGarages.Keys.Count);*/
        }
    }
}
