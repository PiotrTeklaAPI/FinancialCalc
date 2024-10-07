using FinancialCalc.Constants;
using System;
using System.Globalization;
using System.Text;

namespace FinancialCalc.Helpers
{
    public class PathHelper
    {
        private readonly ProjectConstants constants;
        private const string DataFolder = "\\FinancialCalcData";

        public PathHelper(ProjectConstants constants)
        {
            this.constants = constants;
        }
        public string GetDataFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + DataFolder;
        }

        public string GetCurrentFullFilePath()
        {
            var desktopPath = GetDataFolderPath();
            var dateStamp = GetDateStamp();
            var fullPath = $"{desktopPath}\\{dateStamp}{constants.JsonExtension}";
            return fullPath;
        }

        public string GetCurrentFileName()
        {
            return GetDateStamp().ToString();
        }

        private string GetCurrentMonth()
        {
            return new DateTimeFormatInfo().GetMonthName(DateTime.Now.Month);
        }

        private string GetDateStamp()
        {
            StringBuilder stringBuilder = new StringBuilder(GetCurrentMonth());
            stringBuilder.Append(GetCurrentYear());
            return stringBuilder.ToString();
        }

        private int GetCurrentYear()
        {
            return DateTime.Now.Year;
        }

        public string GetFilePath(string fileName)
        {
            return $"{GetDataFolderPath()}\\{fileName}";
        }
    }
}
