using System;
using System.Xml;
using System.Text;

namespace LAny
{
	/// <summary>
	/// Обработка строковых значений
	/// </summary>
	public class cStr
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public cStr(){}

        private const string strXMLversion = "<?xml version='1.0'?>";
        private const string strBegTag = "<tag> <val ";
        private const string strEndTag = " /> </tag>";
        
        public enum tagDataType { Integer, Double, Date };

        
        private static string getXmlStr(string val)
        {
            string tmp = strBegTag;
            tmp += val;
            tmp += strEndTag;
            return tmp;
        }


        /// <summary>
        /// Возвращает параметр string или null в случае ошибки
        /// </summary>
        /// <param name="parName"></param>
        /// <returns></returns>
        public static string getParStr(string strVal, string parName)
        {
            string rez = null;
            try
            {
                string strXML = getXmlStr(strVal);
                System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
                xmldoc.LoadXml(strXML);
                XmlNodeList xnl = xmldoc.GetElementsByTagName("val");

                if (xnl.Count > 0)
                {
                    XmlNode xn = xnl[0].Attributes.GetNamedItem(parName);
                    if (xn != null)
                    {
                        rez = xn.Value;        //return xnl[0].Attributes[parName].Value.ToString();
                        xn = null;
                    }
                }
                xnl = null;
            }
            catch
            {
                rez = null;
            }

            return rez;
        }


        /// <summary>
        /// Возвращает параметр int или null в случае ошибки
        /// </summary>
        /// <param name="parName"></param>
        /// <returns></returns>
        public static int? getParInt(string strVal, string parName)
        {
            try
            {
                return Convert.ToInt32(getParStr(strVal, parName));
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Возвращает параметр double или null в случае ошибки
        /// </summary>
        /// <param name="parName"></param>
        /// <returns></returns>
        public static double? getParDbl(string strVal, string parName)
        {
            try
            {
                return Convert.ToDouble(getParStr(strVal, parName));
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Возвращает параметр DateTime или null в случае ошибки
        /// </summary>
        /// <param name="parName"></param>
        /// <returns></returns>
        public static DateTime? getParDtTm(string strVal, string parName)
        {
            try
            {
                return Convert.ToDateTime(getParStr(strVal, parName));
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Возвращает параметр string (форматированый) или null в случае ошибки
        /// </summary>
        /// <param name="strVal"></param>
        /// <param name="parName"></param>
        /// <param name="format"></param>
        /// <param name="tdt"></param>
        /// <returns></returns>
        public static string getParStrFormat(string strVal, string parName, string format, tagDataType tdt)
        {
            string tmpVal = getParStr(strVal, parName);

            if (tmpVal != null)
            {
                try
                {
                    if(format != null)
                        if (format.Length > 0)
                            if (tdt == tagDataType.Integer)
                            {
                                int t = Convert.ToInt32(tmpVal);
                                return t.ToString(format);
                            }
                            else if (tdt == tagDataType.Double)
                            {
                                double t = Convert.ToDouble(tmpVal);
                                return t.ToString(format);
                            }
                            else if (tdt == tagDataType.Date)
                            {
                                DateTime t = Convert.ToDateTime(tmpVal);
                                return t.ToString(format);
                            }

                    return tmpVal;
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }



		/// <summary>
		/// Замена одного значения другим
		/// </summary>
		/// <param name="strVal"></param>
		/// <param name="strOld"></param>
		/// <param name="strNew"></param>
		/// <returns></returns>
        //public static string strReplace(
        //    string strVal, 
        //    string strOld, 
        //    string strNew)
        //{
        //    return strVal.Replace(strOld, strNew); 
        //}


		/// <summary>
		/// Добавление второго слеша
		/// </summary>
		/// <param name="path"></param>
		public static void addSlash(ref string path)
		{
			int lnt = path.Length - 1;
			if(path.Substring(lnt,1).ToString() != "\\") 
			{
				path += "\\";
			}
		}


		/*
		int BinarySearch(int *mass, int SearchKey, int low, int high)
		{
			int middle;
  
			while(low <= high)
			{
				middle = (low + high)/2;
				
				if(SearchKey == mass[middle])
					return middle;
				else if(SearchKey < mass[middle])
					high = middle - 1;
				else
					low = middle +1;
			}
 
			return -1;
		}  
		*/

		/// <summary>
		/// Получение значения (tag) из строки
		/// </summary>
		/// <param name="val"></param>
		/// <param name="nameTag"></param>
		/// <returns></returns>
		public static string getValStrTag(
            string val, 
            string nameTag)
		{
			//Пример строки
			//$col1=J 
			//$col1=J1 $col2=J2 $col3=J3 
            
            //string val == $col1=J1
            //string nameTag == col1

			string tmp = null;

			int n = val.IndexOf(nameTag);
			if(n < 0)
				return null;
			
			try
			{
				bool ext = true;
				int m = n + nameTag.Length + 1;
				while(ext)
				{
					if(m >= val.Length)
						break;

					string t = val.Substring(m, 1);
					
					if(t.Equals(" "))
					{
						break;
					}
					else
					{
						tmp += t;
					}
					
					m++;
				}
			}
			catch
			{
				return tmp;
			}

			return tmp;
		}


        public static string getConvert1251(
            string cp1251String)
        {
            //string cp1251String = "Тут типа XML документ"; 
            //Encoding cp1251 = Encoding.GetEncoding(1251); 
            //Encoding utf8 = Encoding.UTF8; 
            //byte[] cp1251Bytes = cp1251.GetBytes(cp1251String); 
            //byte[] utf8Bytes = Encoding.Convert(cp1251, utf8, cp1251Bytes); 
            //string utf8String = utf8.GetString(utf8Bytes);

            Encoding cp1251 = Encoding.GetEncoding(1251);
            Encoding utf8 = Encoding.UTF8;
            byte[] cp1251Bytes = cp1251.GetBytes(cp1251String);
            byte[] utf8Bytes = Encoding.Convert(cp1251, utf8, cp1251Bytes);
            string utf8String = utf8.GetString(utf8Bytes);

            return utf8String;
        }



        public static string replDecimal(string valStr)
        {
            try
            {
                valStr = valStr.Replace(",", ".");
                double d = System.Convert.ToDouble(valStr);
            }
            catch
            {
                valStr = valStr.Replace(".", ",");
            }

            return valStr;
        }
	}
}
