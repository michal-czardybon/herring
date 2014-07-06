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
            this.keyboardIntensity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mouseIntensity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rulesTabPage = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelUserStatus = new System.Windows.Forms.Label();
            this.labelUserIs = new System.Windows.Forms.Label();
            this.buttonNextDay = new System.Windows.Forms.Button();
            this.buttonPrevDay = new System.Windows.Forms.Button();
            this.todayButton = new System.Windows.Forms.Button();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.mainTabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
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
            this.mainTabControl.Size = new System.Drawing.Size(975, 514);
            this.mainTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(967, 488);
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
            this.tabPage2.Size = new System.Drawing.Size(967, 488);
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
            this.keyboardIntensity,
            this.mouseIntensity});
            this.activitiesListView.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.activitiesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.activitiesListView.FullRowSelect = true;
            this.activitiesListView.GridLines = true;
            this.activitiesListView.Location = new System.Drawing.Point(3, 3);
            this.activitiesListView.Name = "activitiesListView";
            this.activitiesListView.Size = new System.Drawing.Size(961, 482);
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
            // keyboardIntensity
            // 
            this.keyboardIntensity.Text = "Keyboard";
            this.keyboardIntensity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.keyboardIntensity.Width = 70;
            // 
            // mouseIntensity
            // 
            this.mouseIntensity.Text = "Mouse";
            this.mouseIntensity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mouseIntensity.Width = 70;
            // 
            // rulesTabPage
            // 
            this.rulesTabPage.Location = new System.Drawing.Point(4, 22);
            this.rulesTabPage.Name = "rulesTabPage";
            this.rulesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.rulesTabPage.Size = new System.Drawing.Size(967, 488);
            this.rulesTabPage.TabIndex = 2;
            this.rulesTabPage.Text = "Rules";
            this.rulesTabPage.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelUserStatus);
            this.panel1.Controls.Add(this.labelUserIs);
            this.panel1.Controls.Add(this.buttonNextDay);
            this.panel1.Controls.Add(this.buttonPrevDay);
            this.panel1.Controls.Add(this.todayButton);
            this.panel1.Controls.Add(this.datePicker);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(975, 32);
            this.panel1.TabIndex = 1;
            // 
            // labelUserStatus
            // 
            this.labelUserStatus.AutoSize = true;
            this.labelUserStatus.Location = new System.Drawing.Point(418, 10);
            this.labelUserStatus.Name = "labelUserStatus";
            this.labelUserStatus.Size = new System.Drawing.Size(37, 13);
            this.labelUserStatus.TabIndex = 6;
            this.labelUserStatus.Text = "Active";
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
            this.ClientSize = new System.Drawing.Size(975, 546);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "Herring";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainTabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
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
        private System.Windows.Forms.ColumnHeader mouseIntensity;
        private System.Windows.Forms.ColumnHeader keyboardIntensity;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.Button todayButton;
        private System.Windows.Forms.TabPage rulesTabPage;
        private System.Windows.Forms.ColumnHeader shareHeader;
        private System.Windows.Forms.Button buttonPrevDay;
        private System.Windows.Forms.Button buttonNextDay;
        private System.Windows.Forms.Label labelUserStatus;
        private System.Windows.Forms.Label labelUserIs;
    }
}

