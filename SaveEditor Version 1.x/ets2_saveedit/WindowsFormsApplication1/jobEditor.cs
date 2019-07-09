using saveeditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ets2_saveeditor
{
    public partial class jobEditor : Form
    {
        public string[] moreLines;
        System.Timers.Timer progressBarUpdateTimer2;
        static int progress = 0;

        public static int daeumchenDrehen = 0;
        public static int dual = 0;
        public static int threadcount = 0;

        Thread thread;

        Dictionary<string, string> companyOffering = new Dictionary<string, string>();
        ArrayList offers = new ArrayList();
        ArrayList jobData = new ArrayList();
        public static ArrayList jobSource = new ArrayList();
        ArrayList sourceCompaniesLines = new ArrayList();

        public static ArrayList cargo = new ArrayList();
        public static ArrayList truck = new ArrayList();
        public static ArrayList variant = new ArrayList();
        public static ArrayList jobDestination = new ArrayList();
        public static ArrayList destinationCompany = new ArrayList();
        public static ArrayList expiration = new ArrayList();
        public static ArrayList urgency = new ArrayList();
        public static ArrayList shortestDistance = new ArrayList();
        public static ArrayList ferryTime = new ArrayList();
        public static ArrayList ferryPrice = new ArrayList();
        public static ArrayList trailerPlace = new ArrayList();

        public static int selectedItem = -1;

        ContextMenuStrip viewJobDataContext = new ContextMenuStrip();
        ToolStripMenuItem viewJobData = new ToolStripMenuItem("View Job Data");

        ContextMenuStrip edit = new ContextMenuStrip();
        ToolStripMenuItem editData = new ToolStripMenuItem("Change this value");

        private void splitJobData(string data)
        {
            //MessageBox.Show("data: " + data);
            string[] splittedData = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for(int i = 0; i < splittedData.Length; ++i)
            {
                switch(i)
                {
                    case 0:
                        cargo.Add(splittedData[i]);
                        break;
                    case 1:
                        truck.Add(splittedData[i]);
                        break;
                    case 2:
                        variant.Add(splittedData[i]);
                        break;
                    case 3:
                        jobDestination.Add(splittedData[i]);
                        break;
                    case 4:
                        expiration.Add(splittedData[i]);
                        break;
                    case 5:
                        urgency.Add(splittedData[i]);
                        break;
                    case 6:
                        shortestDistance.Add(splittedData[i]);
                        break;
                    case 7:
                        ferryTime.Add(splittedData[i]);
                        break;
                    case 8:
                        ferryPrice.Add(splittedData[i]);
                        break;
                    case 9:
                        trailerPlace.Add(splittedData[i]);
                        break;
                    default:
                        break;

                }
            }
        }

        private string[] collectRawValues(string element)
        {
            int index = -1;
            for(int i = 0; i < offers.Count; ++i)
            {
                if(offers[i].ToString().Contains(element))
                {
                    index = i;
                }
            }
            string[] data = new string[] {
                jobSource[index].ToString(),
                cargo[index].ToString(),
                truck[index].ToString(),
                variant[index].ToString(),
                jobDestination[index].ToString(),
                //destinationCity[index].ToString(),
                expiration[index].ToString(),
                urgency[index].ToString(),
                shortestDistance[index].ToString(),
                ferryTime[index].ToString(),
                ferryPrice[index].ToString(),
                trailerPlace[index].ToString()
            };
            return data;
        }

        private void editRawValue()
        {
            string text = "";
            for(int i = 0; i < treeView2.SelectedNode.Nodes.Count; ++i)
            {
                text += treeView2.SelectedNode.Nodes[i].Text + Environment.NewLine;
            }

            string rawValue = treeView2.SelectedNode.LastNode.Nodes[0].Text;
            string parent = treeView2.SelectedNode.Parent.Text;
            editValue editValue = new editValue(parent, rawValue, treeView2.SelectedNode.Text);
            editValue.Show();
        }

        private void edit_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("click");
            string nodeText = treeView2.SelectedNode.Text;
            switch(nodeText)
            {
                case "Source":
                    editRawValue();
                    break;
                case "Cargo":
                    editRawValue();
                    break;
                case "Destination":
                    editRawValue();
                    break;
                case "Urgency":
                    editRawValue();
                    break;
                case "Truck":
                    editRawValue();
                    break;
                case "Variant":
                    editRawValue();
                    break;
                case "Expiration Time":
                    editRawValue();
                    break;
                case "Shortest Distance":
                    editRawValue();
                    break;
                case "Ferry Time":
                    editRawValue();
                    break;
                case "Ferry Price":
                    editRawValue();
                    break;
                case "Trailer Place":
                    editRawValue();
                    break;
                default:
                    MessageBox.Show("Unknown Item");
                    break;
            }
        }

        public static void updateTreeView(string nodeText, int index, string property, string newValue)
        {
            //MessageBox.Show("property: " + property);
            for(int i = 0; i < treeView2.Nodes.Count; ++i)
            {
                //MessageBox.Show(treeView2.Nodes[i].Nodes[j].Text);
                if(treeView2.Nodes[i].Text.Contains(nodeText))
                {
                    //treeView2.Nodes[i].Nodes[j]
                    //MessageBox.Show("contains:" + treeView2.Nodes[i].Text);
                    
                    for (int j = 0; j < treeView2.Nodes[i].Nodes.Count; j++)
                    {
                        //MessageBox.Show("text: " + treeView2.Nodes[i].Nodes[j].Text);
                        if (treeView2.Nodes[i].Nodes[j].Text.Contains("Source") && property.Equals("Source"))
                        {
                            //MessageBox.Show("Source found");
                            //jobSource[index] = newValue;
                            string[] splitted = newValue.Split('.');
                            for (int k = 0; k < splitted.Length; ++k)
                            {
                                splitted[k] = splitted[k].Replace(".", "");
                            }
                            treeView2.Nodes[i].Nodes[j].Nodes[0].Text = splitted[0];
                            treeView2.Nodes[i].Nodes[j].Nodes[1].Text = splitted[1];
                            treeView2.Nodes[i].Nodes[j].LastNode.LastNode.Text = jobSource[index].ToString();
                            button3.Enabled = true;
                            break;
                        }
                        else if (treeView2.Nodes[i].Nodes[j].Text.Contains("Destination") && property.Equals("Destination"))
                        {
                            //MessageBox.Show("Dest found");
                            //jobDestination[index] = newValue;
                            string[] splitted = newValue.Split('.');
                            for (int k = 0; k < splitted.Length; ++k)
                            {
                                splitted[k] = Regex.Replace(splitted[k], "target: ", "");
                                splitted[k] = splitted[k].Replace(".", "");
                                splitted[k] = splitted[k].Replace(":", "");
                                splitted[k] = splitted[k].Replace(" ", "");
                                splitted[k] = splitted[k].Replace("\"", "");

                            }
                            treeView2.Nodes[i].Nodes[j].Nodes[0].Text = splitted[0];
                            treeView2.Nodes[i].Nodes[j].Nodes[1].Text = splitted[1];
                            treeView2.Nodes[i].Nodes[j].LastNode.LastNode.Text = jobDestination[index].ToString();
                            button3.Enabled = true;
                            break;
                        }
                        else if(treeView2.Nodes[i].Nodes[j].Text.Contains("Truck") && property.Equals("Truck"))
                        {
                            //MessageBox.Show("Truck found");
                            string displayValue = Regex.Replace(newValue, "company_truck: ", "");
                            displayValue = displayValue.Replace(" ", "");
                            treeView2.Nodes[i].Nodes[j].Nodes[0].Text = displayValue;
                            treeView2.Nodes[i].Nodes[j].LastNode.LastNode.Text = newValue;
                            button3.Enabled = true;
                            break;
                        }
                        else if(treeView2.Nodes[i].Nodes[j].Text.Contains("Cargo") && property.Equals("Cargo"))
                        {
                            //MessageBox.Show("Cargo found");
                            treeView2.Nodes[i].Nodes[j].Nodes[0].Text = Regex.Replace(newValue, "company_truck: ", "");
                            treeView2.Nodes[i].Nodes[j].LastNode.LastNode.Text = newValue;
                            button3.Enabled = true;
                            break;
                        }
                        else if (treeView2.Nodes[i].Nodes[j].Text.Contains("Urgency") && property.Equals("Urgency"))
                        {
                            //MessageBox.Show("Urgency found");
                            treeView2.Nodes[i].Nodes[j].Nodes[0].Text = Regex.Replace(newValue, "cargo: cargo.", "");
                            treeView2.Nodes[i].Nodes[j].LastNode.LastNode.Text = newValue;
                            button3.Enabled = true;
                            break;
                        }
                        else if (treeView2.Nodes[i].Nodes[j].Text.Contains("Variant") && property.Equals("Variant"))
                        {
                            //MessageBox.Show("Trailer Variant found");
                            treeView2.Nodes[i].Nodes[j].Nodes[0].Text = Regex.Replace(newValue, "variant: ", "");
                            treeView2.Nodes[i].Nodes[j].LastNode.LastNode.Text = newValue;
                            button3.Enabled = true;
                            break;
                        }
                        else if (treeView2.Nodes[i].Nodes[j].Text.Contains("Expiration Time") && property.Equals("Expiration Time"))
                        {
                            //MessageBox.Show("Expiration Time found");
                            treeView2.Nodes[i].Nodes[j].Nodes[0].Text = Regex.Replace(newValue, "expiration_time: ", "");
                            treeView2.Nodes[i].Nodes[j].LastNode.LastNode.Text = newValue;
                            button3.Enabled = true;
                            break;
                        }
                        else if (treeView2.Nodes[i].Nodes[j].Text.Contains("Shortest Distance") && property.Equals("Shortest Distance"))
                        {
                            //MessageBox.Show("Shortest Distance found");
                            treeView2.Nodes[i].Nodes[j].Nodes[0].Text = Regex.Replace(newValue, "shortest_distance_km: ", "");
                            treeView2.Nodes[i].Nodes[j].LastNode.LastNode.Text = newValue;
                            button3.Enabled = true;
                            break;
                        }
                        else if (treeView2.Nodes[i].Nodes[j].Text.Contains("Ferry Time") && property.Equals("Ferry Time"))
                        {
                            //MessageBox.Show("Ferry Time found");
                            treeView2.Nodes[i].Nodes[j].Nodes[0].Text = Regex.Replace(newValue, "ferry_time: ", "");
                            treeView2.Nodes[i].Nodes[j].LastNode.LastNode.Text = newValue;
                            button3.Enabled = true;
                            break;
                        }
                        else if (treeView2.Nodes[i].Nodes[j].Text.Contains("Ferry Price") && property.Equals("Ferry Price"))
                        {
                            //MessageBox.Show("Ferry Price found");
                            treeView2.Nodes[i].Nodes[j].Nodes[0].Text = Regex.Replace(newValue, "ferry_price: ", "");
                            treeView2.Nodes[i].Nodes[j].LastNode.LastNode.Text = newValue;
                            button3.Enabled = true;
                            break;
                        }
                        else if (treeView2.Nodes[i].Nodes[j].Text.Contains("Trailer Place") && property.Equals("Trailer Place"))
                        {
                            //MessageBox.Show("Trailer Place found");
                            treeView2.Nodes[i].Nodes[j].Nodes[0].Text = Regex.Replace(newValue, "trailer_place: ", "");
                            treeView2.Nodes[i].Nodes[j].LastNode.LastNode.Text = newValue;
                            button3.Enabled = true;
                            break;
                        }
                    }
                }
            }

            /*int ii = 0;
            foreach (var v in jobSource)
            {
                //Console.WriteLine("Eelement " + ii + ": " + v);
                ii++;
            }*/
        }

        private void viewJobData_Click(object sender, EventArgs e)
        {
            treeView2.Nodes.Clear();
            int index = -1;
            //MessageBox.Show(sender.ToString());
            treeView2.Nodes.Add(treeView1.SelectedNode.Text);
            for (int i = 0; i < offers.Count; ++i)
            {
                if (offers[i].ToString().Contains(treeView1.SelectedNode.Text))
                {
                    index = i;
                    selectedItem = i;
                    break;
                }
            }
            //int index = treeView1.SelectedNode.Index;

            //readJob(treeView1.SelectedNode.Text);

            string data = jobData[index].ToString();
            //string splittedData = Regex.Split(data, " [a-zA-Z0-9_]*[:][ ][a-zA-Z0-9]*);
            string[] splittedData = Regex.Split(data, Environment.NewLine);

            string source = jobSource[index].ToString();
            string[] sourceSplit = source.Split('.'); ;

            treeView2.Nodes[0].Nodes.Add("Source");
            //treeView2.Nodes[0].Nodes.Add("Source Company");
            treeView2.Nodes[0].Nodes.Add("Cargo");
            treeView2.Nodes[0].Nodes.Add("Truck");
            treeView2.Nodes[0].Nodes.Add("Variant");
            treeView2.Nodes[0].Nodes.Add("Destination");
            //treeView2.Nodes[0].Nodes.Add("Destination Company");
            treeView2.Nodes[0].Nodes.Add("Expiration Time");
            treeView2.Nodes[0].Nodes.Add("Urgency");
            treeView2.Nodes[0].Nodes.Add("Shortest Distance");
            treeView2.Nodes[0].Nodes.Add("Ferry Time");
            treeView2.Nodes[0].Nodes.Add("Ferry Price");
            treeView2.Nodes[0].Nodes.Add("Trailer Place");

            treeView2.Nodes[0].Nodes[0].Nodes.Add(sourceSplit[sourceSplit.Length - 1]);
            treeView2.Nodes[0].Nodes[0].Nodes.Add(sourceSplit[0]);

            int offset = 1;
            for (int i = 0; i < splittedData.Length - 1; ++i)
            {
                splittedData[i] = Regex.Replace(splittedData[i], "[a-zA-Z0-9_]*[:]", "");
                //splittedData[i] = Regex.Replace(splittedData[i], "[a-zA-Z0-9_]*[.]", "");
                splittedData[i] = Regex.Replace(splittedData[i], " ", "");
                if (splittedData[i].Contains("cargo"))
                {
                    splittedData[i] = splittedData[i].Replace("cargo.", "");
                    treeView2.Nodes[0].Nodes[i + offset].Nodes.Add(splittedData[i]);
                }
                else
                {
                    if (Regex.IsMatch(splittedData[i], "[a-zA-Z0-9_][.]"))
                    {
                        splittedData[i] = Regex.Replace(splittedData[i], "\"", "");
                        string destination = "";
                        destination = Regex.Replace(splittedData[i], "[a-zA-Z0-9_]*[.]", "");
                        treeView2.Nodes[0].Nodes[i + offset].Nodes.Add(destination);
                        //offset++;
                        destination = Regex.Replace(splittedData[i], "[.][a-zA-Z0-9_]*", "");
                        treeView2.Nodes[0].Nodes[i + offset].Nodes.Add(destination);
                    }
                    else
                    {
                        treeView2.Nodes[0].Nodes[i + offset].Nodes.Add(splittedData[i]);
                    }
                }
            }
            foreach(TreeNode n in treeView2.Nodes[0].Nodes)
            {
                n.Nodes.Add("Raw Value");
                n.ContextMenuStrip = edit;
            }

            treeView2.Nodes[0].ExpandAll();

            for (int i = 0; i < treeView2.Nodes[0].Nodes.Count; ++i)
            {
                for (int j = treeView2.Nodes[0].Nodes[i].Nodes.Count - 1; j < treeView2.Nodes[0].Nodes[i].Nodes.Count; ++j)
                {
                    treeView2.Nodes[0].Nodes[i].Nodes[j].Nodes.Add(collectRawValues(treeView2.Nodes[0].Text)[i]);
                    //MessageBox.Show(collectRawValues(treeView2.Nodes[0].Text)[0]);
                }
            }
            
            

            //MessageBox.Show("Index: " + index);
            //string data = jobData.Contains(tr);
        }

        public jobEditor()
        {
            InitializeComponent();
            viewJobDataContext.Items.AddRange(new ToolStripMenuItem[]
            {
                viewJobData
            });
            viewJobDataContext.Click += new System.EventHandler(this.viewJobData_Click);

            edit.Items.AddRange(new ToolStripMenuItem[]
            {
                editData
            });
                
            edit.Click += new System.EventHandler(this.edit_Click);
            //viewJobDataContext.Click += new System.Windows.Forms.ToolStripItemClickedEventArgs(this.viewJobData_Click);
        }

        private void readJob(string element)
        {
            threadcount++;
            int count = 0;
            string data = "";
            string[] linesdata = ets2_saveeditor_main_form.lines;
            //MessageBox.Show(element);
            for (int j = 0; j < linesdata.Length; ++j)
            {
                //string pattern = "job_offer_data : " + element + " {";
                //MessageBox.Show("'" + pattern + "'");
                if (Regex.IsMatch(linesdata[j], element))
                {
                    //MessageBox.Show("jobMatch");
                    if (count != 0)
                    {
                        //data += j + Environment.NewLine;
                        for (int k = 1; k < 11; ++k)
                        {
                            data += linesdata[j + k] + Environment.NewLine;
                        }
                        jobData.Add(data);
                        splitJobData(data);
                        //MessageBox.Show(data);
                        break;
                    }
                    ++count;
                }
            }

            //doStuff(element);

            this.Invoke(new Action(() => doStuff(element)));        // Hier werden die Nodes in die Treeview hinzugefügt TODO
            //MessageBox.Show("fertig");
        }

        private void doStuff(string element)
        {
            //if(dual == 0)
            //{
            TreeNode n = new TreeNode(element);
            n.ContextMenuStrip = viewJobDataContext;
            treeView1.Nodes[0].Nodes.Add(n);
            --threadcount;
            ++dual;
            //}
            //else
            //{
            //    dual--;
            //}

        }

        private void reportProgress2()
        {
            progressBar1.Value += progress;
            progress = 0;


            /*if (!thread.IsAlive)
            {
                progressBarUpdateTimer.Stop();
                this.progressBar1.Visible = false;
                this.button2.Visible = true;
                MessageBox.Show("Saved savegame modifications");
            }*/
        }

        private void progressBarUpdateTimer2_Elapsed(object sender, EventArgs e)
        {
            // delegate
            this.Invoke(new Action(() => reportProgress2()));
        }

        private void doMoreBackgroundStuff()
        {
            this.Invoke(new Action(() =>
            {
                progressBar1.Value = 0;
            }));
           
            for (int i = 0; i < offers.Count; ++i)
            {
                try
                {
                    readJob(offers[i].ToString());
                    progress++;
                }
                catch (OutOfMemoryException e)
                {
                    MessageBox.Show("out of internal memory at index " + i + Environment.NewLine + "haviong threadcount "
                        + threadcount + Environment.NewLine + Environment.NewLine + "Specific Error Data: " +
                        Environment.NewLine + e.ToString());
                }
            }
            
            this.Invoke(new Action(() =>
            {
                treeView1.Nodes[0].Expand();
                progressBarUpdateTimer2.Stop();
                progressBar1.Value = 0;
                button1.Enabled = true;
                button2.Enabled = true;
            }));
            
        }

        /**
        * reads all the job offers' data
        * author: https://github.com/RayRay5
        */

        private void readJobData()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            ArrayList threadList = new ArrayList();

            for (int i = 0; i < ets2_saveeditor_main_form.lines.Length; ++i)
            {
                if (Regex.IsMatch(ets2_saveeditor_main_form.lines[i], "company : company.volatile."))
                {
                    string companyNameLocation = Regex.Replace(ets2_saveeditor_main_form.lines[i], "company : company.volatile.", "");
                    companyNameLocation = Regex.Replace(companyNameLocation, " {", "");
                    string companyName = Regex.Replace(companyNameLocation, "[.][_a-z]*", "");
                    string companyLocation = Regex.Replace(companyNameLocation, "[_a-z]*[.]", "");

                    int availableCompanyJobs = 0;
                    try
                    {
                        availableCompanyJobs = Int32.Parse(Regex.Replace(ets2_saveeditor_main_form.lines[i + 4], "job_offer: ", ""));
                    }
                    catch(Exception)
                    {
                        try
                        {
                            availableCompanyJobs = Int32.Parse(Regex.Replace(ets2_saveeditor_main_form.lines[i + 5], "job_offer: ", ""));
                        }
                        catch(Exception)
                        {
                            try
                            {
                                availableCompanyJobs = Int32.Parse(Regex.Replace(ets2_saveeditor_main_form.lines[i + 6], "job_offer: ", ""));
                            }
                            catch(Exception)
                            {
                                availableCompanyJobs = Int32.Parse(Regex.Replace(ets2_saveeditor_main_form.lines[i + 7], "job_offer: ", ""));
                            }
                        }
                        //MessageBox.Show(ets2_saveeditor_main_form.lines[i]);
                    }
                    //MessageBox.Show("company: " + companyName + " in city: " + companyLocation + " joboffers: " + availableCompanyJobs.ToString());
                    companyOffering.Add(companyNameLocation, availableCompanyJobs.ToString());

                    for (int j = 0; j < availableCompanyJobs; ++j)
                    {
                        string offerData = ""; //companyNameLocation;
                        offerData += Regex.Replace(ets2_saveeditor_main_form.lines[i + 5 + j], "job_offer\\[[0-9]*\\]: ", "");

                        offers.Add(offerData);// + "@" + companyNameLocation);
                        Thread t = new Thread(() => readJob(offerData));
                        threadList.Add(t);

                        jobSource.Add(companyNameLocation);

                        //string value = offers[companyNameLocation];
                        //MessageBox.Show("company: " + companyNameLocation + Environment.NewLine + "offer: " + value);
                    }
                }
            }

            progressBar1.Maximum = offers.Count;
            progressBarUpdateTimer2 = new System.Timers.Timer();
            progressBarUpdateTimer2.Enabled = true;
            progressBarUpdateTimer2.Elapsed += new ElapsedEventHandler(progressBarUpdateTimer2_Elapsed);
            progressBarUpdateTimer2.Interval = 1000;
            progressBarUpdateTimer2.Start();
            treeView1.Nodes.Add("All Jobs");

            //MessageBox.Show("Starting to read offer data" + Environment.NewLine + "offers length: " + offers.Count);

            /*foreach (var element in offers)
            {
                //MessageBox.Show(element);
                Thread thread = new Thread(() => compare(element.ToString()));
                thread.Start();
                progress++;
            }*/

            /*while(count < threadList.Count)
            {
                if(threadcount > 200)
                {
                    daeumchenDrehen++;
                    Thread.Sleep(100);
                }

                try
                {
                    ((Thread)threadList[count]).Start();
                    progress++;
                }
                catch(OutOfMemoryException e)
                {
                    MessageBox.Show(e.ToString());
                }
                
                count++;
            }*/

            thread = new Thread(() => doMoreBackgroundStuff());
            thread.Start();
            //doMoreBackgroundStuff();

            /*if (offers.Contains("134.e64e.f230"))
            {
                MessageBox.Show("drin");
            }*/

            //compare(offers[0].ToString());
            //compare(offers[1].ToString());
            


        }

        //analyze button
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            treeView1.Nodes.Clear();
            offers.Clear();
            jobData.Clear();
            jobSource.Clear();
            companyOffering.Clear();
            readJobData();
            textBox1.Enabled = true;
        }

        //search button
        private void button2_Click(object sender, EventArgs e)
        {
            ArrayList searchOffers = offers;
            ArrayList searchJobData = jobData;

            ArrayList searchResult = new ArrayList();

            //MessageBox.Show(searchOffers[100].ToString());
            //MessageBox.Show(searchJobData[100].ToString());
            //MessageBox.Show(sourceCompanies[100].ToString());

            string searchTerm = textBox1.Text;
            string[] searchTerms = searchTerm.Split(' ');
            for (int i = 0; i < searchTerms.Length; ++i)
            {
                searchTerms[i] = Regex.Replace(searchTerms[i], " ", "");
                searchTerms[i] = searchTerms[i].ToLower();
                //MessageBox.Show("Search Term: " + searchTerms[i]);
            }

            for (int i = 0; i < searchOffers.Count; ++i)
            {
                for (int j = 0; j < searchTerms.Length; ++j)
                {
                    if (searchOffers[i].ToString().Contains(searchTerms[j]))
                    {
                        searchResult.Add(searchOffers[i]);
                    }

                    if (searchJobData[i].ToString().Contains(searchTerms[j]))
                    {
                        searchResult.Add(searchOffers[i]);
                    }
                }
            }

            treeView1.CollapseAll();
            try
            {
                treeView1.Nodes[1].Remove();
            }
            catch(Exception)
            {

            }
            treeView1.Nodes.Add("Search Results");

            foreach(var result in searchResult)
            {
                TreeNode newNode = new TreeNode(result.ToString());
                newNode.ContextMenuStrip = viewJobDataContext;
                treeView1.Nodes[1].Nodes.Add(newNode);
                //MessageBox.Show("Result: " + result.ToString());
            }
            treeView1.Nodes[1].Expand();
            //Thread analyzeThread = new Thread(() => readJobData(searchJobData[0].ToString()));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //TODO write back stuff into > mainForm.lines[]
            MessageBox.Show("Auftrag: " + treeView2.Nodes[0].Text);
            //if(cargo[0] != treeView2.Nodes[0].Nodes[0].Nod)
            {

            }
            //MessageBox.Show("Index: " + offers.IndexOf(treeView2.Nodes[0].Text));
            //MessageBox.Show("Cargo[0]: " + cargo[0].ToString());
            MessageBox.Show("TODO: Write Back into lines[]");
            MessageBox.Show(sender.ToString());
            button3.Enabled = false;
        }
    }
}
