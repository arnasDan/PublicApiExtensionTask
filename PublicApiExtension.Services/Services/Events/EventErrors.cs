using PublicApiExtension.Models;
using System.Collections.Generic;
using System.Linq;

namespace PublicApiExtension.Services.Services.Events
{
    public static class EventErrors
    {
        public static string NotFound => "Event not found";
        public static string InvalidRange => "Invalid range";
        public static string RangeContainsHolidays(IEnumerable<Holiday> holidays) => $"Range contains holidays: {string.Join("; ", holidays.Select(h => h.Name))}";
    }
}
