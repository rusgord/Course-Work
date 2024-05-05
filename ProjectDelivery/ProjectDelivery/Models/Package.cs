using ProjectDelivery.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectDelivery.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool TypePayment { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        public int Price { get; set; }
        public string? recipientName { get; set; }
        public string? senderName { get; set; }
        [Required]
        public string Phone { get; set; }
        public EnumCities City { get; set; }
        public string? SenderId { get; set; } = null;
        public string? RecipientId { get; set; } = null;
    }
}
