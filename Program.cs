using System;
using TestTask.Handlers;
using TestTask.Services;

namespace TestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new ExecutionContext()
                .SetArguments(args, true);

            var fileReaderHandler = new FileReaderHandler();
            var logFilterHandler = new LogFilterHandler();
            var fileWriterHandler = new FileWriterHandler();

            fileReaderHandler
                .SetNext(logFilterHandler)
                .SetNext(fileWriterHandler);

            fileReaderHandler.HandleRequest(context);

            Console.WriteLine("[INFORMATION] Анализ завершен. Результаты записываются в файл вывода.");
        }
    }
}