using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SchedulingApp.Data;
using SchedulingApp.Models;

namespace SchedulingApp.Forms
{
    public partial class CalendarForm : Form
    {
        private User _currentUser;
        private List<Appointment> _allAppointments;

        public CalendarForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            try
            {
                _allAppointments = AppointmentDAO.GetAllAppointments();

                // Convert UTC to local time
                _allAppointments.ForEach(a =>
                {
                    a.Start = a.Start.ToLocalTime();
                    a.End = a.End.ToLocalTime();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message);
            }
        }

        private void calMonth_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = e.Start.Date;
            lblSelectedDate.Text = "Appointments for: " + selectedDate.ToString("MMMM dd, yyyy");

            // Filter appointments for selected day using lambda
            var dayAppointments = _allAppointments
                .Where(a => a.Start.Date == selectedDate.Date)
                .ToList();

            dgvDayAppointments.DataSource = null;
            dgvDayAppointments.DataSource = dayAppointments;

            // Hide technical columns
            if (dgvDayAppointments.Columns.Count > 0)
            {
                dgvDayAppointments.Columns["AppointmentId"].Visible = false;
                dgvDayAppointments.Columns["CustomerId"].Visible = false;
                dgvDayAppointments.Columns["UserId"].Visible = false;
                dgvDayAppointments.Columns["Url"].Visible = false;
            }

            dgvDayAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDayAppointments.ReadOnly = true;

            if (dayAppointments.Count == 0)
                lblSelectedDate.Text += " — No appointments";
        }
    }
}