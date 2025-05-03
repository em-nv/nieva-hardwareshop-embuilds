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
        //private readonly string connectionString = "server=localhost; database=hardwareshopdb; uid=root; pwd=emman;";
        public string connectionString = "server=localhost; database=hardwareshopdb; uid=root; pwd=emman;";

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

        // get all products
        public DataTable GetAllProducts()
        {
            DataTable dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"
                    SELECT p.id, 
                            p.name AS `Product Name`, 
                            p.description AS `Description`, 
                            c.name AS `Category`, 
                            s.name AS `Supplier`, 
                             CONCAT('Php ', FORMAT(p.price, 2)) AS `Price`
                    FROM products p
                    JOIN categories c ON p.category_id = c.id
                    JOIN suppliers s ON p.supplier_id = s.id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();

            }
            return dt;
        }

        // get all inventory
        public DataTable GetAllInventory()
        {
            DataTable dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"
                    SELECT 
                        p.id AS `ID`,
                        p.name AS `Product Name`,
                        i.stock_available AS `Stock Available`,
                        i.stock_status AS `Stock Status`
                    FROM products p
                    JOIN inventories i ON p.id = i.product_id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();

            }
            return dt;
        }

        // get all sales
        public DataTable GetAllSales()
        {
            DataTable dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"
                    SELECT 
                        sa.id AS `ID`,
                        COALESCE(CONCAT(c.first_name, ' ', c.middle_name, ' ', c.last_name), '') AS `Customer Name`,
                        p.name AS `Product Name`,
                        sa.quantity AS `Quantity`,
                        CONCAT('Php ', FORMAT(sa.total_price, 2)) AS `Total Price`
                    FROM sales sa
                    LEFT JOIN customers c ON sa.customer_id = c.id
                    JOIN products p ON sa.product_id = p.id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();

            }
            return dt;
        }

        // get all transactions
        public DataTable GetAllTransactions()
        {
            DataTable dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"
                    SELECT 
                        t.id AS `ID`,
                        t.sale_id AS `Sale ID`,
                        t.payment_method AS `Payment Method`,
                        CONCAT('Php ', FORMAT(t.amount_paid, 2)) AS `Amount Paid`
                    FROM transactions t
                    JOIN sales s ON t.sale_id = s.id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();

            }
            return dt;
        }

        // get all product categories
        public DataTable GetAllProductCategories()
        {
            DataTable dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"
                    SELECT 
                        id AS `ID`,
                        name AS `Category Name`,
                        description as `Description`
                    FROM categories";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();

            }
            return dt;
        }

        // get all users
        public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"
                    SELECT 
                        id AS `ID`,
                        first_name AS `First Name`,
                        middle_name AS `Middle Name`,
                        last_name AS `Last Name`,
                        username AS `Username`,
                        email AS `Email`
                    FROM users";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();

            }
            return dt;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }


    }
}
