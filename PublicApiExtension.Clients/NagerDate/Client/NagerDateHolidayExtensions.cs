using PublicApiExtension.Clients.NagerDate.Models;
using PublicApiExtension.Models;

namespace PublicApiExtension.Clients.NagerDate.Client
{
    public static class NagerDateHolidayExtensions
    {
        public static Holiday ToHoliday(this NagerDateHoliday holiday)
        {
            return new Holiday(holiday.Date, holiday.Name);
        }
    }
}
