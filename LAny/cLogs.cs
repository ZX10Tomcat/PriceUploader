using System;
using System.Collections.Generic;
using System.Text;

namespace LAny
{
    class cLogs
    {
        public cLogs() { }
        

        public static void writeLog(string txtMessage, string logFileName)
        {
            try
            {
                string fileName = System.Windows.Forms.Application.StartupPath;

                int lnt = fileName.Length - 1;
                if (fileName.Substring(lnt, 1).ToString() != "\\")
                {
                    fileName += "\\";
                }

                fileName += (logFileName + ".log");

                if (!System.IO.File.Exists(fileName))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(fileName)) { }
                }

                string tmp = DateTime.Now.ToString();
                tmp += ";\t";
                tmp += txtMessage;

                using (System.IO.StreamWriter sw = System.IO.File.AppendText(fileName))
                {
                    sw.WriteLine(tmp);
                    sw.Close();
                }
            }
            catch
            {
                return;
            }
        }

    }
}
