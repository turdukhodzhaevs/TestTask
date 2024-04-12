using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TestTask.Models;
using TestTask.Utils;

namespace TestTask.Services
{
    public class ExecutionContext
    {
        public List<string> LogToWrite { get; private set; }
        public LogFilter LogFilter { get; private set; }
        public List<LogEntry> LogEntryList { get; private set; }
        public CommandLineParameters CommandLineParameters { get; private set; }

        public ExecutionContext SetArguments(string[] args, bool fromConfigFile = false)
        {
            if (fromConfigFile)
            {
                // Получение пути к директории, где находится исполняемый файл
                var directory = AppDomain.CurrentDomain.BaseDirectory;
                if (string.IsNullOrWhiteSpace(directory))
                {
                    Console.WriteLine("[ERROR]: Не удалось получить путь к корневому катологу.");
                    return this;
                }

                var json = File.ReadAllText(Path.Combine(directory, "appsettings.json"));
                var settings = JsonConvert.DeserializeObject<CommandLineParameters>(json);

                // Конкатенация пути к файлу
                var filePath = Path.Combine(directory, settings.LogFilePath);
                var outputFilePath = Path.Combine(directory, settings.OutputFilePath);

                CommandLineParameters = new CommandLineParameters
                {
                    LogFilePath = filePath,
                    OutputFilePath = outputFilePath,
                    FilterParameters = new FilterParameters
                    {
                        StartTime = settings.FilterParameters.StartTime,
                        EndTime = settings.FilterParameters.EndTime,
                        AddressStart = settings.FilterParameters.AddressStart,
                        AddressMask = settings.FilterParameters.AddressMask
                    }
                };
                Console.WriteLine("[INFORMATION]: Параметры из файла конфигурации установлены.");
                return this;
            }

            CommandLineParameters = new CommandLineParameters
            {
                LogFilePath = args.ExtractParameterValue("--file-log"),
                OutputFilePath = args.ExtractParameterValue("--file-output"),
                FilterParameters = new FilterParameters
                {
                    StartTime = args.ExtractParameterValue("--time-start"),
                    EndTime = args.ExtractParameterValue("--time-end"),
                    AddressStart = args.ExtractParameterValue("--address-start"),
                    AddressMask = args.ExtractParameterValue("--address-mask")
                }
            };
            Console.WriteLine("[INFORMATION]: Параметры из командной строки установлены.");
            return this;
        }

        public ExecutionContext SetEntryLog(List<LogEntry> logEntryList)
        {
            LogEntryList = logEntryList;
            Console.WriteLine("[INFORMATION]: Логи из файла установлены.");
            return this;
        }

        public ExecutionContext SetLogToWrite(List<string> logToWrite)
        {
            LogToWrite = logToWrite;
            Console.WriteLine("[INFORMATION]: Логи для записи в файл установлены.");
            return this;
        }

        public ExecutionContext SetFilterParameters()
        {
            LogFilter = new LogFilter
            {
                StartDate = ExtractDataHelper.ParseToDate(CommandLineParameters.FilterParameters.StartTime),
                EndDate = ExtractDataHelper.ParseToDate(CommandLineParameters.FilterParameters.EndTime),
                AddressStart = ExtractDataHelper.ParseToIpAddress(CommandLineParameters.FilterParameters.AddressStart),
                AddressMask = ExtractDataHelper.ParseToIpAddress(CommandLineParameters.FilterParameters.AddressMask)
            };
            Console.WriteLine("[INFORMATION]: Параметры для фильтрация лога установлены.");

            return this;
        }
    }
}