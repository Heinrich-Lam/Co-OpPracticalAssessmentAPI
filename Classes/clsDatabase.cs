using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Numerics;
using System.Xml.Linq;

namespace PracticalAssessmentAPI.Classes
{
    public class clsDatabase
    {
        private string connectionString = "Data Source=.;Persist Security Info=True" +
                                        ";User ID=sa" +
                                        ";Password=bull$dog" +
                                        ";Initial Catalog=PracticalAssessment";

        #region "Security."
        public DataTable readSecurity(string Name, string Password)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_SECURITY_READ", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (!string.IsNullOrEmpty(Name))
                            cmd.Parameters.AddWithValue("@Name", Name);
                        if (!string.IsNullOrEmpty(Password))
                            cmd.Parameters.AddWithValue("@Password", Password);

                        conn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for further investigation
                Console.WriteLine($"Error executing USP_SECURITY_READ: {ex.InnerException?.Message}");
                throw;
            }
            return dt;
        }

        public DataTable readContactSecurity(string Name, string Password)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_SECURITY_READ", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (!string.IsNullOrEmpty(Name))
                            cmd.Parameters.AddWithValue("@Name", Name);
                        if (!string.IsNullOrEmpty(Password))
                            cmd.Parameters.AddWithValue("@Password", Password);

                        conn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for further investigation
                Console.WriteLine($"Error executing USP_SECURITY_READ: {ex.InnerException?.Message}");
                throw;
            }
            return dt;
        }
        #endregion

        #region "Contacts Procedures."
        //Read Contacts to Populate List.
        public DataTable readContacts(int History)
        {
            DataTable dt = new DataTable();
            dt.TableName = "CONTACTS";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_CONTACT_READ", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters if they are not empty
                        if (History >= 0)
                            cmd.Parameters.AddWithValue("@History", History);

                        conn.Open();

                        // Execute the stored procedure and get the result set
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dt;
        }

        //Read Contact to populate Edit, or Delete Fields.
        public DataTable readContactInfo(int EntryId)
        {
            DataTable dt = new DataTable();
            dt.TableName = "CONTACTS";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_CONTACT_INFO_READ", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters if they are not empty
                        if (EntryId != 0)
                            cmd.Parameters.AddWithValue("@EntryID", EntryId);

                        conn.Open();

                        // Execute the stored procedure and get the result set
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dt;
        }

        //Insert Contact.
        public DataTable insertContact(string Name, string Email, string Phone, string Address, int History)
        {

            ArrayList spl = new ArrayList();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("USP_CONTACT_INSERT", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Phone", Phone);
                        cmd.Parameters.AddWithValue("@Address", Address);
                        cmd.Parameters.AddWithValue("@History", History);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader); // Load the result set into the DataTable
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred: " + ex.Message);
            }

            return dt;

        }

        //Update Contact
        public DataTable updateContact(int EntryID, string Name, string Email, string Phone, string Address)
        {

            ArrayList spl = new ArrayList();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("USP_CONTACT_UPDATE", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Entry_ID", EntryID);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Phone", Phone);
                        cmd.Parameters.AddWithValue("@Address", Address);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader); // Load the result set into the DataTable
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred: " + ex.Message);
            }

            return dt;

        }

        //Delete Contact (This Marks as history.)
        public DataTable deleteContact(int EntryId)
        {

            ArrayList spl = new ArrayList();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("USP_CONTACT_DELETE", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ENTRY_ID", EntryId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader); // Load the result set into the DataTable
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred: " + ex.Message);
            }

            return dt;

        }
        #endregion

        #region "User Procedures."
        //read User to populate list.
        public DataTable readUser(int History)
        {
            DataTable dt = new DataTable();
            dt.TableName = "USERS";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_USER_READ", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters if they are not empty
                        if (History >= 0)
                            cmd.Parameters.AddWithValue("@History", History);

                        conn.Open();

                        // Execute the stored procedure and get the result set
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dt;
        }

        //Read User to populate Edit, or Delete Fields.
        public DataTable readUserInfo(int EntryId)
        {
            DataTable dt = new DataTable();
            dt.TableName = "USERS";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_USER_INFO_READ", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters if they are not empty
                        if (EntryId != 0)
                            cmd.Parameters.AddWithValue("@EntryID", EntryId);

                        conn.Open();

                        // Execute the stored procedure and get the result set
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dt;
        }

        //Insert User.
        public DataTable insertUser(string Name, string Surname, string Email, string Password, int History)
        {

            ArrayList spl = new ArrayList();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("USP_USER_INSERT", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Surname", Surname);
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@History", History);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader); // Load the result set into the DataTable
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred: " + ex.Message);
            }

            return dt;

        }
        
        //Update User.
        public DataTable updateUser(int EntryID, string Name, string Surname, string Email, string Password)
        {

            ArrayList spl = new ArrayList();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("USP_USER_UPDATE", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Entry_ID", EntryID);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Surname", Surname);
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Password", Password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader); // Load the result set into the DataTable
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred: " + ex.Message);
            }

            return dt;

        }

        //Delete User. (This removes the User from the Database.)
        public DataTable deleteUser(int EntryId)
        {

            ArrayList spl = new ArrayList();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("USP_USER_DELETE", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ENTRY_ID", EntryId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader); // Load the result set into the DataTable
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred: " + ex.Message);
            }
            return dt;

        }
        #endregion
    }
}
