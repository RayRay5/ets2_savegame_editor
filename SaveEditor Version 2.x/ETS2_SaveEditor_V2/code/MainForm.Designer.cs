namespace SaveEditorV2
{
    partial class MainForm
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
            this.loadScriptButton = new System.Windows.Forms.Button();
            this.openScriptFileButton = new System.Windows.Forms.Button();
            this.ScriptPathBox = new System.Windows.Forms.TextBox();
            this.UnitEditorButton = new System.Windows.Forms.Button();
            this.LoadSavegameButton = new System.Windows.Forms.Button();
            this.LoadSavegamePathBox = new System.Windows.Forms.TextBox();
            this.BackUpButton = new System.Windows.Forms.Button();
            this.SaveSavegameButton = new System.Windows.Forms.Button();
            this.BackupPathBox = new System.Windows.Forms.TextBox();
            this.SaveSavegamePathBox = new System.Windows.Forms.TextBox();
            this.AnalyzeSavegameButton = new System.Windows.Forms.Button();
            this.SaveSavegameWriteBackButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.VariableBox = new System.Windows.Forms.TextBox();
            this.CommonUtils = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.licenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changelogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutThisToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemRequirementsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thanksALotToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadScriptButton
            // 
            this.loadScriptButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loadScriptButton.Enabled = false;
            this.loadScriptButton.Location = new System.Drawing.Point(12, 147);
            this.loadScriptButton.Name = "loadScriptButton";
            this.loadScriptButton.Size = new System.Drawing.Size(490, 23);
            this.loadScriptButton.TabIndex = 9;
            this.loadScriptButton.Text = "Load Selected Script";
            this.loadScriptButton.UseVisualStyleBackColor = true;
            this.loadScriptButton.Click += new System.EventHandler(this.loadScriptButton_Click);
            // 
            // openScriptFileButton
            // 
            this.openScriptFileButton.Enabled = false;
            this.openScriptFileButton.Location = new System.Drawing.Point(12, 118);
            this.openScriptFileButton.Name = "openScriptFileButton";
            this.openScriptFileButton.Size = new System.Drawing.Size(96, 23);
            this.openScriptFileButton.TabIndex = 10;
            this.openScriptFileButton.Text = "Open Script";
            this.openScriptFileButton.UseVisualStyleBackColor = true;
            this.openScriptFileButton.Visible = false;
            this.openScriptFileButton.Click += new System.EventHandler(this.openScriptFileButton_Click);
            // 
            // ScriptPathBox
            // 
            this.ScriptPathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScriptPathBox.Enabled = false;
            this.ScriptPathBox.Location = new System.Drawing.Point(0, 0);
            this.ScriptPathBox.Name = "ScriptPathBox";
            this.ScriptPathBox.Size = new System.Drawing.Size(272, 20);
            this.ScriptPathBox.TabIndex = 11;
            this.ScriptPathBox.Visible = false;
            // 
            // UnitEditorButton
            // 
            this.UnitEditorButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UnitEditorButton.Enabled = false;
            this.UnitEditorButton.Location = new System.Drawing.Point(12, 235);
            this.UnitEditorButton.Name = "UnitEditorButton";
            this.UnitEditorButton.Size = new System.Drawing.Size(490, 23);
            this.UnitEditorButton.TabIndex = 0;
            this.UnitEditorButton.Text = "Unit Editor";
            this.UnitEditorButton.UseVisualStyleBackColor = true;
            this.UnitEditorButton.Click += new System.EventHandler(this.UnitEditorButton_Click);
            // 
            // LoadSavegameButton
            // 
            this.LoadSavegameButton.Location = new System.Drawing.Point(12, 31);
            this.LoadSavegameButton.Name = "LoadSavegameButton";
            this.LoadSavegameButton.Size = new System.Drawing.Size(96, 23);
            this.LoadSavegameButton.TabIndex = 1;
            this.LoadSavegameButton.Text = "Load Savegame";
            this.LoadSavegameButton.UseVisualStyleBackColor = true;
            this.LoadSavegameButton.Click += new System.EventHandler(this.LoadSavegameButton_Click);
            // 
            // LoadSavegamePathBox
            // 
            this.LoadSavegamePathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadSavegamePathBox.Enabled = false;
            this.LoadSavegamePathBox.Location = new System.Drawing.Point(114, 31);
            this.LoadSavegamePathBox.Name = "LoadSavegamePathBox";
            this.LoadSavegamePathBox.Size = new System.Drawing.Size(388, 20);
            this.LoadSavegamePathBox.TabIndex = 2;
            // 
            // BackUpButton
            // 
            this.BackUpButton.Enabled = false;
            this.BackUpButton.Location = new System.Drawing.Point(12, 60);
            this.BackUpButton.Name = "BackUpButton";
            this.BackUpButton.Size = new System.Drawing.Size(96, 23);
            this.BackUpButton.TabIndex = 3;
            this.BackUpButton.Text = "Backup";
            this.BackUpButton.UseVisualStyleBackColor = true;
            this.BackUpButton.Click += new System.EventHandler(this.BackUpButton_Click);
            // 
            // SaveSavegameButton
            // 
            this.SaveSavegameButton.Enabled = false;
            this.SaveSavegameButton.Location = new System.Drawing.Point(12, 89);
            this.SaveSavegameButton.Name = "SaveSavegameButton";
            this.SaveSavegameButton.Size = new System.Drawing.Size(96, 23);
            this.SaveSavegameButton.TabIndex = 4;
            this.SaveSavegameButton.Text = "Save Path";
            this.SaveSavegameButton.UseVisualStyleBackColor = true;
            this.SaveSavegameButton.Click += new System.EventHandler(this.SaveSavegameButton_Click);
            // 
            // BackupPathBox
            // 
            this.BackupPathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BackupPathBox.Enabled = false;
            this.BackupPathBox.Location = new System.Drawing.Point(114, 60);
            this.BackupPathBox.Name = "BackupPathBox";
            this.BackupPathBox.Size = new System.Drawing.Size(388, 20);
            this.BackupPathBox.TabIndex = 5;
            // 
            // SaveSavegamePathBox
            // 
            this.SaveSavegamePathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveSavegamePathBox.Enabled = false;
            this.SaveSavegamePathBox.Location = new System.Drawing.Point(114, 89);
            this.SaveSavegamePathBox.Name = "SaveSavegamePathBox";
            this.SaveSavegamePathBox.Size = new System.Drawing.Size(388, 20);
            this.SaveSavegamePathBox.TabIndex = 6;
            // 
            // AnalyzeSavegameButton
            // 
            this.AnalyzeSavegameButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnalyzeSavegameButton.Enabled = false;
            this.AnalyzeSavegameButton.Location = new System.Drawing.Point(12, 176);
            this.AnalyzeSavegameButton.Name = "AnalyzeSavegameButton";
            this.AnalyzeSavegameButton.Size = new System.Drawing.Size(490, 23);
            this.AnalyzeSavegameButton.TabIndex = 7;
            this.AnalyzeSavegameButton.Text = "Analyze Savegame";
            this.AnalyzeSavegameButton.UseVisualStyleBackColor = true;
            this.AnalyzeSavegameButton.Click += new System.EventHandler(this.AnalyzeSavegameButton_Click);
            // 
            // SaveSavegameWriteBackButton
            // 
            this.SaveSavegameWriteBackButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveSavegameWriteBackButton.Enabled = false;
            this.SaveSavegameWriteBackButton.Location = new System.Drawing.Point(12, 264);
            this.SaveSavegameWriteBackButton.Name = "SaveSavegameWriteBackButton";
            this.SaveSavegameWriteBackButton.Size = new System.Drawing.Size(490, 23);
            this.SaveSavegameWriteBackButton.TabIndex = 2;
            this.SaveSavegameWriteBackButton.Text = "Save Savegame";
            this.SaveSavegameWriteBackButton.UseVisualStyleBackColor = true;
            this.SaveSavegameWriteBackButton.Click += new System.EventHandler(this.SaveSavegameWriteBackButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(114, 118);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ScriptPathBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.VariableBox);
            this.splitContainer1.Size = new System.Drawing.Size(388, 23);
            this.splitContainer1.SplitterDistance = 275;
            this.splitContainer1.TabIndex = 12;
            // 
            // VariableBox
            // 
            this.VariableBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VariableBox.Enabled = false;
            this.VariableBox.Location = new System.Drawing.Point(3, 0);
            this.VariableBox.Name = "VariableBox";
            this.VariableBox.Size = new System.Drawing.Size(106, 20);
            this.VariableBox.TabIndex = 0;
            this.VariableBox.Visible = false;
            // 
            // CommonUtils
            // 
            this.CommonUtils.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommonUtils.Enabled = false;
            this.CommonUtils.Location = new System.Drawing.Point(13, 206);
            this.CommonUtils.Name = "CommonUtils";
            this.CommonUtils.Size = new System.Drawing.Size(489, 23);
            this.CommonUtils.TabIndex = 13;
            this.CommonUtils.Text = "Common Utilities";
            this.CommonUtils.UseVisualStyleBackColor = true;
            this.CommonUtils.Click += new System.EventHandler(this.CommonUtils_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(514, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.languageToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.languageToolStripMenuItem.Text = "Language";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.licenseToolStripMenuItem,
            this.versionInfoToolStripMenuItem,
            this.changelogToolStripMenuItem,
            this.aboutThisToolToolStripMenuItem,
            this.systemRequirementsToolStripMenuItem,
            this.thanksALotToToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(24, 20);
            this.toolStripMenuItem1.Text = "?";
            // 
            // licenseToolStripMenuItem
            // 
            this.licenseToolStripMenuItem.Name = "licenseToolStripMenuItem";
            this.licenseToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.licenseToolStripMenuItem.Text = "License";
            this.licenseToolStripMenuItem.Click += new System.EventHandler(this.licenseToolStripMenuItem_Click);
            // 
            // versionInfoToolStripMenuItem
            // 
            this.versionInfoToolStripMenuItem.Name = "versionInfoToolStripMenuItem";
            this.versionInfoToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.versionInfoToolStripMenuItem.Text = "Version Info";
            this.versionInfoToolStripMenuItem.Click += new System.EventHandler(this.versionInfoToolStripMenuItem_Click);
            // 
            // changelogToolStripMenuItem
            // 
            this.changelogToolStripMenuItem.Name = "changelogToolStripMenuItem";
            this.changelogToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.changelogToolStripMenuItem.Text = "Changelog";
            this.changelogToolStripMenuItem.Click += new System.EventHandler(this.changelogToolStripMenuItem_Click);
            // 
            // aboutThisToolToolStripMenuItem
            // 
            this.aboutThisToolToolStripMenuItem.Name = "aboutThisToolToolStripMenuItem";
            this.aboutThisToolToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.aboutThisToolToolStripMenuItem.Text = "About this tool";
            this.aboutThisToolToolStripMenuItem.Click += new System.EventHandler(this.aboutThisToolToolStripMenuItem_Click);
            // 
            // systemRequirementsToolStripMenuItem
            // 
            this.systemRequirementsToolStripMenuItem.Name = "systemRequirementsToolStripMenuItem";
            this.systemRequirementsToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.systemRequirementsToolStripMenuItem.Text = "System Requirements";
            this.systemRequirementsToolStripMenuItem.Click += new System.EventHandler(this.systemRequirementsToolStripMenuItem_Click);
            // 
            // thanksALotToToolStripMenuItem
            // 
            this.thanksALotToToolStripMenuItem.Name = "thanksALotToToolStripMenuItem";
            this.thanksALotToToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.thanksALotToToolStripMenuItem.Text = "Thanks a lot to...";
            this.thanksALotToToolStripMenuItem.Click += new System.EventHandler(this.thanksALotToToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 295);
            this.Controls.Add(this.CommonUtils);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.loadScriptButton);
            this.Controls.Add(this.AnalyzeSavegameButton);
            this.Controls.Add(this.openScriptFileButton);
            this.Controls.Add(this.SaveSavegameWriteBackButton);
            this.Controls.Add(this.UnitEditorButton);
            this.Controls.Add(this.LoadSavegameButton);
            this.Controls.Add(this.SaveSavegamePathBox);
            this.Controls.Add(this.LoadSavegamePathBox);
            this.Controls.Add(this.SaveSavegameButton);
            this.Controls.Add(this.BackupPathBox);
            this.Controls.Add(this.BackUpButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "ETS2/ATS Savegame Editor V2 (Public Beta)";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadScriptButton;
        private System.Windows.Forms.Button openScriptFileButton;
        private System.Windows.Forms.TextBox ScriptPathBox;
        private System.Windows.Forms.Button UnitEditorButton;
        private System.Windows.Forms.Button LoadSavegameButton;
        private System.Windows.Forms.TextBox LoadSavegamePathBox;
        private System.Windows.Forms.Button BackUpButton;
        private System.Windows.Forms.Button SaveSavegameButton;
        private System.Windows.Forms.TextBox BackupPathBox;
        private System.Windows.Forms.TextBox SaveSavegamePathBox;
        private System.Windows.Forms.Button AnalyzeSavegameButton;
        private System.Windows.Forms.Button SaveSavegameWriteBackButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox VariableBox;
        private System.Windows.Forms.Button CommonUtils;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem licenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versionInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changelogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutThisToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemRequirementsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thanksALotToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
    }
}

