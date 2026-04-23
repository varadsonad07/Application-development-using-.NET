using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using StudentTeacherApp.Data;

namespace StudentTeacherApp.Pages.Assignments
{
    public class IndexModel : PageModel
    {
        public class AssignmentInfo
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int TeacherId { get; set; }
            public string TeacherName { get; set; }
            public int StudentId { get; set; }
            public string StudentName { get; set; }
            public string Status { get; set; }
            public string Grade { get; set; }
            public string Feedback { get; set; }
        }

        public List<AssignmentInfo> listAssignments { get; set; } = new List<AssignmentInfo>();

        public void OnGet()
        {
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand(@"
                        SELECT a.id, a.title, a.description, a.teacher_id, t.name as teacher_name, a.student_id, s.name as student_name, a.status, a.grade, a.feedback
                        FROM assignments a
                        JOIN teachers t ON a.teacher_id = t.id
                        JOIN students s ON a.student_id = s.id", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listAssignments.Add(new AssignmentInfo
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                TeacherId = reader.GetInt32(3),
                                TeacherName = reader.GetString(4),
                                StudentId = reader.GetInt32(5),
                                StudentName = reader.GetString(6),
                                Status = reader.GetString(7),
                                Grade = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                Feedback = reader.IsDBNull(9) ? "" : reader.GetString(9)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving assignments: {ex.Message}");
            }
        }
    }
}