using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SheshBesh
{
    public static class Database
    {
        private static SqlConnection Connection { get; set; }

        private const string ConnectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\JetBrainsProjects\RiderProjects\SheshBesh\SheshBesh\BackgammonDatabase.mdf;Integrated Security=True;Connect Timeout=30";

        public static void Connect()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }

        private static bool IsExist(string name)
        {
            string query = "select count(*) from Leaderboard where Username=@Username";
            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@Username", name);
            int count = int.Parse(command.ExecuteScalar().ToString());
            return count > 0;
        }

        public static void InsertUser(string name)
        {
            if (IsExist(name)) return;
            string query = "insert into Leaderboard (Username,Wins,Loses) values(@Username, 0, 0)";
            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@Username", name);
            command.ExecuteNonQuery();
        }

        public static void UpdateUserWins(string name)
        {
            if (IsExist(name)) return;
            string query = "update Leaderboard set Wins = Wins + 1 where Username = @Username";
            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@Username", name);
            command.ExecuteNonQuery();
        }
        public static void UpdateUserLoses(string name)
        {
            if (IsExist(name)) return;
            string query = "update Leaderboard set Loses = Loses + 1 where Username = @Username";
            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@Username", name);
            command.ExecuteNonQuery();
        }

        public static List<UserData> GetLeaderboard()
        {
            string query = "select Username,Wins,Loses from Leaderboard";
            SqlCommand command = new SqlCommand(query, Connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable table = ds.Tables[0];
            List<UserData> data = new List<UserData>();
            foreach (DataRow row in table.Rows)
            {
                data.Add(new UserData
                {
                    Name = row.ItemArray[0].ToString(),
                    Wins = int.Parse(row.ItemArray[1].ToString()),
                    Loses = int.Parse(row.ItemArray[2].ToString())
                });
            }

            return data;
        }

        public static void Close()
        {
            Connection.Close();
        }
    }
}