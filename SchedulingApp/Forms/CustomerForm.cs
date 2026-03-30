using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SchedulingApp.Data;
using SchedulingApp.Models;

namespace SchedulingApp.Forms
{
    public partial class CustomerForm : Form
    {
        private User _currentUser;
        private List<Customer> _customers;
        private int _selectedCustomerId = -1;
        private int _selectedAddressId = -1;

        public CustomerForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                _customers = CustomerDAO.GetAllCustomers();
                dgvCustomers.DataSource = null;
                dgvCustomers.DataSource = _customers;
                dgvCustomers.Columns["CustomerId"].Visible = false;
                dgvCustomers.Columns["AddressId"].Visible = false;
                dgvCustomers.Columns["Active"].Visible = false;
                dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvCustomers.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message);
            }
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                var row = dgvCustomers.SelectedRows[0];
                _selectedCustomerId = (int)row.Cells["CustomerId"].Value;
                _selectedAddressId = (int)row.Cells["AddressId"].Value;
                txtName.Text = row.Cells["CustomerName"].Value.ToString();
                txtAddress.Text = row.Cells["Address1"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                txtCity.Text = row.Cells["City"].Value.ToString();
                txtCountry.Text = row.Cells["Country"].Value.ToString();
            }
        }

        private bool ValidateInputs()
        {
            // Check non-empty
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtCity.Text) ||
                string.IsNullOrWhiteSpace(txtCountry.Text))
            {
                MessageBox.Show("All fields are required and cannot be empty.");
                return false;
            }

            // Check phone — digits and dashes only
            if (!Regex.IsMatch(txtPhone.Text.Trim(), @"^[\d\-]+$"))
            {
                MessageBox.Show("Phone number can only contain digits and dashes (e.g. 555-123-4567).");
                return false;
            }

            return true;
        }

        private Customer BuildCustomerFromInputs()
        {
            return new Customer
            {
                CustomerId = _selectedCustomerId,
                AddressId = _selectedAddressId,
                CustomerName = txtName.Text.Trim(),
                Address1 = txtAddress.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                City = txtCity.Text.Trim(),
                Country = txtCountry.Text.Trim(),
                Active = true
            };
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                var customer = BuildCustomerFromInputs();
                CustomerDAO.AddCustomer(customer, _currentUser.UserName);
                MessageBox.Show("Customer added successfully!");
                ClearInputs();
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId == -1)
            {
                MessageBox.Show("Please select a customer to update.");
                return;
            }

            if (!ValidateInputs()) return;

            try
            {
                var customer = BuildCustomerFromInputs();
                CustomerDAO.UpdateCustomer(customer, _currentUser.UserName);
                MessageBox.Show("Customer updated successfully!");
                ClearInputs();
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId == -1)
            {
                MessageBox.Show("Please select a customer to delete.");
                return;
            }

            var confirm = MessageBox.Show(
                "Are you sure you want to delete this customer? All their appointments will also be deleted.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                CustomerDAO.DeleteCustomer(_selectedCustomerId);
                MessageBox.Show("Customer deleted successfully!");
                ClearInputs();
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting customer: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void ClearInputs()
        {
            _selectedCustomerId = -1;
            _selectedAddressId = -1;
            txtName.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtCity.Text = "";
            txtCountry.Text = "";
            dgvCustomers.ClearSelection();
        }
    }
}