using System;
using System.Net;

namespace TestTask.Models
{
    public class LogEntry
    {
        public IPAddress IpAddress { get; set; }
        public DateTime? Date { get; set; }
    }
}