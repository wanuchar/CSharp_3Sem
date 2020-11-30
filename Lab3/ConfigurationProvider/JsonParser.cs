using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;

namespace ConfigurationProvider
{
    public class JsonParser<T> : IConfigurationParser<T> where T : class
    {
        private readonly string jsonPath;

        public JsonParser(string jsonPath)
        {
            this.jsonPath = jsonPath;
        }

        public T Parse()
        {
            using (var fileStream = new FileStream(jsonPath, FileMode.OpenOrCreate))
            {
                using (var document = JsonDocument.Parse(fileStream))
                {
                    var element = document.RootElement;

                    if (typeof(T).GetProperties().First().Name
                        != element.EnumerateObject().First().Name)
                    {
                        element = element.GetProperty(typeof(T).Name);
                    }

                    try
                    {
                        return JsonSerializer.Deserialize<T>(element.GetRawText());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("JSON file desedeserialization impossible: " + ex.Message);   
                    }
                }
            }
        }
    }
}
