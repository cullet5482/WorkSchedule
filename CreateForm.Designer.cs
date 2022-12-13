namespace WorkSchedule
{
    partial class CreateForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroRadioButton1 = new MetroFramework.Controls.MetroRadioButton();
            this.metroRadioButton2 = new MetroFramework.Controls.MetroRadioButton();
            this.metroRadioButton3 = new MetroFramework.Controls.MetroRadioButton();
            this.workerLabel1 = new MetroFramework.Controls.MetroLabel();
            this.workerLabel2 = new MetroFramework.Controls.MetroLabel();
            this.workerLabel3 = new MetroFramework.Controls.MetroLabel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.okButton = new MetroFramework.Controls.MetroButton();
            this.monthPicker = new WorkSchedule.MonthPicker();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(20, 60);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 264);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.Controls.Add(this.metroLabel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.metroLabel2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.metroRadioButton1, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.metroRadioButton2, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.metroRadioButton3, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.workerLabel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.workerLabel2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.workerLabel3, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.66667F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(476, 168);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroLabel1.Location = new System.Drawing.Point(4, 1);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(325, 32);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "이름";
            this.metroLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroLabel2.Location = new System.Drawing.Point(336, 1);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(136, 32);
            this.metroLabel2.TabIndex = 1;
            this.metroLabel2.Text = "첫날 대체";
            this.metroLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // metroRadioButton1
            // 
            this.metroRadioButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroRadioButton1.AutoSize = true;
            this.metroRadioButton1.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroRadioButton1.Checked = true;
            this.metroRadioButton1.Location = new System.Drawing.Point(380, 48);
            this.metroRadioButton1.Name = "metroRadioButton1";
            this.metroRadioButton1.Size = new System.Drawing.Size(47, 15);
            this.metroRadioButton1.TabIndex = 2;
            this.metroRadioButton1.TabStop = true;
            this.metroRadioButton1.Text = "대체";
            this.metroRadioButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroRadioButton1.UseSelectable = true;
            // 
            // metroRadioButton2
            // 
            this.metroRadioButton2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroRadioButton2.AutoSize = true;
            this.metroRadioButton2.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroRadioButton2.Location = new System.Drawing.Point(380, 92);
            this.metroRadioButton2.Name = "metroRadioButton2";
            this.metroRadioButton2.Size = new System.Drawing.Size(47, 15);
            this.metroRadioButton2.TabIndex = 3;
            this.metroRadioButton2.Text = "대체";
            this.metroRadioButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroRadioButton2.UseSelectable = true;
            // 
            // metroRadioButton3
            // 
            this.metroRadioButton3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroRadioButton3.AutoSize = true;
            this.metroRadioButton3.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroRadioButton3.Location = new System.Drawing.Point(380, 137);
            this.metroRadioButton3.Name = "metroRadioButton3";
            this.metroRadioButton3.Size = new System.Drawing.Size(47, 15);
            this.metroRadioButton3.TabIndex = 4;
            this.metroRadioButton3.Text = "대체";
            this.metroRadioButton3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroRadioButton3.UseSelectable = true;
            // 
            // workerLabel1
            // 
            this.workerLabel1.AutoSize = true;
            this.workerLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workerLabel1.Location = new System.Drawing.Point(4, 34);
            this.workerLabel1.Name = "workerLabel1";
            this.workerLabel1.Size = new System.Drawing.Size(325, 43);
            this.workerLabel1.TabIndex = 5;
            this.workerLabel1.Text = "metroLabel3";
            this.workerLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // workerLabel2
            // 
            this.workerLabel2.AutoSize = true;
            this.workerLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workerLabel2.Location = new System.Drawing.Point(4, 78);
            this.workerLabel2.Name = "workerLabel2";
            this.workerLabel2.Size = new System.Drawing.Size(325, 43);
            this.workerLabel2.TabIndex = 6;
            this.workerLabel2.Text = "metroLabel4";
            this.workerLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // workerLabel3
            // 
            this.workerLabel3.AutoSize = true;
            this.workerLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workerLabel3.Location = new System.Drawing.Point(4, 122);
            this.workerLabel3.Name = "workerLabel3";
            this.workerLabel3.Size = new System.Drawing.Size(325, 45);
            this.workerLabel3.TabIndex = 7;
            this.workerLabel3.Text = "metroLabel5";
            this.workerLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.Controls.Add(this.okButton, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.monthPicker, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(4, 179);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(476, 81);
            this.tableLayoutPanel3.TabIndex = 1;
            this.tableLayoutPanel3.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel3_Paint);
            // 
            // okButton
            // 
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.okButton.Location = new System.Drawing.Point(366, 29);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "확인";
            this.okButton.UseSelectable = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // monthPicker
            // 
            this.monthPicker.CustomFormat = "yyyy MMMM";
            this.monthPicker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.monthPicker.Location = new System.Drawing.Point(31, 31);
            this.monthPicker.Margin = new System.Windows.Forms.Padding(30);
            this.monthPicker.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.monthPicker.Name = "monthPicker";
            this.monthPicker.Size = new System.Drawing.Size(271, 21);
            this.monthPicker.TabIndex = 0;
            // 
            // CreateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 344);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "CreateForm";
            this.Resizable = false;
            this.Text = "근무표 생성";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroRadioButton metroRadioButton1;
        private MetroFramework.Controls.MetroRadioButton metroRadioButton2;
        private MetroFramework.Controls.MetroRadioButton metroRadioButton3;
        private MetroFramework.Controls.MetroLabel workerLabel1;
        private MetroFramework.Controls.MetroLabel workerLabel2;
        private MetroFramework.Controls.MetroLabel workerLabel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private MonthPicker monthPicker;
        private MetroFramework.Controls.MetroButton okButton;
    }
}