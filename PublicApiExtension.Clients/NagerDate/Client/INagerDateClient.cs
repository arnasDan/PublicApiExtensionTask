using PublicApiExtension.Clients.NagerDate.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApiExtension.Clients.NagerDate.Client
{
    public interface INagerDateClient
    {
        Task<List<NagerDateHoliday>> GetHolidays(string countryCode, int year, CancellationToken cancellationToken);
    }
}
