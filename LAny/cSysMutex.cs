using System;
using System.Collections.Generic;
using System.Text;

namespace LAny
{
    public class cSysMutes
    {
        public static System.Threading.Mutex mutex;

        /// <summary>
        /// ���� ������� ���������� TRUE - ��� �������� ��� ��������� �� ���������
        /// ���� ������� ���������� FALSE - ��� �������� ��� ��������� ��������� ��������
        /// </summary>
        /// <returns></returns>
        public static bool instanceExists(string appLabel)
        {
            bool createdNew;
            mutex = new System.Threading.Mutex(false, appLabel, out createdNew);
            return createdNew;
        }

    }
}
