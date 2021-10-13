using System;
using System.ComponentModel.DataAnnotations;

namespace PublicApiExtension.Models.Events
{
    public class EventWriteModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
