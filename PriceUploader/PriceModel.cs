using LAny;
using LOffice;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PriceUploader
{
    public class PriceModel
    {
         //$sql = sprintf("
         //           SELECT *
         //           FROM %sproduct_alias
         //           INNER JOIN %sproduct ON prod_id=pa_prod_id
         //           LEFT JOIN %sproduct_price ON pp_prod_id=prod_id
         //           WHERE pa_code='%s'",
         //           DB_PREFIX, DB_PREFIX, DB_PREFIX, tosql($alias));

        
        private string strConn = string.Empty;



        public string StrDatabase
        {
            get 
            {
                return strDatabase;
            }
            
            set 
            {
                strDatabase = value;
            }
        }
        private string strDatabase = string.Empty;

        public string StrServer
        {
            get
            {
                return strServer;
            }

            set
            {
                strServer = value;
            }
        }
        private string strServer = string.Empty;

        public string StrUserId
        {
            get
            {
                return strUserId;
            }

            set
            {
                strUserId = value;
            }
        }
        private string strUserId = string.Empty;

        public string StrPassword
        {
            get
            {
                return strPassword;
            }

            set
            {
                strPassword = value;
            }
        }
        private string strPassword = string.Empty;

        public string StrPort
        {
            get
            {
                return strPort;
            }

            set
            {
                strPort = value;
            }
        }
        private string strPort = string.Empty;

        private MySqlConnection conn;
        private MySqlDataAdapter dataAdapter;
        private MySqlCommandBuilder commandBuilder;
        private static object _lock = new object();

        public PriceModel() 
        {
            this.strServer = cAppConfig.GetAppSettings("server");
            this.strUserId = cAppConfig.GetAppSettings("userId");
            this.strPassword = cAppConfig.GetAppSettings("password");
            this.strPort = cAppConfig.GetAppSettings("port");
            this.strDatabase = cAppConfig.GetAppSettings("database");

            this.GetStrConn();
            this.conn = GetConn();
        }


        public string GetStrConn()
        {
            strConn = string.Format("server={0};user id={1};password={2};port={3};", 
                strServer, strUserId, strPassword, strPort);

            return strConn;
        }


        public int SaveDatabaseSettings()
        {
            cAppConfig.UpdateAppSetting("database", strDatabase);
            cAppConfig.UpdateAppSetting("server", strServer);
            cAppConfig.UpdateAppSetting("userId", strUserId);
            cAppConfig.UpdateAppSetting("password", strPassword);
            cAppConfig.UpdateAppSetting("port", strPort);
            return 0;
        }


        public MySqlConnection GetConn()
        {
			try 
			{
                conn = new MySqlConnection(GetStrConn());
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

        public int Load_product_and_alias(ref DataTable dataTable, string pa_code)
        {
            return FillData(string.Format("SELECT * FROM product_alias INNER JOIN product ON prod_id=pa_prod_id LEFT JOIN product_price ON pp_prod_id=prod_id where pa_code = '{0}'", pa_code), ref dataTable);
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

        
        public Task<DataTable> Load_product_and_alias()
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
                        string sql = @"SELECT prod_id, prod_name, prod_income_price, prod_text, prod_client_price, prod_price_col1, prod_price_col2, prod_price_col3, 
prod_fixed_price, pa_code, prod_pc_id FROM product_alias INNER JOIN product pr ON pr.prod_id=pa_prod_id";
                        MySqlDataAdapter da = new MySqlDataAdapter(sql, con);
                        commandBuilder = new MySqlCommandBuilder(da);
                        da.Fill(dt);
                    }
                }
                return (dt);
            });
        }

        private MySqlDataAdapter da_import_settings = null;
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
                        da_import_settings = new MySqlDataAdapter("SELECT * FROM import_settings order by is_name", con);
                        commandBuilder = new MySqlCommandBuilder(da_import_settings);
                        da_import_settings.Fill(dt);
                    }
                }
                return (dt);
            });
        }

        public void Update_import_settings(ref DataTable dt)
        {
            DataTable changes = dt.GetChanges();
            if (changes != null)
            {
                da_import_settings.Update(changes);
                dt.AcceptChanges();
            }
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



        public int ImportExcel(string fileName, ref DataTable data)
        {
            cExcelObj exl = new cExcelObj();

            data = new DataTable();
            int res = exl.readExcelFileSQL(fileName, ref data);
          
            return res;
        }

    }

    public class DataObject
    {
        public DataTable dataTable;
        public MySqlDataAdapter mySqlDataAdapter; 
    
    }

    public class Product
    { 
        public object prod_id = 0;
        public object prod_name;
        public object prod_income_price;
        public object prod_text;
        public object prod_client_price;
        public object prod_price_col1;
        public object prod_price_col2;
        public object prod_price_col3;
        public object prod_fixed_price;
        public string pa_code;
        public object prod_pc_id;
    }
        
    public class ImportToDB
    {
        public string prod_name = string.Empty;
        public string prod_code = string.Empty;
        public string prod_income_price = string.Empty;
        public string prod_presense1 = string.Empty;
        public string prod_presense2 = string.Empty;
        public string prod_currency = string.Empty;
        public string prod_client_price = string.Empty;
        public string prod_pc_id = string.Empty;
        public int number = 0;
    
    }


    public class CategoryCharge
    {
        public object cc_pc_id = 0;
        public object cc_price_from = 0;
        public object cc_price_to = 0;
        public object cc_charge = 0;

    }


}
