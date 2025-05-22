using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace EmployeeApp.Pages.Employees
{
    public class Index : PageModel
    {
        public List<Employee> Employees { get; set; } = [];
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        public void OnGet(int pageNumber = 1)
        {
            PageNumber = pageNumber;
            int pageSize = 9; // Show 9 items per page
            int totalCount = 0;

            try
            {
                string connectionString = "Server=MI_BOOK_SOMESH\\SQLEXPRESS;Database=employeeDb;Trusted_Connection=true;TrustServerCertificate=true";
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                // Get total count
                string countQuery = "SELECT COUNT(*) FROM employees";
                SqlCommand countCommand = new SqlCommand(countQuery, connection);
                totalCount = (int)countCommand.ExecuteScalar();
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                // Get paged data
                string query = @"SELECT * FROM employees
                                 ORDER BY employee_id
                                 OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Offset", (PageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Employee employee = new Employee
                    {
                        Id = reader.GetInt32(0),
                        First_Name = reader.GetString(1),
                        Last_Name = reader.GetString(2),
                        Department = reader.GetString(3),
                        JoiningDate = DateOnly.FromDateTime(reader.GetDateTime(4)),
                        Email = reader.GetString(5),
                        ProfilePicture = reader.IsDBNull(6) ? null : reader.GetString(6)
                    };
                    Employees.Add(employee);
                }
            }
            catch (System.Exception)
            {
                Console.WriteLine("Error");
                // Handle the error
                throw;
            }
        }
    }


    public class Employee
    {
        public int Id { get; set; }
        public required string First_Name { get; set; }
        public required string Last_Name { get; set; }
        public required string Department { get; set; }

        public DateOnly JoiningDate { get; set; }
        public required string Email { get; set; }
        public string? ProfilePicture { get; set; } // New property

    }
}