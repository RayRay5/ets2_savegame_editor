using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaveEditorV2
{
    public partial class UnitEditor : Form
    {
        ContextMenu menu = new ContextMenu();
        ContextMenu unitMenu = new ContextMenu();
        static ArrayList resultsList;
        static int nextResult = 0;

        public UnitEditor()
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Form] Unit Editor init started" });
            menu.MenuItems.Add(Language.EditAttribute);
            menu.MenuItems[0].Click += UnitEditorEdit_Click;
            menu.MenuItems.Add(Language.DeleteAttribute);
            menu.MenuItems[1].Click += UnitEditorAttrDelete_Click;

            unitMenu.MenuItems.Add(Language.EditUnitHeader);
            unitMenu.MenuItems[0].Click += UnitEditorEdit_Click;
            unitMenu.MenuItems.Add(Language.DeleteUnit);
            unitMenu.MenuItems[1].Click += UnitEditorDelete_Click;
            unitMenu.MenuItems.Add(Language.AddAttribute);
            unitMenu.MenuItems[2].Click += AttributeAdd_Click;

            InitializeComponent();
            treeView1.LabelEdit = true;

            KeyPreview = true;
            KeyDown += new KeyEventHandler(MakroKeyEvent_Click);
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Form] Unit Editor init finished" });
        }

        private void AttributeAdd_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Add Attribute" });
            //SaveEditorAsALibrary.System.addUnit(new ArrayList() { "newunit : _nameless.123.4567.890a {", "entry: value", "}" });
            int[] indexes = getID(treeView1.SelectedNode);
            SaveEditorAsALibrary.System.addAttribute(indexes[0], "value: new");
            treeView1.Nodes.Clear();
            loadUnitTree();
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Add Attribute finished" });
        }

        private void MakroKeyEvent_Click(object sender, KeyEventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Hotkey] Hotkey use started" });
            if (treeView1.SelectedNode != null)
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        if(treeView1.SelectedNode.Nodes.Count > 0)
                        {
                            UnitEditorDelete_Click(sender, e);
                        }
                        else
                        {
                            UnitEditorAttrDelete_Click(sender, e);
                        }
                        //MessageBox.Show("DELETE");
                        break;
                    case Keys.F2:
                        UnitEditorEdit_Click(sender, e);
                        //MessageBox.Show("F2 EDIT");
                        break;
                    case Keys.Insert:
                        break;
                }
            }
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Hotkey] Hotkey use finished" });
        }

        private void UnitEditorDelete_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Unit Editor Unit Delete started" });
            int[] indexes = getID(treeView1.SelectedNode);
            SaveEditorAsALibrary.System.removeUnit(indexes[0]);
            //treeView1.SelectedNode.Remove();
            treeView1.Nodes.Clear();
            loadUnitTree();
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Unit Editor Unit Delete finished" });
        }

        private void UnitEditorAttrDelete_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Unit Editor Attribute Delete finished" });
            int[] indexes = getID(treeView1.SelectedNode);
            SaveEditorAsALibrary.System.removeAttribute(indexes[0], indexes[1]);
            //treeView1.SelectedNode.Remove();
            treeView1.Nodes.Clear();
            loadUnitTree();
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Unit Editor Attribute Delete finished" });
        }

        private void UnitEditorEdit_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Unit Editor Edit finished" });
            //Console.WriteLine("HI");
            treeView1.SelectedNode.BeginEdit();
            treeView1.AfterLabelEdit -= TreeView1_AfterLabelEdit;
            treeView1.AfterLabelEdit += TreeView1_AfterLabelEdit;
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Unit Editor Edit finished" });
        }

        private void TreeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            this.BeginInvoke(new Action(() => editNode(e.Node)));
        }

        private int[] getID(TreeNode node)
        {
            int[] indexes = new int[2];
            int[] tagArray = (int[])node.Tag;

            for (int i = 0; i < 2; ++i)
            {
                indexes[i] = tagArray[i];
            }

            foreach (int inn in indexes)
            {
                Console.WriteLine("INN: " + inn);
            }

            Console.WriteLine("----------");

            return indexes;
        }

        private void editNode(TreeNode node)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Edit Node started" });
            int[] indexes = getID(node);

            SaveEditorAsALibrary.System.changeAttribute(indexes[0], indexes[1], node.Text);
            SaveEditorAsALibrary.Output.listAttributes((ArrayList) SaveEditorAsALibrary.System.unitList[indexes[0]], true);
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Edit Node finished" });
        }

        private void loadUnitTree()
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Load Unit Tree started" });
            int i = 0;

            foreach (ArrayList unitList in SaveEditorAsALibrary.System.unitList)
            {
                //treeView1.Nodes.Add("ROOT");
                treeView1.Nodes.Add(unitList[0].ToString());
                treeView1.Nodes[treeView1.Nodes.Count - 1].ContextMenu = menu;
                treeView1.Nodes[treeView1.Nodes.Count - 1].Tag = new int[] { treeView1.Nodes.Count - 1, 0 };

                for (int j = 1; j < unitList.Count; ++j)
                {
                    treeView1.Nodes[i].Nodes.Add(unitList[j].ToString());
                    treeView1.Nodes[i].Nodes[treeView1.Nodes[i].Nodes.Count - 1].ContextMenu = menu;

                    treeView1.Nodes[i].Nodes[treeView1.Nodes[i].Nodes.Count - 1].Tag = new int[] { treeView1.Nodes.Count - 1, j };

                    /*string content = "";
                    foreach(int tag in (int[]) treeView1.Nodes[i].Tag)
                    {
                        content += tag.ToString() + " | ";
                    }
                    treeView1.Nodes[i].Nodes.Add(content);*/
                }
                /*foreach (string attribute in unitList)
                {
                    
                }*/
                ++i;
                AddUnitButton.Enabled = true;
                seachTextBox.Enabled = true;
                searchButton.Enabled = true;
            }

            //foreach (TreeNode node in treeView1.Nodes)

            for (int k = 0; k < treeView1.Nodes.Count; ++k)
            {
                if(treeView1.Nodes[k].Nodes.Count > 0)
                {
                    treeView1.Nodes[k].ContextMenu = unitMenu;
                    //menu.MenuItems.Add("ADD ATTR");
                }
            }

            //InitializeComponent();
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Load Unit Tree finished" });
        }

        private void LoadTreeView_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Load Tree View" });
            treeView1.Nodes.Clear();
            loadUnitTree();
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Load Tree View finished" });
        }

        private void AddUnitButton_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Add Unit started" });
            SaveEditorAsALibrary.System.addUnit(new ArrayList() { "newunit : _nameless.123.4567.890a {", "entry: value", "}" });
            treeView1.Nodes.Clear();
            loadUnitTree();
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Add Unit finished" });
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Search started" });
            resultsList = new ArrayList();
            nextResult = 0;
            resultsList = SaveEditorAsALibrary.System.findTerm(seachTextBox.Text);

            /*foreach(string x in xx)
            {
                Console.Write(count + " | ");
                Console.WriteLine(x);
                ++count;
            }*/

            jumpNextResultButton.Enabled = true;
            jumpPreviousResultButton.Enabled = true;
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Search finished" });
        }

        private void jumpNextResultButton_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Jump Next Result started" });
            if (nextResult >= resultsList.Count)
            {
                nextResult = 0;
            }
            treeView1.SelectedNode = treeView1.Nodes[(int) resultsList[nextResult]];
            treeView1.Focus();
            ++nextResult;
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Jump Next Result finished" });
        }

        private void jumpPreviousResultButton_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Jump Previous Result started" });
            if (nextResult < 0)
            {
                nextResult = resultsList.Count - 1;
            }
            treeView1.SelectedNode = treeView1.Nodes[(int)resultsList[nextResult]];
            treeView1.Focus();
            --nextResult;
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Jump Next Result finished" });
        }
    }
}
