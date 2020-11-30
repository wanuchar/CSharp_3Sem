using System;
using System.IO;
using System.Text;
using System.IO.Compression;

namespace FileWatcherService
{
    public static class WatcherOperations
    {
        public static void Move(string compPath, string targetPath)
        {
            File.Move(compPath, targetPath);
            File.Delete(compPath);
        }

        //Архивация
        public static void Compress(string sourceF, string compressedF)
        {
            //поток для чтения исходного файла
            using (FileStream sourceStream = new FileStream(sourceF, FileMode.OpenOrCreate))
            {
                //поток для записи в сжатый файл
                using (FileStream targetStream = File.Create(compressedF))
                {
                    //поток архивации
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        //Деархивация
        public static void Decompress(string compressedF, string targetF)
        {
            //для чтения из сжатого файла
            using (FileStream sourceStream = new FileStream(compressedF, FileMode.OpenOrCreate))
            {
                //поток для записи восстановленного файла
                using (FileStream targetStream = File.Create(targetF))
                {
                    //поток разархивации
                    using (GZipStream decStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decStream.CopyTo(targetStream);
                    }
                }
            }
        }

        //Шифрование(дешифрование)
        public static string Encryption(string FilePath, bool mode, string key)
        {
            if (key == "key123")
            {
                StringBuilder temp = new StringBuilder(FilePath);
                if (!mode)
                {
                    temp.Replace(".crypt", ".txt");
                }
                byte[] curFile = File.ReadAllBytes(FilePath);
                byte[] newFile = Crypt(curFile);
                if (mode)
                {
                    temp.Replace(".txt", ".crypt");
                }
                File.WriteAllBytes(temp.ToString(), newFile);
                return temp.ToString();
            }
            else
            {
                return FilePath;
            }
            
        }

        //шифр
        private static byte[] Crypt(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] ^= 1;
            return bytes;
        }

        //добавление в архив
        public static void Archiving(string filePath, string archive)
        {
            try
            {
                using (ZipArchive zipArchive = ZipFile.Open(archive, ZipArchiveMode.Update))
                {
                    zipArchive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
                }
            }
            catch (Exception ex)
            {

                using (var streamWriter = new StreamWriter(
                       Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorMessage.txt"),
                       true, Encoding.Default))
                {
                    streamWriter.WriteLine("File archiving error: " + ex.Message);
                }
            }
        }
    }
}
