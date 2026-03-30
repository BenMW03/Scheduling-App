using System;
using System.Windows.Forms;
using SchedulingApp.Models;

namespace SchedulingApp.Forms
{
    public partial class MainForm : Form
    {
        private User _currentUser;

        public MainForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            lblWelcome.Text = $"Welcome, {_currentUser.UserName}!";
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            var form = new CustomerForm(_currentUser);
            form.ShowDialog();
        }

        private void btnAppointments_Click(object sender, EventArgs e)
        {
            var form = new AppointmentForm(_currentUser);
            form.ShowDialog();
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            var form = new CalendarForm(_currentUser);
            form.ShowDialog();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            var form = new ReportsForm(_currentUser);
            form.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            var loginForm = new LoginForm();
            loginForm.Show();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}