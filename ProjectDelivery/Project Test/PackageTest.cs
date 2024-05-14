using ProjectDelivery.Models;
using ProjectDelivery.Enums;

namespace Project_Test
{
    public class PackageTest
    {
        [Fact]
        public void PackageModel()
        {
            var package = new Package
            {
                Id = 1,
                TypePayment = true,
                Weight = 10,
                Price = 10,
                Phone = "0998953467",
                City = EnumCities.Dnipro
            };

            Assert.Equal(1, package.Id);
            Assert.Equal(true, package.TypePayment);
            Assert.Equal(10, package.Weight);
            Assert.Equal(10, package.Price);
            Assert.Equal("0998953467", package.Phone);
            Assert.Equal(EnumCities.Dnipro, package.City);
        }
        [Fact]
        public void PackageTrackingModel() 
        {
            var track_package = new TrackingModel
            {
                Id = 1,
                IsFind = true
            };
            Assert.Equal(1, track_package.Id);
            Assert.Equal(true, track_package.IsFind);
        }
    }
}