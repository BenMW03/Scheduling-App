using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SchedulingApp.Models;
using SchedulingApp.Helpers;

namespace SchedulingApp.Data
{
    public static class AppointmentDAO
    {
        public static List<Appointment> GetAllAppointments()
        {
            var appointments = new List<Appointment>();

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT a.appointmentId, a.customerId, a.userId,
                               a.title, a.description, a.location, a.contact,
                               a.type, a.url, a.start, a.end,
                               c.customerName
                        FROM appointment a
                        JOIN customer c ON a.customerId = c.customerId";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appointments.Add(new Appointment
                            {
                                AppointmentId = reader.GetInt32("appointmentId"),
                                CustomerId = reader.GetInt32("customerId"),
                                UserId = reader.GetInt32("userId"),
                                Title = reader.GetString("title"),
                                Description = reader.GetString("description"),
                                Location = reader.GetString("location"),
                                Contact = reader.GetString("contact"),
                                Type = reader.GetString("type"),
                                Url = reader.GetString("url"),
                                // Convert UTC from DB to local time accounting for DST
                                Start = TimeZoneHelper.ToLocalTime(reader.GetDateTime("start")),
                                End = TimeZoneHelper.ToLocalTime(reader.GetDateTime("end")),
                                CustomerName = reader.GetString("customerName")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving appointments: " + ex.Message);
            }

            return appointments;
        }

        public static List<Appointment> GetAppointmentsByUser(int userId)
        {
            var appointments = new List<Appointment>();

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT a.appointmentId, a.customerId, a.userId,
                               a.title, a.description, a.location, a.contact,
                               a.type, a.url, a.start, a.end,
                               c.customerName
                        FROM appointment a
                        JOIN customer c ON a.customerId = c.customerId
                        WHERE a.userId = @userId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                appointments.Add(new Appointment
                                {
                                    AppointmentId = reader.GetInt32("appointmentId"),
                                    CustomerId = reader.GetInt32("customerId"),
                                    UserId = reader.GetInt32("userId"),
                                    Title = reader.GetString("title"),
                                    Description = reader.GetString("description"),
                                    Location = reader.GetString("location"),
                                    Contact = reader.GetString("contact"),
                                    Type = reader.GetString("type"),
                                    Url = reader.GetString("url"),
                                    // Convert UTC from DB to local time accounting for DST
                                    Start = TimeZoneHelper.ToLocalTime(reader.GetDateTime("start")),
                                    End = TimeZoneHelper.ToLocalTime(reader.GetDateTime("end")),
                                    CustomerName = reader.GetString("customerName")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving appointments by user: " + ex.Message);
            }

            return appointments;
        }

        public static void AddAppointment(Appointment appt, string createdBy)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        INSERT INTO appointment
                            (customerId, userId, title, description, location, contact,
                             type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)
                        VALUES
                            (@customerId, @userId, @title, @description, @location, @contact,
                             @type, @url, @start, @end, NOW(), @createdBy, NOW(), @createdBy)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", appt.CustomerId);
                        cmd.Parameters.AddWithValue("@userId", appt.UserId);
                        cmd.Parameters.AddWithValue("@title", appt.Title);
                        cmd.Parameters.AddWithValue("@description", appt.Description);
                        cmd.Parameters.AddWithValue("@location", appt.Location);
                        cmd.Parameters.AddWithValue("@contact", appt.Contact);
                        cmd.Parameters.AddWithValue("@type", appt.Type);
                        cmd.Parameters.AddWithValue("@url", appt.Url);
                        // Convert local time to UTC for storage, accounting for DST
                        cmd.Parameters.AddWithValue("@start", TimeZoneHelper.ToUtcTime(appt.Start));
                        cmd.Parameters.AddWithValue("@end", TimeZoneHelper.ToUtcTime(appt.End));
                        cmd.Parameters.AddWithValue("@createdBy", createdBy);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding appointment: " + ex.Message);
            }
        }

        public static void UpdateAppointment(Appointment appt, string updatedBy)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        UPDATE appointment SET
                            customerId = @customerId, userId = @userId,
                            title = @title, description = @description,
                            location = @location, contact = @contact,
                            type = @type, url = @url,
                            start = @start, end = @end,
                            lastUpdate = NOW(), lastUpdateBy = @updatedBy
                        WHERE appointmentId = @appointmentId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", appt.CustomerId);
                        cmd.Parameters.AddWithValue("@userId", appt.UserId);
                        cmd.Parameters.AddWithValue("@title", appt.Title);
                        cmd.Parameters.AddWithValue("@description", appt.Description);
                        cmd.Parameters.AddWithValue("@location", appt.Location);
                        cmd.Parameters.AddWithValue("@contact", appt.Contact);
                        cmd.Parameters.AddWithValue("@type", appt.Type);
                        cmd.Parameters.AddWithValue("@url", appt.Url);
                        // Convert local time to UTC for storage, accounting for DST
                        cmd.Parameters.AddWithValue("@start", TimeZoneHelper.ToUtcTime(appt.Start));
                        cmd.Parameters.AddWithValue("@end", TimeZoneHelper.ToUtcTime(appt.End));
                        cmd.Parameters.AddWithValue("@updatedBy", updatedBy);
                        cmd.Parameters.AddWithValue("@appointmentId", appt.AppointmentId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating appointment: " + ex.Message);
            }
        }

        public static void DeleteAppointment(int appointmentId)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM appointment WHERE appointmentId = @appointmentId";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@appointmentId", appointmentId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting appointment: " + ex.Message);
            }
        }
    }
}