using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SofTrustTask.Pages
{
    public class ExecuteQuery : PageModel
    {
        public void OnGet()
        {
            IQueryCollection values = Request.Query;

            // ������ ����������� 
            string connectionString = "";

            int currentMessageId = 0;

            using (SqlConnection connection = new(connectionString))
            {
                connection.Open();

                // ��������� ������� �������� � ������� 
                SqlCommand checkContact = 
                    new($"SELECT * FROM Contacts WHERE Email = '{values["Email"]}' AND Telephone = {values["Telephone"]}", connection);

                SqlDataReader contactReader = checkContact.ExecuteReader();

                if (!contactReader.Read())
                {
                    contactReader.Close();
                    new SqlCommand($"INSERT INTO Contacts VALUES('{values["Name"]}', {values["Telephone"]}, '{values["Email"]}')", connection).ExecuteNonQuery();
                }

                contactReader.Close();

                // ������� ������ � ������� ���������

                // �������� ID ��������
                string getContactIdCommandText =
                    $"SELECT ID FROM Contacts WHERE Email = '{values["Email"]}' AND Telephone = {values["Telephone"]}";
                SqlDataReader contactIdReader = new SqlCommand(getContactIdCommandText, connection).ExecuteReader();
                contactIdReader.Read();
                int contactId = (int)contactIdReader["ID"];
                contactIdReader.Close();

                // �������� ID ���� ���������
                string getThemeIdCommandText = $"SELECT ID FROM Themes WHERE Theme = '{values["Theme"]}'";
                SqlDataReader themeIdReader = new SqlCommand(getThemeIdCommandText, connection).ExecuteReader();
                themeIdReader.Read();
                int themeId = (int)themeIdReader["ID"];
                themeIdReader.Close();

                // ������� ����� ������ 
                string newMessageCommandText = $"INSERT INTO Messages VALUES({contactId}, {themeId}, '{values["Message"]}')";
                new SqlCommand(newMessageCommandText, connection).ExecuteNonQuery();

                // �������� ID �������� ��������� 
                string getCurrentMessageIdCommandText =
                    $"SELECT ID FROM Messages WHERE ThemeID = {themeId} AND ContactID = {contactId} AND Message = '{values["Message"]}'";
                SqlDataReader currentMessageIdReader = new SqlCommand(getCurrentMessageIdCommandText, connection).ExecuteReader();
                currentMessageIdReader.Read();
                currentMessageId = (int)currentMessageIdReader["ID"];
                currentMessageIdReader.Close();
            }

            // ����� �������� ������ � ���� ������ ��������� ������������ �� �������� 
            // � ����������� �� �������� �������� ����� �������� �����
            Response.Redirect($"Success?ID={currentMessageId}");
        }
  
    }
}
