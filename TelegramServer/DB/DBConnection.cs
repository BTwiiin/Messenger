using MySql.Data.MySqlClient;

namespace TelegramServer.DB
{
    public class DBConnection
    {
        MySqlConnection connection;
        public DBConnection()
        {
            connection = new MySqlConnection("sever=localhost;port=3306;user=root;" +
                "password=root;database=tgserverdb");
        }
        public void OpenConnection()
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

    }
}
