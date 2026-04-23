using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using StudentTeacherApp.Data;
using System.ComponentModel.DataAnnotations;

namespace StudentTeacherApp.Pages.Students
{
    public class CreateModel : PageModel
    {
        [BindProperty, Required(ErrorMessage = "Enter the name")]
        public string Name { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Enter the email"), EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Enter the class")]
        public string Class { get; set; } = "";

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
                    var command = new MySqlCommand("INSERT INTO students (name, email, class) VALUES (@Name, @Email, @Class)", connection);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Class", Class);
                    int i = command.ExecuteNonQuery();
                    if (i > 0)
                    {
                        TempData["SuccessMessage"] = "Student added successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding student: {ex.Message}");
                ErrorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Students/Index");
        }
    }
}