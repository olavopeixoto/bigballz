using System.Collections.Specialized;

namespace BigBallz.Core.Configuration
{
    public interface IConfigurationManager
    {
        NameValueCollection AppSettings
        {
            get;
        }

        string ConnectionStrings(string name);

        T GetSection<T>(string sectionName);
    }
}
