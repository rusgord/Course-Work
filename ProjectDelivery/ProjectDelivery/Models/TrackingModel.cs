using ProjectDelivery.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectDelivery.Models
{
    public class TrackingModel
    {
        [Required]
        public int Id { get; set; }
        public bool IsFind { get; set; }
        public string? Text { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? City { get; set; }
    }
}
