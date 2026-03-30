using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SchedulingApp.Data;
using SchedulingApp.Models;

namespace SchedulingApp.Forms
{
    public partial class ReportsForm : Form
    {
        private User _currentUser;

        public ReportsForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateAppointmentTypesByMonth();
            GenerateUserSchedules();
            GenerateAppointmentsByCustomer();
        }
        private void GenerateAppointmentTypesByMonth()
        {
            try
            {
                var appointments = AppointmentDAO.GetAllAppointments();

                Func<Appointment, string> getMonth = a => a.Start.ToLocalTime().ToString("MMMM yyyy");
                Func<Appointment, string> getType = a => a.Type;

                var report = appointments
                    .GroupBy(a => new { Month = getMonth(a), Type = getType(a) })
                    .Select(g => new
                    {
                        Month = g.Key.Month,
                        AppointmentType = g.Key.Type,
                        Count = g.Count()
                    })
                    .OrderBy(r => r.Month)
                    .ToList();

                dgvTypes.DataSource = null;
                dgvTypes.DataSource = report;
                dgvTypes.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating types report: " + ex.Message);
            }
        }
        private void GenerateUserSchedules()
        {
            try
            {
                var appointments = AppointmentDAO.GetAllAppointments();

                var report = appointments
                    .OrderBy(a => a.UserId)
                    .ThenBy(a => a.Start)
                    .Select(a => new
                    {
                        UserId = a.UserId,
                        Customer = a.CustomerName,
                        Title = a.Title,
                        Type = a.Type,
                        Start = a.Start.ToLocalTime().ToString("yyyy-MM-dd hh:mm tt"),
                        End = a.End.ToLocalTime().ToString("yyyy-MM-dd hh:mm tt")
                    })
                    .ToList();

                dgvUserSchedule.DataSource = null;
                dgvUserSchedule.DataSource = report;
                dgvUserSchedule.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating user schedule report: " + ex.Message);
            }
        }
        private void GenerateAppointmentsByCustomer()
        {
            try
            {
                var appointments = AppointmentDAO.GetAllAppointments();

                var report = appointments
                    .GroupBy(a => a.CustomerName)
                    .Select(g => new
                    {
                        Customer = g.Key,
                        TotalAppointments = g.Count(),
                        NextAppointment = g.OrderBy(a => a.Start)
                                             .Where(a => a.Start.ToLocalTime() >= DateTime.Now)
                                             .Select(a => a.Start.ToLocalTime().ToString("yyyy-MM-dd hh:mm tt"))
                                             .FirstOrDefault() ?? "None upcoming"
                    })
                    .OrderBy(r => r.Customer)
                    .ToList();

                dgvByCustomer.DataSource = null;
                dgvByCustomer.DataSource = report;
                dgvByCustomer.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating customer report: " + ex.Message);
            }
        }

        private void dgvByCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}