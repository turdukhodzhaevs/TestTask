using System.Collections.Generic;
using TestTask.Models;

namespace TestTask.Interfaces
{
    public interface IFileManager
    {
        List<LogEntry> Read(string path);
        void Write(string path, List<string> data);
    }
}