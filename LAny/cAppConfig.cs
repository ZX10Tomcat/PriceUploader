using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LAny
{
    public class cAppConfig
    {
        public static string GetAppSettings(string key)
        {
            System.Configuration.AppSettingsReader cas = new System.Configuration.AppSettingsReader();
            return (cas.GetValue(key, typeof(string)).ToString());
        }

        /// <summary>
        /// Sample 
        /// Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void UpdateAppSetting(Configuration configuration, string key, string value)
        {
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }

    }
}
