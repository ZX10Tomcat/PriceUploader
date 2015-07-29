using System;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace LAny
{
	/// <summary>
	/// Переходник для вызова функций winapi32
	/// </summary>
	public class cSysWinApi
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Рорup окно

        [DllImport("user32", EntryPoint = "SetWindowPos")]
        public static extern int SetWindowPos(
            int hWnd,
            int hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags);

        /// <summary>
        /// Вывод окна Popup
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="onTop"></param>
        public static void alwaysOnTop(
            int hWnd, 
            bool onTop)
        {
            //const int SWP_NOMOVE = 2;
            const int SWP_NOSIZE = 1;
            const uint FLAGS = SWP_NOSIZE;
            const int HWND_TOPMOST = -1;
            const int HWND_NOTOPMOST = -2;

            if (onTop)
                SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, FLAGS);
            else
                SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, FLAGS);
        }


        /// <summary>
        /// Вывод окна Popup
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="onTop"></param>
        public static void alwaysOnTop(
            int hWnd,
            int x,
            int y,
            bool onTop)
        {
            //const int SWP_NOMOVE = 2;
            const int SWP_NOSIZE = 1;
            const uint FLAGS = SWP_NOSIZE;
            const int HWND_TOPMOST = -1;
            const int HWND_NOTOPMOST = -2;

            if (onTop)
                SetWindowPos(hWnd, HWND_TOPMOST, x, y, 0, 0, FLAGS);
            else
                SetWindowPos(hWnd, HWND_NOTOPMOST, x, y, 0, 0, FLAGS);

        }


        #endregion Рорup окно


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region Функции работы с файлами


		const int READAPI = 0;
		const int WRITEAPI = 1;
		const int READ_WRITE = 2;
		
		const int MAX_PATH = 255;
		const int MAXDWORD = -1;

		///<remarks>INVALID_HANDLE_VALUE = -1</remarks>
		const int INVALID_HANDLE_VALUE = -1;
		///<remarks>FILE_ATTRIBUTE_READONLY = 1</remarks>
		const int FILE_ATTRIBUTE_READONLY = 1;
		///<remarks>FILE_ATTRIBUTE_HIDDEN = 2</remarks>
		const int FILE_ATTRIBUTE_HIDDEN = 2;
		///<remarks>FILE_ATTRIBUTE_SYSTEM = 4</remarks>
		const int FILE_ATTRIBUTE_SYSTEM = 4;
		///<remarks>FILE_ATTRIBUTE_DIRECTORY = 16</remarks>
		const int FILE_ATTRIBUTE_DIRECTORY = 16;
		///<remarks>FILE_ATTRIBUTE_ARCHIVE = 32</remarks>
		const int FILE_ATTRIBUTE_ARCHIVE = 32;
		///<remarks>FILE_ATTRIBUTE_NORMAL = 128</remarks>
		const int FILE_ATTRIBUTE_NORMAL = 128;
		///<remarks>FILE_ATTRIBUTE_TEMPORARY = 256</remarks>
		const int FILE_ATTRIBUTE_TEMPORARY = 256;


		public struct FILETIME
		{
			int dwLowDateTime;
			int dwHighDateTime;
		}


		public struct WIN32_FIND_DATA
		{
			public uint fileAttributes;
			public FILETIME creationTime;
			public FILETIME lastAccessTime;
			public FILETIME lastWriteTime;
			public uint fileSizeHigh;
			public uint fileSizeLow;
			public uint reserved0;
			public uint reserved1;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
			public string fileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=14)]
			public string alternateFileName;
		}


		public struct SYSTEMTIME 
		{
			public short wYear;  
			public short wMonth;  
			public short wDayOfWeek;  
			public short wDay;  
			public short wHour;  
			public short wMinute;  
			public short wSecond;  
			public short wMilliseconds;
		}


		/// <summary>
		/// Закрытие файла (winapi32) 
		/// </summary>
		/// <param name="hFile"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="_lclose")] 
		public static extern int lclose(int hFile);
		
		/// <summary>
		/// Открытие файла (winapi32)
		/// </summary>
		/// <param name="lpPathName"></param>
		/// <param name="iReadWrite"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="_lopen")] 
		public static extern int lopen(
            string lpPathName, 
			int iReadWrite);
		
		/// <summary>
		/// Дата и время файла (winapi32)
		/// </summary>
		/// <param name="lpFileTime"></param>
		/// <param name="lpSystemTime"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="FileTimeToSystemTime")] 
		public static extern int FileTimeToSystemTime(
			ref FILETIME lpFileTime, 
			ref SYSTEMTIME lpSystemTime);
		
		/// <summary>
		/// Дата и время файла (winapi32)
		/// </summary>
		/// <param name="lpFileTime"></param>
		/// <param name="lpLocalFileTime"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="FileTimeToLocalFileTime")] 
		public static extern int FileTimeToLocalFileTime(
			ref FILETIME lpFileTime, 
			ref FILETIME lpLocalFileTime);

		/// <summary>
		/// Получение даты и времени файла (winapi32)
		/// </summary>
		/// <param name="hFile"></param>
		/// <param name="lpCreationTime"></param>
		/// <param name="lpLastAccessTime"></param>
		/// <param name="lpLastWriteTime"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="GetFileTime")] 
		public static extern int GetFileTime(
			int hFile, 
			ref FILETIME lpCreationTime, 
			ref FILETIME lpLastAccessTime, 
			ref FILETIME lpLastWriteTime);
		
		/// <summary>
		/// Поиск файла (winapi32)
		/// </summary>
		/// <param name="lpFileName"></param>
		/// <param name="lpFindFileData"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="FindFirstFileA")] 
		public static extern int FindFirstFile(
			string lpFileName, 
			ref WIN32_FIND_DATA lpFindFileData);
		
		/// <summary>
		/// Поиск файла (winapi32)
		/// </summary>
		/// <param name="hFindFile"></param>
		/// <param name="lpFindFileData"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="FindNextFileA")] 
		public static extern int FindNextFile(
			int hFindFile, 
			ref WIN32_FIND_DATA lpFindFileData);
		
		/// <summary>
		/// Получение атрибутов файла (winapi32)
		/// </summary>
		/// <param name="lpFileName"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="GetFileAttributesA")] 
		public static extern int GetFileAttributes(
			string lpFileName);

		/// <summary>
		/// Установка атрибутов файла (winapi32)
		/// </summary>
		/// <param name="lpFileName"></param>
		/// <param name="dwFileAttributes"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="SetFileAttributesA")] 
		public static extern bool SetFileAttributes(
			string lpFileName,	
			int dwFileAttributes);
		
		/// <summary>
		/// Закрытие файла (?)  (winapi32)
		/// </summary>
		/// <param name="hFindFile"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll")]	
		public static extern int FindClose(
			int hFindFile);
		
		/// <summary>
		/// Перемещение файла (winapi32)
		/// [C#] public static void Move(string sourceFileName, string destFileName);
		/// System.IO.File.Move(sourceFileName, destFileName);
		/// </summary>
		/// <param name="lpExistingFileName"></param>
		/// <param name="lpNewFileName"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="MoveFileA")]
		public static extern int MoveFile(
			string lpExistingFileName, 
			string lpNewFileName);


		/// <summary>
		/// Копирование файла (winapi32)
		/// [C#] public static void Copy(string sourceFileName, string destFileName, bool overwrite);
		/// System.IO.File.Copy(sourceFileName, destFileName, true);
		/// </summary>
		/// <param name="lpExistingFileName"></param>
		/// <param name="lpNewFileName"></param>
		/// <param name="bFailIfExists"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll")]	
		public static extern bool CopyFile(
			string lpExistingFileName,
			string lpNewFileName,
			bool bFailIfExists);

		/// <summary>
		/// Удаление файла (winapi32)
		/// [C#] public static void Delete(string path);
		/// System.IO.File.Delete(path);
		/// </summary>
		/// <param name="lpFileName"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll")]	
		public static extern bool DeleteFile(
			string lpFileName);



        private static string StripNulls(ref string OriginalStr)
		{
			/*
			if ((InStr(OriginalStr, Chr(0)) > 0)) {
				OriginalStr = OriginalStr.PadLeft(InStr(OriginalStr, Chr(0)) - 1);
			}
			StripNulls = OriginalStr;
			*/
			return OriginalStr;
		}


		/// <summary>
		/// Поиск файла
		/// Если p_notIs = 0 означает поиск файла у которого атрибут (p_atr) НЕУСТАНОВЛЕН
		/// Если p_notIs = 1 означает поиск файла у которого атрибут (p_atr) УСТАНОВЛЕН
		/// Если p_atr   = -10, значение p_notIs игнорируется производится поиск всех файлов
		/// Для поиска не hidden файлов  p_atr = 1, p_notIs = 0
		/// </summary>
		/// <param name="p_path">Путь для поиска</param>
		/// <param name="p_mask">Маска для поиска</param>
		/// <param name="p_FileCount">Переменная для количества найденых файлов</param>
		/// <param name="p_DirCount">Переменная для количества найденых каталогов</param>
		/// <param name="p_arrPath">Список для хранения путей к файлам</param>
		/// <param name="p_arrFile">Список для хранения файлов</param>
		/// <param name="p_atr">Атрибут файла</param>
		/// <param name="p_notIs">Установлено значение атрибута или нет (1=установлено 0=не установлено)</param>
		/// <returns></returns>
		private static long findFilesAPI(
            ref string p_path, 
            ref string p_mask, 
            ref long p_FileCount, 
            ref long p_DirCount, 
            ref ArrayList p_arrPath, 
            ref ArrayList p_arrFile, 
            int p_atr, 
            int p_notIs)
		{
			string fileName;
			string DirName;
			ArrayList dirNames = new ArrayList();
			int nDir;
			int hSearch;
			WIN32_FIND_DATA WFD = new WIN32_FIND_DATA();
			bool Cont;
			long rezFindFile = 0;
			int rez = 0;
			int lnt = p_path.Length - 1;
			
			
			if(p_path.Substring(lnt,1).ToString() != "\\") 
			{
				p_path = p_path + "\\";
			}
			
			nDir = 0;
			
			Cont = true;
			hSearch = (int)FindFirstFile(p_path + "*", ref WFD);

			if (hSearch != INVALID_HANDLE_VALUE) 
			{
				while (Cont) 
				{
					DirName = StripNulls(ref WFD.fileName);
					if ((DirName != ".") & (DirName != "..")) 
					{
						rez = GetFileAttributes(p_path + DirName) + FILE_ATTRIBUTE_DIRECTORY;
						if(rez != 0) 
						{
							dirNames.Add(DirName);
							p_DirCount = p_DirCount + 1;
							nDir = nDir + 1;
						}
					}
					rez = FindNextFile(hSearch, ref WFD);
					if(rez != 0) 
						Cont = true;
					else
						Cont = false;

				}
				rez = FindClose(hSearch);
				if(rez != 0)
					Cont = true;
				else
					Cont = false;
			}
			
			hSearch = (int)FindFirstFile(p_path + p_mask,ref WFD);
			Cont = true;
			
			if (hSearch != INVALID_HANDLE_VALUE) 
			{
				while (Cont) 
				{
					fileName = StripNulls(ref WFD.fileName);
					if ((fileName != ".") & (fileName != "..")) 
					{
						rezFindFile = rezFindFile + (WFD.fileSizeHigh * MAXDWORD) + WFD.fileSizeLow;
						//добавление файла в массив						
						if((getFileAtr(p_path, fileName, p_atr) == 1 && p_notIs == 1) 
							|| ((getFileAtr(p_path, fileName, p_atr) == 0 && p_notIs == 0)) )
						{
							p_FileCount = p_FileCount + 1;
							p_arrPath.Add(p_path);
							p_arrFile.Add(fileName);
						}
						else if(p_atr == -10)
						{
							p_FileCount = p_FileCount + 1;
							p_arrPath.Add(p_path);
							p_arrFile.Add(fileName);
						}
					}
					rez = FindNextFile(hSearch, ref WFD);
					if(rez != 0) 
						Cont = true;
					else
						Cont = false;
				}

				rez = FindClose(hSearch);
				if(rez != 0) 
					Cont = true;
				else
					Cont = false;
			}
			if (nDir > 0) 
			{
				for (int i = 0; i <= nDir - 1; i++) 
				{
					string tmp = p_path + dirNames[i].ToString() + "\\";
					rezFindFile = rezFindFile + findFilesAPI(ref tmp, ref p_mask, ref p_FileCount, ref p_DirCount, 
						ref p_arrPath, ref p_arrFile , p_atr, p_notIs);
				}
			}
			return rezFindFile; 
		}


		/// <summary>
		/// Поиск файлов c любыми атрибута
		/// </summary>
		/// <param name="p_path">Путь для поиска</param>
		/// <param name="p_mask">Маска для поиска</param>
		/// <param name="p_arrPath">Список для хранения путей к файлам</param>
		/// <param name="p_arrFile">Список для хранения файлов</param>
		public static void makeFindFile(
            ref string p_path, 
            ref string p_mask, 
            ref ArrayList p_arrPath, 
            ref ArrayList p_arrFile)
		{
			long fileCount = 0;
			long dirCount  = 0; 
			findFilesAPI(ref p_path, ref p_mask, ref fileCount, ref dirCount, ref p_arrPath, ref p_arrFile, -10, 0);
		}


        /// <summary>
        /// Количество файлов c любыми атрибута по указаному пути
        /// </summary>
        /// <param name="p_path">Путь для поиска</param>
        /// <param name="p_mask">Маска для поиска</param>
        public static long clcFilesOnDir(
            ref string p_path,
            ref string p_mask)
        {
            //long p_FileCount = 0;
            //long dirCount = 0;
            //ArrayList p_arrPath = new ArrayList();
            //ArrayList p_arrFile = new ArrayList();

            //findFilesAPI(ref p_path, ref p_mask, ref p_FileCount, ref dirCount, ref p_arrPath, ref p_arrFile, -10, 0);
            //return p_FileCount;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            WIN32_FIND_DATA WFD = new WIN32_FIND_DATA();

            int countFiles = 0;
            bool Cont = true;
            int rez = 0;
            string path = p_path;
            string mask = p_mask;
            string fileName = "";
            int lnt = path.Length - 1;

            if(path.Substring(lnt,1).ToString() != "\\")
                path += "\\";

            string tmp = path + mask;
            int hSearch = (int)FindFirstFile(tmp, ref WFD);

            if (hSearch != INVALID_HANDLE_VALUE)
            {
                while (Cont)
                {
                    fileName = StripNulls(ref WFD.fileName);
                    if ((fileName != ".") & (fileName != ".."))
                        countFiles++;

                    rez = FindNextFile(hSearch, ref WFD);
                    if (rez != 0)
                        Cont = true;
                    else
                        Cont = false;

                }

                rez = FindClose(hSearch);
            }

            return countFiles;
        }


		/// <summary>
		/// Поиск файлов по заданому атрибуту и расчет количества файлов
		/// </summary>
		/// <param name="p_path">Путь для поиска</param>
		/// <param name="p_mask">Маска для поиска</param>
		/// <param name="p_FileCount">Переменная для количества найденых файлов</param>
		/// <param name="p_arrPath">Список для хранения путей к файлам</param>
		/// <param name="p_arrFile">Список для хранения файлов</param>
		/// <param name="p_atr">Атрибут файла</param>
		/// <param name="p_notIs">Установлено значение атрибута или нет (1=установлено 0=не установлено)</param>
		public static void makeFindFile(
            ref string p_path, 
            ref string p_mask, 
            ref long p_FileCount, 
            ref ArrayList p_arrPath, 
            ref ArrayList p_arrFile, 
            int p_atr, 
            int p_notIs)
		{
			long dirCount = 0; 
			findFilesAPI(ref p_path, ref p_mask, ref p_FileCount, ref dirCount, ref p_arrPath, ref p_arrFile, p_atr, p_notIs);
		}


		/// <summary>
		/// Поиск файлов по заданому атрибуту без расчета количества файлов
		/// </summary>
		/// <param name="p_path">Путь для поиска</param>
		/// <param name="p_mask">Маска для поиска</param>
		/// <param name="p_arrPath">Список для хранения путей к файлам</param>
		/// <param name="p_arrFile">Список для хранения файлов</param>
		/// <param name="p_atr">Атрибут файла</param>
		/// <param name="p_notIs">Установлено значение атрибута или нет (1=установлено 0=не установлено)</param>
		public static void makeFindFile(
            ref string p_path, 
            ref string p_mask, 
            ref ArrayList p_arrPath, 
            ref ArrayList p_arrFile, 
            int p_atr, 
            int p_notIs)
		{
			long fileCount = 0;
			long dirCount  = 0; 
			findFilesAPI(ref p_path, ref p_mask, ref fileCount, ref dirCount, ref p_arrPath, ref p_arrFile, p_atr, p_notIs);
		}


		/// <summary>
		/// Проверка атрибута файла заданого p_nBit, если атрибут не установлен функция возвращает 0
		/// </summary>
		/// <param name="p_path">Путь к файлу</param>
		/// <param name="p_fileName">Имя файла</param>
		/// <param name="p_nBit">атрибут файла</param>
		/// <returns>Возвращает 0 если атрибут не установлен</returns>
		public static int getFileAtr(
            string p_path, 
            string p_fileName, 
            int p_nBit)
		{
			string str = null;
			str = getPathFile(p_path, p_fileName); 
			int r = GetFileAttributes(str);
			double resD = (r/(Math.Pow(2, p_nBit)));
			int resI = (int)Math.Floor(resD);
			return (resI % 2);
		}


		/// <summary>
		/// Устанавливает атрибут для файла (для атрибута hidden p_nBit = 1)
		/// </summary>
		/// <param name="p_path">Путь к файлу</param>
		/// <param name="p_fileName">Имя файла</param>
		/// <param name="p_nBit">атрибут файла</param>
		/// <returns>Возвращает true если атрибут установлен</returns>
		public static bool setFileAtr(
            string p_path, 
            string p_fileName, 
            int p_nBit)
		{
			int atr = 0;
			string str = getPathFile(p_path, p_fileName); 
			atr = GetFileAttributes(str);
			atr = atr + (int)Math.Pow(2, p_nBit);
			return (SetFileAttributes(str, atr));
		}


		/// <summary>
		/// Устанавливает атрибут для файла (для атрибута hidden p_nBit = 1)
		/// </summary>
		/// <param name="p_pathFileName">Путь к файлу</param>
		/// <param name="p_nBit">атрибут файла</param>
		/// <returns>Возвращает true если атрибут установлен</returns>
		public static bool setFileAtr(
            string p_pathFileName, 
            int p_nBit)
		{
			int atr = 0;
			string str = p_pathFileName; 
			atr = GetFileAttributes(str);
			atr = atr + (int)Math.Pow(2, p_nBit);
			return (SetFileAttributes(str, atr));
		}


		/// <summary>
		/// Собирает путь и имя файла в одну строку
		/// </summary>
		/// <param name="p_path">Путь к файлу</param>
		/// <param name="p_fileName">Имя файла</param>
		/// <returns>Возвращает собраную строку</returns>
		public static string getPathFile(
            string p_path, 
            string p_fileName)
		{
			
			string str = null;
			str = p_path.Trim().ToString();
				
			if( p_path.Trim().Length > 0)   
			{
				str = p_path.Trim().ToString();
				
				if(p_fileName.Trim().Length > 0)
				{
					if(str.Substring((str.Length-1), 1 ) != "\\")
					{
						str = str + "\\";
					}
					str = str + p_fileName.Trim().ToString(); 
				}
			}
			else
			{
				if(p_fileName.Trim().Length > 0)
				{
					str = p_fileName.Trim().ToString(); 
				}
			}

			return str;
		}


		/// <summary>
		/// Определяет дату и время создания файла
		/// </summary>
		/// <param name="p_pathFileName">Путь к файлу и имя файла</param>
		/// <param name="p_dt">Дата и время создания файла, заполняется в функции</param>
		/// <returns>Если больше нуля то файл найден</returns>
		public static int getFileDate(
            string p_pathFileName, 
            ref DateTime p_dt)
		{
			int hFile = lopen(p_pathFileName, READAPI);
			if(hFile != 0) 
			{
				try
				{
					FILETIME crtTm  = new FILETIME();
					FILETIME lAccTm = new FILETIME();
					FILETIME lWrtTm = new FILETIME();
					FILETIME dtLocal = new FILETIME();
					
					int rez = GetFileTime(hFile, ref crtTm, ref lAccTm, ref lWrtTm); 
					
					if(rez != 0)
					{
						FileTimeToLocalFileTime(ref lWrtTm, ref dtLocal);
						
						SYSTEMTIME dtFile = new SYSTEMTIME(); 
						rez = FileTimeToSystemTime(ref dtLocal, ref dtFile);
						
						if(rez != 0)
						{
							//p_dt = File.GetCreationTime(p_pathFileName);
							//p_dt = File.GetLastAccessTime(p_pathFileName); 
							//p_dt = File.GetLastWriteTime(p_pathFileName);
													  
							//short yy = dtFile.wYear; 
							//short mm = dtFile.wMonth; 
							//short dd = dtFile.wDay; 

							p_dt = new DateTime((int)dtFile.wYear, (int)dtFile.wMonth, (int)dtFile.wDay,
								(int)dtFile.wHour, (int)dtFile.wMinute, (int)dtFile.wSecond);   
						}
					}
					
					if(rez <= 0)
					{
						p_dt = new DateTime(1899, 12, 31, 0, 0, 0);
						hFile = 0;
					}
				}
				catch
				{
					p_dt = new DateTime(1899, 12, 31, 0, 0, 0);
					hFile = 0;
				}
			}
			else
			{
				p_dt = new DateTime(1899, 12, 31, 0, 0, 0);
			}

			lclose(hFile);
			return hFile;

		}

		
		/// <summary>
		/// Поиск одного файла
		/// [C#] public static bool Exists(string path);
		/// System.IO.File.Exists(path);
		/// </summary>
		/// <param name="fndPath">Путь к файлу и имя файла</param>
		/// <returns>Если больше нуля то файл найден</returns>
		public static int findFileOne(string fndPath)
		{
			int ret = 0;
			
			WIN32_FIND_DATA wfd = new WIN32_FIND_DATA();
			
			int handel = FindFirstFile(fndPath, ref wfd);

			if(handel > -1)
			{
				ret = 1;
			}
			else
			{
				ret = 0;
			}

			FindClose(handel);
			
			return ret;
		}


		#endregion Функции работы с файлами
		
		
		///////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region Функции поиска окон


		public delegate bool CallBack( int handle, IntPtr param );		


		private const short SW_SHOWNORMAL = 1;
		private const short SW_SHOWMINIMIZED = 2;
		private const short SW_SHOWMAXIMIZED = 3;

		public static int WM_SYSCOMMAND = 0x0112; 
		public static int SC_CLOSE      = 0xF060;
        //public static int SC_HIDE       = 0x802a;

		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		public struct WINDOWPLACEMENT
		{
			public int Length;
			public int FLAGS;
			public int showCmd;
			public POINTAPI ptMinPosition;
			public POINTAPI ptMaxPosition;
			public RECT rcNormalPosition;
		}

		private static ArrayList strCaptions;
		private static ArrayList lngHandle;
		private static WINDOWPLACEMENT udateCurrentWin;

		public struct POINTAPI
		{
			public int x;
			public int y;
		}

		/// <summary>
		/// Получение текста заголовка окна (winapi32)
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="lpString"></param>
		/// <param name="cch"></param>
		/// <returns></returns>
		[DllImport("user32",EntryPoint="GetWindowTextA")]
		public static extern int GetWindowText(int hwnd, System.Text.StringBuilder lpString, int cch);
		
		/// <summary>
		/// Энумиратор окна (winapi32)
		/// </summary>
		/// <param name="cb"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		[DllImport("user32", ExactSpelling=true)]
		public static extern int EnumWindows(CallBack cb, IntPtr lParam);
		
		/// <summary>
		/// Поиск окна (winapi32)
		/// </summary>
		/// <param name="lpClassName"></param>
		/// <param name="lpWindowName"></param>
		/// <returns></returns>
		[DllImport("user32",EntryPoint="FindWindowA")]
		public static extern int FindWindow(string lpClassName, string lpWindowName);
		
		/// <summary>
		/// (?) (winapi32)
		/// </summary>
		/// <param name="hwnd"></param>
		/// <returns></returns>
		[DllImport("user32")]
		public static extern int SetForegroundWindow(int hwnd);

		/// <summary>
		/// (?) (winapi32)
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="lpwndpl"></param>
		/// <returns></returns>
		[DllImport("user32")]
		public static extern int SetWindowPlacement(int hwnd, ref WINDOWPLACEMENT lpwndpl);

		/// <summary>
		/// (?) (winapi32)
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="lpwndpl"></param>
		/// <returns></returns>
		[DllImport("user32")]
		public static extern int GetWindowPlacement(int hwnd, ref WINDOWPLACEMENT lpwndpl);

		/// <summary>
		/// Посылка сообщения окна (winapi32)
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="Msg"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		[DllImport("user32.dll")] 
		public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam); 

	
		/// <summary>
		/// поиск окна
		/// </summary>
		/// <param name="strCaption">Текст заголовка окна</param>
		/// <param name="lngHan">хендл окна</param>
		/// <returns>Возвращает true если окно найденно</returns>
		public static bool findWindow(ref string strCaption, ref int lngHan)
		{
			bool returnValue;
			int iCount;
			int lngEnum;
			WINDOWPLACEMENT udtCurrWin = new WINDOWPLACEMENT();
			int lngLenArray;
			
			strCaptions = new ArrayList();
			lngHandle   = new ArrayList();

			TextWriter tw = System.Console.Out;
			GCHandle gch = GCHandle.Alloc(tw);
			CallBack cb = new CallBack(captureEnumWindowsProc);
			lngEnum = EnumWindows(cb, (IntPtr)gch);

			iCount = 0;
			
			lngLenArray = strCaptions.Count; 
			
			for(int i = 0; i < lngLenArray; i++)
			{
				int n = ((string)strCaptions[i]).IndexOf(strCaption); 
				//if(((string)strCaptions[i]).Equals(strCaption))
				if(n >= 0) 
				{
					GetWindowPlacement((int)lngHandle[i], ref udtCurrWin);
					
					if(udtCurrWin.showCmd  == SW_SHOWMINIMIZED)
					{
						//udtCurrWin.Length  = Strings.Len(udtCurrWin); 
						udtCurrWin.Length  = Marshal.SizeOf(udtCurrWin);
						udtCurrWin.FLAGS   = 0;
						udtCurrWin.showCmd = SW_SHOWMAXIMIZED;
					}

					udateCurrentWin = udtCurrWin;
					lngHan = (int)lngHandle[i];
					
					iCount++;
				}
			}
			
			if (iCount >= 1)
				returnValue = true;
			else
				returnValue = false;
			
			return returnValue;
		}
		
		
		/// <summary>
		/// Создание списка открытых окон
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		private static bool captureEnumWindowsProc(int handle, IntPtr param )
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder(255);
			
			int cnt = GetWindowText(handle, sb, sb.Capacity + 1);
			if(cnt != 0)
			{
				strCaptions.Add(sb.ToString());
				lngHandle.Add(handle);
			}
			
			return true;
		}   
        		

		/// <summary>
		/// Активизация нужного окна
		/// </summary>
		/// <param name="lngHan"></param>
		/// <returns></returns>
		public static int activateWindow(int lngHan)
		{
			int returnValue;
			returnValue = 0;
			
			WINDOWPLACEMENT tmp = udateCurrentWin;
			SetWindowPlacement(lngHan, ref tmp);
			SetForegroundWindow(lngHan);
			return returnValue;
		}


		/*
		 * Пример
		private void button1_Click(object sender, System.EventArgs e)
		{
			int i = 0;
			string str = @"Calculator";
			if(cWinApi.findWindow(ref str, ref i))
			{
				cWinApi.activateWindow(i); 	
				int j = cWinApi.SendMessage(i, cWinApi.WM_SYSCOMMAND, cWinApi.SC_CLOSE, 0); 
			}	
				
		}
		*/

		#endregion Функции поиска окон


		///////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region Выключение ПК

		#region ShutdownFlag
    
		public const int Force = 0x00000004;
        
		/// EWX_FORCEIFHUNG
		/// </summary>
		public const int ForceIfHung = 0x00000010;

		/// EWX_LOGOFF
		public const int Logoff = 0x00000000;

		/// EWX_POWEROFF 
		public const int Poweroff = 0x00000008;

		/// EWX_REBOOT
		public const int Reboot = 0x00000002;

		/// EWX_SHUTDOWN
		public const int Shutdown = 0x00000001;

		[DllImport("user32.dll", SetLastError=true) ]
		private static extern bool ExitWindowsEx(
            int flag, 
            int reserved);

		#endregion


		#region Privileges related Structures

		[StructLayout(LayoutKind.Sequential, Pack=4)]
			public struct LUID_AND_ATTRIBUTES
		{
			public long Luid;
			public int Attributes;
		}

		[StructLayout(LayoutKind.Sequential, Pack=4)]
			public struct TOKEN_PRIVILEGES
		{
			public int PrivilegeCount;
			public LUID_AND_ATTRIBUTES Privileges;
		}
		#endregion


		#region Privilege related APIs

		[DllImport("advapi32.dll", SetLastError=true)]
		private static extern bool OpenProcessToken(
			IntPtr ProcessHandle,
			int DesiredAccess, 
			ref IntPtr TokenHandle);

		[DllImport("advapi32.dll", SetLastError=true)]
		private static extern bool LookupPrivilegeValue(
			string lpSystemName, 
			string lpName, 
			ref long lpLuid );

		[DllImport("advapi32.dll", SetLastError=true) ]
		private static extern bool AdjustTokenPrivileges(
			IntPtr TokenHandle, 
			bool DisableAllPrivileges,
			ref TOKEN_PRIVILEGES NewState, 
			int BufferLength,
			IntPtr PreviousState,
			IntPtr ReturnLength 
			);

		[DllImport("advapi32.dll", SetLastError=true) ]
		private static extern bool InitiateSystemShutdown(
			string lpMachineName,
			string lpMessage,
			int dwTimeout,
			bool bForceAppsClosed,
			bool bRebootAfterShutdown
			);

		#endregion


		/// <summary>
		/// If the function succeeds, the return value is nonzero.
		/// </summary>
		/// <param name="flag"></param>
		/// <returns></returns>
		public static bool DoExitWindows(int flag)
		{   
			try 
			{
				/// Windows NT/2000/XP
				/// SE_SHUTDOWN_NAME
				if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
					SetShutdownPrivilege();
					/// Windows Me 
					/// Explorer 
					/// EWX_LOGOFF 
					/// ExitWindowsEx() 
				else if (System.Environment.OSVersion.Platform == PlatformID.Win32Windows &&
					((flag & Force) == Force))
					KillExplorer();
			}
			catch  (Exception)
			{
				return false;
			}
			bool result = ExitWindowsEx( flag, 0 );
			return result;
		}
        

		public static bool DoExitRemoteWindows(
            string compName, 
            string msg, 
            int time, 
            bool force, 
            bool reboot)
		{   
			//SetRemoteShutdownPrivilege();
			
			if (InitiateSystemShutdown(compName, msg, time, force, reboot))
				return true;
			else 
				return false;
		}
        

		/// <summary>
		/// Windows NT/2000/XP
		/// SE_SHUTDOWN_NAME
		/// SE_REMOTE_SHUTDOWN_NAME
		/// </summary>
		private static void SetShutdownPrivilege()
		{   
			const int TOKEN_QUERY = 0x00000008;
			const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
			const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
			const int SE_PRIVILEGE_ENABLED = 0x00000002;

			///////////////////////////////////////////////////////////////////////////////////////
			const string SE_REMOTE_SHUTDOWN_NAME = "SeRemoteShutdownPrivilege";
			///////////////////////////////////////////////////////////////////////////////////////
			
			IntPtr hproc = System.Diagnostics.Process.GetCurrentProcess().Handle;

			// Token
			IntPtr hToken = IntPtr.Zero;
			if (!OpenProcessToken( hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref hToken ) )
				throw new Exception("OpenProcessToken");
			
			// LUID 
			long luid = 0;
			if (!LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref luid))
				throw new Exception("LookupPrivilegeValue");
		
			TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES();
			tp.PrivilegeCount = 1;
			tp.Privileges = new LUID_AND_ATTRIBUTES();
			tp.Privileges.Luid = luid;
			tp.Privileges.Attributes = SE_PRIVILEGE_ENABLED;

			if (!AdjustTokenPrivileges( hToken, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero ))
				throw new Exception("AdjustTokenPrivileges");

			///////////////////////////////////////////////////////////////////////////////////////
			if (!LookupPrivilegeValue(null, SE_REMOTE_SHUTDOWN_NAME, ref luid))
				throw new Exception("LookupPrivilegeValue");

			tp.PrivilegeCount = 1;
			tp.Privileges.Attributes = SE_PRIVILEGE_ENABLED;

			if (!AdjustTokenPrivileges( hToken, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero ))
				throw new Exception("AdjustTokenPrivileges");
			///////////////////////////////////////////////////////////////////////////////////////

		}
        		
		
		/// <summary>
		/// Windows Me 
		/// </summary>
		private static void KillExplorer()
		{
			// Windows Me 
			// ExitWindowsEx() 
			if (System.Environment.OSVersion.Platform == PlatformID.Win32Windows)
			{
				System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
				foreach(Process process in processes)
				{
					if(process.ProcessName.StartsWith("explorer"))
						process.Kill();
				}
			}
		}


		/*	
			Пример выключения
			int flag = LWinAPI.cWinApi.cs.Poweroff; 
			LWinAPI.cWinApi.cs.DoExitWindows(flag); 
		*/


		#endregion Выключение ПК

		
		///////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region Звуки

		/// <summary>
		/// Проигрывание звуковоко файла (winapi32)
		/// </summary>
		/// <param name="pszSound"></param>
		/// <param name="hmod"></param>
		/// <param name="fdwSound"></param>
		/// <returns></returns>
		[System.Runtime.InteropServices.DllImport("Winmm")] 
		public static extern bool PlaySound(			
			string pszSound,  
			IntPtr hmod,     
			int fdwSound    
			);
	
		/// <summary>
		/// Проигрывание звуковоко файла
		/// </summary>
		/// <param name="soundFileName"></param>
		/// <returns></returns>
		public static bool playSound(string soundFileName)
		{
			bool r;
			System.IntPtr resHandle = System.IntPtr.Zero;
			r = PlaySound(soundFileName, resHandle, 0);
			return r;
		}


		#endregion Звуки


		///////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region Версия ОС


		[ StructLayout( LayoutKind.Sequential, CharSet=CharSet.Auto )]  
			public class MEMORYSTATUS 
		{
			public int Length;  
			public int MemoryLoad;  
			public int TotalPhys;  
			public int AvailPhys;  
			public int TotalPageFile;  
			public int AvailPageFile;  
			public int TotalVirtual;  
			public int AvailVirtual;	
		}
		
		[ StructLayout( LayoutKind.Sequential )]  
			public class OSVERSIONINFO 
		{
			public int OSVersionInfoSize;  
			public int MajorVersion;  
			public int MinorVersion;  
			public int BuildNumber;  
			public int PlatformId;  
			//public string szCSDVersion;
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst=128 )]    
			public String CSDVersion;
		}
		

		/// <summary>
		/// Память (winapi32)
		/// </summary>
		/// <param name="lpBuffer"></param>
		[System.Runtime.InteropServices.DllImport("kernel32")] 
		public static extern void GlobalMemoryStatus(
			MEMORYSTATUS lpBuffer
			);
		
		 
		/// <summary>
		/// ВерсияОС (winapi32)
		/// </summary>
		/// <param name="lpVersionInfo"></param>
		/// <returns></returns>
		[System.Runtime.InteropServices.DllImport("kernel32")] 
		private static extern bool GetVersionEx(
			[In, Out] OSVERSIONINFO lpVersionInfo
			);


		/// <summary>
		/// ВерсияОС
		/// </summary>
		/// <returns></returns>
		public static string winVer()
		{
			OSVERSIONINFO OV = new OSVERSIONINFO();
			OV.OSVersionInfoSize = Marshal.SizeOf(OV); 
			GetVersionEx(OV);
			
			int c_MajorVersion   = OV.MajorVersion;
			int c_MinorVersion   = OV.MinorVersion;
			int c_BuildNumber    = OV.BuildNumber;
			string c_CSDVersion  = OV.CSDVersion;
			
			string tmp = null;
			string ver = null;

			ver = "Microsoft Windows ";
			
			tmp = null;
			tmp = c_MajorVersion.ToString();
			ver += tmp;
	
			tmp = null;
			tmp = c_MinorVersion.ToString();
			ver += "." + tmp + " (Build ";

			tmp = null;
			tmp = c_BuildNumber.ToString();
			ver += tmp;

			tmp = " " + c_CSDVersion + ")";
			ver += tmp;
				
			return ver;
		}

		/// <summary>
		/// Количество памяти
		/// </summary>
		/// <returns></returns>
		public static string winMem()
		{
			MEMORYSTATUS tmpMem = new MEMORYSTATUS();
			tmpMem.Length = Marshal.SizeOf(tmpMem);
			GlobalMemoryStatus(tmpMem);
			
			string tmp = null; 
			string mem = null;
			
			mem = "Physical memory available to Windows: ";

			int n = tmpMem.TotalPhys / 1024;
			tmp = n.ToString();
	
			mem += tmp;
			tmp = " Kb";
			mem += tmp;
			
			return mem;
		}


		#endregion Версия ОС


    }
}
