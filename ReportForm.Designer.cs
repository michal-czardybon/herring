namespace Herring
{
    partial class ReportForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.datePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.datePickerTo = new System.Windows.Forms.DateTimePicker();
            this.calculateButton = new System.Windows.Forms.Button();
            this.chartBox1 = new System.Windows.Forms.PictureBox();
            this.chartBox2 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.calculateButton);
            this.panel1.Controls.Add(this.datePickerTo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.datePickerFrom);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(773, 58);
            this.panel1.TabIndex = 2;
            // 
            // datePickerFrom
            // 
            this.datePickerFrom.CustomFormat = "dddd, dd MMMM yyyy";
            this.datePickerFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datePickerFrom.Location = new System.Drawing.Point(42, 3);
            this.datePickerFrom.Name = "datePickerFrom";
            this.datePickerFrom.Size = new System.Drawing.Size(257, 22);
            this.datePickerFrom.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "To:";
            // 
            // datePickerTo
            // 
            this.datePickerTo.CustomFormat = "dddd, dd MMMM yyyy";
            this.datePickerTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datePickerTo.Location = new System.Drawing.Point(42, 31);
            this.datePickerTo.Name = "datePickerTo";
            this.datePickerTo.Size = new System.Drawing.Size(257, 22);
            this.datePickerTo.TabIndex = 3;
            // 
            // calculateButton
            // 
            this.calculateButton.Location = new System.Drawing.Point(306, 3);
            this.calculateButton.Name = "calculateButton";
            this.calculateButton.Size = new System.Drawing.Size(147, 50);
            this.calculateButton.TabIndex = 4;
            this.calculateButton.Text = "Calculate";
            this.calculateButton.UseVisualStyleBackColor = true;
            this.calculateButton.Click += new System.EventHandler(this.calculateButton_Click);
            // 
            // chartBox1
            // 
            this.chartBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chartBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.chartBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.chartBox1.Location = new System.Drawing.Point(0, 58);
            this.chartBox1.Name = "chartBox1";
            this.chartBox1.Size = new System.Drawing.Size(773, 158);
            this.chartBox1.TabIndex = 3;
            this.chartBox1.TabStop = false;
            // 
            // chartBox2
            // 
            this.chartBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chartBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.chartBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.chartBox2.Location = new System.Drawing.Point(0, 216);
            this.chartBox2.Name = "chartBox2";
            this.chartBox2.Size = new System.Drawing.Size(773, 158);
            this.chartBox2.TabIndex = 4;
            this.chartBox2.TabStop = false;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 378);
            this.Controls.Add(this.chartBox2);
            this.Controls.Add(this.chartBox1);
            this.Controls.Add(this.panel1);
            this.Name = "ReportForm";
            this.Text = "Report (Under construction)";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker datePickerFrom;
        private System.Windows.Forms.Button calculateButton;
        private System.Windows.Forms.DateTimePicker datePickerTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox chartBox1;
        private System.Windows.Forms.PictureBox chartBox2;
    }
}