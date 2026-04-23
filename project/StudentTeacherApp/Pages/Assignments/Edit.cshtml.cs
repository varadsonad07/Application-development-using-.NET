using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using StudentTeacherApp.Data;
using System.ComponentModel.DataAnnotations;

namespace StudentTeacherApp.Pages.Assignments
{
    public class EditModel : PageModel
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
        public string Status { get; set; } = "";

        [BindProperty]
        public string Grade { get; set; } = "";

        [BindProperty]
        public string Feedback { get; set; } = "";

        public List<SelectListItem> Teachers { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Students { get; set; } = new List<SelectListItem>();

        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id)
        {
            LoadTeachersAndStudents();
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM assignments WHERE id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Title = reader.GetString(1);
                            Description = reader.GetString(2);
                            TeacherId = reader.GetInt32(3);
                            StudentId = reader.GetInt32(4);
                            Status = reader.GetString(5);
                            Grade = reader.IsDBNull(6) ? "" : reader.GetString(6);
                            Feedback = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        }
                        else
                        {
                            Console.WriteLine($"Assignment not found: {id}");
                            ErrorMessage = "Assignment not found.";
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving assignment details: {ex.Message}");
                ErrorMessage = ex.Message;
                return;
            }
        }

        public void OnPost(int id)
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
                    var command = new MySqlCommand("UPDATE assignments SET title = @Title, description = @Description, teacher_id = @TeacherId, student_id = @StudentId, status = @Status, grade = @Grade, feedback = @Feedback WHERE id = @Id", connection);
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@TeacherId", TeacherId);
                    command.Parameters.AddWithValue("@StudentId", StudentId);
                    command.Parameters.AddWithValue("@Status", Status);
                    command.Parameters.AddWithValue("@Grade", Grade);
                    command.Parameters.AddWithValue("@Feedback", Feedback);
                    command.Parameters.AddWithValue("@Id", id);
                    int i = command.ExecuteNonQuery();
                    if (i > 0)
                    {
                        TempData["SuccessMessage"] = "Assignment updated successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating assignment: {ex.Message}");
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