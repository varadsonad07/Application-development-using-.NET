using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using StudentTeacherApp.Data;
using System.ComponentModel.DataAnnotations;

namespace StudentTeacherApp.Pages.Teachers
{
    public class EditModel : PageModel
    {
        [BindProperty, Required(ErrorMessage = "Enter the name")]
        public string Name { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Enter the email"), EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Enter the subject")]
        public string Subject { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id)
        {
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM teachers WHERE id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Name = reader.GetString(1);
                            Email = reader.GetString(2);
                            Subject = reader.GetString(3);
                        }
                        else
                        {
                            Console.WriteLine($"Teacher not found: {id}");
                            ErrorMessage = "Teacher not found.";
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving teacher details: {ex.Message}");
                ErrorMessage = ex.Message;
                return;
            }
        }

        public void OnPost(int id)
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
                    var command = new MySqlCommand("UPDATE teachers SET name = @Name, email = @Email, subject = @Subject WHERE id = @Id", connection);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Subject", Subject);
                    command.Parameters.AddWithValue("@Id", id);
                    int i = command.ExecuteNonQuery();
                    if (i > 0)
                    {
                        TempData["SuccessMessage"] = "Teacher updated successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating teacher: {ex.Message}");
                ErrorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Teachers/Index");
        }
    }
}