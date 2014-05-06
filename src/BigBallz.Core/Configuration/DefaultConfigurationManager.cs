using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace BigBallz.Core.Configuration
{
    public class DefaultConfigurationManager : IConfigurationManager
    {
        public NameValueCollection AppSettings
        {
            [DebuggerStepThrough]
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }

        [DebuggerStepThrough]
        public string ConnectionStrings(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        [DebuggerStepThrough]
        public T GetSection<T>(string sectionName)
        {
            return (T)ConfigurationManager.GetSection(sectionName);
        }
    }
}
