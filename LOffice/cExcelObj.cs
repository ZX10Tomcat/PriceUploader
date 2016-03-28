using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Data.OleDb;
using System.Data;


namespace LOffice
{
	/// <summary>
    /// Summary description for cExcelObj.
	/// </summary>
    public class cExcelObj
    {
        /*
        protected object oExcelApp = null;
        protected object oWorkbooks = null;
        protected object oWorkbook = null;
        protected object oWorksheets = null;
        protected object oWorksheet = null;
        protected object oRange = null;
        */

        public const int EXCEL_XLFILEFORMAT_XLEXCEL8 = 56; //Excel.XlFileFormat.xlExcel8;

        protected int typeAdd = -1;

        public enum typeDataVal { str, num, dbl, date };

        public cExcelObj() { }

        protected void exceptMsg(ref Exception except)
        {
            String errorMessage = "Error: ";
            errorMessage = String.Concat(errorMessage, except.Message);
            errorMessage = String.Concat(errorMessage, " Line: ");
            errorMessage = String.Concat(errorMessage, except.Source);
            System.Windows.Forms.MessageBox.Show(errorMessage, "Error");
        }


        public int readExcelFileSQL(string pathFileDoc, ref DataTable data)
        {
            object oExcelApp = null;
            object oWorkbooks = null;
            object oWorkbook = null;
            object oWorksheets = null;
            object oWorksheet = null;
            object oRange = null;

            int r = initObj(ref oRange, ref oWorksheet, ref oWorksheets, ref oWorkbook, ref oWorkbooks, ref oExcelApp, pathFileDoc);
            if (r < 0)
                return r;

            string connectionString = string.Empty;

            string ext = Path.GetExtension(pathFileDoc);

            ext = ext.Replace(".", "");
            ext = ext.Trim();

            if (ext.ToLower() == "xlsx".ToLower())
                connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 12.0;", pathFileDoc);
            else
                connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathFileDoc);

            string workSheetName = getWorksheetName(ref oWorksheets, 1);
            killExcel(ref oRange, ref oWorksheet, ref oWorksheets, ref oWorkbook, ref oWorkbooks, ref oExcelApp);

