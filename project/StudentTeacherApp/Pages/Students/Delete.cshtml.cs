using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using StudentTeacherApp.Data;

namespace StudentTeacherApp.Pages.Students
{
    public class DeleteModel : PageModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id)
        {
            Id = id;
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT name FROM students WHERE id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Name = reader.GetString(0);
                        }
                        else
                        {
                            ErrorMessage = "Student not found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public void OnPost(int id)
        {
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("DELETE FROM students WHERE id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                    TempData["SuccessMessage"] = "Student deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Students/Index");
        }
    }
}