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
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.autoScrollCheckBox = new System.Windows.Forms.CheckBox();
            this.labelUserStatus = new System.Windows.Forms.Label();
            this.labelUserIs = new System.Windows.Forms.Label();
            this.buttonNextDay = new System.Windows.Forms.Button();
            this.buttonPrevDay = new System.Windows.Forms.Button();
            this.todayButton = new System.Windows.Forms.Button();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.mainTabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.rulesTabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 2000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.tabPage1);
            this.mainTabControl.Controls.Add(this.tabPage2);
            this.mainTabControl.Controls.Add(this.rulesTabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 32);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1260, 514);
            this.mainTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1102, 488);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tasks";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.activitiesListView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1252, 488);
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
            this.activitiesListView.Size = new System.Drawing.Size(1246, 482);
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
            this.rulesTabPage.Size = new System.Drawing.Size(1252, 488);
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
            this.rulesListView.Size = new System.Drawing.Size(1246, 482);
            this.rulesListView.TabIndex = 0;
            this.rulesListView.UseCompatibleStateImageBehavior = false;
            this.rulesListView.View = System.Windows.Forms.View.Details;
            // 
            // processHeader
            // 
            this.processHeader.Text = "process";
            this.processHeader.Width = 120;
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
            // panel1
            // 
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 546);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "Herring";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainTabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.rulesTabPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage tabPage1;
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
    }
}

