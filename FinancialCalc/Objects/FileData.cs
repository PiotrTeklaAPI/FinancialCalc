using System.Collections.Generic;

namespace FinancialCalc.Objects
{
    public class FileData
    {
        public FileInfo FileInfo { get; set; }

        public List<Product> Products { get; set; }
    }
}
