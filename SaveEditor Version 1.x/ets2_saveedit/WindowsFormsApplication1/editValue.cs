using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ets2_saveeditor;
using System.Text.RegularExpressions;

namespace saveeditor
{
    public partial class editValue : Form
    {
        private string parent;
        private string currentValue;
        private string property;

        public editValue(string parent, string currentValue, string property)
        {
            this.parent = parent;
            this.currentValue = currentValue;
            this.property = property;
            InitializeComponent();
            this.textBox1.Text = this.currentValue;
            this.textBox2.Text = this.currentValue;
            this.Text = "Edit Raw Value of " + this.property + " from " + parent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (property)
            {
                case "Source":
                    jobEditor.jobSource[jobEditor.selectedItem] = textBox2.Text;
                    jobEditor.updateTreeView(this.parent, jobEditor.selectedItem, "Source", this.textBox2.Text);
                    break;
                case "Cargo":
                    jobEditor.cargo[jobEditor.selectedItem] = textBox2.Text;
                    jobEditor.updateTreeView(this.parent, jobEditor.selectedItem, "Cargo", this.textBox2.Text);
                    break;
                case "Destination":
                    jobEditor.jobDestination[jobEditor.selectedItem] = textBox2.Text;
                    jobEditor.updateTreeView(this.parent, jobEditor.selectedItem, "Destination", this.textBox2.Text);
                    break;
                case "Urgency":
                    string textboxString = textBox2.Text;
                    int val = Int32.Parse(Regex.Replace(textboxString, "[a-zA-Z_:. ]*", ""));
                    if(val > 2 || val < 0)
                    {
                        MessageBox.Show("Invalid value for \"Urgency\" selected!");
                        return;
                    }
                    jobEditor.urgency[jobEditor.selectedItem] = textBox2.Text;
                    jobEditor.updateTreeView(this.parent, jobEditor.selectedItem, "Urgency", this.textBox2.Text);
                    break;
                case "Truck":
                    jobEditor.truck[jobEditor.selectedItem] = textBox2.Text;
                    jobEditor.updateTreeView(this.parent, jobEditor.selectedItem, "Truck", this.textBox2.Text);
                    break;
                case "Variant":
                    jobEditor.variant[jobEditor.selectedItem] = textBox2.Text;
                    jobEditor.updateTreeView(this.parent, jobEditor.selectedItem, "Variant", this.textBox2.Text);
                    break;
                case "Expiration Time":
                    jobEditor.expiration[jobEditor.selectedItem] = textBox2.Text;
                    jobEditor.updateTreeView(this.parent, jobEditor.selectedItem, "Expiration Time", this.textBox2.Text);
                    break;
                case "Shortest Distance":
                    jobEditor.shortestDistance[jobEditor.selectedItem] = textBox2.Text;
                    jobEditor.updateTreeView(this.parent, jobEditor.selectedItem, "Shortest Distance", this.textBox2.Text);
                    break;
                case "Ferry Time":
                    jobEditor.ferryTime[jobEditor.selectedItem] = textBox2.Text;
                    jobEditor.updateTreeView(this.parent, jobEditor.selectedItem, "Ferry Time", this.textBox2.Text);
                    break;
                case "Ferry Price":
                    jobEditor.ferryPrice[jobEditor.selectedItem] = textBox2.Text;
                    jobEditor.updateTreeView(this.parent, jobEditor.selectedItem, "Ferry Price", this.textBox2.Text);
                    break;
                case "Trailer Place":
                    jobEditor.trailerPlace[jobEditor.selectedItem] = textBox2.Text;
                    jobEditor.updateTreeView(this.parent, jobEditor.selectedItem, "Trailer Place", this.textBox2.Text);
                    break;
                default:
                    MessageBox.Show("Unknown Item. Applying changes failed!");
                    return;
            }

            textBox1.Text = textBox2.Text;
            this.Close();
        }
    }
}
