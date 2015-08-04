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

        public static void UpdateAppSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }

    }
}
