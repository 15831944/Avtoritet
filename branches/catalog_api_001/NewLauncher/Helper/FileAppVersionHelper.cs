namespace NewLauncher.Helper
{
    using NewLauncher.Entities;
    using Newtonsoft.Json;
    using System;
    using System.IO;

    public static class FileAppVersionHelper
    {
        //public static void CreateDirectoryIfNotExist(string dirPath)
        //{
        //    if (!Directory.Exists(dirPath))
        //    {
        //        Directory.CreateDirectory(dirPath);
        //    }
        //}

        public static void CreateFileIfNotExist(string filePath)
        {
            if (!File.Exists(filePath))
            {
                VersionEntity versionEntity = new VersionEntity {
                    Version = "1.0.0.0"
                };
                WriteFile(File.Create(filePath), versionEntity);
            }
        }

        //public static string OpenFile(string filePath)
        //{
        //    string str;
        //    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        //    {
        //        using (StreamReader reader = new StreamReader(stream))
        //        {
        //            str = reader.ReadToEnd();
        //        }
        //    }
        //    return str;
        //}

        public static void UpdateFile(string filePath, string version)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                VersionEntity entity;
                using (StreamReader reader = new StreamReader(stream))
                {
                    entity = JsonConvert.DeserializeObject<VersionEntity>(reader.ReadToEnd());
                    entity.Version = version;
                }
                using (StreamWriter writer = new StreamWriter(stream.Name, false))
                {
                    writer.Write(JsonConvert.SerializeObject(entity));
                }
            }
        }

        private static void WriteFile(Stream fileStream, VersionEntity versionEntity)
        {
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(JsonConvert.SerializeObject(versionEntity));
            }
            fileStream.Close();
        }
    }
}

