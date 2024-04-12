using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestTask.Interfaces;
using TestTask.Models;
using TestTask.Utils;

namespace TestTask.Services
{
    public class FileManager : IFileManager
    {
        /// <summary>
        /// Чтение данных из указонного файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        public List<LogEntry> Read(string path)
        {
            var result = new List<LogEntry>();
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("[INFORMATION] Путь к файлу не указан.");
                return result;
            }
            try
            {
                using var streamReader = File.OpenText(path);
                while (streamReader.ReadLine() is { } data)
                {
                    var ipAddress = ExtractDataHelper.ExtractIpAddress(data);
                    var dateTime = ExtractDataHelper.ExtractDate(data);
                    if (ipAddress == null && dateTime == null)
                        continue;

                    var log = new LogEntry
                    {
                        IpAddress = ipAddress,
                        Date = dateTime.Value
                    };
                    result.Add(log);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"[ERROR] {e.Message}");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"[ERROR] {e.Message}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"[ERROR] Файл не может быть прочитан: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Файл не может быть прочитан: {e.Message}");
            }

            return result;
        }

        /// <summary>
        /// Запись данных в указаный файл 
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="data">Данный для записи</param>
        public void Write(string path, List<string> data)
        {
            if (!data.Any())
            {
                Console.WriteLine("[INFORMATION] Нет даных для записи.");
                return;
            }

            try
            {
                using var outputFile = new StreamWriter(path, true);
                foreach (var address in data)
                {
                    outputFile.WriteLine(address);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"[ERROR] {e.Message}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"[ERROR] Данные не могут быть записаны в файл: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Данные не могут быть записаны в файл: {e.Message}");
            }
        }
    }
}