using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using ConfigurationProvider;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace TIPOLABA
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Data configOptions;
            var optionsManager = new OptionsManager(AppDomain.CurrentDomain.BaseDirectory);
            configOptions = optionsManager.GetOptions<Data>();
            string loggerConnectionString = configOptions.LoggerConnectionString;
            DBApplicationInsights insights = new DBApplicationInsights(loggerConnectionString);
            try
            {
                insights.ClearAction();
                insights.AddAction("Clear ApplicationInsigths table", DateTime.Now);
                DataManager dataManager = new DataManager(insights, configOptions);
                dataManager.Transfer();
            }
            catch (Exception e)
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorFile.txt"), true))
                {
                    sw.WriteLine($"Ошибка в методе Main:{e.Message} \t {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                }
                insights.AddAction("Error: the ApplicationInsigths table is not cleared", DateTime.Now);
            }
        }
    }
}
