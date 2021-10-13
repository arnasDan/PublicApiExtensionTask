using PublicApiExtension.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApiExtension.Services.Services.PublicHolidays
{
    public interface IHolidayProvider
    {
        Task<IEnumerable<Holiday>> GetHolidays(string countryCode, int year, CancellationToken cancellationToken);
    }
}
