using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SchedulingApp.Data;
using SchedulingApp.Models;

namespace SchedulingApp.Forms
{
    public partial class AppointmentForm : Form
    {
        private User _currentUser;
        private List<Appointment> _appointments;
        private List<Customer> _customers;
        private int _selectedAppointmentId = -1;

        public AppointmentForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadCustomers();
            LoadAppointments();
        }

        private void LoadCustomers()
        {
            try
            {
                _customers = CustomerDAO.GetAllCustomers();
                cmbCustomer.DataSource = _customers;
                cmbCustomer.DisplayMember = "CustomerName";
                cmbCustomer.ValueMember = "CustomerId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message);
            }
        }

        private void LoadAppointments()
        {
            try
            {
                _appointments = AppointmentDAO.GetAllAppointments();

                _appointments.ForEach(a =>
                {
                    a.Start = a.Start.ToLocalTime();
                    a.End = a.End.ToLocalTime();
                });

                dgvAppointments.DataSource = null;
                dgvAppointments.DataSource = _appointments;
                dgvAppointments.Columns["AppointmentId"].Visible = false;
                dgvAppointments.Columns["CustomerId"].Visible = false;
                dgvAppointments.Columns["UserId"].Visible = false;
                dgvAppointments.Columns["Url"].Visible = false;
                dgvAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvAppointments.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message);
            }
        }

        private void dgvAppointments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count > 0)
            {
                var row = dgvAppointments.SelectedRows[0];
                _selectedAppointmentId = (int)row.Cells["AppointmentId"].Value;

                int customerId = (int)row.Cells["CustomerId"].Value;
                cmbCustomer.SelectedValue = customerId;

                txtTitle.Text = row.Cells["Title"].Value.ToString();
                txtDescription.Text = row.Cells["Description"].Value.ToString();
                txtLocation.Text = row.Cells["Location"].Value.ToString();
                txtContact.Text = row.Cells["Contact"].Value.ToString();
                txtType.Text = row.Cells["Type"].Value.ToString();
                txtUrl.Text = row.Cells["Url"].Value.ToString();
                dtpStart.Value = (DateTime)row.Cells["Start"].Value;
                dtpEnd.Value = (DateTime)row.Cells["End"].Value;
            }
        }

        private bool ValidateInputs()
        {
            if (cmbCustomer.SelectedItem == null)
            {
                MessageBox.Show("Please select a customer.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(txtLocation.Text) ||
                string.IsNullOrWhiteSpace(txtContact.Text) ||
                string.IsNullOrWhiteSpace(txtType.Text))
            {
                MessageBox.Show("Title, Description, Location, Contact, and Type are required.");
                return false;
            }

            if (dtpEnd.Value <= dtpStart.Value)
            {
                MessageBox.Show("End time must be after start time.");
                return false;
            }

            if (!IsWithinBusinessHours(dtpStart.Value, dtpEnd.Value))
            {
                MessageBox.Show("Appointments must be scheduled Monday-Friday between 9:00 AM and 5:00 PM Eastern Standard Time.");
                return false;
            }

            if (HasOverlappingAppointment(dtpStart.Value, dtpEnd.Value, _selectedAppointmentId))
            {
                MessageBox.Show("This appointment overlaps with an existing appointment.");
                return false;
            }

            return true;
        }

        private bool IsWithinBusinessHours(DateTime start, DateTime end)
        {
            TimeZoneInfo eastern = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime estStart = TimeZoneInfo.ConvertTime(start, TimeZoneInfo.Local, eastern);
            DateTime estEnd = TimeZoneInfo.ConvertTime(end, TimeZoneInfo.Local, eastern);

            if (estStart.DayOfWeek == DayOfWeek.Saturday ||
                estStart.DayOfWeek == DayOfWeek.Sunday ||
                estEnd.DayOfWeek == DayOfWeek.Saturday ||
                estEnd.DayOfWeek == DayOfWeek.Sunday)
                return false;

            TimeSpan businessStart = new TimeSpan(9, 0, 0);
            TimeSpan businessEnd = new TimeSpan(17, 0, 0);

            if (estStart.TimeOfDay < businessStart || estEnd.TimeOfDay > businessEnd)
                return false;

            return true;
        }

        private bool HasOverlappingAppointment(DateTime start, DateTime end, int excludeId)
        {
            foreach (var appt in _appointments)
            {
                if (appt.AppointmentId == excludeId) continue;

                if (start < appt.End && end > appt.Start)
                    return true;
            }
            return false;
        }

        private Appointment BuildAppointmentFromInputs()
        {
            var customer = (Customer)cmbCustomer.SelectedItem;
            return new Appointment
            {
                AppointmentId = _selectedAppointmentId,
                CustomerId = customer.CustomerId,
                UserId = _currentUser.UserId,
                Title = txtTitle.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Location = txtLocation.Text.Trim(),
                Contact = txtContact.Text.Trim(),
                Type = txtType.Text.Trim(),
                Url = txtUrl.Text.Trim(),
                Start = DateTime.SpecifyKind(dtpStart.Value, DateTimeKind.Local),
                End = DateTime.SpecifyKind(dtpEnd.Value, DateTimeKind.Local)
            };
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                var appt = BuildAppointmentFromInputs();
                AppointmentDAO.AddAppointment(appt, _currentUser.UserName);
                MessageBox.Show("Appointment added successfully!");
                ClearInputs();
                LoadAppointments();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding appointment: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedAppointmentId == -1)
            {
                MessageBox.Show("Please select an appointment to update.");
                return;
            }

            if (!ValidateInputs()) return;

            try
            {
                var appt = BuildAppointmentFromInputs();
                AppointmentDAO.UpdateAppointment(appt, _currentUser.UserName);
                MessageBox.Show("Appointment updated successfully!");
                ClearInputs();
                LoadAppointments();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating appointment: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedAppointmentId == -1)
            {
                MessageBox.Show("Please select an appointment to delete.");
                return;
            }

            var confirm = MessageBox.Show(
                "Are you sure you want to delete this appointment?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                AppointmentDAO.DeleteAppointment(_selectedAppointmentId);
                MessageBox.Show("Appointment deleted successfully!");
                ClearInputs();
                LoadAppointments();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting appointment: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void ClearInputs()
        {
            _selectedAppointmentId = -1;
            if (cmbCustomer.Items.Count > 0)
                cmbCustomer.SelectedIndex = 0;
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtLocation.Text = "";
            txtContact.Text = "";
            txtType.Text = "";
            txtUrl.Text = "";
            dtpStart.Value = DateTime.Now;
            dtpEnd.Value = DateTime.Now.AddHours(1);
            dgvAppointments.ClearSelection();
        }

        private void AppointmentForm_Load(object sender, EventArgs e)
        {

        }
    }
}