namespace Herring
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.activitiesListView = new System.Windows.Forms.ListView();
            this.processColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.titleColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.shareHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.keyboardIntensityHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mouseIntensityHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.categoryHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rulesTabPage = new System.Windows.Forms.TabPage();
            this.rulesListView = new System.Windows.Forms.ListView();
            this.processHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.titleHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.keyboardMinHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.keyboardMaxHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mouseMinHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mouseMaxHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusMinHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusMaxHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.categoryNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.categoriesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.summaryListView = new System.Windows.Forms.ListView();
            this.summaryProcessHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.summaryTitleHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.summaryTimeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.periodComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.autoScrollCheckBox = new System.Windows.Forms.CheckBox();
            this.labelUserStatus = new System.Windows.Forms.Label();
            this.labelUserIs = new System.Windows.Forms.Label();
            this.buttonNextDay = new System.Windows.Forms.Button();
            this.buttonPrevDay = new System.Windows.Forms.Button();
            this.todayButton = new System.Windows.Forms.Button();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.chartPanel = new System.Windows.Forms.Panel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.applicationLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.titleLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.chartBox = new System.Windows.Forms.PictureBox();
            this.mainTabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.rulesTabPage.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.notifyIconMenuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBox)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 2000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.tabPage2);
            this.mainTabControl.Controls.Add(this.rulesTabPage);
            this.mainTabControl.Controls.Add(this.tabPage3);
            this.mainTabControl.Controls.Add(this.tabPage5);
            this.mainTabControl.Controls.Add(this.tabPage4);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 78);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1260, 444);
            this.mainTabControl.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.activitiesListView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1252, 418);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Activities";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // activitiesListView
            // 
            this.activitiesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.processColumnHeader,
            this.titleColumnHeader,
            this.shareHeader,
            this.keyboardIntensityHeader,
            this.mouseIntensityHeader,
            this.categoryHeader});
            this.activitiesListView.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.activitiesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.activitiesListView.FullRowSelect = true;
            this.activitiesListView.GridLines = true;
            this.activitiesListView.Location = new System.Drawing.Point(3, 3);
            this.activitiesListView.Name = "activitiesListView";
            this.activitiesListView.Size = new System.Drawing.Size(1246, 412);
            this.activitiesListView.TabIndex = 0;
            this.activitiesListView.UseCompatibleStateImageBehavior = false;
            this.activitiesListView.View = System.Windows.Forms.View.Details;
            // 
            // processColumnHeader
            // 
            this.processColumnHeader.Text = "Process Name";
            this.processColumnHeader.Width = 180;
            // 
            // titleColumnHeader
            // 
            this.titleColumnHeader.Text = "Window Title";
            this.titleColumnHeader.Width = 540;
            // 
            // shareHeader
            // 
            this.shareHeader.Text = "Share";
            this.shareHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.shareHeader.Width = 70;
            // 
            // keyboardIntensityHeader
            // 
            this.keyboardIntensityHeader.Text = "Keyboard";
            this.keyboardIntensityHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.keyboardIntensityHeader.Width = 70;
            // 
            // mouseIntensityHeader
            // 
            this.mouseIntensityHeader.Text = "Mouse";
            this.mouseIntensityHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mouseIntensityHeader.Width = 70;
            // 
            // categoryHeader
            // 
            this.categoryHeader.Text = "Category";
            this.categoryHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.categoryHeader.Width = 150;
            // 
            // rulesTabPage
            // 
            this.rulesTabPage.Controls.Add(this.rulesListView);
            this.rulesTabPage.Location = new System.Drawing.Point(4, 22);
            this.rulesTabPage.Name = "rulesTabPage";
            this.rulesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.rulesTabPage.Size = new System.Drawing.Size(1252, 409);
            this.rulesTabPage.TabIndex = 2;
            this.rulesTabPage.Text = "Rules";
            this.rulesTabPage.UseVisualStyleBackColor = true;
            // 
            // rulesListView
            // 
            this.rulesListView.CheckBoxes = true;
            this.rulesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.processHeader,
            this.titleHeader,
            this.keyboardMinHeader,
            this.keyboardMaxHeader,
            this.mouseMinHeader,
            this.mouseMaxHeader,
            this.statusMinHeader,
            this.statusMaxHeader,
            this.categoryNameHeader});
            this.rulesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rulesListView.Location = new System.Drawing.Point(3, 3);
            this.rulesListView.Name = "rulesListView";
            this.rulesListView.Size = new System.Drawing.Size(1246, 403);
            this.rulesListView.TabIndex = 0;
            this.rulesListView.UseCompatibleStateImageBehavior = false;
            this.rulesListView.View = System.Windows.Forms.View.Details;
            // 
            // processHeader
            // 
            this.processHeader.Text = "process";
            this.processHeader.Width = 150;
            // 
            // titleHeader
            // 
            this.titleHeader.Text = "title";
            this.titleHeader.Width = 300;
            // 
            // keyboardMinHeader
            // 
            this.keyboardMinHeader.Text = "keyboard-min";
            this.keyboardMinHeader.Width = 80;
            // 
            // keyboardMaxHeader
            // 
            this.keyboardMaxHeader.Text = "keyboard-max";
            this.keyboardMaxHeader.Width = 80;
            // 
            // mouseMinHeader
            // 
            this.mouseMinHeader.Text = "mouse-min";
            this.mouseMinHeader.Width = 80;
            // 
            // mouseMaxHeader
            // 
            this.mouseMaxHeader.Text = "mouse-max";
            this.mouseMaxHeader.Width = 80;
            // 
            // statusMinHeader
            // 
            this.statusMinHeader.Text = "status-min";
            this.statusMinHeader.Width = 80;
            // 
            // statusMaxHeader
            // 
            this.statusMaxHeader.Text = "status-max";
            this.statusMaxHeader.Width = 80;
            // 
            // categoryNameHeader
            // 
            this.categoryNameHeader.Text = "category";
            this.categoryNameHeader.Width = 200;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.categoriesListView);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1252, 409);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Categories";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // categoriesListView
            // 
            this.categoriesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.categoriesListView.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.categoriesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoriesListView.FullRowSelect = true;
            this.categoriesListView.GridLines = true;
            this.categoriesListView.Location = new System.Drawing.Point(0, 0);
            this.categoriesListView.Name = "categoriesListView";
            this.categoriesListView.Size = new System.Drawing.Size(1252, 409);
            this.categoriesListView.TabIndex = 0;
            this.categoriesListView.UseCompatibleStateImageBehavior = false;
            this.categoriesListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Category";
            this.columnHeader1.Width = 182;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Total Time";
            this.columnHeader2.Width = 84;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Total Share";
            this.columnHeader3.Width = 99;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.summaryListView);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1252, 409);
            this.tabPage5.TabIndex = 5;
            this.tabPage5.Text = "Summary";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // summaryListView
            // 
            this.summaryListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.summaryProcessHeader,
            this.summaryTitleHeader,
            this.summaryTimeHeader});
            this.summaryListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.summaryListView.FullRowSelect = true;
            this.summaryListView.GridLines = true;
            this.summaryListView.Location = new System.Drawing.Point(0, 0);
            this.summaryListView.Name = "summaryListView";
            this.summaryListView.Size = new System.Drawing.Size(1252, 409);
            this.summaryListView.TabIndex = 0;
            this.summaryListView.UseCompatibleStateImageBehavior = false;
            this.summaryListView.View = System.Windows.Forms.View.Details;
            // 
            // summaryProcessHeader
            // 
            this.summaryProcessHeader.Text = "Process Name";
            this.summaryProcessHeader.Width = 197;
            // 
            // summaryTitleHeader
            // 
            this.summaryTitleHeader.Text = "Window Title";
            this.summaryTitleHeader.Width = 578;
            // 
            // summaryTimeHeader
            // 
            this.summaryTimeHeader.Text = "Total Time";
            this.summaryTimeHeader.Width = 139;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1252, 409);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "Timesheet";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.periodComboBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.autoScrollCheckBox);
            this.panel1.Controls.Add(this.labelUserStatus);
            this.panel1.Controls.Add(this.labelUserIs);
            this.panel1.Controls.Add(this.buttonNextDay);
            this.panel1.Controls.Add(this.buttonPrevDay);
            this.panel1.Controls.Add(this.todayButton);
            this.panel1.Controls.Add(this.datePicker);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1260, 32);
            this.panel1.TabIndex = 1;
            // 
            // periodComboBox
            // 
            this.periodComboBox.Enabled = false;
            this.periodComboBox.FormattingEnabled = true;
            this.periodComboBox.Items.AddRange(new object[] {
            "5 minutes",
            "15 minutes",
            "1 hours",
            "1 day"});
            this.periodComboBox.Location = new System.Drawing.Point(664, 6);
            this.periodComboBox.Name = "periodComboBox";
            this.periodComboBox.Size = new System.Drawing.Size(121, 21);
            this.periodComboBox.TabIndex = 9;
            this.periodComboBox.Text = "5 minutes";
            this.periodComboBox.SelectedIndexChanged += new System.EventHandler(this.periodComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(584, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Time Unit:";
            // 
            // autoScrollCheckBox
            // 
            this.autoScrollCheckBox.AutoSize = true;
            this.autoScrollCheckBox.Location = new System.Drawing.Point(481, 9);
            this.autoScrollCheckBox.Name = "autoScrollCheckBox";
            this.autoScrollCheckBox.Size = new System.Drawing.Size(77, 17);
            this.autoScrollCheckBox.TabIndex = 7;
            this.autoScrollCheckBox.Text = "Auto Scroll";
            this.autoScrollCheckBox.UseVisualStyleBackColor = true;
            this.autoScrollCheckBox.CheckedChanged += new System.EventHandler(this.autoScrollCheckBox_CheckedChanged);
            // 
            // labelUserStatus
            // 
            this.labelUserStatus.AutoSize = true;
            this.labelUserStatus.Location = new System.Drawing.Point(418, 10);
            this.labelUserStatus.Name = "labelUserStatus";
            this.labelUserStatus.Size = new System.Drawing.Size(51, 13);
            this.labelUserStatus.TabIndex = 6;
            this.labelUserStatus.Text = "<not-set>";
            // 
            // labelUserIs
            // 
            this.labelUserIs.AutoSize = true;
            this.labelUserIs.Location = new System.Drawing.Point(376, 10);
            this.labelUserIs.Name = "labelUserIs";
            this.labelUserIs.Size = new System.Drawing.Size(42, 13);
            this.labelUserIs.TabIndex = 5;
            this.labelUserIs.Text = "User is:";
            // 
            // buttonNextDay
            // 
            this.buttonNextDay.BackgroundImage = global::Herring.Properties.Resources._31_16x16;
            this.buttonNextDay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonNextDay.Location = new System.Drawing.Point(290, 4);
            this.buttonNextDay.Name = "buttonNextDay";
            this.buttonNextDay.Size = new System.Drawing.Size(24, 24);
            this.buttonNextDay.TabIndex = 3;
            this.buttonNextDay.UseVisualStyleBackColor = true;
            this.buttonNextDay.Click += new System.EventHandler(this.buttonNextDay_Click);
            // 
            // buttonPrevDay
            // 
            this.buttonPrevDay.BackgroundImage = global::Herring.Properties.Resources._30_16x16;
            this.buttonPrevDay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonPrevDay.Location = new System.Drawing.Point(264, 4);
            this.buttonPrevDay.Name = "buttonPrevDay";
            this.buttonPrevDay.Size = new System.Drawing.Size(24, 24);
            this.buttonPrevDay.TabIndex = 2;
            this.buttonPrevDay.UseVisualStyleBackColor = true;
            this.buttonPrevDay.Click += new System.EventHandler(this.buttonPrevDay_Click);
            // 
            // todayButton
            // 
            this.todayButton.Location = new System.Drawing.Point(316, 4);
            this.todayButton.Name = "todayButton";
            this.todayButton.Size = new System.Drawing.Size(54, 24);
            this.todayButton.TabIndex = 1;
            this.todayButton.Text = "Today";
            this.todayButton.UseVisualStyleBackColor = true;
            this.todayButton.Click += new System.EventHandler(this.todayButton_Click);
            // 
            // datePicker
            // 
            this.datePicker.CustomFormat = "dddd, dd MMMM yyyy";
            this.datePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datePicker.Location = new System.Drawing.Point(4, 5);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(257, 22);
            this.datePicker.TabIndex = 0;
            this.datePicker.ValueChanged += new System.EventHandler(this.datePicker_ValueChanged);
            // 
            // chartPanel
            // 
            this.chartPanel.AutoScroll = true;
            this.chartPanel.BackColor = System.Drawing.Color.White;
            this.chartPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chartPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.chartPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.chartPanel.Location = new System.Drawing.Point(0, 32);
            this.chartPanel.Name = "chartPanel";
            this.chartPanel.Size = new System.Drawing.Size(1260, 55);
            this.chartPanel.TabIndex = 2;
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "Working in the background";
            this.notifyIcon.BalloonTipTitle = "Herring Tracker";
            this.notifyIcon.ContextMenuStrip = this.notifyIconMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "HerringTracker";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // notifyIconMenuStrip
            // 
            this.notifyIconMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeMenuItem});
            this.notifyIconMenuStrip.Name = "notifyIconMenuStrip";
            this.notifyIconMenuStrip.Size = new System.Drawing.Size(104, 26);
            // 
            // closeMenuItem
            // 
            this.closeMenuItem.Name = "closeMenuItem";
            this.closeMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeMenuItem.Text = "Close";
            this.closeMenuItem.Click += new System.EventHandler(this.closeMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.applicationLabel,
            this.titleLabel,
            this.statsLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 522);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1260, 24);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(42, 19);
            this.statusLabel.Text = "<text>";
            // 
            // applicationLabel
            // 
            this.applicationLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.applicationLabel.Name = "applicationLabel";
            this.applicationLabel.Size = new System.Drawing.Size(61, 19);
            this.applicationLabel.Text = "<parent>";
            // 
            // titleLabel
            // 
            this.titleLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(47, 19);
            this.titleLabel.Text = "<title>";
            // 
            // statsLabel
            // 
            this.statsLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statsLabel.Name = "statsLabel";
            this.statsLabel.Size = new System.Drawing.Size(51, 19);
            this.statsLabel.Text = "<stats>";
            // 
            // chartBox
            // 
            this.chartBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chartBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.chartBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.chartBox.Location = new System.Drawing.Point(0, 32);
            this.chartBox.Name = "chartBox";
            this.chartBox.Size = new System.Drawing.Size(1260, 46);
            this.chartBox.TabIndex = 0;
            this.chartBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 546);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.chartBox);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Herring";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainTabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.rulesTabPage.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.notifyIconMenuStrip.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView activitiesListView;
        private System.Windows.Forms.ColumnHeader processColumnHeader;
        private System.Windows.Forms.ColumnHeader titleColumnHeader;
        private System.Windows.Forms.ColumnHeader mouseIntensityHeader;
        private System.Windows.Forms.ColumnHeader keyboardIntensityHeader;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.Button todayButton;
        private System.Windows.Forms.TabPage rulesTabPage;
        private System.Windows.Forms.ColumnHeader shareHeader;
        private System.Windows.Forms.Button buttonPrevDay;
        private System.Windows.Forms.Button buttonNextDay;
        private System.Windows.Forms.Label labelUserStatus;
        private System.Windows.Forms.Label labelUserIs;
        private System.Windows.Forms.CheckBox autoScrollCheckBox;
        private System.Windows.Forms.ColumnHeader categoryHeader;
        private System.Windows.Forms.ListView rulesListView;
        private System.Windows.Forms.ColumnHeader processHeader;
        private System.Windows.Forms.ColumnHeader titleHeader;
        private System.Windows.Forms.ColumnHeader keyboardMinHeader;
        private System.Windows.Forms.ColumnHeader keyboardMaxHeader;
        private System.Windows.Forms.ColumnHeader mouseMinHeader;
        private System.Windows.Forms.ColumnHeader mouseMaxHeader;
        private System.Windows.Forms.ColumnHeader statusMinHeader;
        private System.Windows.Forms.ColumnHeader statusMaxHeader;
        private System.Windows.Forms.ColumnHeader categoryNameHeader;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListView categoriesListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Panel chartPanel;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.ListView summaryListView;
        private System.Windows.Forms.ColumnHeader summaryProcessHeader;
        private System.Windows.Forms.ColumnHeader summaryTitleHeader;
        private System.Windows.Forms.ColumnHeader summaryTimeHeader;
        private System.Windows.Forms.ComboBox periodComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem closeMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripStatusLabel titleLabel;
        private System.Windows.Forms.ToolStripStatusLabel statsLabel;
        private System.Windows.Forms.ToolStripStatusLabel applicationLabel;
        private System.Windows.Forms.PictureBox chartBox;
    }
}

