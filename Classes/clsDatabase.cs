﻿using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Numerics;
using System.Xml.Linq;

namespace PracticalAssessmentAPI.Classes
{
    public class clsDatabase
    {
        //private string connectionString = "Data Source=.;Persist Security Info=True" +
        //                                ";User ID=sa" +
        //                                ";Password=bull$dog" +
        //                                ";Initial Catalog=PracticalAssessment";

        private string connectionString = "Data Source=DESKTOP-5PM27UM;Persist Security Info=True" +
                                        ";User ID=DESKTOP-5PM27UM\\heinr" +
                                        ";Password=804422" +
                                        ";Initial Catalog=PracticalAssessment";

        private const string errorSplit = "||||";

        #region "Security Calls."
        public DataTable readSecurity(string name, string password, string access)
        {
            DataTable dt = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_SECURITY_READ", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters if they are not empty
                        if (!string.IsNullOrEmpty(name))
                            cmd.Parameters.AddWithValue("@Name", name);
                        if (!string.IsNullOrEmpty(password))
                            cmd.Parameters.AddWithValue("@Password", password);
                        if (!string.IsNullOrEmpty(access))
                            cmd.Parameters.AddWithValue("@Access", access);

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
        #endregion

        public DataTable readContact(string name)
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
                        if (!string.IsNullOrEmpty(name))
                            cmd.Parameters.AddWithValue("@Name", name);
                        //if (!string.IsNullOrEmpty(email))
                        //    cmd.Parameters.AddWithValue("@Email", email);
                        //if (!string.IsNullOrEmpty(phone))
                        //    cmd.Parameters.AddWithValue("@Phone", phone);
                        //if (!string.IsNullOrEmpty(address))
                        //    cmd.Parameters.AddWithValue("@Address", address);

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
        public DataTable insertContact(string Name, string Email, string Phone, string Address)
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

        public DataTable updateContact(string Name, string Email, string Phone, string Address)
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
    }
}
