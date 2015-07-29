using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace LAny
{
    public class cSysReg
    {
        cSysReg() { }

        public static string getStrKey(
            string regPath,
            string nameKey)
        {
            return getStrKeyLocalMachine(regPath, nameKey);
        }


        public static string getStrKeyLocalMachine(
            string regPath,
            string nameKey)
        {
            RegistryKey rk;
            try
            {
                rk = Registry.LocalMachine.OpenSubKey(regPath, false);
                string tmp = rk.GetValue(nameKey).ToString();
                return tmp;
            }
            catch
            {
                return null;
            }
        }


        public static string getStrKeyCurrentUser(
            string regPath,
            string nameKey)
        {
            RegistryKey rk;
            try
            {
                rk = Registry.CurrentUser.OpenSubKey(regPath, false);
                string tmp = rk.GetValue(nameKey).ToString();
                return tmp;
            }
            catch
            {
                return null;
            }
        }



    }


}
