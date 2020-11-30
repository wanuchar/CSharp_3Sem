using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ConfigurationProvider;

namespace FileWatcherService
{
    public class Worker : BackgroundService
    {
        Logger logger;

        public Worker()
        {
            InitializeComponent();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            logger = new Logger();
            Thread loggerStart = new Thread(new ThreadStart(logger.Start));
            loggerStart.Start();
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.Stop();
            Thread.Sleep(1000);
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            if (components != null)
            {
                components.Dispose();
            }
            base.Dispose();
        }

        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
    }


 

    class Logger
    {
        private readonly WatcherOptions configOptions;
        FileSystemWatcher watcher, watcherTarget;
        object obj = new object();
        bool enabled = true;
        public Logger()
        {
            var optionsManager = new OptionsManager("/Users/user/Documents/ISP/3 sem/Lab3/FileWatcher/FileWatcher/bin");
            configOptions = optionsManager.GetOptions<WatcherOptions>();

            watcher = new FileSystemWatcher(configOptions.StorageOptions.SourceDirectory);
            watcherTarget = new FileSystemWatcher(configOptions.StorageOptions.TargetDirectory);

            watcher.Deleted += Watcher_Deleted;
            watcher.Created += Watcher_Created;
            watcher.Changed += Watcher_Changed;
            watcher.Renamed += Watcher_Renamed;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            watcherTarget.EnableRaisingEvents = true;
            while (enabled)
            {
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            watcherTarget.EnableRaisingEvents = false;
            enabled = false;
        }

        //rename
        public void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "Rename in " + e.FullPath;
            string filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath);
        }

        public void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "Changed";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        public void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "Created";
            string filePath = e.FullPath;

            if (filePath.Contains(".gz") || filePath.Contains(".crypt"))
            {
                return;
            }

            string temp = WatcherOperations.Encryption(filePath, true, configOptions.CryptOptions.CryptKey);
            File.Delete(filePath);

            filePath = temp;

            RecordEntry(fileEvent, filePath);


            string compPath = Path.ChangeExtension(filePath, "gz");
            
            WatcherOperations.Compress(filePath, compPath);
            File.Delete(filePath);

            string targetPath = configOptions.StorageOptions.TargetDirectory;
            string[] words = compPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            targetPath += "/" + words[words.Length - 1];
            
            WatcherOperations.Move(compPath, targetPath);

            filePath = targetPath;
            targetPath = Path.ChangeExtension(targetPath, ".crypt");

            WatcherOperations.Decompress(filePath, targetPath);
            temp = WatcherOperations.Encryption(targetPath, false, configOptions.CryptOptions.CryptKey);
            WatcherOperations.Archiving(temp, configOptions.ArchiveOptions.ArchiveName);


            File.Delete(filePath);
            File.Delete(targetPath);
        }

        public void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "Deleted";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }


        //Запись в лог файл
        private void RecordEntry(string fileEvent, string filePath)
        {
            lock (obj)
            {
                using (StreamWriter sw = new StreamWriter("/Users/user/Documents/ISP/3 sem/Lab2/DocLog.txt", true))
                {

                    if (filePath.Contains(".DS_Store"))
                    {
                        return;
                    }
                    sw.WriteLine(String.Format("{0} file {1} was {2}", DateTime.Now.ToString("dd / MM / yyyy HH: mm:ss"), filePath, fileEvent));
                    sw.Flush();
                }
            }
        }
    }
}
