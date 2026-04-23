using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using StudentTeacherApp.Data;

namespace StudentTeacherApp.Pages.Students
{
    public class IndexModel : PageModel
    {
        public class StudentInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Class { get; set; }
        }

        public List<StudentInfo> listStudents { get; set; } = new List<StudentInfo>();

        public void OnGet()
        {
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM students", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listStudents.Add(new StudentInfo
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Class = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving students: {ex.Message}");
            }
        }
    }
}