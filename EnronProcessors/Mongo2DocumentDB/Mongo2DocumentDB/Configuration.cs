using System.Configuration;

namespace Mongo2DocumentDB
{
    public class Configuration
    {
        public Configuration()
        {
        }

        public int BatchSize
        {
            get
            {
                int batchSize;
                if (int.TryParse(ConfigurationManager.AppSettings.Get("BatchSize"), out batchSize))
                    return batchSize;               

                return 10;
            }
        }

        public string MailBoxMatchPattern => ConfigurationManager.AppSettings.Get("MailBoxMatchPattern");
    }
}