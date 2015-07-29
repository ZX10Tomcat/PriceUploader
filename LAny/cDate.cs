using System;

namespace LAny
{
	/// <summary>
	/// Обработка значений дата/время
	/// </summary>
	public class cDate
	{
        public static string monthName = "Январь;Февраль;Март;Апрель;Май;Июнь;Июль;Август;Сентябрь;Октябрь;Ноябрь;Декабрь";

		/// <summary>
		/// Конструктор
		/// </summary>
		public cDate()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		
		/// <summary>
		/// Форматрирование даты 
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string getDate_ddMMyyyy(ref DateTime dt)
		{
			return (dt.ToString("dd") + "/" + dt.ToString("MM") + "/" + dt.ToString("yyyy"));  
		}

		/// <summary>
		/// Форматрирование даты
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string getDate_ddMM_HHmm(ref DateTime dt)
		{
			return (dt.ToString("dd") + "/" + dt.ToString("MM") + " " + " " + dt.ToString("HH:mm"));  
		}


        /// <summary>
        /// Дата 31-12-1899 (null)
        /// </summary>
        /// <returns></returns>
        public static DateTime nullDate()
        {
            return new DateTime(1899, 12, 31, 0, 0, 0);
        }

        
        /// <summary>
		/// Дата по умолчанию 01-01-1900
		/// </summary>
		/// <returns></returns>
		public static DateTime defaultDate()
		{
			return new DateTime(1900, 1, 1, 0, 0, 0); 
		}
		

		/// <summary>
		/// Дата по умолчанию 01-01-2000
		/// </summary>
		/// <returns></returns>
		public static DateTime defaultDate2k()
		{
			return new DateTime(2000, 1, 1, 0, 0, 0); 
		}

		
		/// <summary>
		/// Дата 01-01-2100
		/// </summary>
		/// <returns></returns>
		public static DateTime largeDate()
		{
			return new DateTime(2100, 1, 1, 0, 0, 0); 
		}

		
		/// <summary>
		/// Первое число месяца
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static DateTime firstDay(ref DateTime dt)
		{
			int yy = dt.Year; 
			int MM = dt.Month; 
			return new DateTime(yy, MM, 1, 0, 0, 0); 
		}


		/// <summary>
		/// Формирует дату или дату/время
        /// type == "DATE" формат dd-mm-yyyy
        /// type != "DATE" формат dd-mm-yyyy HH:mm
		/// </summary>
		/// <param name="val"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static DateTime getDateTime(
            string val, 
            string type)
		{
			DateTime dt;
			if(type.Equals("DATE") )
			{
				dt = new DateTime(DateTime.Parse(val).Year, 
					DateTime.Parse(val).Month, 
					DateTime.Parse(val).Day);
			}
			else //DateTime
			{
				dt = new DateTime(DateTime.Parse(val).Year, 
					DateTime.Parse(val).Month, 
					DateTime.Parse(val).Day,
					DateTime.Parse(val).Hour,
					DateTime.Parse(val).Minute,
					0);
			}
		
			return dt;
		}


		/// <summary>
		/// Формирует дату и время 0:00:00 
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static DateTime getDate_TimeZero(DateTime val)
		{
			DateTime dt = new DateTime(val.Year, val.Month, val.Day, 0,0,0);
			return dt;
		}


		/// <summary>
		/// Формирует дату последнего дня месяца
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static DateTime getLastDayMnth(DateTime val)
		{
			DateTime dt = new DateTime();
			
			try
			{
				dt = new DateTime(val.Year, val.Month, 31);
				return dt;
			}
			catch{}

			try
			{
				dt = new DateTime(val.Year, val.Month, 30);
				return dt;
			}
			catch{}
			

			try
			{
				dt = new DateTime(val.Year, val.Month, 29);
				return dt;
			}
			catch{}


			try
			{
				dt = new DateTime(val.Year, val.Month, 28);
				return dt;
			}
			catch{}

			
			return dt;
		}


        /// <summary>
        /// Номер месяца
        /// </summary>
        /// <param name="monthStr"></param>
        /// <returns></returns>
        public static string getDateMonthNumEng(string monthStr) 
        {
            string tmp = "";

            switch (monthStr)
            {
                case "Jan":
                    tmp = "01";
                    break;
                case "Feb":
                    tmp = "02";
                    break;
                case "Mar":
                    tmp = "03";
                    break;
                case "Apr":
                    tmp = "04";
                    break;
                case "May":
                    tmp = "05";
                    break;
                case "Jun":
                    tmp = "06";
                    break;
                case "Jul":
                    tmp = "07";
                    break;
                case "Aug":
                    tmp = "08";
                    break;
                case "Sep":
                    tmp = "09";
                    break;
                case "Oct":
                    tmp = "10";
                    break;
                case "Nov":
                    tmp = "11";
                    break;
                case "Dec":
                    tmp = "12";
                    break;
            }

            return tmp;
        }


        /// <summary>
        /// Изменение окончания последнего дня месяца
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string chgOkonchMnth(DateTime dt)
        {
            string tmpVal = dt.ToString("MMMM");
            tmpVal = tmpVal.ToLower();

            int tmp = dt.Month;

            switch (tmp)
            {
                case 1:
                    tmpVal = tmpVal.Replace("ь", "я");
                    break;
                case 2:
                    tmpVal = tmpVal.Replace("ь", "я");
                    break;
                case 3:
                    tmpVal += "a";
                    break;
                case 4:
                    tmpVal = tmpVal.Replace("ь", "я");
                    break;
                case 5:
                    tmpVal = tmpVal.Replace("й", "я");
                    break;
                case 6:
                    tmpVal = tmpVal.Replace("ь", "я");
                    break;
                case 7:
                    tmpVal = tmpVal.Replace("ь", "я");
                    break;
                case 8:
                    tmpVal = tmpVal += "a";
                    break;
                case 9:
                    tmpVal = tmpVal.Replace("ь", "я");
                    break;
                case 10:
                    tmpVal = tmpVal.Replace("ь", "я");
                    break;
                case 11:
                    tmpVal = tmpVal.Replace("ь", "я");
                    break;
                case 12:
                    tmpVal = tmpVal.Replace("ь", "я");
                    break;
            }

            return tmpVal;
        }

        //public enum typeDatePart
        //{
        //    day = 1, 
        //    month = 2,
        //    year = 3 
        //}


        //public static int datePart(typeDatePart tdp, DateTime dt)
        //{
        //    if (tdp == typeDatePart.day)
        //    { 
            
        //    }

        //}


        /// <summary>
        /// Проверка попадания даты в диапазон
        /// </summary>
        /// <param name="dtChk"></param>
        /// <param name="dtBeg"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public static bool dateBetween(ref DateTime dtChk, ref DateTime dtBeg, ref DateTime dtEnd)
        {
            if (dtChk >= dtBeg && dtChk <= dtEnd)
                return true;
            else
                return false;
        
        }

	}
}
