using Newtonsoft.Json;
using System;

namespace FinancialCalc.Objects
{
    public class FileInfo
    {
        [JsonConstructor]
        public FileInfo() { }

        public FileInfo(string Path,  string Name, DateTime LastModification)
        {
            this.Name = Name;
            this.Path = Path;
            this.LastModification = LastModification;
        }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Path { get; set; }

        [JsonProperty]
        public DateTime LastModification { get; set; }
    }
}
