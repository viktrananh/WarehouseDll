using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WarehouseDll
{
    public class SqlClass
    {

        MySqlCommand Acomman;
        MySqlConnection Aconnection;
        SqlConnection sqlconnection;
        string IPserver = "";
        string DatabaseName = "";
        string User = "";
        string PassWord = "";

        public SqlClass(string IPserver, string DatabaseName, string User, string PassWord)
        {
            this.IPserver = IPserver;
            this.DatabaseName = DatabaseName;
            this.User = User;
            this.PassWord = PassWord;
        }
        //
        public DataTable GetDataMySQL(string Comman)
        {
            DataTable dt = new DataTable();
            if (openConnect(IPserver, DatabaseName, User, PassWord))
            {
                dt = GetDataTableMySQL(Comman);
                closeConnect();
            }
            else
            {
                return null;
            }
            return dt;
            //

        }

        public DataTable GetProcedureDataMySQL(string procedureName, string[] keys, string[] parameters)
        {
            DataTable dt = new DataTable();
            if (openConnect(IPserver, DatabaseName, User, PassWord))
            {
                dt = GetProcedureDataTableMySQL(procedureName, keys, parameters);
                closeConnect();
            }
            else
            {
                return null;
            }
            return dt;
            //

        }

        public DataTable GetDataSQL(string Comman)
        {

            Comman = Comman.Trim().Replace("\r\n", " ");
            Comman = Comman.Replace("\n", " ").Replace("\t", " ").Replace("\r", " ");
            DataTable dt = new DataTable();
            if (openSQLConnect(IPserver, DatabaseName, User, PassWord))
            {
                dt = GetDataTableSQL(Comman);
            }
            else
            {
                return null;
            }
            return dt;
            //

        }
        private DataTable GetDataTableSQL(string cmd)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd, sqlconnection);
                da.Fill(dt);
                sqlconnection.Close();
                return dt;
            }
            catch (Exception ex)
            {
                string Emess = ex.Message;
                return null;
            }
        }
        private bool openSQLConnect(string IPserver, string DatabaseName, string User, string PassWord)
        {
            try
            {
                string newconnectString = $"Data Source={IPserver};Initial Catalog={DatabaseName};User id={User};Password={PassWord};";
                sqlconnection = new SqlConnection(newconnectString);
                sqlconnection.Open();
                return true;
            }
            catch (Exception Ex)
            {
                string Exm = Ex.Message;
                return false;
            }
        }
        public bool InsertDataMySQL(string Comman)
        {
            if (openConnect(IPserver, DatabaseName, User, PassWord))
            {
                if (ExecuteMySQL(Comman))
                {
                    closeConnect();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }
        public string sqlerror = "";
        public bool InsertDataSQL(string Comman)
        {

            if (openSQLConnect(IPserver, DatabaseName, User, PassWord))
            {
                SqlCommand sqlCommand = new SqlCommand(Comman);
                try
                {
                    sqlCommand.Connection = sqlconnection;
                    sqlCommand.ExecuteNonQuery();

                }
                catch (Exception EX)
                {
                    sqlerror = EX.ToString();
                    sqlconnection.Close();
                    return false;
                }

                sqlconnection.Close();
                return true;
            }
            else
                return false;
        }

        public void InsertPictureToMySQL_ROSE(string IPserver, string DatabaseName, string User, string PassWord, string mysqlcmd, string prefix, string DataPicture)
        {
            string MyConString = "SERVER = " + IPserver + ";" + "DATABASE = " + DatabaseName + ";" + "UID = " + User + ";" + "PASSWORD = " + PassWord + ";charset=utf8;SslMode=none;";
            MySqlConnection connection = new MySqlConnection(MyConString);
            //----------------
            connection.Open();
            using (var cmd = new MySqlCommand(mysqlcmd, connection))
            {
                cmd.Parameters.Add(prefix, MySqlDbType.Blob);
                cmd.Parameters[prefix].Value = DataPicture;
                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }
        // Save picture
        public void InsertPictureToMySQL(string IPserver, string DatabaseName, string User, string PassWord, string TableName, string ColumsName, Byte[] DataPicture)
        {
            //string MyConString = "SERVER = " + IPserver + ";" + "DATABASE = " + DatabaseName + ";" + "UID = " + User + ";" + "PASSWORD = " + PassWord + ";charset=utf8;SslMode=none;";
            //// string MyConString = "SERVER = " + IPserver + ";" + "DATABASE = " + DatabaseName + ";" + "UID = " + User + ";" + "PASSWORD = " + PassWord;
            //string Comman = "INSERT INTO " + TableName + " (" + ColumsName + ") VALUES (?image);";
            ////create command and assign the query and connection from the constructor
            //MySqlConnection connection = new MySqlConnection(MyConString);
            ////----------------
            //connection.Open();
            //var data = DataPicture;
            //using (var cmd = new MySqlCommand(Comman, connection))
            //{
            //    cmd.Parameters.Add("?image", data);
            //    cmd.ExecuteNonQuery();
            //}
            //connection.Close();
        }
        //
        // Save picture
        public void UpdatePictureToMySQL(string IPserver, string DatabaseName, string User, string PassWord, string TableName, string ColumsName, string ColumsAddr, string RowAddr, Byte[] DataPicture)
        {
            //string MyConString = "SERVER = " + IPserver + ";" + "DATABASE = " + DatabaseName + ";" + "UID = " + User + ";" + "PASSWORD = " + PassWord + ";charset=utf8;SslMode=none;";
            ////string MyConString = "SERVER = " + IPserver + ";" + "DATABASE = " + DatabaseName + ";" + "UID = " + User + ";" + "PASSWORD = " + PassWord;
            //string Comman = "UPDATE " + TableName + " SET `" + ColumsName + "`=?image WHERE `" + ColumsAddr + "`='" + RowAddr + "';";
            ////
            ////create command and assign the query and connection from the constructor
            //MySqlConnection connection = new MySqlConnection(MyConString);
            ////----------------
            //connection.Open();
            //var data = DataPicture;
            //using (var cmd = new MySqlCommand(Comman, connection))
            //{
            //    cmd.Parameters.Add("?image", data);
            //    cmd.ExecuteNonQuery();
            //}
            //connection.Close();
        }
        //
        public byte[] RetrieveImage(string IPserver, string DatabaseName, string User, string PassWord, string Comman)
        {
            byte[] imageData = null;
            string MyConString = "SERVER = " + IPserver + ";" + "DATABASE = " + DatabaseName + ";" + "UID = " + User + ";" + "PASSWORD = " + PassWord;
            MySqlConnection connection = new MySqlConnection(MyConString);
            connection.Open();
            //
            MySqlCommand cmd = new MySqlCommand(Comman, connection);
            MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
            reader.Read();
            long bytesize = reader.GetBytes(0, 0, null, 0, 0);
            imageData = new byte[bytesize];
            long bytesread = 0;
            int curpos = 0;
            int chunkSize = 1;
            while (bytesread < bytesize)
            {
                bytesread += reader.GetBytes(0, curpos, imageData, curpos, chunkSize);
                curpos += chunkSize;
            }
            connection.Close();
            return imageData;
        }
        //
        // tao database
        public void CreateDatabase(string IPserver, string User, string PassWord, string Comman)
        {
            string MyConString = "SERVER = " + IPserver + ";" + "UID = " + User + ";" + "PASSWORD = " + PassWord;
            //create command and assign the query and connection from the constructor
            MySqlConnection connection = new MySqlConnection(MyConString);
            MySqlCommand cmd = new MySqlCommand(Comman, connection);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        //====================================================================
        public bool openConnect()
        {
            return openConnect(IPserver, DatabaseName, User, PassWord);
        }
        //
        private bool openConnect(string IPserver, string DatabaseName, string User, string PassWord)
        {
            try
            {
                string MyConString = "SERVER = " + IPserver + ";" + "DATABASE = " + DatabaseName + ";" + "UID = " + User + ";" + "PASSWORD = " + PassWord + ";charset=utf8;SslMode=none;";
                Aconnection = new MySqlConnection(MyConString);
                Acomman = new MySqlCommand();
                Acomman = Aconnection.CreateCommand();
                Aconnection.Open();
                return true;
            }
            catch (Exception Ex)
            {
                string Exm = Ex.Message;
                return false;
            }
        }
        //
        public bool closeConnect()
        {
            try
            {
                Aconnection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //
        private DataTable GetDataTableMySQL(string Comman1)
        {
            DataTable dt = new DataTable();
            Acomman.CommandType = CommandType.Text;// vua them
            Acomman.CommandText = Comman1;
            Acomman.Connection = Aconnection; // vua them
            // =========================================
            try
            {
                MySqlDataAdapter readerAdapter = new MySqlDataAdapter(Acomman);
                readerAdapter.Fill(dt); // DO DU LIEU VAO TABLE
                return dt;
            }
            catch (Exception ex)
            {
                string Emess = ex.Message;
                return null;
            }
        }

        private DataTable GetProcedureDataTableMySQL(string procedureName, string[] keys, string[] parameters)
        {
            DataTable dt = new DataTable();
            Acomman.CommandType = CommandType.StoredProcedure;// vua them
            Acomman.CommandText = procedureName;
            for (int i = 0; i < keys.Length; i++)
            {
                Acomman.Parameters.AddWithValue(keys[i], parameters[i]);
            }
            //Acomman.Parameters.AddRange(parameters);
            Acomman.Connection = Aconnection; // vua them
            // =========================================
            try
            {
                MySqlDataAdapter readerAdapter = new MySqlDataAdapter(Acomman);
                readerAdapter.Fill(dt); // DO DU LIEU VAO TABLE
                return dt;
            }
            catch (Exception ex)
            {
                string Emess = ex.Message;
                return null;
            }
        }
        //
        private bool ExecuteMySQL(string Comman)
        {
            Acomman.CommandType = CommandType.Text;// vua them
            Acomman.CommandText = Comman;
            Acomman.Connection = Aconnection; // vua them
            // =========================================
            MySqlTransaction transaction;
            transaction = Aconnection.BeginTransaction();// bat dau qua trinh transaction
            Acomman.Transaction = transaction;
            //===================================
            try
            {
                Acomman.ExecuteNonQuery();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                string data = ex.Message;
                return false;
            }
        }
    }
}
