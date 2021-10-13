using System;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApiExtension.Services.Services.PublicHolidays
{
    public interface IPublicHolidayService
    {
        Task<bool> IsDateHoliday(DateTime date, CancellationToken cancellationToken);
        Task<bool> DoesDateRangeContainHoliday(DateTime start, DateTime end, CancellationToken cancellationToken);
    }
}
