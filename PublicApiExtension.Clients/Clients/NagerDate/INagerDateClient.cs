using PublicApiExtension.Clients.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApiExtension.Clients.Clients.NagerDate
{
    public interface INagerDateClient
    {
        Task<List<Holiday>> GetHolidays(string countryCode, int year, CancellationToken cancellationToken);
    }
}
