using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

//using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace embuilds
{
    internal class conn_DB
    {
        private readonly string connectionString = "server=localhost; database=hardwareshopdb; uid=root; pwd=emman;";

        // user login validation
        public bool ValidateLogin(string username, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        // get all customers
        public DataTable GetAllCustomers()
        {
            DataTable dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"SELECT id, 
                     first_name AS `First Name`, 
                     middle_name AS `Middle Name`, 
                     last_name AS `Last Name`, 
                     phone_number AS `Phone Number`, 
                     address AS `Address`
                FROM customers";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();
            }
            return dt;
        }

        // get all suppliers
        public DataTable GetAllSuppliers()
        {
            DataTable dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"SELECT id, 
                     name AS `Supplier Name`, 
                     email AS `Email`, 
                     contact_person AS `Contact Person`, 
                     contact_person_number AS `Contact Person Number`, 
                     address AS `Address`,
                     status AS `Status`
                FROM suppliers";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();
            }
            return dt;
        }

    }
}
