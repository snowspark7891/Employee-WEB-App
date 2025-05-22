using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace EmployeeApp.Pages.Employees
{
    public class Edit : PageModel
    {

        [BindProperty]
        public int Id { get; set; }

        [BindProperty, Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public required string First_Name { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        public required string Last_Name { get; set; } = "";
        [BindProperty, Required(ErrorMessage = "Department is required")]
        public required string Department { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Joining Date is required")]
        public DateOnly JoiningDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        // public required string JoiningDateString { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        [BindProperty, Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; } = "";


        public string ErrorMessage { get; set; } = "";
        public void OnGet(int Id)
        {
            try
            {
                string connectionString = "Server=MI_BOOK_SOMESH\\SQLEXPRESS;Database=employeeDb;Trusted_Connection=true;TrustServerCertificate=true";
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string query = "SELECT * FROM employees WHERE employee_id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", Id);
                using (SqlDataReader reader = command.ExecuteReader())
                {



                    if (reader.Read())
                    {
                        Id = reader.GetInt32(0);
                        First_Name = reader.GetString(1);
                        Last_Name = reader.GetString(2);
                        Department = reader.GetString(3);
                        JoiningDate = DateOnly.FromDateTime(reader.GetDateTime(4));
                        Email = reader.GetString(5);
                    }
                    else
                    {
                        Response.Redirect("/Employees/Index");
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                throw;
            }
        }

        public void OnPost(int Id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Save the employee to the database
                    string connectionString = "Server=MI_BOOK_SOMESH\\SQLEXPRESS;Database=employeeDb;Trusted_Connection=true;TrustServerCertificate=true";
                    using SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    string query = "UPDATE employees SET First_Name = @First_Name, Last_Name = @Last_Name, Department = @Department, Joining_Date = @JoiningDate, Email = @Email WHERE employee_id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@First_Name", First_Name);
                    command.Parameters.AddWithValue("@Last_Name", Last_Name);
                    command.Parameters.AddWithValue("@Department", Department);
                    command.Parameters.AddWithValue("@JoiningDate", JoiningDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@Email", Email);

                    command.ExecuteNonQuery();

                    // Redirect to the index page after saving
                    Response.Redirect("/Employees/Index");
                }
                catch (System.Exception ex)
                {
                    ErrorMessage = ex.Message;
                    throw;
                }
            }
            else
            {
                // Handle validation errors
                ErrorMessage = "Please correct the errors and try again.";
            }
        }
    }
}