using PublicApiExtension.Clients.Clients.NagerDate;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PublicApiExtension.Services.PublicHolidays
{
    public class PublicHolidayService
    {
        private readonly ConcurrentDictionary<(int Year, string CountryCode), List<string>> _holidays = new();
        private readonly INagerDateClient _client;

        public PublicHolidayService(INagerDateClient client)
        {
            _client = client;
        }
    }
}
