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


        public event EventHandler InsertDataError = delegate { };


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

        public int CheckConnect(string strDatabase, string strServer, string strUserId, string strPassword, string strPort)
        {
            MySqlConnection cn = null;
            int res = 0;
            try
            {
                cn = new MySqlConnection(string.Format("server={0};user id={1};password={2};port={3};",
                    strServer, strUserId, strPassword, strPort));
                cn.Open();

                cn.ChangeDatabase(strDatabase);
            }
            catch { }
            finally
            {
                if (cn != null && cn.State == ConnectionState.Open)
                {
                    cn.Close();
                    res = 0;
                }
                else
                {
                    res = -1;
                }
                cn = null;
            }

            return res;
        }

        public int SaveDatabaseSettings()
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            cAppConfig.UpdateAppSetting(config, "database", strDatabase);
            cAppConfig.UpdateAppSetting(config, "server", strServer);
            cAppConfig.UpdateAppSetting(config, "userId", strUserId);
            cAppConfig.UpdateAppSetting(config, "password", strPassword);
            cAppConfig.UpdateAppSetting(config, "port", strPort);
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

        public void InsertData(DataTable table, string supplier_id, string price_type)
        {
            if (table == null)
                return;

            if (table.Rows.Count == 0)
                return;

            int product_id = 0;
            double product_client_price = 0;
            double product_fixed_price = 0;
            
            MySqlCommand cmd = null;
            string sql = string.Empty;

            //select set_value from engine_settings where set_name = 'euro_rate'
            //select set_value from engine_settings where set_name = 'currency_rate_cash'
            //$price = $price * EURO_RATE / CURRENCY_RATE_CASH;
            double euro_rate = 0;
            double currency_rate_cash = 0;
                
            MySqlDataReader rdr = null;

            try
            {
                this.conn = GetConn();
                sql = string.Format("select set_value from engine_settings where set_name = 'euro_rate'");
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                rdr.Read();
                euro_rate = rdr.GetDouble(0);
                conn.Close();

                conn = this.GetConn();
                sql = string.Format("select set_value from engine_settings where set_name = 'currency_rate_cash'");
                rdr = null;
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                rdr.Read();
                currency_rate_cash = rdr.GetDouble(0);
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                string error = ex.Source;
                throw;
            }

            int rowNumber = 0;
            // Проверка валидности
            foreach (DataRow row in table.Rows)
            {
                if (row.Field<string>("is_selected") == "True" && string.IsNullOrEmpty(row.Field<string>("prod_pc_id")))
                {
                    InsertDataError(rowNumber + 1, null);
                    return;
                }

                rowNumber++;
            }

            rowNumber = 0;
            foreach (DataRow row in table.Rows)
            {
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                bool is_new = row.Field<bool>("is_new");
                string is_selected_TMP = row.Field<string>("is_selected");

                bool is_selected = false;
                if (!string.IsNullOrEmpty(is_selected_TMP)
                    && is_selected_TMP.ToLower() == "true")
                {
                    is_selected = true;
                }

                string prod_id = row.Field<string>("prod_id");
                string prod_name = row.Field<string>("prod_name");
                string code = row.Field<string>("prod_code");
                string new_code = row.Field<string>("prod_new_code");
                string prod_income_price = row.Field<string>("prod_income_price");
                string prod_client_price = row.Field<string>("prod_client_price");
                string prod_pc_id = row.Field<string>("prod_pc_id");
                string color = row.Field<string>("color");

                if (string.IsNullOrEmpty(color))
                    color = string.Empty;

                if (string.IsNullOrEmpty(new_code))
                    new_code = string.Empty;

                if (string.IsNullOrEmpty(code))
                    code = new_code;

                if (!string.IsNullOrEmpty(prod_pc_id)
                    && ((is_new && is_selected) || (color.ToLower() == "green")))
                {
                    product_client_price = 0;
                    if (!string.IsNullOrEmpty(prod_client_price))
                        product_client_price = System.Convert.ToDouble(prod_client_price);

                    product_fixed_price = 0;
                    if (!string.IsNullOrEmpty(prod_income_price))
                        product_fixed_price = System.Convert.ToDouble(prod_income_price);

                    product_id = 0;
                    if (string.IsNullOrEmpty(prod_id))  //новое значение
                    {

                        try
                        {
                            //$sql = sprintf("INSERT INTO %sproduct SET prod_pc_id=%d, prod_name='%s', prod_text='', prod_disabled='Y', prod_vat='Y', prod_actuality=%d, prod_postdate=%d, prod_last_update=%d, prod_last_user_id=%d",
                            //    DB_PREFIX, $new_product_category[$num], tosql($product_names[$num]), $preset['is_actuality'], ctime(), ctime(), $this->auth->get_id());
                            //$this->db->query($sql);
                            //if ($this->db->insert_id())
                            //{
                            //    $prod_id = $this->db->insert_id();

                            //    $code = trim($codes[$num]);
                            //    $sql = sprintf("INSERT INTO %sproduct_alias SET pa_prod_id=%d, pa_code='%s'", DB_PREFIX, $prod_id, tosql($code));
                            //    $this->db->query($sql);
                            //}

                            conn = this.GetConn();
                            sql = string.Format("INSERT INTO product SET prod_pc_id={0}, prod_name='{1}', prod_text='', prod_disabled='Y', prod_vat='Y', prod_actuality={2}, prod_postdate={3}, prod_last_update={4}, prod_last_user_id={5}",
                                prod_pc_id, prod_name, 1, unixTimestamp, unixTimestamp, 0);
                            cmd = new MySqlCommand(sql, conn);
                            cmd.ExecuteNonQuery();
                            conn.Close();

                            conn = this.GetConn();
                            product_id = 0;    //нужно получить ID
                            sql = string.Format("SELECT max(prod_id) FROM product");
                            rdr = null;
                            cmd = new MySqlCommand(sql, conn);
                            rdr = cmd.ExecuteReader();
                            rdr.Read();
                            product_id = rdr.GetInt32(0);
                            conn.Close();

                            conn = this.GetConn();
                            sql = string.Format("INSERT INTO product_alias SET pa_prod_id={0}, pa_code='{1}'",
                                product_id, code);
                            cmd = new MySqlCommand(sql, conn);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            conn.Close();
                            string error = ex.Source;
                            throw;
                        }
                    }
                    else
                    {
                        product_id = System.Convert.ToInt32(prod_id);
                    }

                    if (product_id > 0 /* && product_price > 0 */ )
                    {
                        //$sql = sprintf('UPDATE %sproduct SET prod_price_sup_id=%d, prod_fixed_price=%f WHERE prod_id=%d', DB_PREFIX, $supplier, $price, $product_info['prod_id']);

                        if (price_type == "RRC" && product_fixed_price > 0)
                        {
                            try
                            {
                                conn = this.GetConn();    
                                sql = string.Format("UPDATE product SET prod_price_sup_id={0}, prod_fixed_price={1} WHERE prod_id={2}",
                                    supplier_id, product_fixed_price, product_id);
                                cmd = new MySqlCommand(sql, conn);
                                cmd.ExecuteNonQuery();
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                conn.Close();
                                string error = ex.Source;
                                throw;
                            }
                        }
                        else
                        {
                            //$sql = sprintf('INSERT INTO %sproduct_price SET pp_prod_id=%d, pp_sup_id=%d, pp_price=%f, pp_postdate=%d', DB_PREFIX, $prod_id, $supplier, $price, ctime());
                            //$this->db->query($sql);

                            //$client_price = $this->get_product_custom_price($price, $product_info['prod_pc_id']);
                            //$sql = sprintf('UPDATE %sproduct SET prod_price_sup_id=%d, prod_price_update_timestamp=%d, prod_income_price=%f, prod_client_price=%f, prod_actuality=%d WHERE prod_id=%d', DB_PREFIX, $supplier, ctime(), $price, $client_price, $preset['is_actuality'], $product_info['prod_id']);
                            //$this->db->query($sql);

                            try
                            {
                                if (price_type == "EURO" && currency_rate_cash > 0)
                                    product_client_price = currency_rate_cash * euro_rate / currency_rate_cash;

                                conn = this.GetConn();
                                sql = string.Format("INSERT INTO product_price SET pp_prod_id={0}, pp_sup_id={1}, pp_price={2}, pp_postdate={3}",
                                    product_id, supplier_id, product_client_price, unixTimestamp);
                                cmd = new MySqlCommand(sql, conn);
                                cmd.ExecuteNonQuery();
                                conn.Close();

                                conn = this.GetConn();
                                sql = string.Format("UPDATE product SET prod_price_sup_id={0}, prod_price_update_timestamp={1}, prod_income_price={2}, prod_client_price={3}, prod_actuality={4} WHERE prod_id={5}",
                                    supplier_id, unixTimestamp, product_fixed_price, product_client_price, true, product_id);
                                cmd = new MySqlCommand(sql, conn);
                                cmd.ExecuteNonQuery();
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                conn.Close();
                                string error = ex.Source;
                                throw;
                            }
                        }


                        if (is_new)
                        {
                            try
                            {
                                //$sql = sprintf("INSERT INTO %sproduct_alias SET pa_prod_id=%d, pa_code='%s'", DB_PREFIX, $prod_id, tosql($code));
                                conn = this.GetConn();
                                sql = string.Format("INSERT INTO product_alias SET pa_prod_id={0}, pa_code='{1}'",
                                    product_id, code);
                                cmd = new MySqlCommand(sql, conn);
                                cmd.ExecuteNonQuery();
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                conn.Close();
                                string error = ex.Source;
                                throw;
                            }
                        }

                    }


                    try
                    {
                        //$sql = sprintf('INSERT INTO %sproduct_price SET pp_prod_id=%d, pp_sup_id=%d, pp_price=%f, pp_postdate=%d', DB_PREFIX, $prod_id, $supplier, $price, ctime());
                        conn = this.GetConn();
                        sql = string.Format("INSERT INTO product_price SET pp_prod_id={0}, pp_sup_id={1}, pp_price={2}, pp_postdate={3}",
                            product_id, supplier_id, product_fixed_price, unixTimestamp);
                        cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        string error = ex.Source;
                        throw;
                    }
                }

                rowNumber++;
            }        
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
                        MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM supplier ORDER BY sup_name", con);
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
        public int number = 0;
        public string prod_name = string.Empty;
        public string prod_code = string.Empty;
        public string prod_income_price = string.Empty;
        public string prod_presense1 = string.Empty;
        public string prod_presense2 = string.Empty;
        public string prod_currency = string.Empty;
        public string prod_client_price = string.Empty;
        public string prod_pc_id = string.Empty;
        public string product_pa_code = string.Empty;
        public string product_prod_pc_id = string.Empty;
        public string prod_id = string.Empty;
        public string prod_fixed_price = string.Empty;
    }


    public class CategoryCharge
    {
        public object cc_pc_id = 0;
        public object cc_price_from = 0;
        public object cc_price_to = 0;
        public object cc_charge = 0;

    }


}
