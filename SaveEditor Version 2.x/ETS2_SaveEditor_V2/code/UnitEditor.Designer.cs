/**
 * SaveEditor Version V2
 * By https://github.com/RayRay5
 * Licensed under GNU General Public License v3.0
 */

namespace SaveEditorV2
{
    partial class UnitEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LoadTreeView = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.jumpPreviousResultButton = new System.Windows.Forms.Button();
            this.jumpNextResultButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.seachTextBox = new System.Windows.Forms.TextBox();
            this.AddUnitButton = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoadTreeView
            // 
            this.LoadTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadTreeView.Location = new System.Drawing.Point(3, 4);
            this.LoadTreeView.Name = "LoadTreeView";
            this.LoadTreeView.Size = new System.Drawing.Size(290, 23);
            this.LoadTreeView.TabIndex = 0;
            this.LoadTreeView.Text = Language.LoadUnitTree;
            this.LoadTreeView.UseVisualStyleBackColor = true;
            this.LoadTreeView.Click += new System.EventHandler(this.LoadTreeView_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(13, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.jumpPreviousResultButton);
            this.splitContainer1.Panel1.Controls.Add(this.jumpNextResultButton);
            this.splitContainer1.Panel1.Controls.Add(this.searchButton);
            this.splitContainer1.Panel1.Controls.Add(this.seachTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.AddUnitButton);
            this.splitContainer1.Panel1.Controls.Add(this.LoadTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.treeView1);
            this.splitContainer1.Size = new System.Drawing.Size(775, 426);
            this.splitContainer1.SplitterDistance = 296;
            this.splitContainer1.TabIndex = 1;
            // 
            // jumpPreviousResultButton
            // 
            this.jumpPreviousResultButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jumpPreviousResultButton.Enabled = false;
            this.jumpPreviousResultButton.Location = new System.Drawing.Point(4, 118);
            this.jumpPreviousResultButton.Name = "jumpPreviousResultButton";
            this.jumpPreviousResultButton.Size = new System.Drawing.Size(289, 23);
            this.jumpPreviousResultButton.TabIndex = 5;
            this.jumpPreviousResultButton.Text = Language.PreviousResult;
            this.jumpPreviousResultButton.UseVisualStyleBackColor = true;
            this.jumpPreviousResultButton.Click += new System.EventHandler(this.jumpPreviousResultButton_Click);
            // 
            // jumpNextResultButton
            // 
            this.jumpNextResultButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jumpNextResultButton.Enabled = false;
            this.jumpNextResultButton.Location = new System.Drawing.Point(4, 147);
            this.jumpNextResultButton.Name = "jumpNextResultButton";
            this.jumpNextResultButton.Size = new System.Drawing.Size(289, 23);
            this.jumpNextResultButton.TabIndex = 4;
            this.jumpNextResultButton.Text = Language.NextResult;
            this.jumpNextResultButton.UseVisualStyleBackColor = true;
            this.jumpNextResultButton.Click += new System.EventHandler(this.jumpNextResultButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Enabled = false;
            this.searchButton.Location = new System.Drawing.Point(3, 88);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(290, 23);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = Language.Search;
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // seachTextBox
            // 
            this.seachTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seachTextBox.Enabled = false;
            this.seachTextBox.Location = new System.Drawing.Point(3, 62);
            this.seachTextBox.Name = "seachTextBox";
            this.seachTextBox.Size = new System.Drawing.Size(290, 20);
            this.seachTextBox.TabIndex = 2;
            // 
            // AddUnitButton
            // 
            this.AddUnitButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddUnitButton.Enabled = false;
            this.AddUnitButton.Location = new System.Drawing.Point(3, 33);
            this.AddUnitButton.Name = "AddUnitButton";
            this.AddUnitButton.Size = new System.Drawing.Size(290, 23);
            this.AddUnitButton.TabIndex = 1;
            this.AddUnitButton.Text = Language.AddUnit;
            this.AddUnitButton.UseVisualStyleBackColor = true;
            this.AddUnitButton.Click += new System.EventHandler(this.AddUnitButton_Click);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Location = new System.Drawing.Point(3, 4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(469, 419);
            this.treeView1.TabIndex = 0;
            // 
            // UnitEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "UnitEditor";
            this.Text = "UnitEditor (ETS2 SavegameEditor V2 Development Beta)";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LoadTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button AddUnitButton;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox seachTextBox;
        private System.Windows.Forms.Button jumpNextResultButton;
        private System.Windows.Forms.Button jumpPreviousResultButton;
    }
}