using System;

namespace BDModel
{
    public class DataOptions
    {
        public string ConnectionString { get; set; }
        public string LoggerConnectionString { get; set; }
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
    }
}
