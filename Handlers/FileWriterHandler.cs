using System;
using System.Linq;
using TestTask.Interfaces;
using TestTask.Services;

namespace TestTask.Handlers
{
    public class FileWriterHandler : IHandler
    {
        private IHandler _nextHandler;
        private readonly IFileManager _fileManager = new FileManager();

        public IHandler SetNext(IHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public ExecutionContext HandleRequest(ExecutionContext request)
        {
            if (string.IsNullOrWhiteSpace(request.CommandLineParameters.OutputFilePath))
            {
                Console.WriteLine("[ERROR]: Не указан путь к файлу записти.");
                return null;
            }

            if (!request.LogToWrite.Any())
            {
                Console.WriteLine("[INFORMATION]: Данные для записи пусты, запись не будет выполнен.");
                return null;
            }

            _fileManager.Write(request.CommandLineParameters.OutputFilePath, request.LogToWrite);
            Console.WriteLine("[INFORMATION]: Запись данных в файл.");
            return _nextHandler?.HandleRequest(request);
        }
    }
}