using PublicApiExtension.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApiExtension.Services.Services.PublicHolidays
{
    public interface IPublicHolidayService
    {
        Task<List<Holiday>> GetHolidaysInRange(DateTime start, DateTime end, CancellationToken cancellationToken);
    }
}
