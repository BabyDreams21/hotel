using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace hotel
{
    class Utils
    {
        public static string conn = "Data Source =DESKTOP-M5SO53H\\SQLEXPRESS;Initial catalog = backuphotel2;Integrated Security = true;";
    }

    class Encrypt
    {
        public static string enc(string data)
        {
            using (SHA256Managed managed = new SHA256Managed())
            {
                byte[] b = managed.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(b);
            }
        }

    }

    class Session
    {
        public static int id { set; get; }
        public static string Username { set; get; }
        public static string name { set; get; }
        public static string Email { set; get; }
        public static string Address { set; get; }
        public static DateTime DateOfBirth { set; get; }
        public static int JobID { set; get; }
    }

  
    class Command
    {
        public static DataTable GetData(string com)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlDataAdapter adapter = new SqlDataAdapter(com, connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public static void exec(string com)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            connection.Open();
            SqlCommand command = new SqlCommand(com, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    class Selected
    {
        public static int id { set; get; }
        public static string name { set; get; }

    }

    public class Model
    {
        public static int id { set; get; }
        public static string name { set; get; }
        public static string username { set; get; }
        public static int job_id { set; get; }
    }
}
