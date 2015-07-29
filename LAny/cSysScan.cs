using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace LAny
{
    public class cSysScan
    {
        public static string m_hostName = null;
        public int m_portBeg = 0;
        public int m_portEnd = 0;
        public int m_timeOut = 0;
        public string m_ProtocolType = null;

        public void initialisation(
            string hostName, 
            int portBeg, 
            int portEnd, 
            int timeOut)
        {
            m_hostName = getHostName(hostName);
            m_portBeg = portBeg;
            m_portEnd = portEnd;
            m_timeOut = timeOut;
        }


        /// <summary>
        /// Подключение сокет  к порту  по номеру (int port)
        /// тип протокола TCP
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Socket connectSocket(int port)
        {
            return (connectSocket(port, ProtocolType.Tcp));
        }
        

        /// <summary>
        /// Подключение сокет  к порту  по номеру (int port)
        /// тип протокола определяется параметром  (ProtocolType val)
        /// Пример значения val ProtocolType.Tcp
        /// </summary>
        /// <param name="port"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Socket connectSocket(
            int port, 
            ProtocolType val)
        {
            Socket s = null;
            IPHostEntry iphe = null;

            try
            {
                
                //iphe = Dns.Resolve(m_hostName);
                iphe = Dns.GetHostEntry(m_hostName);

                foreach (IPAddress ipad in iphe.AddressList)
                {
                    IPEndPoint ipe = new IPEndPoint(ipad, port);

                    Socket tmpS = new Socket(ipe.AddressFamily, SocketType.Stream, val);

                    tmpS.Connect(ipe);

                    if (tmpS.Connected)
                    {
                        s = tmpS;
                        break;
                    }
                    else
                        continue;
                }
            }
            catch { }

            return s;
        }


        public static bool socketOpen(int port, ProtocolType val)
        {
            Socket s = connectSocket(port, val);

            if (s == null)
                return false;
            else
                return true;
        }


        public static bool socketOpen(int port)
        {
            Socket s = connectSocket(port, ProtocolType.Tcp);

            if (s == null)
                return false;
            else
                return true;
        }


        public static string getIp(string host)
        {
            string str = null;
            try
            {
                // Получение ip-адреса.
                //IPAddress ip = Dns.GetHostByName(host).AddressList[0];
                IPAddress ip = Dns.GetHostEntry(host).AddressList[0];
                
                str = ip.ToString();
            }
            catch
            {
                str = host;
            }

            return str;
        }


        public static string getHostName(string host)
        {
            string str = null;

            try
            {
                // Получение имени хоста
                //str = Dns.GetHostByAddress(host).HostName;
                str = Dns.GetHostEntry(host).HostName;
                
            }
            catch
            {
                str = host;
            }

            return str;
        }

    }
}
