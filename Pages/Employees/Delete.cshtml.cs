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
    public class Delete : PageModel
    {


        public void OnGet()
        {
        }
        
        public void OnPost(int Id)
        {
            try
            {
                string connectionString = "Server=MI_BOOK_SOMESH\\SQLEXPRESS;Database=employeeDb;Trusted_Connection=true;TrustServerCertificate=true";
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string query = "DELETE FROM employees WHERE employee_id = @Id"; // Change to "Id" if needed
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
                Response.Redirect("/Employees/Index");
            }
            catch (System.Exception)
            {
                Console.WriteLine("Error");
                // Handle the error
                throw;
            }
        }
    }
}