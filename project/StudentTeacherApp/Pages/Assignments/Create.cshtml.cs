using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using StudentTeacherApp.Data;
using System.ComponentModel.DataAnnotations;

namespace StudentTeacherApp.Pages.Assignments
{
    public class CreateModel : PageModel
    {
        [BindProperty, Required(ErrorMessage = "Enter the title")]
        public string Title { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Enter the description")]
        public string Description { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Select a teacher")]
        public int TeacherId { get; set; }

        [BindProperty, Required(ErrorMessage = "Select a student")]
        public int StudentId { get; set; }

        [BindProperty]
        public string Status { get; set; } = "Assigned";

        public List<SelectListItem> Teachers { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Students { get; set; } = new List<SelectListItem>();

        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
            LoadTeachersAndStudents();
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadTeachersAndStudents();
                return;
            }
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("INSERT INTO assignments (title, description, teacher_id, student_id, status) VALUES (@Title, @Description, @TeacherId, @StudentId, @Status)", connection);
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@TeacherId", TeacherId);
                    command.Parameters.AddWithValue("@StudentId", StudentId);
                    command.Parameters.AddWithValue("@Status", Status);
                    int i = command.ExecuteNonQuery();
                    if (i > 0)
                    {
                        TempData["SuccessMessage"] = "Assignment created successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating assignment: {ex.Message}");
                ErrorMessage = ex.Message;
                LoadTeachersAndStudents();
                return;
            }
            Response.Redirect("/Assignments/Index");
        }

        private void LoadTeachersAndStudents()
        {
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    var teacherCommand = new MySqlCommand("SELECT id, name FROM teachers", connection);
                    using (var reader = teacherCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Teachers.Add(new SelectListItem { Value = reader.GetInt32(0).ToString(), Text = reader.GetString(1) });
                        }
                    }
                    var studentCommand = new MySqlCommand("SELECT id, name FROM students", connection);
                    using (var reader = studentCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Students.Add(new SelectListItem { Value = reader.GetInt32(0).ToString(), Text = reader.GetString(1) });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}