using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SchedulingApp.Models;

namespace SchedulingApp.Data
{
    public static class CustomerDAO
    {
        public static List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();

            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT c.customerId, c.customerName, c.addressId,
                               a.address, a.phone,
                               ci.city, co.country
                        FROM customer c
                        JOIN address a  ON c.addressId = a.addressId
                        JOIN city ci    ON a.cityId    = ci.cityId
                        JOIN country co ON ci.countryId = co.countryId
                        WHERE c.active = 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                CustomerId = reader.GetInt32("customerId"),
                                CustomerName = reader.GetString("customerName"),
                                AddressId = reader.GetInt32("addressId"),
                                Address1 = reader.GetString("address"),
                                Phone = reader.GetString("phone"),
                                City = reader.GetString("city"),
                                Country = reader.GetString("country"),
                                Active = true
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving customers: " + ex.Message);
            }

            return customers;
        }

        public static void AddCustomer(Customer customer, string createdBy)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Insert country
                            int countryId = InsertOrGetCountry(conn, transaction, customer.Country, createdBy);

                            // 2. Insert city
                            int cityId = InsertOrGetCity(conn, transaction, customer.City, countryId, createdBy);

                            // 3. Insert address
                            int addressId = InsertAddress(conn, transaction, customer.Address1, cityId, customer.Phone, createdBy);

                            // 4. Insert customer
                            string query = @"
                                INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy)
                                VALUES (@name, @addressId, 1, NOW(), @createdBy, NOW(), @createdBy)";

                            using (var cmd = new MySqlCommand(query, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@name", customer.CustomerName);
                                cmd.Parameters.AddWithValue("@addressId", addressId);
                                cmd.Parameters.AddWithValue("@createdBy", createdBy);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding customer: " + ex.Message);
            }
        }

        public static void UpdateCustomer(Customer customer, string updatedBy)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            int countryId = InsertOrGetCountry(conn, transaction, customer.Country, updatedBy);
                            int cityId = InsertOrGetCity(conn, transaction, customer.City, countryId, updatedBy);

                            // Update address
                            string updateAddress = @"
                                UPDATE address SET address = @address, cityId = @cityId,
                                phone = @phone, lastUpdate = NOW(), lastUpdateBy = @updatedBy
                                WHERE addressId = @addressId";

                            using (var cmd = new MySqlCommand(updateAddress, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@address", customer.Address1);
                                cmd.Parameters.AddWithValue("@cityId", cityId);
                                cmd.Parameters.AddWithValue("@phone", customer.Phone);
                                cmd.Parameters.AddWithValue("@updatedBy", updatedBy);
                                cmd.Parameters.AddWithValue("@addressId", customer.AddressId);
                                cmd.ExecuteNonQuery();
                            }

                            // Update customer
                            string updateCustomer = @"
                                UPDATE customer SET customerName = @name,
                                lastUpdate = NOW(), lastUpdateBy = @updatedBy
                                WHERE customerId = @customerId";

                            using (var cmd = new MySqlCommand(updateCustomer, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@name", customer.CustomerName);
                                cmd.Parameters.AddWithValue("@updatedBy", updatedBy);
                                cmd.Parameters.AddWithValue("@customerId", customer.CustomerId);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating customer: " + ex.Message);
            }
        }

        public static void DeleteCustomer(int customerId)
        {
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Delete appointments first to avoid FK constraint errors
                            string deleteAppts = "DELETE FROM appointment WHERE customerId = @customerId";
                            using (var cmd = new MySqlCommand(deleteAppts, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@customerId", customerId);
                                cmd.ExecuteNonQuery();
                            }

                            // Soft delete — sets active = 0 rather than removing the row
                            string deleteCustomer = "UPDATE customer SET active = 0 WHERE customerId = @customerId";
                            using (var cmd = new MySqlCommand(deleteCustomer, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@customerId", customerId);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting customer: " + ex.Message);
            }
        }

        // ── Private helpers ──────────────────────────────────────────────────

        private static int InsertOrGetCountry(MySqlConnection conn, MySqlTransaction t, string countryName, string user)
        {
            string select = "SELECT countryId FROM country WHERE country = @country LIMIT 1";
            using (var cmd = new MySqlCommand(select, conn, t))
            {
                cmd.Parameters.AddWithValue("@country", countryName);
                var result = cmd.ExecuteScalar();
                if (result != null) return Convert.ToInt32(result);
            }

            string insert = "INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                            "VALUES (@country, NOW(), @user, NOW(), @user)";
            using (var cmd = new MySqlCommand(insert, conn, t))
            {
                cmd.Parameters.AddWithValue("@country", countryName);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        private static int InsertOrGetCity(MySqlConnection conn, MySqlTransaction t, string cityName, int countryId, string user)
        {
            string select = "SELECT cityId FROM city WHERE city = @city AND countryId = @countryId LIMIT 1";
            using (var cmd = new MySqlCommand(select, conn, t))
            {
                cmd.Parameters.AddWithValue("@city", cityName);
                cmd.Parameters.AddWithValue("@countryId", countryId);
                var result = cmd.ExecuteScalar();
                if (result != null) return Convert.ToInt32(result);
            }

            string insert = "INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                            "VALUES (@city, @countryId, NOW(), @user, NOW(), @user)";
            using (var cmd = new MySqlCommand(insert, conn, t))
            {
                cmd.Parameters.AddWithValue("@city", cityName);
                cmd.Parameters.AddWithValue("@countryId", countryId);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        private static int InsertAddress(MySqlConnection conn, MySqlTransaction t, string address, int cityId, string phone, string user)
        {
            string insert = @"
                INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy)
                VALUES (@address, '', @cityId, '', @phone, NOW(), @user, NOW(), @user)";
            using (var cmd = new MySqlCommand(insert, conn, t))
            {
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@cityId", cityId);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }
    }
}