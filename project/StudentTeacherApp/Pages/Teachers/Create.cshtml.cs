using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using StudentTeacherApp.Data;
using System.ComponentModel.DataAnnotations;

namespace StudentTeacherApp.Pages.Teachers
{
    public class CreateModel : PageModel
    {
        [BindProperty, Required(ErrorMessage = "Enter the name")]
        public string Name { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Enter the email"), EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Enter the subject")]
        public string Subject { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Form is Invalid");
                return;
            }
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("INSERT INTO teachers (name, email, subject) VALUES (@Name, @Email, @Subject)", connection);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Subject", Subject);
                    int i = command.ExecuteNonQuery();
                    if (i > 0)
                    {
                        TempData["SuccessMessage"] = "Teacher added successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding teacher: {ex.Message}");
                ErrorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Teachers/Index");
        }
    }
}