using System;

namespace FileWatcherService
{
    public class WatcherOptions
    {
        public StorageOptions StorageOptions { get; set; }
        public ArchiveOptions ArchiveOptions { get; set; }
        public CryptOptions CryptOptions { get; set; }
    }

    public class StorageOptions
    {
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
    }

    public class ArchiveOptions
    {
        public string ArchiveName { get; set; }
    }

    public class CryptOptions
    {
        public string CryptKey { get; set; }
    }
}