            var adapter = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}$]", workSheetName), connectionString);
            adapter.Fill(data);

            return data != null ? data.Rows.Count : -1;
        }

        public int readExcelFileSQLWithSaveAs(string pathFileDoc, ref DataTable data)
        {
            object oExcelApp = null;
            object oWorkbooks = null;
            object oWorkbook = null;
            object oWorksheets = null;
            object oWorksheet = null;
            object oRange = null;

            int r = initObj(ref oRange, ref oWorksheet, ref oWorksheets, ref oWorkbook, ref oWorkbooks, ref oExcelApp, pathFileDoc);
            if (r < 0)
                return r;

            string connectionString = string.Empty;

            string ext = Path.GetExtension(pathFileDoc);

            string file = string.Format("{0}\\{1}.xls", Path.GetDirectoryName(pathFileDoc), DateTime.Now.Ticks.ToString());

            cExcelObj.saveAs(ref oWorkbook, file, EXCEL_XLFILEFORMAT_XLEXCEL8);

            connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", file);

            string workSheetName = cExcelObj.getWorksheetName(ref oWorksheets, 1);
            killExcel(ref oRange, ref oWorksheet, ref oWorksheets, ref oWorkbook, ref oWorkbooks, ref oExcelApp);

            var adapter = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}$]", workSheetName), connectionString);
            adapter.Fill(data);

            File.Delete(file);

            return data != null ? data.Rows.Count : -1;
        }


        public int excelFileSaveAs(string pathFileDoc, ref string filenameSaved)
        {
            object oExcelApp = null;
            object oWorkbooks = null;
            object oWorkbook = null;
            object oWorksheets = null;
            object oWorksheet = null;
            object oRange = null;

            int r = initObj(ref oRange, ref oWorksheet, ref oWorksheets, ref oWorkbook, ref oWorkbooks, ref oExcelApp, pathFileDoc);
            if (r < 0)
                return r;

            string ext = Path.GetExtension(pathFileDoc);
            filenameSaved = string.Format("{0}\\{1}.xls", Path.GetDirectoryName(pathFileDoc), DateTime.Now.Ticks.ToString());
            cExcelObj.saveAs(ref oWorkbook, filenameSaved, EXCEL_XLFILEFORMAT_XLEXCEL8);

            killExcel(ref oRange, ref oWorksheet, ref oWorksheets, ref oWorkbook, ref oWorkbooks, ref oExcelApp);

            return 0;
        }
        

        #region public Static Function


        private static System.Globalization.CultureInfo cultInfo()
        {
            //"ru-RU" "en-US"
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            ci.DateTimeFormat.DateSeparator = ".";
            ci.DateTimeFormat.FirstDayOfWeek = System.DayOfWeek.Monday;
            ci.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            ci.DateTimeFormat.ShortTimePattern = "HH:mm";

            return ci;
        }


        public static void createExcelApplication(ref object oExcelApp)
        {
            ///////////////////////////////////////////////////////////////////
            //string sAppProgID = "Excel.Application";

            ///////////////////////////////////////////////////////////////////
            //"Microsoft.Office.Interop.Excel.Application";

            string sAppProgID = "Excel.Application";
            Type tExcelObj = Type.GetTypeFromProgID(sAppProgID);
            oExcelApp = Activator.CreateInstance(tExcelObj);
        }


        public static string getWorksheetName(
            ref object in_oWorksheets,
            int numberSheet)
        {
            try
            {
                object worksheet = null;
                getWorksheet(ref in_oWorksheets, ref worksheet, numberSheet);
                object name = worksheet.GetType().InvokeMember("Name", BindingFlags.GetProperty, null, worksheet, null, cultInfo());
                string retVal = name.ToString();
                worksheet = null;
                name = null;
                return retVal;
            }
            catch
            {
                return null;
            }
        }


        public static int getWorksheetIndex(
            ref object in_oWorksheets,
            string nameSheet)
        {
            try
            {
                object worksheet = null;
                getWorksheet(ref in_oWorksheets, ref worksheet, nameSheet);
                object tmp = getWorksheetIndex(ref worksheet);
                int indexSht = int.Parse(tmp.ToString());
                worksheet = null;
                return indexSht;
            }
            catch
            {
                return -1;
            }
        }


        public static int getWorksheetIndex(ref object in_oWorksheet)
        {
            try
            {
                object tmp = in_oWorksheet.GetType().InvokeMember("Index", BindingFlags.GetProperty, null, in_oWorksheet, null, cultInfo());
                int indexSht = int.Parse(tmp.ToString());
                return indexSht;
            }
            catch
            {
                return -1;
            }
        }


        public static int getWorksheetsCount(ref object in_oWorksheets)
        {
            object[] args = new object[1];
            args = null;
            int cnt = -1;

            try
            {
                object tmp = null;
                tmp = in_oWorksheets.GetType().InvokeMember("Count", BindingFlags.GetProperty, null, in_oWorksheets, args, cultInfo());
                cnt = int.Parse(tmp.ToString());
                return cnt;
            }
            catch
            {
                return -1;
            }
        }


        public static void getWorksheetDelete(
            ref object in_oWorksheets,
            int index)
        {
            //((Excel.Worksheet)objBook.Sheets.get_Item(2)).Delete(); 

            object[] args = new object[1];
            args[0] = null;

            try
            {
                object worksheet = null;
                getWorksheet(ref in_oWorksheets, ref worksheet, index);
                worksheet.GetType().InvokeMember("Delete", BindingFlags.GetProperty, null, worksheet, args, cultInfo());
            }
            catch
            {
                return;
            }
        }


        public static void getCell(
            ref object in_oWorksheet,
            ref object oRange,
            int row,
            int col)
        {
            object[] args = new object[2];
            args[0] = row;
            args[1] = col;

            oRange = in_oWorksheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty, null, in_oWorksheet, args, cultInfo());
        }


        public static void findCell(
            string findVal,
            ref object in_oWorksheet,
            ref object oRange)
        {
            object tmpRangeFnd = null;
            //object range = null; 
            //getCell(ref in_oWorksheet, ref oRange, 1, 1);		

            int Excel_XlFindLookIn_xlValues = -4163;
            int Excel_XlLookAt_xlPart = 2;
            int Excel_XlSearchOrder_xlByRows = 1;
            int Excel_XlSearchOrder_xlByColumns = 2;
            int Excel_XlSearchDirection_xlNext = 1;


            object[] args = new object[6];
            args[0] = findVal;
            args[1] = Missing.Value;
            args[2] = Excel_XlFindLookIn_xlValues;
            args[3] = Excel_XlLookAt_xlPart;
            args[4] = Excel_XlSearchOrder_xlByRows;         //Excel_XlSearchOrder_xlByColumns;
            args[5] = Excel_XlSearchDirection_xlNext;

            tmpRangeFnd = oRange.GetType().InvokeMember("Find", BindingFlags.InvokeMethod, null, oRange, args, cultInfo());

            try
            {
                while (tmpRangeFnd == null)
                {
                    args = new object[1];
                    args[0] = tmpRangeFnd;

                    tmpRangeFnd = oRange.GetType().InvokeMember("FindNext", BindingFlags.InvokeMethod, null, oRange, args, cultInfo());
                }
            }
            catch
            {
                tmpRangeFnd = null;
                oRange = null;
            }

            oRange = tmpRangeFnd;
        }


        public static void getWorkbook(
            ref object in_oWorkbooks,
            ref object oWorkbook,
            string fileName)
        {
            try
            {
                object[] args = new object[1];
                args[0] = fileName;

                oWorkbook = in_oWorkbooks.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, in_oWorkbooks, args, cultInfo());
            }
            catch
            {
                return;
            }
        }


        public static void saveAs(
            ref object in_oWorkbook,
            string fileName)
        {
            try
            {
                object[] args = new object[1];
                args[0] = fileName;

                in_oWorkbook.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, in_oWorkbook, args, cultInfo());
            }
            catch
            {
                return;
            }
        }

        public static void saveAs(
            ref object in_oWorkbook,
            string fileName,
            int fileFormat = EXCEL_XLFILEFORMAT_XLEXCEL8,        
            string password = "", 
            string writeResPassword ="", 
            bool readOnlyRecommended = false,
            bool createBackup = false )
        {
            try
            {
                object[] args = new object[6];
                args[0] = fileName;
                args[1] = fileFormat;
                args[2] = password; 
                args[3] = writeResPassword; 
                args[4] = readOnlyRecommended;
                args[5] = createBackup;

                in_oWorkbook.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, in_oWorkbook, args, cultInfo());
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.StackTrace);
                return;
            }
        }


        public static void save(ref object in_oWorkbook)
        {
            in_oWorkbook.GetType().InvokeMember("Save", BindingFlags.InvokeMethod, null, in_oWorkbook, null, cultInfo());
        }


        public static string getRangeValue(
            ref object in_oWorksheet,
            string cellAdr)
        {
            object[] args = new object[2];
            args[0] = cellAdr;
            args[1] = Missing.Value;
            object Range = in_oWorksheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, in_oWorksheet, args, cultInfo());
            object val = Range.GetType().InvokeMember("Value", BindingFlags.GetProperty, null, Range, null, cultInfo());

            return val != null ? val.ToString() : null;
        }

        public static void getRangeValue(
            ref object in_oRange,
            ref string cellVal)
        {
            object[] args = new object[1];
            args[0] = Missing.Value;

            object tmp = in_oRange.GetType().InvokeMember("Value", BindingFlags.GetProperty, null, in_oRange, null, cultInfo());

            try
            {
                cellVal = tmp.ToString();
            }
            catch
            {
                cellVal = null;
            }
        }


        public static void setRangeValue(
            ref object in_oRange,
            string cellVal)
        {
            object[] args = new object[1];
            args[0] = cellVal;

            in_oRange.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, in_oRange, args, cultInfo());
        }


        public static void setRangeValue(
            ref object in_oWorksheet,
            ref object in_oRange,
            string cellAdr,
            string cellVal)
        {
            object[] args = new object[2];
            args[0] = cellAdr;
            args[1] = Missing.Value;

            in_oRange = in_oWorksheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, in_oWorksheet, args, cultInfo());

            args = new object[1];
            args[0] = cellVal;
            in_oRange.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, in_oRange, args, cultInfo());
        }


        public static void getRange(
            ref object in_oWorksheet,
            ref object oRange,
            string cellAdr)
        {
            object[] args = new object[2];
            args[0] = cellAdr;
            args[1] = Missing.Value;
            oRange = in_oWorksheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, in_oWorksheet, args, cultInfo());
        }


        public static void getRanges(
            ref object in_oWorksheet,
            ref object oRange,
            string adr1,
            string adr2)
        {
            object[] args = new object[2];
            args[0] = adr1;
            args[1] = adr2;
            oRange = in_oWorksheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, in_oWorksheet, args, cultInfo());
        }


        public static string getRangeAddressLocal(ref object in_oRange)
        {
            object[] args = new object[5];
            int Excel_XlReferenceStyle_xlA1 = 1;

            args[0] = Missing.Value;
            args[1] = Missing.Value;
            args[2] = Excel_XlReferenceStyle_xlA1;
            args[3] = Missing.Value;
            args[4] = Missing.Value;
            //string str1 = range.get_AddressLocal(Type.Missing,Type.Missing, Excel.XlReferenceStyle.xlA1,Type.Missing,Type.Missing);

            object tmp = in_oRange.GetType().InvokeMember("AddressLocal", BindingFlags.GetProperty, null, in_oRange, args, cultInfo());
            return tmp.ToString();
        }


        public static void setRangeBorders(
            ref object in_oRange,
            string adr1,
            string adr2,
            int val)
        {
            /*
            object[] args = new object[2];
            args[0] = adr1;
            args[1] = adr2;
            object tmp = in_oRange.GetType().InvokeMember("Borders", BindingFlags.GetProperty, null, in_oRange, args);

            //Excel.XlBorderWeight.xlThin = 2
            args = new object[1];
            args[0] = val;
            tmp.GetType().InvokeMember("Weight", BindingFlags.SetProperty, null, tmp, args);
            */
        }


        public static void setRangeBorders(
            ref object in_oRange,
            int val)
        {
            //Excel.XlBorderWeight.xlThin = 2
            object borders = in_oRange.GetType().InvokeMember("Borders", BindingFlags.GetProperty, null, in_oRange, null, cultInfo());
            object[] args = new object[1];
            args[0] = val;
            borders.GetType().InvokeMember("Weight", BindingFlags.SetProperty, null, borders, args, cultInfo());
        }


        public static void setRangeSelect(ref object in_oRange)
        {
            try
            {
                object tmp = in_oRange.GetType().InvokeMember("Select", BindingFlags.InvokeMethod, null, in_oRange, null, cultInfo());
            }
            catch { }
        }


        public static void getWorksheet(
            ref object in_oWorksheets,
            ref object oWorksheet,
            int numberSheet)
        {
            try
            {
                object[] args = new object[1];
                args[0] = numberSheet;
                oWorksheet = in_oWorksheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, in_oWorksheets, args, cultInfo());
            }
            catch
            {
                oWorksheet = null;
            }
        }


        public static void getWorksheet(
            ref object in_oWorksheets,
            ref object oWorksheet,
            string nameSheet)
        {
            try
            {
                object[] args = new object[1];
                args[0] = nameSheet;
                oWorksheet = in_oWorksheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, in_oWorksheets, args, cultInfo());
            }
            catch
            {
                oWorksheet = null;
            }
        }


        public static void getWorksheets(
            ref object in_oWorkbook,
            ref object oWorksheets)
        {
            oWorksheets = in_oWorkbook.GetType().InvokeMember("Worksheets", BindingFlags.GetProperty, null, in_oWorkbook, null, cultInfo());
        }


        public static void getWorkbook(
            ref object in_oWorkbooks,
            ref object oWorkbook)
        {
            oWorkbook = in_oWorkbooks.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, in_oWorkbooks, null, cultInfo());
        }


        public static void getWorkbooks(
            ref object in_oExcelApp,
            ref object oWorkbooks)
        {
            oWorkbooks = in_oExcelApp.GetType().InvokeMember("Workbooks", BindingFlags.GetProperty, null, in_oExcelApp, null, cultInfo());
        }


        public static void setNullComObject(ref object in_Exl)
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(in_Exl);
            in_Exl = null;
        }


        public static void quitExcel(ref object in_oExcelApp)
        {
            in_oExcelApp.GetType().InvokeMember("Quit", BindingFlags.InvokeMethod, null, in_oExcelApp, null, cultInfo());
        }


        public static void closeWorkbooks(ref object in_oWorkbooks)
        {
            in_oWorkbooks.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, in_oWorkbooks, null, cultInfo());
        }


        public static void closeWorkbook(
            ref object in_oWorkbook,
            int typeClose)
        {
            object[] args = new object[3];
            args[0] = typeClose;
            args[1] = null;
            args[2] = null;
            in_oWorkbook.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, in_oWorkbook, args, cultInfo());
        }


        public static void copyWorksheet(
            ref object in_oWorksheet1,
            ref object in_oWorksheet2)
        {
            //((Excel.Worksheet)objBook.Sheets.get_Item(1)).Copy(Type.Missing, objBook.Sheets.get_Item(num));
            object[] args = new object[2];
            args[0] = Missing.Value;
            args[1] = in_oWorksheet2;
            in_oWorksheet1.GetType().InvokeMember("Copy", BindingFlags.InvokeMethod, null, in_oWorksheet1, args, cultInfo());
        }



        /// <summary>
        /// Копирование выделенной области
        /// </summary>
        /// <param name="in_oExcelApp"></param>
        public static void selectCopy(ref object in_oExcelApp)
        {
            object selection = in_oExcelApp.GetType().InvokeMember("Selection", BindingFlags.GetProperty, null, in_oExcelApp, null, cultInfo());
            selection.GetType().InvokeMember("Copy", BindingFlags.InvokeMethod, null, selection, null, cultInfo());
        }


        public static void selectPaste(ref object in_oExcelApp)
        {
            object activesheet = in_oExcelApp.GetType().InvokeMember("ActiveSheet", BindingFlags.GetProperty, null, in_oExcelApp, null, cultInfo());
            activesheet.GetType().InvokeMember("Paste", BindingFlags.InvokeMethod, null, activesheet, null, cultInfo());
        }


        public static void selectClearContents(ref object in_oExcelApp)
        {
            object selection = in_oExcelApp.GetType().InvokeMember("Selection", BindingFlags.GetProperty, null, in_oExcelApp, null, cultInfo());
            selection.GetType().InvokeMember("ClearContents", BindingFlags.InvokeMethod, null, selection, null, cultInfo());
        }


        /// <summary>
        /// Странно работает, скорее не работает 
        /// </summary>
        /// <param name="in_oExcelApp"></param>
        /// <param name="rowsAdr"></param>
        public static void selectRows(
            ref object in_oExcelApp,
            string rowsAdr)
        {
            object rows = in_oExcelApp.GetType().InvokeMember("Rows", BindingFlags.GetProperty, null, in_oExcelApp, null, cultInfo());

            object[] args = new object[2];
            args[0] = rowsAdr;
            args[1] = Missing.Value;
            rows.GetType().InvokeMember("Select", BindingFlags.GetProperty, null, rows, null, cultInfo());
        }


        public static void selectRows(
            ref object in_oWorksheet,
            ref object in_oRange,
            string rowsAdr)
        {
            LOffice.cExcelObj.getRange(ref in_oWorksheet, ref in_oRange, rowsAdr);
            LOffice.cExcelObj.setRangeSelect(ref in_oRange);
        }


        public static void copyWorksheet(ref object in_oWorksheet)
        {
            //((Excel.Worksheet)objBook.Sheets.get_Item(1)).Copy(objBook.Sheets.get_Item(1), Type.Missing);
            object[] args = new object[2];
            args[0] = in_oWorksheet;
            args[1] = Missing.Value;
            in_oWorksheet.GetType().InvokeMember("Copy", BindingFlags.InvokeMethod, null, in_oWorksheet, args, cultInfo());
        }


        public static void setWorksheetName(
            ref object in_oWorksheet,
            string nameWS)
        {
            object[] args = new object[1];
            args[0] = nameWS;
            in_oWorksheet.GetType().InvokeMember("Name", BindingFlags.SetProperty, null, in_oWorksheet, args, cultInfo());
        }


        public static void deleteRange(ref object in_oRange)
        {
            object[] args = new object[1];
            args[0] = Missing.Value;
            in_oRange.GetType().InvokeMember("Delete", BindingFlags.InvokeMethod, null, in_oRange, args, cultInfo());
        }


        public static void mergeRange(ref object in_oExcelApp)
        {
            object selection = in_oExcelApp.GetType().InvokeMember("Selection", BindingFlags.GetProperty, null, in_oExcelApp, null, cultInfo());
            selection.GetType().InvokeMember("Merge", BindingFlags.InvokeMethod, null, selection, null, cultInfo());
        }


        public static void unmergeRange(ref object in_oExcelApp)
        {
            object selection = in_oExcelApp.GetType().InvokeMember("Selection", BindingFlags.GetProperty, null, in_oExcelApp, null, cultInfo());
            selection.GetType().InvokeMember("UnMerge", BindingFlags.InvokeMethod, null, selection, null, cultInfo());
        }


        public static void insertRows(ref object in_oRange)
        {
            int Excel_XlInsertShiftDirection_xlShiftDown = -4121;

            object[] args = new object[2];
            args[0] = Excel_XlInsertShiftDirection_xlShiftDown;
            args[1] = Missing.Value;

            in_oRange.GetType().InvokeMember("Insert", BindingFlags.InvokeMethod, null, in_oRange, args, cultInfo());
        }


        public static void deleteRows(ref object in_oRange)
        {
            int Excel_XlDeleteShiftDirection_xlShiftUp = -4162;

            object[] args = new object[1];
            args[0] = Excel_XlDeleteShiftDirection_xlShiftUp;

            in_oRange.GetType().InvokeMember("Delete", BindingFlags.InvokeMethod, null, in_oRange, args, cultInfo());
        }


        public static string getRowAdr(int index)
        {
            string tmp = index.ToString() + ":" + index.ToString();
            return tmp;
        }


        public static string getRowAdr(
            int index,
            string begCell,
            string endCell)
        {
            string tmp = begCell + index.ToString() + ":" + endCell + index.ToString();
            return tmp;
        }


        public static string getRowAdr(string val)
        {
            string rez = null;

            string s = val;
            string[] substrings = s.Split(':');

            for (int i = 0; i < substrings.Length; i++)
            {
                string tmp = substrings[i];

                int cnt = substrings[i].Length;
                for (int n = 0; n < cnt; n++)
                {
                    try
                    {
                        int m = System.Convert.ToInt32(tmp.Substring(n, 1));
                        rez += m.ToString();
                    }
                    catch { }
                }

                if (substrings.Length > 1)
                {
                    if (rez.IndexOf(':') < 0)
                        rez += ":";
                }
                else
                    rez = rez + ":" + rez;
            }

            return rez;
        }


        public static string getRowAdrLet(string val, int index)
        {
            string rez = null;

            try
            {
                string s = val;
                string[] substrings = s.Split(':');

                string tmp = substrings[index];

                tmp = tmp.Replace("$", "");

                int cnt = tmp.Length;
                for (int n = 0; n < cnt; n++)
                {
                    try
                    {
                        int m = System.Convert.ToInt32(tmp.Substring(n, 1));
                    }
                    catch
                    {
                        rez += tmp.Substring(n, 1);
                    }
                }

            }
            catch
            {
                return null;
            }

            return rez;
        }


        #endregion public Static Function

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        #region Функции работы с файлом Excel


        /// <summary>
        /// Слияние имени файла и пути
        /// </summary>
        /// <param name="pathPat"></param>
        /// <param name="filePat"></param>
        /// <param name="pathDoc"></param>
        /// <param name="fileDoc"></param>
        /// <param name="pathFilePat"></param>
        /// <param name="pathFileDoc"></param>
        protected virtual void createFileName(
            string path,
            string file,
            ref string pathFile)
        {
            pathFile = path;
            LAny.cStr.addSlash(ref pathFile);
            pathFile += file;
        }


        /// <summary>
        /// Проверка открыт файл документа Excel_ем или нет
        /// </summary>
        /// <param name="fileDoc"></param>
        /// <returns></returns>
        protected virtual int chkOpenFilesWin(string fileDoc)
        {
            int i = 0;
            string str = @"Microsoft Excel - " + fileDoc.Replace(".xls", "");
            if (LAny.cSysWinApi.findWindow(ref str, ref i))
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                string message = "Документ \"" + fileDoc + "\" уже открыт. \n";
                message += "Переключиться в  него?";
                string caption = "Вопрос";
                System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.YesNo;
                System.Windows.Forms.DialogResult result;

                result = System.Windows.Forms.MessageBox.Show(null, message, caption, buttons,
                    System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);

                if (result == System.Windows.Forms.DialogResult.Yes)
                    LAny.cSysWinApi.activateWindow(i);
                else
                    return -1;

            }
            return 1;
        }


        /// <summary>
        /// Проверка существования конечного файла документа
        /// </summary>
        /// <param name="pathFileDoc"></param>
        /// <param name="fileDoc"></param>
        /// <returns></returns>
        protected virtual int chkFileDocWin(
            string pathFileDoc,
            string fileDoc)
        {
            if (System.IO.File.Exists(pathFileDoc))
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                string message = "Документ \"" + fileDoc + "\" уже создан. \n";
                message += "Пересоздать его?";
                string caption = "Вопрос";
                System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.YesNo;
                System.Windows.Forms.DialogResult result;

                result = System.Windows.Forms.MessageBox.Show(null, message, caption, buttons,
                    System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    System.IO.File.Delete(pathFileDoc);
                    return 1;
                }
                else
                {
                    startExcel(pathFileDoc);
                    return -1;
                }
            }

            return 1;
        }


        /// <summary>
        /// Копирование шаблона в рабочий каталог
        /// только WinForms
        /// </summary>
        /// <param name="pathFilePat"></param>
        /// <param name="pathFileDoc"></param>
        /// <returns></returns>
        protected virtual int copyPatternWin(
            string pathFilePat,
            string pathFileDoc)
        {
            try
            {
                System.IO.File.Copy(pathFilePat, pathFileDoc, true);
                return 1;
            }
            catch (Exception theException)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                exceptMsg(ref theException);

                return -1;
            }
        }


        /// <summary>
        /// Запуск Excel
        /// только WinForms
        /// </summary>
        /// <param name="pathFileDoc"></param>
        public virtual void startExcel(string pathFileDoc)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            System.Diagnostics.Process.Start("Excel", pathFileDoc);
        }


        #endregion Функции работы с файлом Excel



        #region Функции работы с объектом ExcelApp


        /// <summary>
        /// Создание объекта ExcelApp
        /// </summary>
        /// <param name="oRange"></param>
        /// <param name="oWorksheet"></param>
        /// <param name="oWorksheets"></param>
        /// <param name="oWorkbook"></param>
        /// <param name="oWorkbooks"></param>
        /// <param name="oExcelApp"></param>
        /// <param name="pathFileDoc"></param>
        /// <returns></returns>
        protected virtual int initObj(
            ref object oRange,
            ref object oWorksheet,
            ref object oWorksheets,
            ref object oWorkbook,
            ref object oWorkbooks,
            ref object oExcelApp,
            string pathFileDoc)
        {
            LOffice.cExcelObj.createExcelApplication(ref oExcelApp);
            LOffice.cExcelObj.getWorkbooks(ref oExcelApp, ref oWorkbooks);
            LOffice.cExcelObj.getWorkbook(ref oWorkbooks, ref oWorkbook, pathFileDoc);
            LOffice.cExcelObj.getWorksheets(ref oWorkbook, ref oWorksheets);
            LOffice.cExcelObj.getWorksheet(ref oWorksheets, ref oWorksheet, 1);
            return 1;
        }


        /// <summary>
        /// Создание объекта ExcelApp
        /// только WinForms
        /// </summary>
        /// <param name="oRange"></param>
        /// <param name="oWorksheet"></param>
        /// <param name="oWorksheets"></param>
        /// <param name="oWorkbook"></param>
        /// <param name="oWorkbooks"></param>
        /// <param name="oExcelApp"></param>
        /// <returns></returns>
        protected virtual int initObjWin(
            ref object oRange,
            ref object oWorksheet,
            ref object oWorksheets,
            ref object oWorkbook,
            ref object oWorkbooks,
            ref object oExcelApp,
            string pathFileDoc)
        {
            try
            {
                LOffice.cExcelObj.createExcelApplication(ref oExcelApp);
                LOffice.cExcelObj.getWorkbooks(ref oExcelApp, ref oWorkbooks);
                LOffice.cExcelObj.getWorkbook(ref oWorkbooks, ref oWorkbook, pathFileDoc);
                LOffice.cExcelObj.getWorksheets(ref oWorkbook, ref oWorksheets);
                LOffice.cExcelObj.getWorksheet(ref oWorksheets, ref oWorksheet, 1);
                return 1;
            }
            catch (Exception theException)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                exceptMsg(ref theException);
                killExcel(ref oRange, ref oWorksheet, ref oWorksheets, ref oWorkbook, ref oWorkbooks, ref oExcelApp);
                return -1;
            }
        }



        /// <summary>
        /// Уничтожение объекта Excel.
        /// </summary>
        /// <param name="oRange"></param>
        /// <param name="oWorksheet"></param>
        /// <param name="oWorksheets"></param>
        /// <param name="oWorkbook"></param>
        /// <param name="oWorkbooks"></param>
        /// <param name="oExcelApp"></param>
        protected virtual void killExcel(
            ref object oRange,
            ref object oWorksheet,
            ref object oWorksheets,
            ref object oWorkbook,
            ref object oWorkbooks,
            ref object oExcelApp)
        {

            object[] args = new object[3];
            args[0] = 0;
            args[1] = null;
            args[2] = null;

            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");

            if (oWorkbook != null)
                oWorkbook.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, oWorkbook, args, ci);

            if (oWorkbooks != null)
                oWorkbooks.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, oWorkbooks, null, ci);

            if (oExcelApp != null)
                oExcelApp.GetType().InvokeMember("Quit", BindingFlags.InvokeMethod, null, oExcelApp, null);

            try
            {
                Marshal.ReleaseComObject(oRange);
            }
            catch { }

            try
            {
                Marshal.ReleaseComObject(oWorksheet);
            }
            catch { }

            try
            {
                Marshal.ReleaseComObject(oWorksheets);
            }
            catch { }

            try
            {
                Marshal.ReleaseComObject(oWorkbook);
            }
            catch { }

            try
            {
                Marshal.ReleaseComObject(oWorkbooks);
            }
            catch { }

            try
            {
                Marshal.ReleaseComObject(oExcelApp);
            }
            catch { }

            ci = null;

            oRange = null;
            oWorksheet = null;
            oWorksheets = null;
            oWorkbook = null;
            oWorkbooks = null;
            oExcelApp = null;

            // Вызываем сборщик мусора для немедленной очистки памяти
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //GC.Collect(); 
            //GC.GetTotalMemory(true);
        }

        #endregion Функции работы с объектом ExcelApp



        #region Функции работы с Rows


        protected virtual void rowsInsert(
            ref object in_oWorksheet,
            ref object in_oRange,
            string cellAdr)
        {
            LOffice.cExcelObj.getRange(ref in_oWorksheet, ref in_oRange, cellAdr);
            LOffice.cExcelObj.insertRows(ref in_oRange);
        }


        protected virtual void rowsDelete(
            ref object in_oWorksheet,
            ref object in_oRange,
            string cellAdr)
        {
            this.rangeSelect(ref in_oWorksheet, ref in_oRange, cellAdr);
            LOffice.cExcelObj.deleteRows(ref in_oRange);
        }


        protected virtual void rowsCopy(
            ref object in_oExcelApp,
            string cellAdrBeg,
            string cellAdrEnd)
        {
            LOffice.cExcelObj.selectRows(ref in_oExcelApp, cellAdrBeg);
            LOffice.cExcelObj.selectCopy(ref in_oExcelApp);
            LOffice.cExcelObj.selectRows(ref in_oExcelApp, cellAdrEnd);
            LOffice.cExcelObj.selectPaste(ref in_oExcelApp);
        }


        protected virtual void rowsCopy(
            ref object in_oExcelApp,
            ref object in_oWorksheet,
            ref object in_oRange,
            string cellAdrBeg,
            string cellAdrEnd)
        {
            LOffice.cExcelObj.selectRows(ref in_oWorksheet, ref in_oRange, cellAdrBeg);
            LOffice.cExcelObj.selectCopy(ref in_oExcelApp);
            LOffice.cExcelObj.selectRows(ref in_oWorksheet, ref in_oRange, cellAdrEnd);
            LOffice.cExcelObj.selectPaste(ref in_oExcelApp);
        }



        protected virtual string rowGetIndex(
            string cellAdr,
            bool abs)
        {
            string[] substrings = cellAdr.Split(':');
            int cnt = substrings.Length;

            string rez = null;

            if (abs)
                rez = "$";

            for (int i = 0; i < cnt; i++)
            {
                string tmp = substrings[i];

                for (int n = 0; n < tmp.Length; n++)
                {
                    try
                    {
                        string t = tmp.Substring(n, 1);
                        int q = System.Convert.ToInt32(t);
                        rez += q.ToString();
                    }
                    catch { }
                }

                if (cnt == 1)
                {
                    rez = rez + ":" + rez;
                }
                else
                {
                    if (i == 0)
                    {
                        if (abs)
                            rez += ":$";
                        else
                            rez += ":";
                    }
                }
            }

            return rez;
        }



        #endregion Функции работы с Rows



        #region Функции работы с Range


        /// <summary>
        /// Выбор ячейки
        /// </summary>
        /// <param name="in_oWorksheet"></param>
        /// <param name="in_oRange"></param>
        /// <param name="cellAdr"></param>
        protected virtual void rangeSelect(
            ref object in_oWorksheet,
            ref object in_oRange,
            string cellAdr)
        {
            LOffice.cExcelObj.getRange(ref in_oWorksheet, ref in_oRange, cellAdr);
            LOffice.cExcelObj.setRangeSelect(ref in_oRange);
        }


        /// <summary>
        /// Копирование 
        /// </summary>
        /// <param name="in_oExcelApp"></param>
        /// <param name="in_oWorksheet"></param>
        /// <param name="in_oRange"></param>
        /// <param name="cellAdr"></param>
        protected virtual void rangeCopy(
            ref object in_oExcelApp,
            ref object in_oWorksheet,
            ref object in_oRange,
            string cellAdr)
        {

            LOffice.cExcelObj.getRange(ref in_oWorksheet, ref in_oRange, cellAdr);
            LOffice.cExcelObj.setRangeSelect(ref in_oRange);
            LOffice.cExcelObj.selectCopy(ref in_oExcelApp);
        }


        /// <summary>
        /// Вставка
        /// </summary>
        /// <param name="in_oExcelApp"></param>
        /// <param name="in_oWorksheet"></param>
        /// <param name="in_oRange"></param>
        /// <param name="cellAdr"></param>
        protected virtual void rangePaste(
            ref object in_oExcelApp,
            ref object in_oWorksheet,
            ref object in_oRange,
            string cellAdr)
        {
            LOffice.cExcelObj.getRange(ref in_oWorksheet, ref in_oRange, cellAdr);
            LOffice.cExcelObj.setRangeSelect(ref in_oRange);
            LOffice.cExcelObj.selectPaste(ref in_oExcelApp);
        }


        /// <summary>
        /// Копирование и вставка
        /// </summary>
        /// <param name="in_oExcelApp"></param>
        /// <param name="in_oWorksheet"></param>
        /// <param name="in_oRange"></param>
        /// <param name="cellAdrBeg"></param>
        /// <param name="cellAdrEnd"></param>
        protected virtual void rangeCopyMake(
            ref object in_oExcelApp,
            ref object in_oWorksheet,
            ref object in_oRange,
            string cellAdrBeg,
            string cellAdrEnd)
        {
            rangeCopy(ref in_oExcelApp, ref in_oWorksheet, ref in_oRange, cellAdrBeg);
            rangePaste(ref in_oExcelApp, ref in_oWorksheet, ref in_oRange, cellAdrEnd);
        }


        /// <summary>
        /// Очистка значений из ячейки
        /// </summary>
        /// <param name="in_oExcelApp"></param>
        /// <param name="in_oWorksheet"></param>
        /// <param name="in_oRange"></param>
        /// <param name="cellAdr"></param>
        protected virtual void rangeClean(
            ref object in_oExcelApp,
            ref object in_oWorksheet,
            ref object in_oRange,
            string cellAdr)
        {
            LOffice.cExcelObj.getRange(ref in_oWorksheet, ref in_oRange, cellAdr);
            LOffice.cExcelObj.setRangeSelect(ref in_oRange);
            LOffice.cExcelObj.selectClearContents(ref in_oExcelApp);
        }


        /// <summary>
        /// Объединение ячеек
        /// </summary>
        /// <param name="in_oExcelApp"></param>
        /// <param name="in_oWorksheet"></param>
        /// <param name="in_oRange"></param>
        /// <param name="cellAdr"></param>
        protected virtual void rangeMerge(
            ref object in_oExcelApp,
            ref object in_oWorksheet,
            ref object in_oRange,
            string cellAdr)
        {
            LOffice.cExcelObj.getRange(ref in_oWorksheet, ref in_oRange, cellAdr);
            LOffice.cExcelObj.setRangeSelect(ref in_oRange);
            LOffice.cExcelObj.mergeRange(ref in_oExcelApp);
        }


        /// <summary>
        /// Отмена объединение ячеек
        /// </summary>
        /// <param name="in_oExcelApp"></param>
        /// <param name="in_oWorksheet"></param>
        /// <param name="in_oRange"></param>
        /// <param name="cellAdr"></param>
        protected virtual void rangeUnMerge(
            ref object in_oExcelApp,
            ref object in_oWorksheet,
            ref object in_oRange,
            string cellAdr)
        {
            LOffice.cExcelObj.getRange(ref in_oWorksheet, ref in_oRange, cellAdr);
            LOffice.cExcelObj.setRangeSelect(ref in_oRange);
            LOffice.cExcelObj.unmergeRange(ref in_oExcelApp);
        }


        /// <summary>
        /// Вставка значения в ячейку
        /// </summary>
        /// <param name="objSheet"></param>
        /// <param name="range"></param>
        /// <param name="nameFieldAdr"></param>
        /// <param name="val"></param>
        protected virtual void rangeSetVal(
            ref object in_oWorksheet,
            ref object in_oRange,
            string cellAdr,
            string val)
        {
            try
            {
                LOffice.cExcelObj.setRangeValue(ref in_oWorksheet, ref in_oRange, cellAdr, val);
            }
            catch
            {
                return;
            }
        }


        /// <summary>
        /// Вставка значения в ячейку
        /// последний параметр - значение вставляется в ячейку вместо пустого
        /// </summary>
        /// <param name="objSheet"></param>
        /// <param name="range"></param>
        /// <param name="fldAdr"></param>
        /// <param name="val"></param>
        /// <param name="valEmpty"></param>
        protected virtual void rangeSetVal(
            ref object in_oWorksheet,
            ref object in_oRange,
            string cellAdr,
            string val,
            string valEmpty)
        {
            try
            {
                if (val.Length > 0)
                    rangeSetVal(ref in_oWorksheet, ref in_oRange, cellAdr, val);
                else
                    rangeSetVal(ref in_oWorksheet, ref in_oRange, cellAdr, valEmpty);
            }
            catch
            {
                rangeSetVal(ref in_oWorksheet, ref in_oRange, cellAdr, valEmpty);
            }
        }


        ///// <summary>
        ///// Вставка значения в ячейку (формат format ДАТА)
        ///// </summary>
        ///// <param name="objSheet"></param>
        ///// <param name="range"></param>
        ///// <param name="nameFldAdr"></param>
        ///// <param name="dt"></param>
        ///// <param name="format"></param>
        //protected virtual void rangeSetVal_Dt(
        //    ref object in_oWorksheet, 
        //    ref object in_oRange, 
        //    string cellAdr, 
        //    DateTime dtVal, 
        //    string format)
        //{
        //    try
        //    {
        //        string tmpVal = dtVal.ToString(format);
        //        rangeSetVal(ref in_oWorksheet, ref in_oRange, cellAdr, tmpVal);
        //    }
        //    catch { }

        //}


        ///// <summary>
        ///// Вставка значения в ячейку (формат месяц)
        ///// </summary>
        ///// <param name="objSheet"></param>
        ///// <param name="range"></param>
        ///// <param name="nameFldAdr"></param>
        ///// <param name="dt"></param>
        //protected virtual void rangeSetVal_DT_MMMM_okonch(
        //    ref object in_oWorksheet, 
        //    ref object in_oRange, 
        //    string cellAdr, 
        //    DateTime dtVal)
        //{
        //    try
        //    {
        //        string tmpVal = chgOkonchMnth(dtVal);
        //        rangeSetVal(ref in_oWorksheet, ref in_oRange, cellAdr, tmpVal);
        //    }
        //    catch { }
        //}


        /// <summary>
        /// Поиск ячейки по шаблону (patternFind) и вставка значения
        /// </summary>
        /// <param name="in_oWorksheet"></param>
        /// <param name="in_oRange"></param>
        /// <param name="patternFind"></param>
        /// <param name="val"></param>
        protected virtual void rangePtrFindSetVal(
            ref object in_oWorksheet,
            ref object in_oRange,
            string patternFind,
            string val)
        {
            try
            {
                string cellAdr = null;
                cellAdr = rangeFind(patternFind, ref in_oWorksheet, ref in_oRange);
                if (cellAdr != null)
                {
                    rangeSetVal(ref in_oWorksheet, ref in_oRange, cellAdr, val);
                }
            }
            catch { }
        }


        /// <summary>
        /// Возвращеет адрес ячейки или null
        /// </summary>
        /// <param name="findVal"></param>
        /// <param name="in_oWorksheet"></param>
        /// <param name="oRange"></param>
        /// <returns></returns>
        protected virtual string rangeFind(
            string findVal,
            ref object in_oWorksheet,
            ref object oRange)
        {
            try
            {
                LOffice.cExcelObj.getRange(ref in_oWorksheet, ref oRange, "A1");
                LOffice.cExcelObj.findCell(findVal, ref in_oWorksheet, ref oRange);
                return LOffice.cExcelObj.getRangeAddressLocal(ref oRange);
            }
            catch
            {
                return null;
            }

        }


        protected virtual string rangeGetLetter(string cellAdr)
        {
            string s = "ZERRO" + cellAdr;
            string[] substrings = s.Split('$');

            return substrings[1];
        }


        protected virtual int rangeGetIndex(string cellAdr)
        {
            string s = "ZERRO" + cellAdr;
            string[] substrings = s.Split('$');

            s = substrings[2].Replace(":", "");
            return System.Convert.ToInt32(s);
        }


        protected virtual string rangeGetBegAdr(string cellAdr)
        {
            string s = cellAdr;
            string[] substrings = s.Split(':');

            s = substrings[0].Replace("$", "");
            return s;
        }


        protected virtual string rangeGetEndAdr(string cellAdr)
        {
            string s = cellAdr;
            string[] substrings = s.Split(':');
            s = substrings[1].Replace("$", "");

            return s;
        }




        #endregion Функции работы с Range

    }

}

