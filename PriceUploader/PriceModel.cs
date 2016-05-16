using Excel;
using LAny;
using LOffice;
using log4net;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PriceUploader
{
    public class PriceModel
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PriceModel));
        public DataTable TableCategoryCharge = null;
        //public DataTable TableImportSettings = null;
        public DataTable TablePriceCategory = null;
        public DataTable TableProduct = null;
        public DataTable TableProductAlias = null;
        public DataTable TableProductCategory = null;
        //public DataTable TableProductPrice = null;
        public DataTable TableSupplier = null;
        //public DataTable TableExcelData = null;
        public DataTable TableProductAndAlias = null;
        public List<Product> products = new List<Product>();
        public List<ImportToDB> excelList = new List<ImportToDB>();
        public List<ImportToDB> listImportToDB = new List<ImportToDB>();
        public int ProdActuality = 0;
        public string PresenseValue = string.Empty;
        public string GRNsign = string.Empty;

        public double Euro_rate = 0;
        public double Currency_rate_cash = 0;


        //$sql = sprintf("
        //           SELECT *
        //           FROM %sproduct_alias
        //           INNER JOIN %sproduct ON prod_id=pa_prod_id
        //           LEFT JOIN %sproduct_price ON pp_prod_id=prod_id
        //           WHERE pa_code='%s'",
        //           DB_PREFIX, DB_PREFIX, DB_PREFIX, tosql($alias));

        public List<CategoryCharge> categoryCharge = new List<CategoryCharge>();
        public List<Category> categories = new List<Category>();

        public event EventHandler InsertDataError = delegate { };
        public event EventHandler OnAddRow = delegate { };
        public event EventHandler OnSaveAll = delegate { };

        public const string RRC = "РРЦ";

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

            LoadCurrency();
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
                dataAdapter.SelectCommand.CommandTimeout = 1600;
                commandBuilder = new MySqlCommandBuilder(dataAdapter);
                
                dataAdapter.Fill(dataTable);
                rows = dataTable.Rows != null ? dataTable.Rows.Count : -1;
            }
            
            return rows;
        }

        public void InsertData(DataTable table, string supplier_id, string price_type)
        {
            log.Info("InsertData(DataTable table, string supplier_id, string price_type)");

            if (table == null)
                return;

            if (table.Rows.Count == 0)
                return;

            log.Info("table.Rows.Count = " + table.Rows.Count.ToString());

            AddRowInfo addRowInfo = new AddRowInfo();
            DateTime dateTimeBeg = DateTime.Now;

            log.Info("DateTime dateTimeBeg = DateTime.Now;");

            int product_id = 0;
            double product_client_price = 0;
            double product_fixed_price = 0;

            string client_price = string.Empty;
            string fixed_price = string.Empty;

            MySqlCommand cmd = null;
            log.Info("MySqlCommand cmd = null;");
            string sql = string.Empty;

            //select set_value from engine_settings where set_name = 'euro_rate'
            //select set_value from engine_settings where set_name = 'currency_rate_cash'
            //$price = $price * EURO_RATE / CURRENCY_RATE_CASH;
            

            int resultExecut = -888;
            //MySqlDataReader rdr = null;

            log.Info("MySqlDataReader rdr = null;");

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

            conn = this.GetConn();
            MySqlTransaction trans = conn.BeginTransaction();
            log.Info("MySqlTransaction trans = conn.BeginTransaction();");

            rowNumber = 0;
            try
            {

                # region foreach (DataRow row in table.Rows)
                
                foreach (DataRow row in table.Rows)
                {
                    table.Rows[rowNumber]["prod_id_is_new"] = false;

                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                    bool is_new = row.Field<bool>("is_new");
                    string is_selected_TMP = row.Field<string>("is_selected");

                    bool is_selected = false;
                    if (!string.IsNullOrEmpty(is_selected_TMP) && is_selected_TMP.ToLower() == "true")
                        is_selected = true;

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

                    if (!string.IsNullOrEmpty(prod_pc_id) && !string.IsNullOrEmpty(code) && ((is_new && is_selected) || (color.ToLower() == "green")))
                    {
                        //product_client_price = 0;
                        //if (!string.IsNullOrEmpty(prod_client_price))
                        //    product_client_price = System.Convert.ToDouble(prod_client_price);

                        //product_fixed_price = 0;
                        //if (!string.IsNullOrEmpty(prod_income_price))
                        //    product_fixed_price = System.Convert.ToDouble(prod_income_price);

                        product_id = 0;
                        if (string.IsNullOrEmpty(prod_id))  //новое значение
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

                            //conn = this.GetConn();
                            
                            sql = string.Format("INSERT INTO product SET prod_pc_id={0}, prod_name='{1}', prod_text='', prod_disabled='Y', prod_vat='Y', prod_actuality={2}, prod_postdate={3}, prod_last_update={4}, prod_last_user_id={5}",
                                prod_pc_id, prod_name, this.ProdActuality, unixTimestamp, unixTimestamp, 2);

                            cmd = new MySqlCommand(sql, conn);
                            cmd.CommandTimeout = 0;
                            resultExecut = cmd.ExecuteNonQuery();
                            product_id = (int)cmd.LastInsertedId;
                            Debug.WriteLine("1. INSERT INTO product => resultExecut: " + resultExecut);
                            log.Info(sql);

                            table.Rows[rowNumber]["prod_id"] = product_id;
                            table.Rows[rowNumber]["prod_id_is_new"] = true;
                            
                            sql = string.Format("INSERT INTO product_alias SET pa_prod_id={0}, pa_code='{1}'", product_id, code);
                            cmd = new MySqlCommand(sql, conn);
                            cmd.CommandTimeout = 0;
                            resultExecut = cmd.ExecuteNonQuery();
                            Debug.WriteLine("2. INSERT INTO product_alias => resultExecut: " + resultExecut);
                            log.Info(sql);
                        }
                    }

                    rowNumber++;
                }

                # endregion foreach (DataRow row in table.Rows)


                # region foreach (DataRow row in table.Rows) 2

                string sqlQuery_product = string.Empty;
                string sqlQuery_product_prev1 = string.Empty;
                string sqlQuery_product_prev2 = string.Empty;

                string sqlQuery_product_alias = string.Empty    /* "INSERT INTO product_alias (pa_prod_id, pa_code) VALUES" */;
                string sqlQuery_product_alias_END = string.Empty;
                string sqlQuery_product_price = string.Empty    /* "INSERT INTO product_price (pp_prod_id, pp_sup_id, pp_price, pp_postdate) VALUES" */;
                string sqlQuery_product_price_END = string.Empty;

                rowNumber = 0;
                foreach (DataRow row in table.Rows)
                {
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                    bool prod_id_is_new = row.Field<bool>("prod_id_is_new");
                    bool is_new = row.Field<bool>("is_new");
                    string is_selected_TMP = row.Field<string>("is_selected");

                    bool is_selected = false;
                    if (!string.IsNullOrEmpty(is_selected_TMP) && is_selected_TMP.ToLower() == "true")
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
                    string prod_qty = row.Field<string>("prod_qty");
                    //bool is_presence = row.Field<bool>("is_presence");
                    string prod_currency = row.Field<string>("prod_currency");

                    string prod_presense1 = row.Field<string>("prod_presense1");
                    string prod_presense2 = row.Field<string>("prod_presense2");

                    var presenseArray = this.PresenseValue.Split(',');
                    bool is_presence = false;

                    if(presenseArray != null)
                        foreach (string item in presenseArray)
                        {
                            if (item.Trim().ToLower() == prod_presense1.Trim().ToLower()
                                || item.Trim().ToLower() == prod_presense2.Trim().ToLower())
                            {
                                is_presence = true;
                                break;
                            }
                        }


                    if (string.IsNullOrEmpty(color))
                        color = string.Empty;

                    if (string.IsNullOrEmpty(new_code))
                        new_code = string.Empty;

                    if (string.IsNullOrEmpty(code))
                        code = new_code;

                    //if (rowNumber == 10)
                    //    rowNumber = rowNumber;

                    if (!string.IsNullOrEmpty(prod_pc_id) && !string.IsNullOrEmpty(code) && ((is_new && is_selected) || (color.ToLower() == "green")))
                    {
                        product_id = System.Convert.ToInt32(prod_id);
                    }
                    else
                    {
                        if (rowNumber % 16 == 0)
                        {
                            addRowInfo.index = rowNumber;
                            addRowInfo.timeRuning = DateTime.Now - dateTimeBeg;
                            Debug.WriteLine("rowNumber: " + rowNumber);
                            OnAddRow(addRowInfo, null);
                        }

                        rowNumber++;
                        continue;
                    }
                    
                    product_client_price = 0;
                    if (!string.IsNullOrEmpty(prod_client_price))
                        product_client_price = System.Convert.ToDouble(PriceModel.ConvertSeparator(prod_client_price));

                    product_fixed_price = 0;
                    if (!string.IsNullOrEmpty(prod_income_price))
                        product_fixed_price = Math.Round(System.Convert.ToDouble(PriceModel.ConvertSeparator(prod_income_price)), 2) ;

                    if (this.GRNsign == prod_currency)
                    {
                        double price = 0;
                        //if (!string.IsNullOrEmpty(prod_income_price))
                        //    price = Math.Round(System.Convert.ToDouble(PriceModel.ConvertSeparator(prod_income_price)), 2);

                        //if (price > 0 && this.Currency_rate_cash > 0)
                        //    price = price / this.Currency_rate_cash;

                        //sprod_income_price = price.ToString();
                    }
                    else if (price_type == "EURO" && this.Currency_rate_cash > 0)
                    {
                        double price = 0;
                        if (!string.IsNullOrEmpty(prod_income_price))
                            price = Math.Round(System.Convert.ToDouble(PriceModel.ConvertSeparator(prod_income_price)), 2);

                        if (price > 0 && this.Currency_rate_cash > 0)
                            price = price * this.Euro_rate / this.Currency_rate_cash; 

                        prod_income_price = price.ToString();
                    }

                    string s = CalcClientPrice(ref this.categoryCharge, prod_income_price, GetCategoryChargeIDFromCategories(prod_pc_id), prod_qty, price_type, code);
                    if (!string.IsNullOrEmpty(s))
                        product_client_price = System.Convert.ToDouble(PriceModel.ConvertSeparator(s));

                    //if (!prod_id_is_new  /* product_id > 0 */ /* && product_price > 0 */ )
                    {
                        //$sql = sprintf('UPDATE %sproduct SET prod_price_sup_id=%d, prod_fixed_price=%f WHERE prod_id=%d', DB_PREFIX, $supplier, $price, $product_info['prod_id']);

                        if (price_type == RRC && product_fixed_price > 0)
                        {
                            fixed_price = product_fixed_price.ToString();
                            fixed_price = fixed_price.Replace(',', '.');

                            if (is_presence)
                            {
                                sql = string.Format("UPDATE product SET prod_price_sup_id={0}, prod_fixed_price={1}, prod_price_update_timestamp={3} WHERE prod_id={2};\n",
                                    supplier_id, fixed_price, product_id, unixTimestamp);
                            }
                            else
                            {
                                sql = string.Format("UPDATE product SET prod_price_sup_id={0}, prod_fixed_price={1} WHERE prod_id={2};\n",
                                    supplier_id, fixed_price, product_id);
                            }

                            if (sql != sqlQuery_product_prev1)
                                sqlQuery_product = string.Concat(sqlQuery_product, sql);

                            sqlQuery_product_prev1 = sql;

                            Debug.WriteLine("3. UPDATE product => resultExecut: " + resultExecut);
                            log.Info(sql);
                        }
                        else
                        {
                            //$sql = sprintf('INSERT INTO %sproduct_price SET pp_prod_id=%d, pp_sup_id=%d, pp_price=%f, pp_postdate=%d', DB_PREFIX, $prod_id, $supplier, $price, ctime());
                            //$this->db->query($sql);

                            //$client_price = $this->get_product_custom_price($price, $product_info['prod_pc_id']);
                            //$sql = sprintf('UPDATE %sproduct SET prod_price_sup_id=%d, prod_price_update_timestamp=%d, prod_income_price=%f, prod_client_price=%f, prod_actuality=%d WHERE prod_id=%d', DB_PREFIX, $supplier, ctime(), $price, $client_price, $preset['is_actuality'], $product_info['prod_id']);
                            //$this->db->query($sql);

                            if (product_client_price > 0)
                            { 
                                client_price = product_client_price.ToString();
                                client_price = client_price.Replace(',', '.');

                                //sql = string.Format("INSERT INTO product_price SET pp_prod_id={0}, pp_sup_id={1}, pp_price={2}, pp_postdate={3}",
                                //    product_id, supplier_id, client_price, unixTimestamp);
                                //sqlQuery.Add(sql);

                                //Debug.WriteLine("4. INSERT INTO product_price => resultExecut: " + resultExecut);

                                fixed_price = product_fixed_price.ToString();
                                fixed_price = fixed_price.Replace(',', '.');

                                if (is_presence)
                                {
                                    sql = string.Format("UPDATE product SET prod_price_sup_id={0}, prod_price_update_timestamp={1}, prod_income_price={2}, prod_client_price={3}, prod_actuality={4} WHERE prod_id={5};\n",
                                        supplier_id, unixTimestamp, fixed_price, client_price, this.ProdActuality, product_id);
                                }
                                else
                                {
                                    sql = string.Format("UPDATE product SET prod_price_sup_id={0}, prod_income_price={1}, prod_client_price={2}, prod_actuality={3} WHERE prod_id={4};\n",
                                        supplier_id, fixed_price, client_price, this.ProdActuality, product_id);
                                }

                                if (sql != sqlQuery_product_prev2)
                                    sqlQuery_product = string.Concat(sqlQuery_product, sql);

                                sqlQuery_product_prev2 = sql;
                                Debug.WriteLine("5. UPDATE product => resultExecut: " + resultExecut);
                                log.Info(sql);
                            }
                        }

                        //if (is_new)
                        //{
                        //    //$sql = sprintf("INSERT INTO %sproduct_alias SET pa_prod_id=%d, pa_code='%s'", DB_PREFIX, $prod_id, tosql($code));

                        //    //sql = string.Format("INSERT INTO product_alias SET pa_prod_id={0}, pa_code='{1}'",  product_id, code);
                        //    sql = string.Format("INSERT INTO product_alias (pa_prod_id, pa_code) VALUES( {0}, '{1}' );\n", product_id, code);
                        //    sqlQuery_product_alias_END = string.Concat(sqlQuery_product_alias_END, sql);

                        //    Debug.WriteLine("6. INSERT INTO product_alias => resultExecut: " + resultExecut);
                        //    log.Info(sql);
                        //}
                    }

                    //$sql = sprintf('INSERT INTO %sproduct_price SET pp_prod_id=%d, pp_sup_id=%d, pp_price=%f, pp_postdate=%d', DB_PREFIX, $prod_id, $supplier, $price, ctime());

                    if (product_fixed_price > 0)
                    {
                        fixed_price = product_fixed_price.ToString();
                        fixed_price = fixed_price.Replace(',', '.');

                        //sql = string.Format("INSERT INTO product_price SET pp_prod_id={0}, pp_sup_id={1}, pp_price={2}, pp_postdate={3}", product_id, supplier_id, fixed_price, unixTimestamp);
                        sql = string.Format("INSERT INTO product_price (pp_prod_id, pp_sup_id, pp_price, pp_postdate) VALUES( {0}, {1}, {2}, {3} );\n", product_id, supplier_id, fixed_price, unixTimestamp);
                        sqlQuery_product_price_END = string.Concat(sqlQuery_product_price_END, sql);

                        Debug.WriteLine("7. INSERT INTO product_price => resultExecut: " + resultExecut);
                        log.Info(sql);
                    }

                    if (rowNumber % 16 == 0)
                    {
                        addRowInfo.index = rowNumber;
                        addRowInfo.timeRuning = DateTime.Now - dateTimeBeg;
                        Debug.WriteLine("rowNumber: " + rowNumber);
                        OnAddRow(addRowInfo, null);
                    }

                    rowNumber++;
                }
                
                # endregion foreach (DataRow row in table.Rows) 2

                if (!string.IsNullOrEmpty(sqlQuery_product))
                {
                    sqlQuery_product = sqlQuery_product.Remove(sqlQuery_product.Length - 1);
                    cmd = new MySqlCommand(sqlQuery_product, conn);
                    cmd.CommandTimeout = 0;
                    resultExecut = cmd.ExecuteNonQuery();
                    log.Info(sqlQuery_product);
                }

                if (!string.IsNullOrEmpty(sqlQuery_product_alias_END))
                {
                    sqlQuery_product_alias = string.Concat(sqlQuery_product_alias, sqlQuery_product_alias_END);
                    //sqlQuery_product_alias = sqlQuery_product_alias.Remove(sqlQuery_product_alias.Length - 1);
                    cmd = new MySqlCommand(sqlQuery_product_alias, conn);
                    cmd.CommandTimeout = 0;
                    resultExecut = cmd.ExecuteNonQuery();
                    log.Info(sqlQuery_product_alias);
                }

                if (!string.IsNullOrEmpty(sqlQuery_product_price_END))
                {
                    sqlQuery_product_price = string.Concat(sqlQuery_product_price, sqlQuery_product_price_END);
                    //sqlQuery_product_price = sqlQuery_product_price.Remove(sqlQuery_product_price.Length - 1);
                    cmd = new MySqlCommand(sqlQuery_product_price, conn);
                    cmd.CommandTimeout = 0;
                    resultExecut = cmd.ExecuteNonQuery();
                    log.Info(sqlQuery_product_price);
                }

                trans.Commit();

                log.Info("trans.Commit();");

                addRowInfo.index = (rowNumber - 1);
                addRowInfo.timeRuning = DateTime.Now - dateTimeBeg;
                //Debug.WriteLine("rowNumber: " + rowNumber);
                OnAddRow(addRowInfo, null);
                OnSaveAll(this, null);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                string error = ex.Source;
                //throw;

                log.Error(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }

        }


        private int GetCategoryChargeIDFromCategories(string str_pc_id)
        {
            int pc_id = -1;
            if (!string.IsNullOrEmpty(str_pc_id))
                pc_id = Convert.ToInt32(str_pc_id);
            int res = -1;
            var c = categories.FirstOrDefault(i => i.pc_id == pc_id);
            if (c != null)
                res = c.pc_id_from_category_charge;
            return res;
        }

        
        //public void InsertData3(DataTable table, string supplier_id, string price_type)
        //{
        //    if (table == null)
        //        return;

        //    if (table.Rows.Count == 0)
        //        return;

        //    AddRowInfo addRowInfo = new AddRowInfo();
        //    DateTime dateTimeBeg = DateTime.Now;

        //    int product_id = 0;
        //    double product_client_price = 0;
        //    double product_fixed_price = 0;

        //    string client_price = string.Empty;
        //    string fixed_price = string.Empty;

        //    MySqlCommand cmd = null;
        //    string sql = string.Empty;

        //    //select set_value from engine_settings where set_name = 'euro_rate'
        //    //select set_value from engine_settings where set_name = 'currency_rate_cash'
        //    //$price = $price * EURO_RATE / CURRENCY_RATE_CASH;
        //    double euro_rate = 0;
        //    double currency_rate_cash = 0;

        //    int resultExecut = -888;
        //    MySqlDataReader rdr = null;

        //    try
        //    {
        //        this.conn = GetConn();
        //        sql = string.Format("select set_value from engine_settings where set_name = 'euro_rate'");
        //        cmd = new MySqlCommand(sql, conn);
        //        rdr = cmd.ExecuteReader();
        //        rdr.Read();
        //        euro_rate = rdr.GetDouble(0);
        //        conn.Close();

        //        conn = this.GetConn();
        //        sql = string.Format("select set_value from engine_settings where set_name = 'currency_rate_cash'");
        //        rdr = null;
        //        cmd = new MySqlCommand(sql, conn);
        //        rdr = cmd.ExecuteReader();
        //        rdr.Read();
        //        currency_rate_cash = rdr.GetDouble(0);
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        conn.Close();
        //        string error = ex.Source;
        //        throw;
        //    }

        //    int rowNumber = 0;
        //    // Проверка валидности
        //    foreach (DataRow row in table.Rows)
        //    {
        //        if (row.Field<string>("is_selected") == "True" && string.IsNullOrEmpty(row.Field<string>("prod_pc_id")))
        //        {
        //            InsertDataError(rowNumber + 1, null);
        //            return;
        //        }

        //        rowNumber++;
        //    }

        //    conn = this.GetConn();
        //    MySqlTransaction trans = conn.BeginTransaction();

        //    rowNumber = 0;
        //    try
        //    {
        //        foreach (DataRow row in table.Rows)
        //        {
        //            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        //            bool is_new = row.Field<bool>("is_new");
        //            string is_selected_TMP = row.Field<string>("is_selected");

        //            bool is_selected = false;
        //            if (!string.IsNullOrEmpty(is_selected_TMP)
        //                && is_selected_TMP.ToLower() == "true")
        //            {
        //                is_selected = true;
        //            }

        //            string prod_id = row.Field<string>("prod_id");
        //            string prod_name = row.Field<string>("prod_name");
        //            string code = row.Field<string>("prod_code");
        //            string new_code = row.Field<string>("prod_new_code");
        //            string prod_income_price = row.Field<string>("prod_income_price");
        //            string prod_client_price = row.Field<string>("prod_client_price");
        //            string prod_pc_id = row.Field<string>("prod_pc_id");
        //            string color = row.Field<string>("color");

        //            if (string.IsNullOrEmpty(color))
        //                color = string.Empty;

        //            if (string.IsNullOrEmpty(new_code))
        //                new_code = string.Empty;

        //            if (string.IsNullOrEmpty(code))
        //                code = new_code;

        //            if (!string.IsNullOrEmpty(prod_pc_id)
        //                && !string.IsNullOrEmpty(code)
        //                && ((is_new && is_selected) || (color.ToLower() == "green")))
        //            {
        //                product_client_price = 0;
        //                if (!string.IsNullOrEmpty(prod_client_price))
        //                    product_client_price = System.Convert.ToDouble(prod_client_price);

        //                product_fixed_price = 0;
        //                if (!string.IsNullOrEmpty(prod_income_price))
        //                    product_fixed_price = System.Convert.ToDouble(prod_income_price);

        //                product_id = 0;
        //                if (string.IsNullOrEmpty(prod_id))  //новое значение
        //                {
        //                    //$sql = sprintf("INSERT INTO %sproduct SET prod_pc_id=%d, prod_name='%s', prod_text='', prod_disabled='Y', prod_vat='Y', prod_actuality=%d, prod_postdate=%d, prod_last_update=%d, prod_last_user_id=%d",
        //                    //    DB_PREFIX, $new_product_category[$num], tosql($product_names[$num]), $preset['is_actuality'], ctime(), ctime(), $this->auth->get_id());
        //                    //$this->db->query($sql);
        //                    //if ($this->db->insert_id())
        //                    //{
        //                    //    $prod_id = $this->db->insert_id();

        //                    //    $code = trim($codes[$num]);
        //                    //    $sql = sprintf("INSERT INTO %sproduct_alias SET pa_prod_id=%d, pa_code='%s'", DB_PREFIX, $prod_id, tosql($code));
        //                    //    $this->db->query($sql);
        //                    //}

        //                    //conn = this.GetConn();
        //                    sql = string.Format("INSERT INTO product SET prod_pc_id={0}, prod_name='{1}', prod_text='', prod_disabled='Y', prod_vat='Y', prod_actuality={2}, prod_postdate={3}, prod_last_update={4}, prod_last_user_id={5}",
        //                        prod_pc_id, prod_name, 1, unixTimestamp, unixTimestamp, 0);
        //                    cmd = new MySqlCommand(sql, conn);
        //                    resultExecut = cmd.ExecuteNonQuery();
        //                    product_id = (int)cmd.LastInsertedId;
        //                    Debug.WriteLine("1. INSERT INTO product => resultExecut: " + resultExecut);

        //                    sql = string.Format("INSERT INTO product_alias SET pa_prod_id={0}, pa_code='{1}'",
        //                        product_id, code);
        //                    cmd = new MySqlCommand(sql, conn);
        //                    resultExecut = cmd.ExecuteNonQuery();
        //                    Debug.WriteLine("2. INSERT INTO product_alias => resultExecut: " + resultExecut);
        //                }
        //                else
        //                {
        //                    product_id = System.Convert.ToInt32(prod_id);
        //                }

        //                if (product_id > 0 /* && product_price > 0 */ )
        //                {
        //                    //$sql = sprintf('UPDATE %sproduct SET prod_price_sup_id=%d, prod_fixed_price=%f WHERE prod_id=%d', DB_PREFIX, $supplier, $price, $product_info['prod_id']);

        //                    if (price_type == "RRC" && product_fixed_price > 0)
        //                    {
        //                        fixed_price = product_fixed_price.ToString();
        //                        fixed_price = fixed_price.Replace(',', '.');

        //                        sql = string.Format("UPDATE product SET prod_price_sup_id={0}, prod_fixed_price={1} WHERE prod_id={2}",
        //                            supplier_id, fixed_price, product_id);
        //                        cmd = new MySqlCommand(sql, conn);
        //                        resultExecut = cmd.ExecuteNonQuery();
        //                        Debug.WriteLine("3. UPDATE product => resultExecut: " + resultExecut);
        //                    }
        //                    else
        //                    {
        //                        //$sql = sprintf('INSERT INTO %sproduct_price SET pp_prod_id=%d, pp_sup_id=%d, pp_price=%f, pp_postdate=%d', DB_PREFIX, $prod_id, $supplier, $price, ctime());
        //                        //$this->db->query($sql);

        //                        //$client_price = $this->get_product_custom_price($price, $product_info['prod_pc_id']);
        //                        //$sql = sprintf('UPDATE %sproduct SET prod_price_sup_id=%d, prod_price_update_timestamp=%d, prod_income_price=%f, prod_client_price=%f, prod_actuality=%d WHERE prod_id=%d', DB_PREFIX, $supplier, ctime(), $price, $client_price, $preset['is_actuality'], $product_info['prod_id']);
        //                        //$this->db->query($sql);

        //                        if (price_type == "EURO" && currency_rate_cash > 0)
        //                            product_client_price = currency_rate_cash * euro_rate / currency_rate_cash;

        //                        client_price = product_client_price.ToString();
        //                        client_price = client_price.Replace(',', '.');

        //                        sql = string.Format("INSERT INTO product_price SET pp_prod_id={0}, pp_sup_id={1}, pp_price={2}, pp_postdate={3}",
        //                            product_id, supplier_id, client_price, unixTimestamp);
        //                        cmd = new MySqlCommand(sql, conn);
        //                        resultExecut = cmd.ExecuteNonQuery();
        //                        Debug.WriteLine("4. INSERT INTO product_price => resultExecut: " + resultExecut);

        //                        fixed_price = product_fixed_price.ToString();
        //                        fixed_price = fixed_price.Replace(',', '.');

        //                        sql = string.Format("UPDATE product SET prod_price_sup_id={0}, prod_price_update_timestamp={1}, prod_income_price={2}, prod_client_price={3}, prod_actuality={4} WHERE prod_id={5}",
        //                            supplier_id, unixTimestamp, fixed_price, client_price, true, product_id);
        //                        cmd = new MySqlCommand(sql, conn);
        //                        resultExecut = cmd.ExecuteNonQuery();
        //                        Debug.WriteLine("5. UPDATE product => resultExecut: " + resultExecut);
        //                    }


        //                    if (is_new)
        //                    {
        //                        //$sql = sprintf("INSERT INTO %sproduct_alias SET pa_prod_id=%d, pa_code='%s'", DB_PREFIX, $prod_id, tosql($code));

        //                        sql = string.Format("INSERT INTO product_alias SET pa_prod_id={0}, pa_code='{1}'",
        //                            product_id, code);
        //                        cmd = new MySqlCommand(sql, conn);
        //                        resultExecut = cmd.ExecuteNonQuery();
        //                        Debug.WriteLine("6. INSERT INTO product_alias => resultExecut: " + resultExecut);
        //                    }
        //                }


        //                //$sql = sprintf('INSERT INTO %sproduct_price SET pp_prod_id=%d, pp_sup_id=%d, pp_price=%f, pp_postdate=%d', DB_PREFIX, $prod_id, $supplier, $price, ctime());

        //                fixed_price = product_fixed_price.ToString();
        //                fixed_price = fixed_price.Replace(',', '.');

        //                sql = string.Format("INSERT INTO product_price SET pp_prod_id={0}, pp_sup_id={1}, pp_price={2}, pp_postdate={3}",
        //                    product_id, supplier_id, fixed_price, unixTimestamp);
        //                cmd = new MySqlCommand(sql, conn);
        //                resultExecut = cmd.ExecuteNonQuery();
        //                Debug.WriteLine("7. INSERT INTO product_price => resultExecut: " + resultExecut);
        //            }

        //            if (rowNumber % 16 == 0)
        //            {
        //                addRowInfo.index = rowNumber;
        //                addRowInfo.timeRuning = DateTime.Now - dateTimeBeg;
        //                Debug.WriteLine("rowNumber: " + rowNumber);
        //                OnAddRow(addRowInfo, null);
        //            }

        //            rowNumber++;
        //        }

        //        trans.Commit();

        //        addRowInfo.index = (rowNumber - 1);
        //        addRowInfo.timeRuning = DateTime.Now - dateTimeBeg;
        //        Debug.WriteLine("rowNumber: " + rowNumber);
        //        OnAddRow(addRowInfo, null);

        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //        string error = ex.Source;
        //        //throw;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
            
        //}
                
        //public void InsertData2(DataTable table, string supplier_id, string price_type)
        //{
        //    if (table == null)
        //        return;

        //    if (table.Rows.Count == 0)
        //        return;

        //    AddRowInfo addRowInfo = new AddRowInfo();
        //    DateTime dateTimeBeg = DateTime.Now;

        //    int product_id = 0;
        //    double product_client_price = 0;
        //    double product_fixed_price = 0;

        //    string client_price = string.Empty;
        //    string fixed_price = string.Empty;

        //    MySqlCommand cmd = null;
        //    string sql = string.Empty;

        //    //select set_value from engine_settings where set_name = 'euro_rate'
        //    //select set_value from engine_settings where set_name = 'currency_rate_cash'
        //    //$price = $price * EURO_RATE / CURRENCY_RATE_CASH;
        //    double euro_rate = 0;
        //    double currency_rate_cash = 0;

        //    int resultExecut = -888;
        //    MySqlDataReader rdr = null;

        //    try
        //    {
        //        this.conn = GetConn();
        //        sql = string.Format("select set_value from engine_settings where set_name = 'euro_rate'");
        //        cmd = new MySqlCommand(sql, conn);
        //        rdr = cmd.ExecuteReader();
        //        rdr.Read();
        //        euro_rate = rdr.GetDouble(0);
        //        conn.Close();

        //        conn = this.GetConn();
        //        sql = string.Format("select set_value from engine_settings where set_name = 'currency_rate_cash'");
        //        rdr = null;
        //        cmd = new MySqlCommand(sql, conn);
        //        rdr = cmd.ExecuteReader();
        //        rdr.Read();
        //        currency_rate_cash = rdr.GetDouble(0);
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        conn.Close();
        //        string error = ex.Source;
        //        throw;
        //    }

        //    int rowNumber = 0;
        //    // Проверка валидности
        //    foreach (DataRow row in table.Rows)
        //    {
        //        if (row.Field<string>("is_selected") == "True" && string.IsNullOrEmpty(row.Field<string>("prod_pc_id")))
        //        {
        //            InsertDataError(rowNumber + 1, null);
        //            return;
        //        }

        //        rowNumber++;
        //    }


        //    conn = this.GetConn();

        //    rowNumber = 0;
        //    foreach (DataRow row in table.Rows)
        //    {
        //        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        //        bool is_new = row.Field<bool>("is_new");
        //        string is_selected_TMP = row.Field<string>("is_selected");

        //        bool is_selected = false;
        //        if (!string.IsNullOrEmpty(is_selected_TMP)
        //            && is_selected_TMP.ToLower() == "true")
        //        {
        //            is_selected = true;
        //        }

        //        string prod_id = row.Field<string>("prod_id");
        //        string prod_name = row.Field<string>("prod_name");
        //        string code = row.Field<string>("prod_code");
        //        string new_code = row.Field<string>("prod_new_code");
        //        string prod_income_price = row.Field<string>("prod_income_price");
        //        string prod_client_price = row.Field<string>("prod_client_price");
        //        string prod_pc_id = row.Field<string>("prod_pc_id");
        //        string color = row.Field<string>("color");

        //        if (string.IsNullOrEmpty(color))
        //            color = string.Empty;

        //        if (string.IsNullOrEmpty(new_code))
        //            new_code = string.Empty;

        //        if (string.IsNullOrEmpty(code))
        //            code = new_code;

        //        if (!string.IsNullOrEmpty(prod_pc_id) 
        //            && !string.IsNullOrEmpty(code)
        //            && ((is_new && is_selected) || (color.ToLower() == "green")))
        //        {
        //            product_client_price = 0;
        //            if (!string.IsNullOrEmpty(prod_client_price))
        //                product_client_price = System.Convert.ToDouble(prod_client_price);

        //            product_fixed_price = 0;
        //            if (!string.IsNullOrEmpty(prod_income_price))
        //                product_fixed_price = System.Convert.ToDouble(prod_income_price);

        //            product_id = 0;
        //            if (string.IsNullOrEmpty(prod_id))  //новое значение
        //            {

        //                try
        //                {
        //                    //$sql = sprintf("INSERT INTO %sproduct SET prod_pc_id=%d, prod_name='%s', prod_text='', prod_disabled='Y', prod_vat='Y', prod_actuality=%d, prod_postdate=%d, prod_last_update=%d, prod_last_user_id=%d",
        //                    //    DB_PREFIX, $new_product_category[$num], tosql($product_names[$num]), $preset['is_actuality'], ctime(), ctime(), $this->auth->get_id());
        //                    //$this->db->query($sql);
        //                    //if ($this->db->insert_id())
        //                    //{
        //                    //    $prod_id = $this->db->insert_id();

        //                    //    $code = trim($codes[$num]);
        //                    //    $sql = sprintf("INSERT INTO %sproduct_alias SET pa_prod_id=%d, pa_code='%s'", DB_PREFIX, $prod_id, tosql($code));
        //                    //    $this->db->query($sql);
        //                    //}

        //                    //conn = this.GetConn();
        //                    sql = string.Format("INSERT INTO product SET prod_pc_id={0}, prod_name='{1}', prod_text='', prod_disabled='Y', prod_vat='Y', prod_actuality={2}, prod_postdate={3}, prod_last_update={4}, prod_last_user_id={5}",
        //                        prod_pc_id, prod_name, 1, unixTimestamp, unixTimestamp, 0);
        //                    cmd = new MySqlCommand(sql, conn);
        //                    resultExecut = cmd.ExecuteNonQuery();
        //                    product_id = (int)cmd.LastInsertedId;
        //                    Debug.WriteLine("1. INSERT INTO product => resultExecut: " + resultExecut);
        //                    //conn.Close();

        //                    //conn = this.GetConn();
        //                    //product_id = 0;    //нужно получить ID
        //                    //sql = string.Format("SELECT max(prod_id) FROM product");
        //                    //rdr = null;
        //                    //cmd = new MySqlCommand(sql, conn);
        //                    //rdr = cmd.ExecuteReader();
        //                    //rdr.Read();
        //                    //product_id = rdr.GetInt32(0);
        //                    //conn.Close();

        //                    //conn = this.GetConn();
        //                    sql = string.Format("INSERT INTO product_alias SET pa_prod_id={0}, pa_code='{1}'",
        //                        product_id, code);
        //                    cmd = new MySqlCommand(sql, conn);
        //                    resultExecut = cmd.ExecuteNonQuery();
        //                    Debug.WriteLine("2. INSERT INTO product_alias => resultExecut: " + resultExecut);
        //                    //conn.Close();
        //                }
        //                catch (Exception ex)
        //                {
        //                    //conn.Close();
        //                    string error = ex.Source;
        //                    throw;
        //                }
        //            }
        //            else
        //            {
        //                product_id = System.Convert.ToInt32(prod_id);
        //            }

        //            if (product_id > 0 /* && product_price > 0 */ )
        //            {
        //                //$sql = sprintf('UPDATE %sproduct SET prod_price_sup_id=%d, prod_fixed_price=%f WHERE prod_id=%d', DB_PREFIX, $supplier, $price, $product_info['prod_id']);

        //                if (price_type == "RRC" && product_fixed_price > 0)
        //                {
        //                    try
        //                    {
        //                        fixed_price = product_fixed_price.ToString();
        //                        fixed_price = fixed_price.Replace(',', '.');

        //                        //conn = this.GetConn();    
        //                        sql = string.Format("UPDATE product SET prod_price_sup_id={0}, prod_fixed_price={1} WHERE prod_id={2}",
        //                            supplier_id, fixed_price, product_id);
        //                        cmd = new MySqlCommand(sql, conn);
        //                        resultExecut = cmd.ExecuteNonQuery();
        //                        Debug.WriteLine("3. UPDATE product => resultExecut: " + resultExecut);
        //                        //conn.Close();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        //conn.Close();
        //                        string error = ex.Source;
        //                        throw;
        //                    }
        //                }
        //                else
        //                {
        //                    //$sql = sprintf('INSERT INTO %sproduct_price SET pp_prod_id=%d, pp_sup_id=%d, pp_price=%f, pp_postdate=%d', DB_PREFIX, $prod_id, $supplier, $price, ctime());
        //                    //$this->db->query($sql);

        //                    //$client_price = $this->get_product_custom_price($price, $product_info['prod_pc_id']);
        //                    //$sql = sprintf('UPDATE %sproduct SET prod_price_sup_id=%d, prod_price_update_timestamp=%d, prod_income_price=%f, prod_client_price=%f, prod_actuality=%d WHERE prod_id=%d', DB_PREFIX, $supplier, ctime(), $price, $client_price, $preset['is_actuality'], $product_info['prod_id']);
        //                    //$this->db->query($sql);

        //                    try
        //                    {
        //                        if (price_type == "EURO" && currency_rate_cash > 0)
        //                            product_client_price = currency_rate_cash * euro_rate / currency_rate_cash;

        //                        client_price = product_client_price.ToString();
        //                        client_price = client_price.Replace(',', '.');

        //                        //conn = this.GetConn();
        //                        sql = string.Format("INSERT INTO product_price SET pp_prod_id={0}, pp_sup_id={1}, pp_price={2}, pp_postdate={3}",
        //                            product_id, supplier_id, client_price, unixTimestamp);
        //                        cmd = new MySqlCommand(sql, conn);
        //                        resultExecut = cmd.ExecuteNonQuery();
        //                        Debug.WriteLine("4. INSERT INTO product_price => resultExecut: " + resultExecut);
        //                        //conn.Close();

        //                        fixed_price = product_fixed_price.ToString();
        //                        fixed_price = fixed_price.Replace(',', '.');

        //                        //conn = this.GetConn();
        //                        sql = string.Format("UPDATE product SET prod_price_sup_id={0}, prod_price_update_timestamp={1}, prod_income_price={2}, prod_client_price={3}, prod_actuality={4} WHERE prod_id={5}",
        //                            supplier_id, unixTimestamp, fixed_price, client_price, true, product_id);
        //                        cmd = new MySqlCommand(sql, conn);
        //                        resultExecut = cmd.ExecuteNonQuery();
        //                        Debug.WriteLine("5. UPDATE product => resultExecut: " + resultExecut);
        //                        //conn.Close();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        //conn.Close();
        //                        string error = ex.Source;
        //                        throw;
        //                    }
        //                }


        //                if (is_new)
        //                {
        //                    try
        //                    {
        //                        //$sql = sprintf("INSERT INTO %sproduct_alias SET pa_prod_id=%d, pa_code='%s'", DB_PREFIX, $prod_id, tosql($code));
        //                        //conn = this.GetConn();
        //                        sql = string.Format("INSERT INTO product_alias SET pa_prod_id={0}, pa_code='{1}'",
        //                            product_id, code);
        //                        cmd = new MySqlCommand(sql, conn);
        //                        resultExecut = cmd.ExecuteNonQuery();
        //                        Debug.WriteLine("6. INSERT INTO product_alias => resultExecut: " + resultExecut);
        //                        //conn.Close();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        //conn.Close();
        //                        string error = ex.Source;
        //                        throw;
        //                    }
        //                }
        //            }


        //            try
        //            {
        //                //$sql = sprintf('INSERT INTO %sproduct_price SET pp_prod_id=%d, pp_sup_id=%d, pp_price=%f, pp_postdate=%d', DB_PREFIX, $prod_id, $supplier, $price, ctime());

        //                fixed_price = product_fixed_price.ToString();
        //                fixed_price = fixed_price.Replace(',', '.');

        //                //conn = this.GetConn();
        //                sql = string.Format("INSERT INTO product_price SET pp_prod_id={0}, pp_sup_id={1}, pp_price={2}, pp_postdate={3}",
        //                    product_id, supplier_id, fixed_price, unixTimestamp);
        //                cmd = new MySqlCommand(sql, conn);
        //                resultExecut = cmd.ExecuteNonQuery();
        //                Debug.WriteLine("7. INSERT INTO product_price => resultExecut: " + resultExecut);
        //                //conn.Close();
        //            }
        //            catch (Exception ex)
        //            {
        //                //conn.Close();
        //                string error = ex.Source;
        //                //throw;
        //            }
        //        }
                
        //        if (rowNumber % 16 == 0)
        //        {
        //            addRowInfo.index = rowNumber;
        //            addRowInfo.timeRuning = DateTime.Now - dateTimeBeg;
        //            Debug.WriteLine("rowNumber: " + rowNumber);
        //            OnAddRow(addRowInfo, null);
        //        }
                
        //        rowNumber++;
        //    }

        //    addRowInfo.index = (rowNumber-1);
        //    addRowInfo.timeRuning = DateTime.Now - dateTimeBeg;
        //    Debug.WriteLine("rowNumber: " + rowNumber);
        //    OnAddRow(addRowInfo, null);

        //    conn.Close();
        //}
        
        //public void InsertData1(DataTable table, string supplier_id, string price_type)
        //{
        //    List<string> queryList = new List<string>();

        //    if (table == null)
        //        return;

        //    if (table.Rows.Count == 0)
        //        return;

        //    AddRowInfo addRowInfo = new AddRowInfo();
        //    DateTime dateTimeBeg = DateTime.Now;

        //    int product_id = 0;
        //    double product_client_price = 0;
        //    double product_fixed_price = 0;

        //    string client_price = string.Empty;
        //    string fixed_price = string.Empty;

        //    MySqlCommand cmd = null;
        //    string sql = string.Empty;

        //    //select set_value from engine_settings where set_name = 'euro_rate'
        //    //select set_value from engine_settings where set_name = 'currency_rate_cash'
        //    //$price = $price * EURO_RATE / CURRENCY_RATE_CASH;
        //    double euro_rate = 0;
        //    double currency_rate_cash = 0;

        //    int resultExecut = -888;
        //    MySqlDataReader rdr = null;

        //    try
        //    {
        //        this.conn = GetConn();
        //        sql = string.Format("select set_value from engine_settings where set_name = 'euro_rate'");
        //        cmd = new MySqlCommand(sql, conn);
        //        rdr = cmd.ExecuteReader();
        //        rdr.Read();
        //        euro_rate = rdr.GetDouble(0);
        //        conn.Close();

        //        conn = this.GetConn();
        //        sql = string.Format("select set_value from engine_settings where set_name = 'currency_rate_cash'");
        //        rdr = null;
        //        cmd = new MySqlCommand(sql, conn);
        //        rdr = cmd.ExecuteReader();
        //        rdr.Read();
        //        currency_rate_cash = rdr.GetDouble(0);
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        conn.Close();
        //        string error = ex.Source;
        //        throw;
        //    }

        //    int rowNumber = 0;
        //    // Проверка валидности
        //    foreach (DataRow row in table.Rows)
        //    {
        //        if (row.Field<string>("is_selected") == "True" && string.IsNullOrEmpty(row.Field<string>("prod_pc_id")))
        //        {
        //            InsertDataError(rowNumber + 1, null);
        //            return;
        //        }

        //        rowNumber++;
        //    }

        //    rowNumber = 0;
        //    foreach (DataRow row in table.Rows)
        //    {
        //        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        //        bool is_new = row.Field<bool>("is_new");
        //        string is_selected_TMP = row.Field<string>("is_selected");

        //        bool is_selected = false;
        //        if (!string.IsNullOrEmpty(is_selected_TMP)
        //            && is_selected_TMP.ToLower() == "true")
        //        {
        //            is_selected = true;
        //        }

        //        string prod_id = row.Field<string>("prod_id");
        //        string prod_name = row.Field<string>("prod_name");
        //        string code = row.Field<string>("prod_code");
        //        string new_code = row.Field<string>("prod_new_code");
        //        string prod_income_price = row.Field<string>("prod_income_price");
        //        string prod_client_price = row.Field<string>("prod_client_price");
        //        string prod_pc_id = row.Field<string>("prod_pc_id");
        //        string color = row.Field<string>("color");

        //        if (string.IsNullOrEmpty(color))
        //            color = string.Empty;

        //        if (string.IsNullOrEmpty(new_code))
        //            new_code = string.Empty;

        //        if (string.IsNullOrEmpty(code))
        //            code = new_code;

        //        if (!string.IsNullOrEmpty(prod_pc_id)
        //            && !string.IsNullOrEmpty(code)
        //            && ((is_new && is_selected) || (color.ToLower() == "green")))
        //        {
        //            product_client_price = 0;
        //            if (!string.IsNullOrEmpty(prod_client_price))
        //                product_client_price = System.Convert.ToDouble(prod_client_price);

        //            product_fixed_price = 0;
        //            if (!string.IsNullOrEmpty(prod_income_price))
        //                product_fixed_price = System.Convert.ToDouble(prod_income_price);

        //            product_id = 0;
        //            if (string.IsNullOrEmpty(prod_id))  //новое значение
        //            {

        //                try
        //                {
        //                    //$sql = sprintf("INSERT INTO %sproduct SET prod_pc_id=%d, prod_name='%s', prod_text='', prod_disabled='Y', prod_vat='Y', prod_actuality=%d, prod_postdate=%d, prod_last_update=%d, prod_last_user_id=%d",
        //                    //    DB_PREFIX, $new_product_category[$num], tosql($product_names[$num]), $preset['is_actuality'], ctime(), ctime(), $this->auth->get_id());
        //                    //$this->db->query($sql);
        //                    //if ($this->db->insert_id())
        //                    //{
        //                    //    $prod_id = $this->db->insert_id();

        //                    //    $code = trim($codes[$num]);
        //                    //    $sql = sprintf("INSERT INTO %sproduct_alias SET pa_prod_id=%d, pa_code='%s'", DB_PREFIX, $prod_id, tosql($code));
        //                    //    $this->db->query($sql);
        //                    //}

                           
        //                    sql = string.Format("INSERT INTO product SET prod_pc_id={0}, prod_name='{1}', prod_text='', prod_disabled='Y', prod_vat='Y', prod_actuality={2}, prod_postdate={3}, prod_last_update={4}, prod_last_user_id={5}",
        //                        prod_pc_id, prod_name, 1, unixTimestamp, unixTimestamp, 0);
        //                    cmd = new MySqlCommand(sql, conn);
        //                    //resultExecut = cmd.ExecuteNonQuery();
        //                    queryList.Add(sql);
        //                    Debug.WriteLine("1. INSERT INTO product => resultExecut: " + resultExecut);
                           

        //                    //conn = this.GetConn();
        //                    //product_id = 0;    //нужно получить ID
        //                    //sql = string.Format("SELECT max(prod_id) FROM product");
        //                    //rdr = null;
        //                    //cmd = new MySqlCommand(sql, conn);
        //                    //rdr = cmd.ExecuteReader();
        //                    //rdr.Read();
        //                    //product_id = rdr.GetInt32(0);
        //                    //conn.Close();

        //                    product_id = (int)cmd.LastInsertedId;

        //                    sql = string.Format("INSERT INTO product_alias SET pa_prod_id={0}, pa_code='{1}'",
        //                        product_id, code);
        //                    cmd = new MySqlCommand(sql, conn);
        //                    //resultExecut = cmd.ExecuteNonQuery();
        //                    queryList.Add(sql);
        //                    Debug.WriteLine("2. INSERT INTO product_alias => resultExecut: " + resultExecut);
        //                }
        //                catch (Exception ex)
        //                {
        //                    conn.Close();
        //                    string error = ex.Source;
        //                    throw;
        //                }
        //            }
        //            else
        //            {
        //                product_id = System.Convert.ToInt32(prod_id);
        //            }

        //            if (product_id > 0 /* && product_price > 0 */ )
        //            {
        //                //$sql = sprintf('UPDATE %sproduct SET prod_price_sup_id=%d, prod_fixed_price=%f WHERE prod_id=%d', DB_PREFIX, $supplier, $price, $product_info['prod_id']);

        //                if (price_type == "RRC" && product_fixed_price > 0)
        //                {
        //                    try
        //                    {
        //                        fixed_price = product_fixed_price.ToString();
        //                        fixed_price = fixed_price.Replace(',', '.');

                                
        //                        sql = string.Format("UPDATE product SET prod_price_sup_id={0}, prod_fixed_price={1} WHERE prod_id={2}",
        //                            supplier_id, fixed_price, product_id);
        //                        cmd = new MySqlCommand(sql, conn);
        //                        //resultExecut = cmd.ExecuteNonQuery();
        //                        queryList.Add(sql);
        //                        Debug.WriteLine("3. UPDATE product => resultExecut: " + resultExecut);
                                
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        conn.Close();
        //                        string error = ex.Source;
        //                        throw;
        //                    }
        //                }
        //                else
        //                {
        //                    //$sql = sprintf('INSERT INTO %sproduct_price SET pp_prod_id=%d, pp_sup_id=%d, pp_price=%f, pp_postdate=%d', DB_PREFIX, $prod_id, $supplier, $price, ctime());
        //                    //$this->db->query($sql);

        //                    //$client_price = $this->get_product_custom_price($price, $product_info['prod_pc_id']);
        //                    //$sql = sprintf('UPDATE %sproduct SET prod_price_sup_id=%d, prod_price_update_timestamp=%d, prod_income_price=%f, prod_client_price=%f, prod_actuality=%d WHERE prod_id=%d', DB_PREFIX, $supplier, ctime(), $price, $client_price, $preset['is_actuality'], $product_info['prod_id']);
        //                    //$this->db->query($sql);

        //                    try
        //                    {
        //                        if (price_type == "EURO" && currency_rate_cash > 0)
        //                            product_client_price = currency_rate_cash * euro_rate / currency_rate_cash;

        //                        client_price = product_client_price.ToString();
        //                        client_price = client_price.Replace(',', '.');

                                
        //                        sql = string.Format("INSERT INTO product_price SET pp_prod_id={0}, pp_sup_id={1}, pp_price={2}, pp_postdate={3}",
        //                            product_id, supplier_id, client_price, unixTimestamp);
        //                        cmd = new MySqlCommand(sql, conn);
        //                        //resultExecut = cmd.ExecuteNonQuery();
        //                        queryList.Add(sql);
        //                        Debug.WriteLine("4. INSERT INTO product_price => resultExecut: " + resultExecut);
                                

        //                        fixed_price = product_fixed_price.ToString();
        //                        fixed_price = fixed_price.Replace(',', '.');
                                
        //                        sql = string.Format("UPDATE product SET prod_price_sup_id={0}, prod_price_update_timestamp={1}, prod_income_price={2}, prod_client_price={3}, prod_actuality={4} WHERE prod_id={5}",
        //                            supplier_id, unixTimestamp, fixed_price, client_price, true, product_id);
        //                        cmd = new MySqlCommand(sql, conn);
        //                        //resultExecut = cmd.ExecuteNonQuery();
        //                        queryList.Add(sql);
        //                        Debug.WriteLine("5. UPDATE product => resultExecut: " + resultExecut);
                                
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        conn.Close();
        //                        string error = ex.Source;
        //                        throw;
        //                    }
        //                }


        //                if (is_new)
        //                {
        //                    try
        //                    {
        //                        //$sql = sprintf("INSERT INTO %sproduct_alias SET pa_prod_id=%d, pa_code='%s'", DB_PREFIX, $prod_id, tosql($code));
                                
        //                        sql = string.Format("INSERT INTO product_alias SET pa_prod_id={0}, pa_code='{1}'",
        //                            product_id, code);
        //                        cmd = new MySqlCommand(sql, conn);
        //                        //resultExecut = cmd.ExecuteNonQuery();
        //                        queryList.Add(sql);
        //                        Debug.WriteLine("6. INSERT INTO product_alias => resultExecut: " + resultExecut);
                                
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        conn.Close();
        //                        string error = ex.Source;
        //                        throw;
        //                    }
        //                }
        //            }


        //            try
        //            {
        //                //$sql = sprintf('INSERT INTO %sproduct_price SET pp_prod_id=%d, pp_sup_id=%d, pp_price=%f, pp_postdate=%d', DB_PREFIX, $prod_id, $supplier, $price, ctime());

        //                fixed_price = product_fixed_price.ToString();
        //                fixed_price = fixed_price.Replace(',', '.');

                        
        //                sql = string.Format("INSERT INTO product_price SET pp_prod_id={0}, pp_sup_id={1}, pp_price={2}, pp_postdate={3}",
        //                    product_id, supplier_id, fixed_price, unixTimestamp);
        //                cmd = new MySqlCommand(sql, conn);
        //                //resultExecut = cmd.ExecuteNonQuery();
        //                queryList.Add(sql);
        //                Debug.WriteLine("7. INSERT INTO product_price => resultExecut: " + resultExecut);
        //            }
        //            catch (Exception ex)
        //            {
        //                conn.Close();
        //                string error = ex.Source;
        //                throw;
        //            }
        //        }

        //        addRowInfo.index = rowNumber;
        //        addRowInfo.timeRuning = DateTime.Now - dateTimeBeg;
        //        Debug.WriteLine("rowNumber: " + rowNumber);

        //        OnAddRow(addRowInfo, null);
        //        rowNumber++;

        //    }
        //}

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
                        using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM category_charge", con))
                        {
                            da.SelectCommand.CommandTimeout = 600;
                            commandBuilder = new MySqlCommandBuilder(da);
                            da.Fill(dt);
                        }
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
                        string sql = @"SELECT prod_id, prod_name, prod_income_price, prod_text, prod_client_price, prod_price_col1, prod_price_col2, prod_price_col3, prod_fixed_price, pa_code, prod_pc_id, prod_qty FROM product_alias INNER JOIN product pr ON pr.prod_id=pa_prod_id";
                        using (MySqlDataAdapter da = new MySqlDataAdapter(sql, con))
                        {
                            da.SelectCommand.CommandTimeout = 600;
                            commandBuilder = new MySqlCommandBuilder(da);
                            da.Fill(dt);
                        }
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
                        using (da_import_settings = new MySqlDataAdapter("SELECT * FROM import_settings order by is_name", con))
                        {
                            da_import_settings.SelectCommand.CommandTimeout = 600;
                            commandBuilder = new MySqlCommandBuilder(da_import_settings);
                            da_import_settings.Fill(dt);
                        }
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
                foreach(DataRow row in changes.Rows)
                {
                    if (row.RowState == DataRowState.Deleted)
                    {
                        //da_import_settings.Update(changes);
                        //dt.AcceptChanges();
                        continue;
                    }

                    var is_id = row.Field<int>("is_id");
                    var is_name =  row.Field<string>("is_name");
                    var is_start_row = row.Field<int>("is_start_row");
                    var is_code_col = row.Field<string>("is_code_col");
                    var is_price_col = row.Field<string>("is_price_col");
                    var is_name_col = row.Field<string>("is_name_col");
                    var is_actuality = row.Field<int>("is_actuality");
                    var is_presense1_col = row.Field<string>("is_presense1_col");
                    var is_presense2_col = row.Field<string>("is_presense2_col");
                    var is_presense_symbol = row.Field<string>("is_presense_symbol");
                    var is_currency_col = row.Field<string>("is_currency_col");
                    var is_uah_flag = row.Field<string>("is_uah_flag");

                    conn = this.GetConn();

                    string sql = string.Empty;

                    if (is_id > 0)
                    {
                        sql = string.Format("UPDATE import_settings SET is_name = '{0}', is_start_row = {1}, is_code_col = '{2}', is_price_col = '{3}', is_name_col = '{4}', is_actuality = {5}, is_presense1_col = '{6}', is_presense2_col = '{7}', is_presense_symbol = '{8}', is_currency_col = '{9}', is_uah_flag  = '{10}' WHERE is_id={11}",
                            is_name, is_start_row, is_code_col, is_price_col, is_name_col, is_actuality, is_presense1_col, is_presense2_col, is_presense_symbol, is_currency_col, is_uah_flag, is_id);
                    }
                    else
                    {
                        sql = string.Format("INSERT INTO import_settings SET is_name = '{0}', is_start_row = {1}, is_code_col = '{2}', is_price_col = '{3}', is_name_col = '{4}', is_actuality = {5}, is_presense1_col = '{6}', is_presense2_col = '{7}', is_presense_symbol = '{8}', is_currency_col = '{9}', is_uah_flag  = '{10}'",
                            is_name, is_start_row, is_code_col, is_price_col, is_name_col, is_actuality, is_presense1_col, is_presense2_col, is_presense_symbol, is_currency_col, is_uah_flag);
                    }
                
                    var cmd = new MySqlCommand(sql, conn);
                    cmd.CommandTimeout = 0;
                
                    cmd.ExecuteNonQuery();
                }


            }
        }

        public void Delete_row_import_settings(int is_id)
        {
            conn = this.GetConn();
            string sql = string.Empty;
            if (is_id > 0)
            {
                sql = string.Format("DELETE FROM import_settings WHERE is_id={0}", is_id);
                var cmd = new MySqlCommand(sql, conn);
                cmd.CommandTimeout = 0;

                cmd.ExecuteNonQuery();
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
                        using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM price_category", con))
                        {
                            da.SelectCommand.CommandTimeout = 600;
                            commandBuilder = new MySqlCommandBuilder(da);
                            da.Fill(dt);
                        }
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
                        using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM product", con))
                        {
                            da.SelectCommand.CommandTimeout = 600;
                            commandBuilder = new MySqlCommandBuilder(da);
                            da.Fill(dt);
                        }
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
                        using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM product_alias", con))
                        {
                            da.SelectCommand.CommandTimeout = 600;
                            commandBuilder = new MySqlCommandBuilder(da);
                            da.Fill(dt);
                        }
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
                        using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM product_category", con))
                        {
                            da.SelectCommand.CommandTimeout = 600;
                            commandBuilder = new MySqlCommandBuilder(da);
                            da.Fill(dt);
                        }
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
                        using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM product_price", con))
                        {
                            da.SelectCommand.CommandTimeout = 600;
                            commandBuilder = new MySqlCommandBuilder(da);
                            da.Fill(dt);
                        }
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
                        using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM supplier ORDER BY sup_name", con))
                        {
                            da.SelectCommand.CommandTimeout = 600;
                            commandBuilder = new MySqlCommandBuilder(da);
                            da.Fill(dt);
                        }
                    }
                }
                return dt;
            });
        }


        public void LoadCurrency()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(strConn))
                {
                    if (con != null)
                    {
                        con.Open();
                        System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
                        con.ChangeDatabase(cas.GetValue("dataBase", typeof(string)).ToString());

                        string sql = string.Format("select set_value from engine_settings where set_name = 'euro_rate'");
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        cmd.CommandTimeout = 0;
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        rdr.Read();
                        string str_euro_rate = rdr.GetString(0);
                        this.Euro_rate = Convert.ToDouble(PriceModel.ConvertSeparator(str_euro_rate))   /* rdr.GetDouble(0) */;
                        log.Info(sql);
                    }
                }


                using (MySqlConnection con = new MySqlConnection(strConn))
                {
                    if (con != null)
                    {
                        con.Open();
                        System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
                        con.ChangeDatabase(cas.GetValue("dataBase", typeof(string)).ToString());

                        string sql = string.Format("select set_value from engine_settings where set_name = 'currency_rate_cash'");
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        cmd.CommandTimeout = 0;
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        rdr.Read();
                        string str_currency_rate_cash = rdr.GetString(0);
                        this.Currency_rate_cash = Convert.ToDouble(PriceModel.ConvertSeparator(str_currency_rate_cash))   /* rdr.GetDouble(0) */;
                        log.Info(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                string error = ex.Source;
                throw;
            }
        }




        public int ImportExcel(string fileName, ref DataTable data, string formatName)
        {
            cExcelObj exl = new cExcelObj();
            data = new DataTable();
            
            //int res = exl.readExcelFileSQL(fileName, ref data);
            //int res = exl.readExcelFileSQLWithSaveAs(fileName, ref data);

            string filenameSaveAs = fileName;
            int index = formatName.ToLower().IndexOf("dclink");

            if (index >= 0)
                if(Path.GetExtension(fileName) == ".xls")
                    exl.excelFileSaveAs(fileName, ref filenameSaveAs, cExcelObj.EXCEL_XLFILEFORMAT_XLEXCEL8);
                else if(Path.GetExtension(fileName) == ".xlsx")
                    exl.excelFileSaveAs(fileName, ref filenameSaveAs, cExcelObj.EXCEL_XLFILEFORMAT_XLOPENXMLWORKBOOK);

            int res = readExcelFile(filenameSaveAs, ref data);

            if (fileName != filenameSaveAs)
                File.Delete(filenameSaveAs);

            return res;
        }

        public int readExcelFile(string pathFileDoc, ref DataTable data)
        {
            using (var stream = new FileStream(pathFileDoc, FileMode.Open))
            {
                IExcelDataReader reader = null;
                if (Path.GetExtension(pathFileDoc) == ".xls")
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                else if (Path.GetExtension(pathFileDoc) == ".xlsx")
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                if (reader == null)
                    return -1;

                DataSet result = reader.AsDataSet();

                if (result.Tables == null)
                    return -1;

                int? countTables = result.Tables.Count;
                if (countTables != null && countTables > 0)
                    data = result.Tables[0];
            } 
            
            return data != null ? data.Rows.Count : -1;
        }


        public string CalcClientPrice(ref List<CategoryCharge> _categoryCharge, object _recived_price, object prod_pc_id, string prod_qty, string importCurrency, object prod_code)
        {
            if (importCurrency == RRC)
            {
                if(string.IsNullOrEmpty(_recived_price.ToString()))
                    return "0";
                else
                    return PriceModel.ConvertSeparator(_recived_price.ToString());
            }

            int _prod_pc_id = 0;
            double price = 0;

            int qty = string.IsNullOrEmpty(prod_qty) == false ? System.Convert.ToInt32(prod_qty) : 0;
            double? res = null;
            if (_recived_price != null
                && _categoryCharge != null
                && prod_pc_id != null
                && !string.IsNullOrEmpty(prod_pc_id.ToString()) )
            {
                _prod_pc_id = System.Convert.ToInt32(prod_pc_id);
                price = System.Convert.ToDouble(PriceModel.ConvertSeparator(_recived_price.ToString()));
                res = CalcClientPriceSub(ref _categoryCharge, _prod_pc_id, price);

                if (qty > 0)
                {
                    Product prod = products.FirstOrDefault(a => a.pa_code == prod_code.ToString());
                    double? priceDB = System.Convert.ToDouble(PriceModel.ConvertSeparator(prod.prod_client_price.ToString()));
                    if (priceDB.GetValueOrDefault() > res.GetValueOrDefault())
                        res = priceDB;
                }
            }

            if (res != null)
                return Math.Round((double)res, 2).ToString();

            return null;
        }

        private double? CalcClientPriceSub(ref List<CategoryCharge> _categoryCharge, int prod_pc_id, double price)
        {
            double? res = null;

            var findProductPrice = this.products.FirstOrDefault(i => i.prod_pc_id == (object)prod_pc_id);

            CategoryCharge findCharge = _categoryCharge.FirstOrDefault(
                f => (System.Convert.ToInt32(f.cc_pc_id) == prod_pc_id && (double)price >= System.Convert.ToDouble(f.cc_price_from) && (double)price < System.Convert.ToDouble(f.cc_price_to))
                    || (System.Convert.ToInt32(f.cc_pc_id) == prod_pc_id && (double)price > System.Convert.ToDouble(f.cc_price_from) && (double)price >= System.Convert.ToDouble(f.cc_price_to)));

            if (findCharge != null)
                res = price + ((price * System.Convert.ToInt32(findCharge.cc_charge)) / 100);

            if (res != null && findProductPrice != null
                && findProductPrice.prod_income_price != null
                && Convert.ToDouble(findProductPrice.prod_income_price) > price)
            {
                res = Convert.ToDouble(findProductPrice.prod_income_price);
            }

            return res;
        }

        public static string ConvertSeparator(string val)
        {
            string result = string.Empty;

            string separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            result = val.Replace(",", ".");
            result = result.Replace(".", separator);

            return result;
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
        public object prod_qty;
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
        public string prod_qty = string.Empty;
        public string is_presense = string.Empty;
    }


    public class CategoryCharge
    {
        public object cc_pc_id = 0;
        public object cc_price_from = 0;
        public object cc_price_to = 0;
        public object cc_charge = 0;

    }


    public class AddRowInfo
    {
        public int index = -1;
        public TimeSpan timeRuning;
    }

}
