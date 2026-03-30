using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using SchedulingApp.Data;
using SchedulingApp.Models;

namespace SchedulingApp.Forms
{
    public partial class LoginForm : Form
    {
        private string _userLanguage;

        public LoginForm()
        {
            InitializeComponent();
            DetectLanguage();
            TranslateUI();
        }

        private void DetectLanguage()
        {
            _userLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            if (_userLanguage != "es")
                _userLanguage = "en";
        }

        private void TranslateUI()
        {
            if (_userLanguage == "es")
            {
                lblTitle.Text = "Iniciar Sesión";
                lblUsername.Text = "Usuario:";
                lblPassword.Text = "Contraseña:";
                btnLogin.Text = "Entrar";
                lblLocation.Text = "Ubicación: " + GetUserLocation();
            }
            else
            {
                lblTitle.Text = "Login";
                lblUsername.Text = "Username:";
                lblPassword.Text = "Password:";
                btnLogin.Text = "Login";
                lblLocation.Text = "Location: " + GetUserLocation();
            }
        }

        private string GetUserLocation()
        {
            return Helpers.TimeZoneHelper.GetUserTimeZoneName();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                string msg = _userLanguage == "es"
                    ? "Por favor ingrese usuario y contraseña."
                    : "Please enter a username and password.";
                MessageBox.Show(msg);
                return;
            }

            try
            {
                User user = UserDAO.GetUserByCredentials(username, password);

                if (user == null)
                {
                    string msg = _userLanguage == "es"
                        ? "El nombre de usuario y la contraseña no coinciden."
                        : "The username and password do not match.";
                    MessageBox.Show(msg);
                    LogLogin(username, false);
                    return;
                }

                LogLogin(username, true);
                CheckUpcomingAppointments(user);

                var mainForm = new MainForm(user);
                mainForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error: " + ex.Message);
            }
        }

        private void CheckUpcomingAppointments(User user)
        {
            try
            {
                var appointments = AppointmentDAO.GetAppointmentsByUser(user.UserId);
                DateTime now = DateTime.Now;

                var upcoming = appointments.FindAll(a =>
                    a.Start.ToLocalTime() > now &&
                    a.Start.ToLocalTime() <= now.AddMinutes(15));

                if (upcoming.Count > 0)
                {
                    var appt = upcoming[0];
                    string msg = $"You have an appointment in less than 15 minutes!\n\n" +
                                 $"Type: {appt.Type}\n" +
                                 $"Customer: {appt.CustomerName}\n" +
                                 $"Time: {appt.Start.ToLocalTime():hh:mm tt}";
                    MessageBox.Show(msg, "Upcoming Appointment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking appointments: " + ex.Message);
            }
        }

        private void LogLogin(string username, bool success)
        {
            try
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Login_History.txt");
                string status = success ? "SUCCESS" : "FAILED";
                string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {username} | {status}";
                File.AppendAllText(logPath, entry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Logging error: " + ex.Message);
            }
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void lblLocation_Click(object sender, EventArgs e)
        {

        }
    }
}