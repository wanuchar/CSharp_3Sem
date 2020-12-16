using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BDModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DataManager
{
    public class DataManager
    {
        private readonly DataOptions configOptions;
        private readonly DataInsights insights;
        public DataManager(DataInsights insights, DataOptions configOptions)
        {
            this.insights = insights;
            this.configOptions = configOptions;
        }

        public void Transfer()
        {
            string connectionString = configOptions.ConnectionString;
            DataAdventure dbAdventure = new DataAdventure(connectionString);
            string personPath = AppDomain.CurrentDomain.BaseDirectory;
            personPath = Path.Combine(personPath.Substring(0, personPath.Length - 20), "Person");
            DirectoryInfo personFolder = new DirectoryInfo(personPath);
            if (!personFolder.Exists)
            {
                personFolder.Create();
            }
            int count = dbAdventure.GetId(insights);
            for (int begin = 0, end = 500, idName = 1; begin < count; begin += 500, end += 500, idName++)
            {
                string fileName = $"Person_{idName}.xml";
                string fullFileName = Path.Combine(personPath, fileName);
                dbAdventure.GetPerson(begin, end, fullFileName, insights);
                File.Copy(fullFileName, Path.Combine(configOptions.SourceDirectory, fileName));
                File.Copy(Path.ChangeExtension(fullFileName, "xsd"),
                            Path.Combine(configOptions.SourceDirectory, Path.ChangeExtension(fileName, "xsd")));
            }
            insights.AddAction("Success: Main. Data transfer to DataManager", DateTime.Now);
        }

    }
}
