namespace SchedulingApp.Forms
{
    partial class ReportsForm
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
            this.tabReports = new System.Windows.Forms.TabControl();
            this.tabTypes = new System.Windows.Forms.TabPage();
            this.dgvTypes = new System.Windows.Forms.DataGridView();
            this.tabUserSchedule = new System.Windows.Forms.TabPage();
            this.dgvUserSchedule = new System.Windows.Forms.DataGridView();
            this.tabByCustomer = new System.Windows.Forms.TabPage();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.dgvByCustomer = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabReports.SuspendLayout();
            this.tabTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTypes)).BeginInit();
            this.tabUserSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserSchedule)).BeginInit();
            this.tabByCustomer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvByCustomer)).BeginInit();
            this.SuspendLayout();
            // 
            // tabReports
            // 
            this.tabReports.Controls.Add(this.tabTypes);
            this.tabReports.Controls.Add(this.tabUserSchedule);
            this.tabReports.Controls.Add(this.tabByCustomer);
            this.tabReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReports.Location = new System.Drawing.Point(0, 0);
            this.tabReports.Name = "tabReports";
            this.tabReports.SelectedIndex = 0;
            this.tabReports.Size = new System.Drawing.Size(569, 452);
            this.tabReports.TabIndex = 0;
            this.tabReports.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // tabTypes
            // 
            this.tabTypes.Controls.Add(this.button2);
            this.tabTypes.Controls.Add(this.dgvTypes);
            this.tabTypes.Location = new System.Drawing.Point(4, 22);
            this.tabTypes.Name = "tabTypes";
            this.tabTypes.Padding = new System.Windows.Forms.Padding(3);
            this.tabTypes.Size = new System.Drawing.Size(561, 426);
            this.tabTypes.TabIndex = 0;
            this.tabTypes.Text = "Appointment Types by Month";
            this.tabTypes.UseVisualStyleBackColor = true;
            // 
            // dgvTypes
            // 
            this.dgvTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTypes.Location = new System.Drawing.Point(3, 3);
            this.dgvTypes.Name = "dgvTypes";
            this.dgvTypes.Size = new System.Drawing.Size(555, 420);
            this.dgvTypes.TabIndex = 0;
            this.dgvTypes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTypes_CellContentClick);
            // 
            // tabUserSchedule
            // 
            this.tabUserSchedule.Controls.Add(this.button1);
            this.tabUserSchedule.Controls.Add(this.dgvUserSchedule);
            this.tabUserSchedule.Location = new System.Drawing.Point(4, 22);
            this.tabUserSchedule.Name = "tabUserSchedule";
            this.tabUserSchedule.Padding = new System.Windows.Forms.Padding(3);
            this.tabUserSchedule.Size = new System.Drawing.Size(561, 426);
            this.tabUserSchedule.TabIndex = 1;
            this.tabUserSchedule.Text = "User Schedules";
            this.tabUserSchedule.UseVisualStyleBackColor = true;
            // 
            // dgvUserSchedule
            // 
            this.dgvUserSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUserSchedule.Location = new System.Drawing.Point(3, 3);
            this.dgvUserSchedule.Name = "dgvUserSchedule";
            this.dgvUserSchedule.Size = new System.Drawing.Size(555, 420);
            this.dgvUserSchedule.TabIndex = 0;
            // 
            // tabByCustomer
            // 
            this.tabByCustomer.Controls.Add(this.btnGenerate);
            this.tabByCustomer.Controls.Add(this.dgvByCustomer);
            this.tabByCustomer.Location = new System.Drawing.Point(4, 22);
            this.tabByCustomer.Name = "tabByCustomer";
            this.tabByCustomer.Padding = new System.Windows.Forms.Padding(3);
            this.tabByCustomer.Size = new System.Drawing.Size(561, 426);
            this.tabByCustomer.TabIndex = 2;
            this.tabByCustomer.Text = "Appointments by Customer";
            this.tabByCustomer.UseVisualStyleBackColor = true;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(452, 387);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(101, 31);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Generate Reports";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dgvByCustomer
            // 
            this.dgvByCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvByCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvByCustomer.Location = new System.Drawing.Point(3, 3);
            this.dgvByCustomer.Name = "dgvByCustomer";
            this.dgvByCustomer.Size = new System.Drawing.Size(555, 420);
            this.dgvByCustomer.TabIndex = 0;
            this.dgvByCustomer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvByCustomer_CellContentClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(452, 387);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 31);
            this.button1.TabIndex = 2;
            this.button1.Text = "Generate Reports";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(452, 387);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 31);
            this.button2.TabIndex = 2;
            this.button2.Text = "Generate Reports";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // ReportsForm
            // 
            this.ClientSize = new System.Drawing.Size(569, 452);
            this.Controls.Add(this.tabReports);
            this.Name = "ReportsForm";
            this.tabReports.ResumeLayout(false);
            this.tabTypes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTypes)).EndInit();
            this.tabUserSchedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserSchedule)).EndInit();
            this.tabByCustomer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvByCustomer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabReports;
        private System.Windows.Forms.TabPage tabTypes;
        private System.Windows.Forms.TabPage tabUserSchedule;
        private System.Windows.Forms.TabPage tabByCustomer;
        private System.Windows.Forms.DataGridView dgvTypes;
        private System.Windows.Forms.DataGridView dgvUserSchedule;
        private System.Windows.Forms.DataGridView dgvByCustomer;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}