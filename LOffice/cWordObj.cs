using System;
using System.Runtime.InteropServices;
using System.Reflection;


namespace LOffice
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class cWordObj
	{
		public static string wrdVersion;
		public cWordObj(){}

		public static void createWordApplication(ref object in_oWord)
		{
			string sAppProgID = "Word.Application";
			Type tWordObj = Type.GetTypeFromProgID(sAppProgID);
			in_oWord = Activator.CreateInstance(tWordObj);
			wrdVersion = wordVersion(ref in_oWord);
		}

		public static string wordVersion(ref object in_oWord)
		{
			object tmp = null;
			tmp = in_oWord.GetType().InvokeMember("Version", BindingFlags.GetProperty, null, in_oWord, null); 
			return tmp.ToString(); 
		}
			

		public static void getDocuments(
            ref object in_oWord, 
            ref object oDocuments)
		{
			oDocuments = in_oWord.GetType().InvokeMember("Documents", BindingFlags.GetProperty, null, in_oWord, null);
		}


		public static void getDocument(
            ref object in_oWord, 
            ref object oDocument, 
            ref string fileName)
		{
			//thisDocument = (Word.Document) thisApplication.Documents.Item(ref filename);

			object tmp = in_oWord.GetType().InvokeMember("Documents", BindingFlags.GetProperty, null, in_oWord, null);
			
			object [] args = new object[1];
			args[0] = fileName;
			oDocument = tmp.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, tmp, args);
		}


		public static void getActiveDocument(
            ref object in_oWord, 
            ref object oDocument)
		{
			oDocument = in_oWord.GetType().InvokeMember("ActiveDocument",BindingFlags.GetProperty,null,in_oWord,null);
		}


		public static void setNullComObject(ref object in_obj)
		{
			System.Runtime.InteropServices.Marshal.ReleaseComObject(in_obj);
			in_obj = null;
		}


		public static int openDocuments(
            ref object in_oDocuments, 
            string fileName)
		{
			/*
				thisApplication.Documents.Open(ref filename, ref confirmConversions, ref readOnly, ref addToRecentFiles, 
				ref passwordDocument, ref passwordTemplate, ref revert, ref writePasswordDocument, ref writePasswordTemplate, 
				ref format, ref encoding, ref visible, ref openAndRepair, ref documentDirection, ref noEncodingDialog);
			
			    Documents.Open 
					FileName:="resume.doc", 
					ConfirmConversions:=False, 
					ReadOnly:=False, 
					AddToRecentFiles:=False, 
					PasswordDocument:="", 
					PasswordTemplate:="", 
					Revert:=False, 
					WritePasswordDocument:="", 
					WritePasswordTemplate:=""
					Format:=wdOpenFormatAuto
					Encoding:=1251
			
			*/

			try
			{
				object [] args = new object[11];
				args[0] = fileName;
				args[1] = Missing.Value;
				args[2] = Missing.Value;
				args[3] = Missing.Value;
				args[4] = Missing.Value;
				args[5] = Missing.Value;
				args[6] = Missing.Value;
				args[7] = Missing.Value;
				args[8] = Missing.Value;
				args[9] = 0;
				args[10] = 1251;

				in_oDocuments.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, in_oDocuments, args);

				return 0;
			}
			catch
			{
				return -1;
			}

		}


		public static int saveAsDocument(
            ref object in_oDocument, 
            string fileName)
		{
			/*	
				thisDocument.SaveAs(ref fileName, ref fileFormat, ref lockComments, ref password, 
				ref addToRecentFiles, ref writePassword,ref readOnlyRecommended, ref embedTrueTypeFonts,
				ref saveNativePictureFormat, ref saveFormsData, ref saveAsAOCELetter, 
				ref encoding, ref insertLineBreaks, ref allowSubstitutions, ref lineEnding, ref addBiDiMarks);

				ActiveDocument.SaveAs 
					FileName:="jhgjk.asc", 
					FileFormat:=100, 
					LockComments:=False, 
					Password:="", 
					AddToRecentFiles:=True, 
					WritePassword:="",
					ReadOnlyRecommended:=False, 
					EmbedTrueTypeFonts:=False, _
					SaveNativePictureFormat:=False, 
					SaveFormsData:=False, 
					SaveAsAOCELetter:=False
			*/	

			//На моем компе работает с параметром 100
			//На сервере работает с параметром 113

			try
			{
				object [] args = new object[11];
				args[0] = fileName;
				args[1] = 113;
				args[2] = Missing.Value;
				args[3] = Missing.Value;
				args[4] = true;
				args[5] = Missing.Value;
				args[6] = Missing.Value;
				args[7] = Missing.Value;
				args[8] = Missing.Value;
				args[9] = Missing.Value;
				args[10] = Missing.Value;

				in_oDocument.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, in_oDocument, args);

				return 0;
			}
			catch
			{
				return -1;
			}
		}


		public static int saveAsDocument(
            ref object in_oDocument, 
            string fileName, 
            int typeSave)
		{
			
			//typeSave на моем компе работает с параметром 100
			//typeSave на сервере работает с параметром 113

			try
			{
				object [] args = new object[11];
				args[0] = fileName;
				args[1] = typeSave;
				args[2] = Missing.Value;
				args[3] = Missing.Value;
				args[4] = true;
				args[5] = Missing.Value;
				args[6] = Missing.Value;
				args[7] = Missing.Value;
				args[8] = Missing.Value;
				args[9] = Missing.Value;
				args[10] = Missing.Value;

				in_oDocument.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, in_oDocument, args);

				return 0;
			}
			catch
			{
				return -1;
			}
		}


		
		public static void closeDocument(ref object in_oDocument)
		{
			object [] args = new object[3];
			args[0] = Missing.Value;
			args[1] = Missing.Value;
			args[2] = Missing.Value;

			in_oDocument.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, in_oDocument, args);
		}


		public static void closeDocuments(ref object in_oDocuments)
		{
			
			object [] args = new object[3];
			args[0] = Missing.Value;
			args[1] = Missing.Value;
			args[2] = Missing.Value;

			in_oDocuments.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, in_oDocuments, args);
		}

		
		public static void quitWord(ref object in_oWord)
		{
			object [] args = new object[3];
			args[0] = Missing.Value;
			args[1] = Missing.Value;
			args[2] = Missing.Value;

			in_oWord.GetType().InvokeMember("Quit", BindingFlags.InvokeMethod, null, in_oWord, args);
		}




	}
}
