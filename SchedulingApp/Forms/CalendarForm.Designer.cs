namespace SchedulingApp.Forms
{
    partial class CalendarForm
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
            this.calMonth = new System.Windows.Forms.MonthCalendar();
            this.dgvDayAppointments = new System.Windows.Forms.DataGridView();
            this.lblSelectedDate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDayAppointments)).BeginInit();
            this.SuspendLayout();
            // 
            // calMonth
            // 
            this.calMonth.Location = new System.Drawing.Point(40, 200);
            this.calMonth.Name = "calMonth";
            this.calMonth.TabIndex = 0;
            this.calMonth.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calMonth_DateSelected);
            // 
            // dgvDayAppointments
            // 
            this.dgvDayAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDayAppointments.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvDayAppointments.Location = new System.Drawing.Point(0, 0);
            this.dgvDayAppointments.Name = "dgvDayAppointments";
            this.dgvDayAppointments.Size = new System.Drawing.Size(594, 150);
            this.dgvDayAppointments.TabIndex = 1;
            // 
            // lblSelectedDate
            // 
            this.lblSelectedDate.AutoSize = true;
            this.lblSelectedDate.Location = new System.Drawing.Point(279, 200);
            this.lblSelectedDate.Name = "lblSelectedDate";
            this.lblSelectedDate.Size = new System.Drawing.Size(103, 13);
            this.lblSelectedDate.TabIndex = 2;
            this.lblSelectedDate.Text = "Please select a date";
            // 
            // CalendarForm
            // 
            this.ClientSize = new System.Drawing.Size(594, 455);
            this.Controls.Add(this.lblSelectedDate);
            this.Controls.Add(this.dgvDayAppointments);
            this.Controls.Add(this.calMonth);
            this.Name = "CalendarForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDayAppointments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar calMonth;
        private System.Windows.Forms.DataGridView dgvDayAppointments;
        private System.Windows.Forms.Label lblSelectedDate;
    }
}