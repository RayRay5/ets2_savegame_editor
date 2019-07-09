using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/**
 * SaveEditor Version V2
 * By https://github.com/RayRay5
 * Licensed under GNU General Public License v3.0
 */

namespace SaveEditorV2
{
    public partial class MainForm : Form
    {
        // TODO (fast DONE) Makro Funktion schreiben
        OpenFileDialog ofd = new OpenFileDialog();
        SaveFileDialog sfd = new SaveFileDialog();
        SaveFileDialog bfd = new SaveFileDialog();
        OpenFileDialog afd = new OpenFileDialog();

        public MainForm()
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Form] Main Form init started" });
            MinimumSize = new Size(530, 334);
            //MinimumSize = new Size(530, 285);
            MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Right, 334);

            if(SaveEditorAsALibrary.System.checkNETVersion() == false)
            {
                MessageBox.Show("Unsupported .NET Framework Version detected");
                return;
            }
            
            InitializeComponent();
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Form] Main Form init finished" });
        }

        private void UnitEditorButton_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Start Unit Editor" });
            UnitEditor unit = new UnitEditor();
            unit.Size = new Size(800, 600);
            unit.Show();
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Unit Editor start finished" });
        }

        private void LoadSavegameButton_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Load Savegame Path" });
            ofd.Filter = "ETS2 /ATS Savegames (*.sii)|*.sii";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //dir = Directory.GetCurrentDirectory();
                LoadSavegamePathBox.Text = ofd.FileName;
                SaveSavegameButton.Enabled = true;
                BackUpButton.Enabled = true;
                //saveOldFile(ofd.FileName);
            }
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Load Savegame Path finished" });
        }

        private void BackUpButton_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Backup Path" });
            bfd.Filter = "ETS2 / ATS Savegames (*.sii)|*.sii";
            if (bfd.ShowDialog() == DialogResult.OK)
            {
                //dir = Directory.GetCurrentDirectory();
                BackupPathBox.Text = bfd.FileName;
                //saveOldFile(ofd.FileName);
            }
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Backup Path Button finished" });
        }

        private void SaveSavegameButton_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Save Savegame Path" });
            sfd.Filter = "ETS2 / ATS Savegames (*.sii)|*.sii";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                //dir = Directory.GetCurrentDirectory();
                SaveSavegamePathBox.Text = sfd.FileName;
                //saveOldFile(ofd.FileName);
                AnalyzeSavegameButton.Enabled = true;
                ScriptPathBox.Enabled = true;
                //VariableBox.Enabled = true;       // TODO
                openScriptFileButton.Enabled = true;
                //loadScriptButton.Enabled = true;
                //CommonUtils.Enabled = true;
            }
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Save Savegame Path finsied" });
        }

        private void openScriptFileButton_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Automation Script Path" });
            afd.Filter = "SaveEditor Automation Language Files (*.txt)|*.txt";
            if (afd.ShowDialog() == DialogResult.OK)
            {
                ScriptPathBox.Text = afd.FileName;
                loadScriptButton.Enabled = true;
            }
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Automation Script Path finished" });
        }

        private void AnalyzeSavegameButton_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Analyze Savegame" });

            try
            {
                SaveEditorAsALibrary.System.resetSystem();
            }
            catch(Exception)
            {
                // This one may throw an exception if the reset function is triggered on the first run of this function in a "session"
                // Don't worry, that is correct
            }

            MessageBox.Show("Editing a savegame while on an active World of Trucks Contract is not advised!" + 
                Environment.NewLine + "Editing a savegame before finishing the first mission may also cause the game to behave weird and crash randomly." + 
                Environment.NewLine + "", "NOTICE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            try
            {
                //SaveEditorAsALibrary.Input.path = @"C:\users\rapha\desktop\game.sii";
                SaveEditorAsALibrary.Input.path = LoadSavegamePathBox.Text;
                SaveEditorAsALibrary.Input.readSavegame();
                if (SaveEditorAsALibrary.System.checkDecryptState() == false)
                {
                    MessageBox.Show("Savegame is not decrypted. Aborting!", "Encryption detected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                SaveEditorAsALibrary.System.prepareUnits();
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show("Savegame loaded successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }));

                UnitEditorButton.Enabled = true;
                SaveSavegameWriteBackButton.Enabled = true;
                CommonUtils.Enabled = true;
            }
            catch(Exception)
            {
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show("Savegame loading failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }

            if(BackupPathBox.Text.Length > 0)
            {
                File.WriteAllLines(BackupPathBox.Text.ToString(), SaveEditorAsALibrary.System.lines);
            }
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Analyze Savegame finished" });
        }

        private void SaveSavegameWriteBackButton_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Write Back" });
            ArrayList writeBackLines = new ArrayList();
            writeBackLines.Add("SiiNunit");
            writeBackLines.Add("{");

            foreach (ArrayList al in SaveEditorAsALibrary.System.unitList)
            {
                foreach(string s in al)
                {
                    writeBackLines.Add(s);
                }
                writeBackLines.Add("");
            }

            writeBackLines.Add("}");

            foreach (string s in writeBackLines)
            {
                //Console.WriteLine(s);
            }

            string[] writeBackLinesArray = (string[])writeBackLines.ToArray(typeof(string));

            string savepath = SaveSavegamePathBox.Text.ToString();

            File.WriteAllLines(SaveSavegamePathBox.Text.ToString(), writeBackLinesArray);

            LoadSavegamePathBox.Text = "";
            BackupPathBox.Text = "";
            SaveSavegamePathBox.Text = "";
            SaveEditorAsALibrary.System.resetSystem();

            BackUpButton.Enabled = false;
            SaveSavegameButton.Enabled = false;
            AnalyzeSavegameButton.Enabled = false;
            SaveSavegameWriteBackButton.Enabled = false;
            UnitEditorButton.Enabled = false;

            this.Invoke(new Action(() => MessageBox.Show("Saving Savegame successful!", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information)));
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Write Back finished" });
        }

        private void loadScriptButton_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Run Script" });
            //SaveEditorAsALibrary.Input.path = @"C:\users\rapha\desktop\mmmmm.sii";
            SaveEditorAsALibrary.Input.path = LoadSavegamePathBox.Text;
            SaveEditorAsALibrary.Input.readSavegame();
            SaveEditorAsALibrary.System.prepareUnits();

            //string path = @"C:\users\rapha\desktop\money_change.txt";
            string path = ScriptPathBox.Text;
            string[] content = File.ReadAllLines(path);
            Console.WriteLine("content_script: ");
            foreach(string s in content)
            {
                Console.WriteLine(s);
            }
            /*
            dynamic context = null;
            int selectedUnit = 0;
            int selectedAttr = 0;
            int selectedLine = 0;

            for (int i = 0; i < content.Length; ++i)
            {
                if (content[i].StartsWith(@"//"))
                {
                    continue;
                }
                
                switch(content[i])
                {
                    case "findUnit":
                        Console.WriteLine(content[++i]);
                        ArrayList x = SaveEditorAsALibrary.System.findTerm(content[i]);
                        Console.WriteLine(content[i]);
                        foreach(var n in x)
                        {
                            Console.WriteLine("vv: " + n);
                        }

                        dynamic nn = new ArrayList();
                        nn.Add(x);
                        context = nn;

                        selectedUnit = (int) x[0];
                        selectedAttr = 0;

                        //selectedLine = 0;
                        /*for(int a = 0; a < selectedUnit; ++a)
                        {
                            ArrayList aList = (ArrayList) SaveEditorAsALibrary.System.unitList[a];
                            foreach(string aa in aList)
                            {
                                ++selectedLine;
                            }
                        }
                        Console.WriteLine("LINE: " + selectedLine);//
                        break;
                    case "findAttr":
                        int contentIndex = ++i;

                        ArrayList l = (ArrayList) context;
                        //int res1 = (int) context[0];

                        for(int iiii = 0; iiii < l.Count; ++iiii)
                        {
                            ArrayList attrL = (ArrayList)SaveEditorAsALibrary.System.unitList[selectedUnit];

                            for (int j = 0; j < attrL.Count; ++j)
                            {
                                //Console.WriteLine("connnn; " + content[contentIndex]);
                                if(attrL[j].ToString().Contains(content[contentIndex]))
                                {
                                    Console.WriteLine("ATTR FOUND at " + contentIndex + " with value " + attrL[j] + " indexed at " + j);
                                    
                                    /*dynamic cc = new ArrayList();
                                    cc.Add(selectedUnit);
                                    cc.Add(j);//
                                    selectedAttr = j;
                                }
                            }
                        }
                        break;
                    case "goto":
                        /*dynamic query = context;
                        ArrayList gotoList = (ArrayList) query;
                        int i1 = (int)gotoList[0];
                        int i2 = (int)gotoList[1];
                        selectedUnit = i1;
                        selectedAttr = i2;//
                        Console.WriteLine("I1: " + selectedUnit + " I2: " + selectedAttr);
                        break;
                    case "jumpUnit":
                        int jumperU = Int32.Parse(content[++i]);
                        selectedUnit += jumperU;
                        break;
                    case "jumpAttr":
                        int jumperA = Int32.Parse(content[++i]);
                        selectedAttr += jumperA;
                        break;
                    case "addUnit":
                        SaveEditorAsALibrary.System.addUnit(new ArrayList() { "newunit : _nameless.123.4567.890a {", "entry: SAL_automated_value", "}" });
                        break;
                    case "addAttr":
                        ArrayList LList = (ArrayList) SaveEditorAsALibrary.System.unitList[selectedUnit];
                        int attrCounter = LList.Count - 1;
                        SaveEditorAsALibrary.System.insertAttribute(selectedUnit, "value: newer", attrCounter);
                        break;
                    case "change":
                        SaveEditorAsALibrary.System.changeAttribute(selectedUnit, selectedAttr, content[++i]);
                        break;
                    case "save":
                        /*foreach(string s in SaveEditorAsALibrary.System.lines)
                        {
                            SaveEditorAsALibrary.Output.debugOutput(s);
                        }//

                        ////////
                        ArrayList writeBackLines = new ArrayList();
                        writeBackLines.Add("SiiNunit");
                        writeBackLines.Add("{");

                        foreach (ArrayList al in SaveEditorAsALibrary.System.unitList)
                        {
                            foreach (string s in al)
                            {
                                writeBackLines.Add(s);
                            }
                            writeBackLines.Add("");
                        }

                        writeBackLines.Add("}");

                        foreach (string s in writeBackLines)
                        {
                            //Console.WriteLine(s);
                        }

                        string[] writeBackLinesArray = (string[])writeBackLines.ToArray(typeof(string));

                        //string savepath = SaveSavegamePathBox.Text.ToString();
                        string savepath = @"C:\users\rapha\desktop\eddddd.sii";

                        File.WriteAllLines(savepath, writeBackLinesArray);
                        ///////
                        break;
                    case "selectUnit":
                        selectedUnit = Int32.Parse(content[++i]);
                        break;
                    case "selectAttr":
                        selectedAttr = Int32.Parse(content[++i]);
                        break;
                    default:
                        break;

                }
            }*/
            //SaveEditorScriptingLanguage.Handler.handle(content, SaveSavegamePathBox.Text.ToString());
            //DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Script finished" });
        }

        private void CommonUtils_Click(object sender, EventArgs e)
        {
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Start Common Utils" });
            //UnitEditor unit = new UnitEditor();
            //unit.Size = new Size(800, 600);
            //unit.Show();
            CommonUtilities utils = new CommonUtilities();
            utils.Size = new Size(800, 600);
            utils.Show();
            DebugHandling.logGUIStroke(new string[] { "[GUI] [Button] Common Utils start finished" });
        }

        private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("GNU General Public License v3.0 from June 29th, 2007." + Environment.NewLine + Environment.NewLine +
                "You can access the full license at" + Environment.NewLine +
                "https://raw.githubusercontent.com/RayRay5/ets2_savegame_editor/master/LICENSE", "License", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void versionInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Tool Version: " + Program.__version + Environment.NewLine + Environment.NewLine
                + "Library Version: " + SaveEditorAsALibrary.System.__version + Environment.NewLine + Environment.NewLine;
                /*+ "Scripting Version: " + SaveEditorScriptingLanguage.System.__version + " | feature version: " + SaveEditorScriptingLanguage.System.__language_version;*/
            MessageBox.Show(message, "Version Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Initial Release of Version 2. There are lots of new fatures and bugfixes", "Changelog", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void aboutThisToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ETS2 Savegame Editor by https://github.com/RayRay5", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void systemRequirementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Required: .NET Framework 4.7.1" + Environment.NewLine +
                "OS: Windows 10" + Environment.NewLine +
                "No other special requirements";
            MessageBox.Show(message, "System Requirements", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void thanksALotToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Thank you and everyone else for using this tool." + Environment.NewLine +
                "Also a big thank you to all testers" + Environment.NewLine +
                "And a special 'Thank you' to:" + Environment.NewLine +
                "W4chund for TESTING and FINDING a truck load of BUGS and for VERIFYING the function of EDITS created with this tool";
            MessageBox.Show(message, "THANK YOU!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    public partial class DebugHandling
    {
        public static readonly string logFilePath = Directory.GetCurrentDirectory() + @"\saveeditor_log.txt";

        public DebugHandling()
        {

        }

        public static void logGUIStroke(string[] action)
        {
            if (Program.debug == true)
            {
                File.AppendAllLines(logFilePath, action);
            }
        }
    }
}
