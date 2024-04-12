using System;
using System.Linq;
using TestTask.Interfaces;
using TestTask.Services;

namespace TestTask.Handlers
{
    public class LogFilterHandler : IHandler
    {
        private IHandler _nextHandler;

        public IHandler SetNext(IHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public ExecutionContext HandleRequest(ExecutionContext request)
        {
            if (request.CommandLineParameters.FilterParameters == null)
                return null;
            
            request.SetFilterParameters();

            Console.WriteLine("[INFORMATION]: Фильтрация лога.");
            var filterResult = request.LogEntryList.Filter(request.LogFilter);
            if (!filterResult.Any()) return null;
            request.SetLogToWrite(filterResult);

            return _nextHandler?.HandleRequest(request);
        }
    }
}