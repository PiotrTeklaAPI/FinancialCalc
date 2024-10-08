using System;
using System.IO;

namespace FinancialCalc.Services
{
    public class FileWatcherService
    {
        private readonly FileSystemWatcher fileWatcher;

        public event EventHandler<FileSystemEventArgs> FileChanged;

        public FileWatcherService(string path, string filter)
        {
            fileWatcher = new FileSystemWatcher
            {
                Path = path,
                Filter = filter,
                InternalBufferSize = 32768,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            fileWatcher.Created += OnFileChanged;
            fileWatcher.Deleted += OnFileChanged;
            fileWatcher.Renamed += OnFileChanged;
            fileWatcher.EnableRaisingEvents = true;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            FileChanged?.Invoke(this, e);
        }
    }
}
