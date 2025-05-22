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
    public class Create : PageModel
    {
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



        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
               try
               {
                   // Save the employee to the database
                string connectionString = "Server=MI_BOOK_SOMESH\\SQLEXPRESS;Database=employeeDb;Trusted_Connection=true;TrustServerCertificate=true";
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string query = "INSERT INTO employees (First_Name, Last_Name, Department, Joining_Date, Email) VALUES (@First_Name, @Last_Name, @Department, @JoiningDate, @Email)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@First_Name", First_Name);
                command.Parameters.AddWithValue("@Last_Name", Last_Name);
                command.Parameters.AddWithValue("@Department", Department);
                command.Parameters.AddWithValue("@JoiningDate", JoiningDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@Email", Email);

                command.ExecuteNonQuery();

                // Redirect to the index page after saving
                Response.Redirect("/Employees/Index");
               }
               catch (System.Exception)
               {
                    Console.WriteLine("Error");
                throw;
               }
            }
           
        }
    }
}