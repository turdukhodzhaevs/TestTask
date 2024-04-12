using System;
using System.Net;

namespace TestTask.Models
{
    public class LogFilter
    {
      
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IPAddress AddressStart { get; set; }
        public IPAddress AddressMask { get; set; }
    }
}