using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using StudentTeacherApp.Data;

namespace StudentTeacherApp.Pages.Teachers
{
    public class IndexModel : PageModel
    {
        public class TeacherInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Subject { get; set; }
        }

        public List<TeacherInfo> listTeachers { get; set; } = new List<TeacherInfo>();

        public void OnGet()
        {
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM teachers", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listTeachers.Add(new TeacherInfo
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Subject = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving teachers: {ex.Message}");
            }
        }
    }
}