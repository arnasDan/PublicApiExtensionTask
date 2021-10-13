using PublicApiExtension.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApiExtension.Services.Services.PublicHolidays
{
    public class PublicHolidayService : IPublicHolidayService
    {
        private readonly ConcurrentDictionary<int, IEnumerable<Holiday>> _holidays = new();
        private readonly IHolidayProvider _holidayProvider;
        private readonly string _countryCode;

        public PublicHolidayService(IHolidayProvider holidayProvider, string countryCode)
        {
            _holidayProvider = holidayProvider;
            _countryCode = countryCode;
        }

        public async Task<List<Holiday>> GetHolidaysInRange(DateTime start, DateTime end, CancellationToken cancellationToken)
        {
            start = start.Date;
            end = end.Date;

            if (end < start)
                throw new ArgumentException("End date must be after start date", nameof(end));

            var holidays = (await Task.WhenAll(Enumerable
                .Range(start.Year, end.Year - start.Year)
                .Select(async year => await GetHolidays(year, cancellationToken))))
                .SelectMany(h => h);

            return holidays
                .Where(h => h.Date >= start.Date && h.Date <= end.Date)
                .ToList();
        }

        private async Task<IEnumerable<Holiday>> GetHolidays(int year, CancellationToken cancellationToken)
        {
            if (_holidays.TryGetValue(year, out var holidays))
                return holidays;

            holidays = await _holidayProvider.GetHolidays(_countryCode, year, cancellationToken);

            _holidays[year] = holidays;

            return holidays;
        } 
    }
}
