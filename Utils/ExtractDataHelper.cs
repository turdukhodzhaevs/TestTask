using System;
using System.Net;
using System.Text.RegularExpressions;

namespace TestTask.Utils
{
    public static class ExtractDataHelper
    {
        private const string Pattern =
            @"(?<ip>\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}):(?<datetime>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})";

        public static string ExtractParameterValue(this string[] args, string key)
        {
            var index = Array.FindIndex(args, x => x.Equals(key));
            if (index < 0)
                return string.Empty;
            if (index + 1 > args.Length - 1)
                return string.Empty;

            return args[index + 1];
        }

        public static DateTime? ParseToDate(string param)
        {
            if (DateTime.TryParse(param, out var date)) return date;
            Console.WriteLine($"[ERROR] Некорректный формат времени: {param}. Пропускается.");
            return null;
        }

        public static IPAddress ParseToIpAddress(string param)
        {
            if (IPAddress.TryParse(param, out var addressStart)) return addressStart;
            Console.WriteLine("[ERROR] Некорректный формат IP-адреса для address-start.");
            return null;
        }

        public static DateTime? ExtractDate(string param)
        {
            var match = Regex.Match(param, Pattern);

            if (match.Success)
            {
                var dateTimeString = match.Groups["datetime"].Value;
                if (DateTime.TryParse(dateTimeString, out var date)) return date;
                Console.WriteLine($"[ERROR] Некорректный формат времени: {param}. Пропускается.");
                return null;
            }

            return null;
        }

        public static IPAddress ExtractIpAddress(string param)
        {
            var match = Regex.Match(param, Pattern);

            if (match.Success)
            {
                string ipAddress = match.Groups["ip"].Value;
                if (IPAddress.TryParse(ipAddress, out var addressStart)) return addressStart;
                Console.WriteLine("[ERROR] Некорректный формат IP-адреса для address-start.");
                return null;
            }

            return null;
        }

        public static int? ExtractAddressMask(string param)
        {
            if (int.TryParse(param, out var mask)) return mask;
            Console.WriteLine("[ERROR] Некорректный формат маски подсети для address-mask.");
            return null;
        }
    }
}