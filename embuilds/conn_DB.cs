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
        public int ValidateLogin(string username, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT id FROM users WHERE username = @username AND password = @password";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
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
                CASE 
                    WHEN sa.customer_id IS NULL THEN 'Walk-in Customer'
                    WHEN c.id IS NULL THEN 'Customer Deleted'
                    ELSE CONCAT(
                        c.first_name, 
                        IF(c.middle_name IS NULL OR c.middle_name = '', '', CONCAT(' ', c.middle_name)),
                        ' ', 
                        c.last_name
                    ) 
                END AS `Customer Name`,
                p.name AS `Product Name`,
                sa.quantity AS `Quantity`,
                CONCAT('₱', FORMAT(sa.total_price, 2)) AS `Total Price`,
                sa.created_at AS `Date`
            FROM sales sa
            LEFT JOIN customers c ON sa.customer_id = c.id
            JOIN products p ON sa.product_id = p.id
            ORDER BY sa.created_at DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
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

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM customers WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", customerId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle the case where the trigger prevents deletion
                if (ex.Number == 1644) // SQLSTATE '45000' error code
                {
                    throw new Exception("Cannot delete customer with existing transactions");
                }
                throw;
            }
        }

        public DataTable GetAllDeletedCustomers()
        {
            DataTable dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var query = @"SELECT 
             id,
             customer_id AS `Customer ID`,
             first_name AS `First Name`, 
             middle_name AS `Middle Name`, 
             last_name AS `Last Name`, 
             phone_number AS `Phone Number`, 
             address AS `Address`,
             deleted_at AS `Deleted At`
        FROM deleted_customers";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();
            }
            return dt;
        }

        public bool RestoreCustomer(int deletedRecordId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // First get the customer data from deleted_customers
                    string selectQuery = @"SELECT * FROM deleted_customers WHERE id = @id";
                    MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                    selectCmd.Parameters.AddWithValue("@id", deletedRecordId);

                    // Store the values we need
                    int? customerId = null;
                    string firstName = null;
                    string middleName = null;
                    string lastName = null;
                    string phoneNumber = null;
                    string address = null;
                    DateTime deletedAt = DateTime.Now;

                    using (MySqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customerId = reader["customer_id"] != DBNull.Value ? Convert.ToInt32(reader["customer_id"]) : (int?)null;
                            firstName = reader["first_name"].ToString();
                            middleName = reader["middle_name"].ToString();
                            lastName = reader["last_name"].ToString();
                            phoneNumber = reader["phone_number"].ToString();
                            address = reader["address"].ToString();
                            deletedAt = reader["deleted_at"] != DBNull.Value ? Convert.ToDateTime(reader["deleted_at"]) : DateTime.Now;
                        }
                        else
                        {
                            return false; // Record not found in deleted_customers
                        }
                    }

                    // Now perform the restore operation
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Insert into customers table
                            string insertQuery = @"INSERT INTO customers 
                                        (id, first_name, middle_name, last_name, phone_number, address, created_at, updated_at)
                                        VALUES (@id, @first_name, @middle_name, @last_name, @phone_number, @address, @created_at, @updated_at)";

                            MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn, transaction);
                            insertCmd.Parameters.AddWithValue("@id", customerId ?? 0); // Use original ID or 0 (auto-increment)
                            insertCmd.Parameters.AddWithValue("@first_name", firstName);
                            insertCmd.Parameters.AddWithValue("@middle_name", middleName);
                            insertCmd.Parameters.AddWithValue("@last_name", lastName);
                            insertCmd.Parameters.AddWithValue("@phone_number", phoneNumber);
                            insertCmd.Parameters.AddWithValue("@address", address);
                            insertCmd.Parameters.AddWithValue("@created_at", deletedAt); // Use deleted_at as created_at
                            insertCmd.Parameters.AddWithValue("@updated_at", DateTime.Now);

                            // Delete from deleted_customers
                            string deleteQuery = "DELETE FROM deleted_customers WHERE id = @id";
                            MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, conn, transaction);
                            deleteCmd.Parameters.AddWithValue("@id", deletedRecordId);

                            insertCmd.ExecuteNonQuery();
                            deleteCmd.ExecuteNonQuery();

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Error during restore transaction: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error restoring customer: " + ex.Message);
            }
        }
    }
}
