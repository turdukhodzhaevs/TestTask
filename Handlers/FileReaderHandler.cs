using System;
using System.Linq;
using TestTask.Interfaces;
using TestTask.Services;

namespace TestTask.Handlers
{
    public class FileReaderHandler : IHandler
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
            Console.WriteLine("[INFORMATION]: Чтение данных из файла.");
            var readResult = _fileManager.Read(request.CommandLineParameters.LogFilePath);
            if (!readResult.Any())
                return null;
            request.SetEntryLog(readResult);
            return _nextHandler?.HandleRequest(request);
        }
    }
}