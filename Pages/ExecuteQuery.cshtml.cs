using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SofTrustTask.Pages
{
    public class ExecuteQuery : PageModel
    {
        public void OnGet()
        {
            IQueryCollection values = Request.Query;

            // Строка подключения 
            string connectionString = "";

            int currentMessageId = 0;

            using (SqlConnection connection = new(connectionString))
            {
                connection.Open();

                // Проверяем наличие контакта в таблице 
                SqlCommand checkContact = 
                    new($"SELECT * FROM Contacts WHERE Email = '{values["Email"]}' AND Telephone = {values["Telephone"]}", connection);

                SqlDataReader contactReader = checkContact.ExecuteReader();

                if (!contactReader.Read())
                {
                    contactReader.Close();
                    new SqlCommand($"INSERT INTO Contacts VALUES('{values["Name"]}', {values["Telephone"]}, '{values["Email"]}')", connection).ExecuteNonQuery();
                }

                contactReader.Close();

                // Создаем запись в таблице сообщений

                // Получаем ID контакта
                string getContactIdCommandText =
                    $"SELECT ID FROM Contacts WHERE Email = '{values["Email"]}' AND Telephone = {values["Telephone"]}";
                SqlDataReader contactIdReader = new SqlCommand(getContactIdCommandText, connection).ExecuteReader();
                contactIdReader.Read();
                int contactId = (int)contactIdReader["ID"];
                contactIdReader.Close();

                // Получаем ID темы сообщения
                string getThemeIdCommandText = $"SELECT ID FROM Themes WHERE Theme = '{values["Theme"]}'";
                SqlDataReader themeIdReader = new SqlCommand(getThemeIdCommandText, connection).ExecuteReader();
                themeIdReader.Read();
                int themeId = (int)themeIdReader["ID"];
                themeIdReader.Close();

                // Создаем новую запись 
                string newMessageCommandText = $"INSERT INTO Messages VALUES({contactId}, {themeId}, '{values["Message"]}')";
                new SqlCommand(newMessageCommandText, connection).ExecuteNonQuery();

                // Получаем ID текущего сообщения 
                string getCurrentMessageIdCommandText =
                    $"SELECT ID FROM Messages WHERE ThemeID = {themeId} AND ContactID = {contactId} AND Message = '{values["Message"]}'";
                SqlDataReader currentMessageIdReader = new SqlCommand(getCurrentMessageIdCommandText, connection).ExecuteReader();
                currentMessageIdReader.Read();
                currentMessageId = (int)currentMessageIdReader["ID"];
                currentMessageIdReader.Close();
            }

            // После отправки данных в базу данных переносим пользователя на страницу 
            // с информацией об успешной отправке формы обратной связи
            Response.Redirect($"Success?ID={currentMessageId}");
        }
  
    }
}
