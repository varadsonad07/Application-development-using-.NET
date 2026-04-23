using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using StudentTeacherApp.Data;

namespace StudentTeacherApp.Pages.Assignments
{
    public class DeleteModel : PageModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id)
        {
            Id = id;
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT title FROM assignments WHERE id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Title = reader.GetString(0);
                        }
                        else
                        {
                            ErrorMessage = "Assignment not found.";
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
                    var command = new MySqlCommand("DELETE FROM assignments WHERE id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                    TempData["SuccessMessage"] = "Assignment deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Assignments/Index");
        }
    }
}