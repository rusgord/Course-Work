using ProjectDelivery.Models;
using ProjectDelivery.Enums;
using Microsoft.AspNetCore.Identity;


namespace Project_Test
{
    public class AccountTest
    {
        [Fact]
        public void AccModel()
        {
            var acc = new AccModel
            {
                Name = "Test",
                Roles = EnumRoles.User,
                City = EnumCities.Kharkiv,
                PhoneNumber = "0999644686",
                Email = "user@mail.com",
                PasswordHash = "aljkf834jfdk;fo8w"
            };
            Assert.Equal("Test", acc.Name);
            Assert.Equal(EnumRoles.User, acc.Roles);
            Assert.Equal(EnumCities.Kharkiv, acc.City);
            Assert.Equal("0999644686", acc.PhoneNumber);
            Assert.Equal("user@mail.com", acc.Email);
            Assert.Equal("aljkf834jfdk;fo8w", acc.PasswordHash);
        }
        [Fact]
        public void AccLoginModel()
        {
            var acc = new AccModel
            {
                Name = "Test",
                Roles = EnumRoles.User,
                City = EnumCities.Kharkiv,
                PhoneNumber = "0999644686",
                Email = "user@mail.com",
                PasswordHash = "aljkf834jfdk;fo8w"
            };
            var login = new LoginViewModel{
                Phone = acc.PhoneNumber,
                Password = acc.PasswordHash,
                RememberMe = false
            };
            Assert.Equal(acc.PhoneNumber, login.Phone);
            Assert.Equal(acc.PasswordHash, login.Password);
            Assert.Equal(false, login.RememberMe);
        }
        [Fact]
        public void AccRegisterModel() 
        {
            var acc = new AccModel
            {
                Name = "Test",
                Roles = EnumRoles.User,
                City = EnumCities.Kharkiv,
                PhoneNumber = "0999644686",
                Email = "user@mail.com",
                PasswordHash = "aljkf834jfdk;fo8w"
            };
            var register = new RegisterViewModel
            {
                Name = acc.Name,
                Email = acc.Email,
                PhoneNumber= acc.PhoneNumber,
                Password = acc.PasswordHash,
                ConfirmPassword = "aljkf834jfdk;fo8w"
            };
            Assert.Equal(acc.Name, register.Name);
            Assert.Equal(acc.Email, register.Email);
            Assert.Equal(acc.PhoneNumber, register.PhoneNumber);
            Assert.Equal(acc.PasswordHash, register.Password);
            Assert.Equal("aljkf834jfdk;fo8w", register.ConfirmPassword);
        }
    }
}
