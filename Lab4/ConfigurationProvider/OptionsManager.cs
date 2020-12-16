using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ConfigurationProvider
{
    public class OptionsManager
    {
        private readonly string path = null;

        public OptionsManager(string path)
        {
            if (File.Exists(path))
            {
                this.path = (Path.GetExtension(path) == ".xml"
                    || Path.GetExtension(path) == ".json") ? path : null;
            }
            else if (Directory.Exists(path))
            {
                var fileEntries = from file in Directory.GetFiles(path) where
                                Path.GetExtension(file) == ".xml" ||
                                Path.GetExtension(file) == ".json" select file;
                this.path = fileEntries.Count() != 0 ? fileEntries.First() : null;
            }
        }

        public T GetOptions<T>() where T : class
        {
            try
            {
                if (path == null)
                {
                    throw new Exception("Error: Configuration file not found.");
                }
            }
            catch
            {
                throw;
            }

            var provider = new Provider<T>(path);
            return provider.configurationParser.Parse();
        }
    }
}
