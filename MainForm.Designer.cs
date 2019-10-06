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
                boldFont.Dispose();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.reportButton = new System.Windows.Forms.Button();
            this.trackCheckBox = new System.Windows.Forms.CheckBox();
            this.exitButton = new System.Windows.Forms.Button();
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
            this.processLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.titleLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.windowLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.documentUrlLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timeStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.nextPoint = new System.Windows.Forms.ToolStripStatusLabel();
            this.summaryMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.followLinkInSummaryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.summaryListView = new System.Windows.Forms.ListView();
            this.summaryProcessHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.summaryTitleHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.summaryDocumentHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.summaryTimeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.summaryTopTimeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.categoriesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.activitiesListView = new Herring.OptimizedListView();
            this.processColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.titleColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.subtitleHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.shareHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.keyboardIntensityHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mouseIntensityHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.categoryHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.activitiesMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyProjectKaiserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.followLinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.rangeTextLabel = new System.Windows.Forms.Label();
            this.chart = new Herring.ChartView();
            this.chartMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.markWorkingHoursToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.notifyIconMenuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.summaryMenuStrip.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.rulesTabPage.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.activitiesMenuStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.chartMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 2000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.reportButton);
            this.panel1.Controls.Add(this.trackCheckBox);
            this.panel1.Controls.Add(this.exitButton);
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
            this.panel1.Size = new System.Drawing.Size(1294, 32);
            this.panel1.TabIndex = 1;
            // 
            // reportButton
            // 
            this.reportButton.Location = new System.Drawing.Point(662, 4);
            this.reportButton.Name = "reportButton";
            this.reportButton.Size = new System.Drawing.Size(54, 24);
            this.reportButton.TabIndex = 10;
            this.reportButton.Text = "Report...";
            this.reportButton.UseVisualStyleBackColor = true;
            this.reportButton.Click += new System.EventHandler(this.reportButton_Click);
            // 
            // trackCheckBox
            // 
            this.trackCheckBox.AutoSize = true;
            this.trackCheckBox.Checked = true;
            this.trackCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackCheckBox.Location = new System.Drawing.Point(565, 9);
            this.trackCheckBox.Name = "trackCheckBox";
            this.trackCheckBox.Size = new System.Drawing.Size(91, 17);
            this.trackCheckBox.TabIndex = 9;
            this.trackCheckBox.Text = "Track Activity";
            this.trackCheckBox.UseVisualStyleBackColor = true;
            this.trackCheckBox.CheckedChanged += new System.EventHandler(this.trackCheckBox_CheckedChanged);
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.Location = new System.Drawing.Point(1213, 5);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 8;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
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
            this.notifyIconMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
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
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processLabel,
            this.titleLabel,
            this.windowLabel,
            this.documentUrlLabel,
            this.statsLabel,
            this.timeStatusLabel,
            this.nextPoint});
            this.statusStrip.Location = new System.Drawing.Point(0, 682);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1294, 24);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip";
            // 
            // processLabel
            // 
            this.processLabel.Name = "processLabel";
            this.processLabel.Size = new System.Drawing.Size(78, 19);
            this.processLabel.Text = "<process>";
            // 
            // appTitleLabel
            // 
            this.titleLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(72, 19);
            this.titleLabel.Text = "<title>";
            // 
            // windowLabel
            // 
            this.windowLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.windowLabel.Name = "windowLabel";
            this.windowLabel.Size = new System.Drawing.Size(69, 19);
            this.windowLabel.Text = "<window>";
            // 
            // documentUrlLabel
            // 
            this.documentUrlLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.documentUrlLabel.Name = "addressLabel";
            this.documentUrlLabel.Size = new System.Drawing.Size(67, 19);
            this.documentUrlLabel.Text = "<document-url>";
            // 
            // statsLabel
            // 
            this.statsLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statsLabel.Name = "statsLabel";
            this.statsLabel.Size = new System.Drawing.Size(51, 19);
            this.statsLabel.Text = "<stats>";
            // 
            // timeStatusLabel
            // 
            this.timeStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.timeStatusLabel.Name = "timeStatusLabel";
            this.timeStatusLabel.Size = new System.Drawing.Size(51, 19);
            this.timeStatusLabel.Text = "<time>";
            // 
            // nextPoint
            // 
            this.nextPoint.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.nextPoint.Name = "nextPoint";
            this.nextPoint.RightToLeftAutoMirrorImage = true;
            this.nextPoint.Size = new System.Drawing.Size(49, 19);
            this.nextPoint.Text = "<next>";
            // 
            // summaryMenuStrip
            // 
            this.summaryMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.summaryMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyMenuItem,
            this.followLinkInSummaryMenuItem});
            this.summaryMenuStrip.Name = "sampleMenuStrip";
            this.summaryMenuStrip.Size = new System.Drawing.Size(135, 48);
            // 
            // copyMenuItem
            // 
            this.copyMenuItem.Name = "copyMenuItem";
            this.copyMenuItem.Size = new System.Drawing.Size(134, 22);
            this.copyMenuItem.Text = "Copy";
            this.copyMenuItem.Click += new System.EventHandler(this.copyMenuItem_Click);
            // 
            // followLinkInSummaryMenuItem
            // 
            this.followLinkInSummaryMenuItem.Name = "followLinkInSummaryMenuItem";
            this.followLinkInSummaryMenuItem.Size = new System.Drawing.Size(134, 22);
            this.followLinkInSummaryMenuItem.Text = "Follow Link";
            this.followLinkInSummaryMenuItem.Click += new System.EventHandler(this.followLinkSummaryMenuItem_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.summaryListView);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1286, 574);
            this.tabPage5.TabIndex = 5;
            this.tabPage5.Text = "Summary";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // summaryListView
            // 
            this.summaryListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.summaryProcessHeader,
            this.summaryTitleHeader,
            this.summaryDocumentHeader,
            this.summaryTimeHeader,
            this.summaryTopTimeHeader});
            this.summaryListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.summaryListView.FullRowSelect = true;
            this.summaryListView.GridLines = true;
            this.summaryListView.Location = new System.Drawing.Point(0, 0);
            this.summaryListView.Name = "summaryListView";
            this.summaryListView.Size = new System.Drawing.Size(1286, 574);
            this.summaryListView.TabIndex = 0;
            this.summaryListView.UseCompatibleStateImageBehavior = false;
            this.summaryListView.View = System.Windows.Forms.View.Details;
            this.summaryListView.SelectedIndexChanged += new System.EventHandler(this.summaryListView_SelectedIndexChanged);
            this.summaryListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.summaryListView_MouseClick);
            // 
            // summaryProcessHeader
            // 
            this.summaryProcessHeader.Text = "Process Name";
            this.summaryProcessHeader.Width = 197;
            // 
            // summaryTitleHeader
            // 
            this.summaryTitleHeader.Text = "Application Title";
            this.summaryTitleHeader.Width = 524;
            // 
            // summaryDocumentHeader
            // 
            this.summaryDocumentHeader.Text = "Document";
            this.summaryDocumentHeader.Width = 240;
            // 
            // summaryTimeHeader
            // 
            this.summaryTimeHeader.Text = "Total Time";
            this.summaryTimeHeader.Width = 139;
            // 
            // summaryTopTimeHeader
            // 
            this.summaryTopTimeHeader.Text = "Top Time";
            this.summaryTopTimeHeader.Width = 118;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.categoriesListView);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1286, 574);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Categories";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // categoriesListView
            // 
            this.categoriesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.categoriesListView.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.categoriesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoriesListView.FullRowSelect = true;
            this.categoriesListView.GridLines = true;
            this.categoriesListView.Location = new System.Drawing.Point(0, 0);
            this.categoriesListView.Name = "categoriesListView";
            this.categoriesListView.Size = new System.Drawing.Size(1286, 574);
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
            this.columnHeader2.Width = 99;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Total Top Time";
            this.columnHeader3.Width = 102;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Total Share";
            this.columnHeader4.Width = 99;
            // 
            // rulesTabPage
            // 
            this.rulesTabPage.Controls.Add(this.rulesListView);
            this.rulesTabPage.Location = new System.Drawing.Point(4, 22);
            this.rulesTabPage.Name = "rulesTabPage";
            this.rulesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.rulesTabPage.Size = new System.Drawing.Size(1286, 574);
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
            this.rulesListView.FullRowSelect = true;
            this.rulesListView.GridLines = true;
            this.rulesListView.Location = new System.Drawing.Point(3, 3);
            this.rulesListView.Name = "rulesListView";
            this.rulesListView.Size = new System.Drawing.Size(1280, 568);
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.activitiesListView);
            this.tabPage2.Controls.Add(this.panel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1286, 574);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Activities";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // activitiesListView
            // 
            this.activitiesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.processColumnHeader,
            this.titleColumnHeader,
            this.subtitleHeader,
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
            this.activitiesListView.Size = new System.Drawing.Size(1280, 532);
            this.activitiesListView.TabIndex = 0;
            this.activitiesListView.UseCompatibleStateImageBehavior = false;
            this.activitiesListView.View = System.Windows.Forms.View.Details;
            this.activitiesListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.activitiesListView_MouseClick);
            // 
            // processColumnHeader
            // 
            this.processColumnHeader.Text = "Process Name";
            this.processColumnHeader.Width = 180;
            // 
            // titleColumnHeader
            // 
            this.titleColumnHeader.Text = "Window Title";
            this.titleColumnHeader.Width = 524;
            // 
            // subtitleHeader
            // 
            this.subtitleHeader.Text = "Subtitle";
            this.subtitleHeader.Width = 240;
            // 
            // shareHeader
            // 
            this.shareHeader.Text = "Share";
            this.shareHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // keyboardIntensityHeader
            // 
            this.keyboardIntensityHeader.Text = "Keyboard";
            this.keyboardIntensityHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mouseIntensityHeader
            // 
            this.mouseIntensityHeader.Text = "Mouse";
            this.mouseIntensityHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // categoryHeader
            // 
            this.categoryHeader.Text = "Category";
            this.categoryHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.categoryHeader.Width = 150;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(3, 535);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1280, 36);
            this.panel3.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(86, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Time range:";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(154, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(112, 20);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(5, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Filter";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.tabPage1);
            this.mainTabControl.Controls.Add(this.tabPage2);
            this.mainTabControl.Controls.Add(this.rulesTabPage);
            this.mainTabControl.Controls.Add(this.tabPage3);
            this.mainTabControl.Controls.Add(this.tabPage5);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 82);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1294, 600);
            this.mainTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1286, 574);
            this.tabPage1.TabIndex = 6;
            this.tabPage1.Text = "Start";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1280, 568);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome to Herring Activity Tracker";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // activitiesMenuStrip
            // 
            this.activitiesMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.activitiesMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.copyProjectKaiserToolStripMenuItem,
            this.followLinkToolStripMenuItem});
            this.activitiesMenuStrip.Name = "sampleMenuStrip";
            this.activitiesMenuStrip.Size = new System.Drawing.Size(185, 70);
            this.activitiesMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.activitiesMenuStrip_Opening);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem1.Text = "Copy";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.copyFromActivitiesItem_Click);
            // 
            // copyProjectKaiserToolStripMenuItem
            // 
            this.copyProjectKaiserToolStripMenuItem.Name = "copyProjectKaiserToolStripMenuItem";
            this.copyProjectKaiserToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.copyProjectKaiserToolStripMenuItem.Text = "Copy (Project Kaiser)";
            this.copyProjectKaiserToolStripMenuItem.Click += new System.EventHandler(this.copyProjectKaiserToolStripMenuItem_Click);
            // 
            // followLinkToolStripMenuItem
            // 
            this.followLinkToolStripMenuItem.Name = "followLinkToolStripMenuItem";
            this.followLinkToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.followLinkToolStripMenuItem.Text = "Follow Link";
            this.followLinkToolStripMenuItem.Click += new System.EventHandler(this.followLinkActivitiesMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1162, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Time:";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(1205, 10);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(34, 13);
            this.timeLabel.TabIndex = 6;
            this.timeLabel.Text = "00:00";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rangeLabel);
            this.panel2.Controls.Add(this.rangeTextLabel);
            this.panel2.Controls.Add(this.timeLabel);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.chart);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1294, 50);
            this.panel2.TabIndex = 1;
            // 
            // rangeLabel
            // 
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.Location = new System.Drawing.Point(1205, 28);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(22, 13);
            this.rangeLabel.TabIndex = 8;
            this.rangeLabel.Text = "--:--";
            // 
            // rangeTextLabel
            // 
            this.rangeTextLabel.AutoSize = true;
            this.rangeTextLabel.Location = new System.Drawing.Point(1162, 28);
            this.rangeTextLabel.Name = "rangeTextLabel";
            this.rangeTextLabel.Size = new System.Drawing.Size(43, 13);
            this.rangeTextLabel.TabIndex = 7;
            this.rangeTextLabel.Text = "Length:";
            // 
            // chart
            // 
            this.chart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.chart.ContextMenuStrip = this.chartMenu;
            this.chart.Location = new System.Drawing.Point(3, 0);
            this.chart.Name = "chart";
            this.chart.SelectedBar = null;
            this.chart.Size = new System.Drawing.Size(1156, 49);
            this.chart.TabIndex = 4;
            this.chart.TabStop = false;
            this.chart.SelectionChanged += new System.EventHandler(this.chart_selectionChanged);
            this.chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartBox_MouseMove);
            this.chart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_MouseUp);
            // 
            // chartMenu
            // 
            this.chartMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.chartMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeEventToolStripMenuItem,
            this.addEventToolStripMenuItem,
            this.toolStripMenuItem2,
            this.markWorkingHoursToolStripMenuItem});
            this.chartMenu.Name = "chartMenu";
            this.chartMenu.Size = new System.Drawing.Size(191, 76);
            this.chartMenu.Opening += new System.ComponentModel.CancelEventHandler(this.chartMenu_Opening);
            // 
            // removeEventToolStripMenuItem
            // 
            this.removeEventToolStripMenuItem.Enabled = false;
            this.removeEventToolStripMenuItem.Name = "removeEventToolStripMenuItem";
            this.removeEventToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.removeEventToolStripMenuItem.Text = "Remove event";
            // 
            // addEventToolStripMenuItem
            // 
            this.addEventToolStripMenuItem.Name = "addEventToolStripMenuItem";
            this.addEventToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.addEventToolStripMenuItem.Text = "Add new event..";
            this.addEventToolStripMenuItem.Click += new System.EventHandler(this.addEventToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(187, 6);
            // 
            // markWorkingHoursToolStripMenuItem
            // 
            this.markWorkingHoursToolStripMenuItem.Name = "markWorkingHoursToolStripMenuItem";
            this.markWorkingHoursToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.markWorkingHoursToolStripMenuItem.Text = "Mark \'Working Hours\'";
            this.markWorkingHoursToolStripMenuItem.Click += new System.EventHandler(this.markWorkingHoursToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 706);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Herring";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.notifyIconMenuStrip.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.summaryMenuStrip.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.rulesTabPage.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.activitiesMenuStrip.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.chartMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.Button todayButton;
        private System.Windows.Forms.Button buttonPrevDay;
        private System.Windows.Forms.Button buttonNextDay;
        private System.Windows.Forms.Label labelUserStatus;
        private System.Windows.Forms.Label labelUserIs;
        private System.Windows.Forms.CheckBox autoScrollCheckBox;
        private System.Windows.Forms.Panel chartPanel;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem closeMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel processLabel;
        private System.Windows.Forms.ToolStripStatusLabel windowLabel;
        private System.Windows.Forms.ToolStripStatusLabel statsLabel;
        private System.Windows.Forms.ToolStripStatusLabel titleLabel;
        private System.Windows.Forms.ContextMenuStrip summaryMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel documentUrlLabel;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.ListView summaryListView;
        private System.Windows.Forms.ColumnHeader summaryProcessHeader;
        private System.Windows.Forms.ColumnHeader summaryTitleHeader;
        private System.Windows.Forms.ColumnHeader summaryDocumentHeader;
        private System.Windows.Forms.ColumnHeader summaryTimeHeader;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView categoriesListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TabPage rulesTabPage;
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
        private System.Windows.Forms.TabPage tabPage2;
        private OptimizedListView activitiesListView;
        private System.Windows.Forms.ColumnHeader processColumnHeader;
        private System.Windows.Forms.ColumnHeader titleColumnHeader;
        private System.Windows.Forms.ColumnHeader subtitleHeader;
        private System.Windows.Forms.ColumnHeader shareHeader;
        private System.Windows.Forms.ColumnHeader keyboardIntensityHeader;
        private System.Windows.Forms.ColumnHeader mouseIntensityHeader;
        private System.Windows.Forms.ColumnHeader categoryHeader;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.ContextMenuStrip activitiesMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.CheckBox trackCheckBox;
        private System.Windows.Forms.ToolStripMenuItem followLinkInSummaryMenuItem;
        private System.Windows.Forms.ToolStripMenuItem followLinkToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader summaryTopTimeHeader;
        private System.Windows.Forms.ToolStripStatusLabel timeStatusLabel;
        private System.Windows.Forms.Button reportButton;
        private System.Windows.Forms.ToolStripMenuItem copyProjectKaiserToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private ChartView chart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.Label rangeTextLabel;
        private System.Windows.Forms.ContextMenuStrip chartMenu;
        private System.Windows.Forms.ToolStripMenuItem removeEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem markWorkingHoursToolStripMenuItem;
      private System.Windows.Forms.ToolStripStatusLabel nextPoint;
   }
}

