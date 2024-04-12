using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TestTask.Models;

namespace TestTask.Services
{
    public static class FilterIpAddressService
    {
        public static List<string> Filter(this List<LogEntry> addresses, LogFilter logFilterParam)
        {
            addresses = FilterByDate(addresses, logFilterParam);
            addresses = FilterByStartAddressAndSubMask(addresses, logFilterParam);
            return addresses
                .GroupBy(x => x.IpAddress)
                .Select(r => $"{r.Key}: {r.Count()}")
                .ToList();
        }

        private static List<LogEntry> FilterByDate(this List<LogEntry> addresses,
            LogFilter logFilterParam)
        {
            var filterResult = new List<LogEntry>();

            if (!logFilterParam.StartDate.HasValue || !logFilterParam.EndDate.HasValue)
                return addresses;

            var startDate = logFilterParam.StartDate.Value.Date;
            var endDate = logFilterParam.EndDate.Value.Date;

            foreach (var address in addresses)
            {
                if (address.Date.Value.Date >= startDate && address.Date.Value.Date <= endDate)
                {
                    filterResult.Add(address);
                }
            }

            return filterResult;
        }

        static List<LogEntry> FilterByStartAddressAndSubMask(this List<LogEntry> addresses,
            LogFilter logFilterParam)
        {
            if (logFilterParam.AddressStart == null)
                return addresses;

            // Формируем нижнюю границу диапазона адресов
            var startBytes = logFilterParam.AddressStart.GetAddressBytes();

            // Формируем верхнюю границу диапазона адресов
            IPAddress endAddress = null;
            if (logFilterParam.AddressMask != null)
            {
                var endBytes = new byte[startBytes.Length];
                var maskBytes = logFilterParam.AddressMask.GetAddressBytes();
                for (var i = 0; i < startBytes.Length; i++)
                {
                    endBytes[i] = (byte)(startBytes[i] | ~maskBytes[i]);
                }

                endAddress = new IPAddress(endBytes);
            }


            // Фильтруем список по адресам, находящимся в диапазоне от начального до конечного
            var filterResult = new List<LogEntry>();
            foreach (var ip in addresses)
            {
                if (IsInRange(ip.IpAddress, logFilterParam.AddressStart, endAddress))
                {
                    filterResult.Add(ip);
                }
            }

            return filterResult;
        }

        // Проверка соответствия адреса заданному диапазону
        static bool IsInRange(IPAddress ipAddress, IPAddress startAddress, IPAddress endAddress = null)
        {
            var ipBytes = ipAddress.GetAddressBytes();
            var startBytes = startAddress.GetAddressBytes();
            if (endAddress != null)
            {
                var endBytes = endAddress.GetAddressBytes();
                for (var i = 0; i < ipBytes.Length; i++)
                {
                    if (ipBytes[i] < startBytes[i] || ipBytes[i] > endBytes[i])
                        return false;
                }
            }
            else
            {
                for (var i = 0; i < ipBytes.Length; i++)
                {
                    if (ipBytes[i] < startBytes[i])
                        return false;
                }
            }

            return true;
        }
    }
}