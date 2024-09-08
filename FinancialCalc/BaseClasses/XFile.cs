using System.IO;

namespace FinancialCalc.BaseClasses
{
    public static class XFile
    {
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public static bool SaveToFile(string data, string filePath, bool overwrite = false)
        {
            bool fileExists = FileExists(filePath);
            if (fileExists && overwrite is false)
            {
                return UpdateFile(data, filePath);
            }

            try
            {
                using (FileStream fileStream = File.Create(filePath))
                {
                    using StreamWriter streamWriter = new StreamWriter(fileStream);
                    streamWriter.Write(data);
                    streamWriter.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool UpdateFile(string data, string filePath)
        {
            if (!FileExists(filePath))
            {
                return false;
            }

            try
            {
                using (FileStream fileStream = File.Open(filePath, FileMode.Append, FileAccess.Write))
                {
                    using StreamWriter streamWriter = new StreamWriter(fileStream);
                    streamWriter.Write(data);
                    streamWriter.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ReadFile(string filePath, out string data)
        {
            data = null;
            if (File.Exists(filePath) is false)
            {
                return false;
            }

            try
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    using StreamReader streamReader = new(fileStream);
                    data = streamReader.ReadToEnd();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
