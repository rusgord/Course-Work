using ProjectDelivery.Enums;

namespace ProjectDelivery.Models
{
    public class CalcModel
    {
        public double Weight { get; set; }
        public double Cost { get; set; }
        public EnumCities City { get; set; }
        public double Price { get; set; } = 0;
    }
}
