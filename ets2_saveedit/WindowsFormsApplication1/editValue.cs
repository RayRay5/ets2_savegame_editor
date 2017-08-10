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
                    newForm.jobSource[newForm.selectedItem] = textBox2.Text;
                    newForm.updateTreeView(this.parent, newForm.selectedItem, "Source", this.textBox2.Text);
                    break;
                case "Cargo":
                    newForm.cargo[newForm.selectedItem] = textBox2.Text;
                    newForm.updateTreeView(this.parent, newForm.selectedItem, "Cargo", this.textBox2.Text);
                    break;
                case "Destination":
                    newForm.jobDestination[newForm.selectedItem] = textBox2.Text;
                    newForm.updateTreeView(this.parent, newForm.selectedItem, "Destination", this.textBox2.Text);
                    break;
                case "Urgency":
                    string textboxString = textBox2.Text;
                    int val = Int32.Parse(Regex.Replace(textboxString, "[a-zA-Z_:. ]*", ""));
                    if(val > 2 || val < 0)
                    {
                        MessageBox.Show("Invalid value for \"Urgency\" selected!");
                        return;
                    }
                    newForm.urgency[newForm.selectedItem] = textBox2.Text;
                    newForm.updateTreeView(this.parent, newForm.selectedItem, "Urgency", this.textBox2.Text);
                    break;
                case "Truck":
                    newForm.truck[newForm.selectedItem] = textBox2.Text;
                    newForm.updateTreeView(this.parent, newForm.selectedItem, "Truck", this.textBox2.Text);
                    break;
                case "Variant":
                    newForm.variant[newForm.selectedItem] = textBox2.Text;
                    newForm.updateTreeView(this.parent, newForm.selectedItem, "Variant", this.textBox2.Text);
                    break;
                case "Expiration Time":
                    newForm.expiration[newForm.selectedItem] = textBox2.Text;
                    newForm.updateTreeView(this.parent, newForm.selectedItem, "Expiration Time", this.textBox2.Text);
                    break;
                case "Shortest Distance":
                    newForm.shortestDistance[newForm.selectedItem] = textBox2.Text;
                    newForm.updateTreeView(this.parent, newForm.selectedItem, "Shortest Distance", this.textBox2.Text);
                    break;
                case "Ferry Time":
                    newForm.ferryTime[newForm.selectedItem] = textBox2.Text;
                    newForm.updateTreeView(this.parent, newForm.selectedItem, "Ferry Time", this.textBox2.Text);
                    break;
                case "Ferry Price":
                    newForm.ferryPrice[newForm.selectedItem] = textBox2.Text;
                    newForm.updateTreeView(this.parent, newForm.selectedItem, "Ferry Price", this.textBox2.Text);
                    break;
                case "Trailer Place":
                    newForm.trailerPlace[newForm.selectedItem] = textBox2.Text;
                    newForm.updateTreeView(this.parent, newForm.selectedItem, "Trailer Place", this.textBox2.Text);
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
