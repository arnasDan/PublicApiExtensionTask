using PublicApiExtension.Clients.NagerDate.Client;
using PublicApiExtension.Models;
using PublicApiExtension.Services.Services.PublicHolidays;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApiExtension.Clients.NagerDate
{
    public class NagerDateHolidayProvider : IHolidayProvider
    {
        private readonly INagerDateClient _nagerDateClient;

        public NagerDateHolidayProvider(INagerDateClient nagerDateClient)
        {
            _nagerDateClient = nagerDateClient;
        }

        public async Task<IEnumerable<Holiday>> GetHolidays(string countryCode, int year, CancellationToken cancellationToken)
        {
            var holidays = await _nagerDateClient.GetHolidays(countryCode, year, cancellationToken);

            return holidays.Select(h => h.ToHoliday());
        }
    }
}
