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
        public static string GetAppSettings(Configuration configuration, string key)
        {
            string res = string.Empty;
            if (configuration.AppSettings.Settings[key] != null)
                res = configuration.AppSettings.Settings[key].Value;
            return (res);
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
            configuration.AppSettings.SectionInformation.ForceSave = true;
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

    }
}

