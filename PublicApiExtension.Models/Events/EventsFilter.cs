using System;

namespace PublicApiExtension.Models.Events
{
    public class EventsFilter
    {
        public string Name { get; set; }
        public DateTime? StartsBefore { get; set; }
        public DateTime? EndsAfter { get; set; }
    }
}
