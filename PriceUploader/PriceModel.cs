using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PriceUploader
{
    public class PriceModel
    {
        private string strConn = string.Empty;
        private string strDatabase = string.Empty;
        private string server = "78.46.43.211";
        private string userId = "akara_tomcat";
        private string password = "tomcat";
        private string port= "3306";

        private MySqlConnection conn;
        private MySqlDataAdapter dataAdapter;
        private MySqlCommandBuilder commandBuilder;
        private object _lock = new object();

        public PriceModel() 
        {
            this.GetStrConn();
            this.GetStrDataBase();
            this.conn = GetConn();
        }

        //public string GetStrConn()
        //{ 
        //    System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
        //    strConn = cas.GetValue("strConn", typeof(string)).ToString();
        //    return strConn;
        //}

        //public string GetStrDataBase()
        //{
        //    System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
        //    strDatabase = cas.GetValue("dataBase", typeof(string)).ToString();
        //    return strDatabase;
        //}

        public string GetStrConn(string server ="78.46.43.211",
            string userId = "akara_tomcat",
            string password = "tomcat",
            string port = "3306")
        {
            this.server = server;
            this.userId = userId;
            this.password = password;
            this.port = port;

            strConn = string.Format("server={0};user id={1};password={2};port={3};",
                this.server,
                this.userId,
                this.password,
                this.port);

            return strConn;
        }

        public string GetStrDataBase(string database = "akara_inteamtest")
        {
            strDatabase = database;
            return strDatabase;
        }

        public string GetServer()
        {
            System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
            return cas.GetValue("server", typeof(string)).ToString();
        }

        public string GetUserId()
        {
            System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
            return cas.GetValue("userId", typeof(string)).ToString();
        }
        
        public string GetPass()
        {
            System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
            return cas.GetValue("password", typeof(string)).ToString();
        }


        public string GetPort()
        {
            System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
            return cas.GetValue("port", typeof(string)).ToString();
        }

        public string GetDatabase()
        {
            System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
            return cas.GetValue("database", typeof(string)).ToString();
        }

        public int SaveDatabasSettings()
        {
            return 0;
        }


        public MySqlConnection GetConn()
        {
			try 
			{
                conn = new MySqlConnection(strConn);
				conn.Open();

                conn.ChangeDatabase(strDatabase);	
			}
			catch  
			{
                conn = null;				
			}

            return conn;
        }


        private int FillData(string sqlQuery, ref DataTable dataTable)
        {
            int rows = -1;
			dataTable = new DataTable();
            if (conn == null || (conn != null && conn.State != ConnectionState.Open))
                this.conn = GetConn();
            
            if (conn != null && conn.State == ConnectionState.Open)
            {
                dataAdapter = new MySqlDataAdapter(sqlQuery, this.conn);
                commandBuilder = new MySqlCommandBuilder(dataAdapter);
                dataAdapter.Fill(dataTable);
                rows = dataTable.Rows != null ? dataTable.Rows.Count : -1;
            }
            
            return rows;
        }


        #region load
        public int Load_category_charge(ref DataTable dataTable)
        {
            return FillData("SELECT * FROM category_charge", ref dataTable);
        }

        public int Load_import_settings(ref DataTable dataTable)
        {
            return FillData("SELECT * FROM import_settings", ref dataTable);
        }

        public int Load_price_category(ref DataTable dataTable)
        {
            return FillData("SELECT * FROM price_category", ref dataTable);
        }
        
        public int Load_product(ref DataTable dataTable)
        {
            return FillData("SELECT * FROM product", ref dataTable);
        }
        
        public int Load_product_alias(ref DataTable dataTable)
        {
            return FillData("SELECT * FROM product_alias", ref dataTable);
        }
        
        public int Load_product_category(ref DataTable dataTable)
        {
            return FillData("SELECT * FROM product_category", ref dataTable);
        }
        
        public int Load_product_price(ref DataTable dataTable)
        {
            return FillData("SELECT * FROM product_price", ref dataTable);
        }

        public int Load_supplier(ref DataTable dataTable)
        {
            return FillData("SELECT * FROM supplier", ref dataTable);
        }
        #endregion load


        public Task<DataTable> Load_category_charge()
        {
            return Task<DataTable>.Factory.StartNew(() =>
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(strConn))
                {
                    if (con != null)
                    {
                        con.Open();
                        System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
                        con.ChangeDatabase(cas.GetValue("dataBase", typeof(string)).ToString());
                        MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM category_charge", con);
                        commandBuilder = new MySqlCommandBuilder(da);
                        da.Fill(dt);
                    }
                }
                return dt;
                
            });
        }

        public Task<DataTable> Load_import_settings()
        {
            return Task<DataTable>.Factory.StartNew(() =>
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(strConn))
                {
                    if (con != null)
                    {
                        con.Open();
                        System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
                        con.ChangeDatabase(cas.GetValue("dataBase", typeof(string)).ToString());
                        MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM import_settings", con);
                        commandBuilder = new MySqlCommandBuilder(da);
                        da.Fill(dt);
                    }
                }
                return dt;
            });
        }

        public Task<DataTable> Load_price_category()
        {
            return Task<DataTable>.Factory.StartNew(() =>
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(strConn))
                {
                    if (con != null)
                    {
                        con.Open();
                        System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
                        con.ChangeDatabase(cas.GetValue("dataBase", typeof(string)).ToString());
                        MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM price_category", con);
                        commandBuilder = new MySqlCommandBuilder(da);
                        da.Fill(dt);
                    }
                }
                return dt;
            });
        }

        public Task<DataTable> Load_product()
        {
            return Task<DataTable>.Factory.StartNew(() =>
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(strConn))
                {
                    if (con != null)
                    {
                        con.Open();
                        System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
                        con.ChangeDatabase(cas.GetValue("dataBase", typeof(string)).ToString());
                        MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM product", con);
                        commandBuilder = new MySqlCommandBuilder(da);
                        da.Fill(dt);
                    }
                }
                return dt;
            });
        }

        public Task<DataTable> Load_product_alias()
        {
            return Task<DataTable>.Factory.StartNew(() =>
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(strConn))
                {
                    if (con != null)
                    {
                        con.Open();
                        System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
                        con.ChangeDatabase(cas.GetValue("dataBase", typeof(string)).ToString());
                        MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM product_alias", con);
                        commandBuilder = new MySqlCommandBuilder(da);
                        da.Fill(dt);
                    }
                }
                return dt;
            });
        }

        public Task<DataTable> Load_product_category()
        {
            return Task<DataTable>.Factory.StartNew(() =>
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(strConn))
                {
                    if (con != null)
                    {
                        con.Open();
                        System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
                        con.ChangeDatabase(cas.GetValue("dataBase", typeof(string)).ToString());
                        MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM product_category", con);
                        commandBuilder = new MySqlCommandBuilder(da);
                        da.Fill(dt);
                    }
                }
                return dt;
            });
        }

        public Task<DataTable> Load_product_price()
        {
            return Task<DataTable>.Factory.StartNew(() =>
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(strConn))
                {
                    if (con != null)
                    {
                        con.Open();
                        System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
                        con.ChangeDatabase(cas.GetValue("dataBase", typeof(string)).ToString());
                        MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM product_price", con);
                        commandBuilder = new MySqlCommandBuilder(da);
                        da.Fill(dt);
                    }
                }
                return dt;
            });
        }

        public Task<DataTable> Load_supplier()
        {
            return Task<DataTable>.Factory.StartNew(() =>
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(strConn))
                {
                    if (con != null)
                    {
                        con.Open();
                        System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
                        con.ChangeDatabase(cas.GetValue("dataBase", typeof(string)).ToString());	
                        MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM supplier", con);
                        commandBuilder = new MySqlCommandBuilder(da);
                        da.Fill(dt);
                    }
                }
                return dt;
            });
        }



    }
}
