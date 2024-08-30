using FinancialCalc.Constants;
using System;
using System.Globalization;
using System.Text;

namespace FinancialCalc.Helpers
{
    public class PathHelper
    {
        private readonly ProjectConstants constants;

        public PathHelper(ProjectConstants constants)
        {
            this.constants = constants;
        }
        public static string GetDesktopPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        public string GetCurrentFullFilePath()
        {
            var desktopPath = GetDesktopPath();
            var dateStamp = GetDateStamp();
            var fullPath = $"{desktopPath}{dateStamp}.{constants.JsonExtension}";
            return fullPath;
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

    }
}
