using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SofTrustTask.Pages
{
    public class SuccessModel : PageModel
    {
        public async void OnGet()
        {
            int messageId = int.Parse(Request.Query["ID"]);

            // Строка подключения 
            string connectionString = "";
            using SqlConnection connection = new(connectionString);
            SqlDataReader reader = new SqlCommand($"SELECT * FROM Messages WHERE ID = {messageId}", connection).ExecuteReader();

            if (reader.Read()) await Response.WriteAsync($"{reader["ID"]}, {reader["ThemeID"]}, {reader["ContactID"]}, {reader["Message"]}");
        }
    }
}
