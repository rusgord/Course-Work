using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectDelivery.Classes;
using ProjectDelivery.Data;
using ProjectDelivery.Enums;
using ProjectDelivery.Models;
using System.Security.Claims;

namespace ProjectDelivery.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AccModel> _userManager;
        public ServiceController(ApplicationDbContext context, UserManager<AccModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Tracking(int id)
        {
            TrackingModel trackingModel = new TrackingModel();
            trackingModel.Id = id;
            return View(trackingModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Tracking(TrackingModel trackingModel)
        {
            if (ModelState.IsValid)
            {
                var info = _context.Packages.FirstOrDefault(p => p.Id == trackingModel.Id);
                if (info != null)
                {
                    trackingModel.Name = info.recipientName;
                    trackingModel.City = info.City.ToString();
                    trackingModel.Description = info.Description;
                    trackingModel.IsFind = true;
                    return View(trackingModel);
                }
                else
                {
                    trackingModel.IsFind = false;
                    trackingModel.Text = "Не знайдено!";
                    return View(trackingModel);
                }
            }
            return View(trackingModel);
        }
        public async Task<IActionResult> Calculate(int w, int c, EnumCities city)
        {
            CalcModel model = new CalcModel
            {
                Weight = w,
                Cost = c,
                City = city
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(CalcModel model)
        {
            if (ModelState.IsValid)
            {
                double w = model.Weight;
                double c = model.Cost;
                int price = 80;
                if (w > 5 && w <= 10)
                    price = 90;
                if (w > 10 && w <= 25)
                    price = 120;
                model.Price = c * 0.01 + price;
                View(model);
            }
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Package package)
        {
            if (ModelState.IsValid)
            {
                var recipient = await new FindingUser().FindByPhoneNumberAsync(_userManager, package.Phone);
                string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (recipient != null && currentUserId != recipient.Id)
                {
                    package.RecipientId = recipient.Id;
                    package.recipientName = recipient.Name;
                }
                if (currentUserId != null)
                {
                    package.SenderId = currentUserId;
                    var user = await _userManager.GetUserAsync(User);
                    package.senderName = user?.Name;
                    _context.Packages.Add(package);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(package);
        }
        public async Task<IActionResult> Help()
        {
            List<ArticleModel> model = _context.Articles.ToList();
            return View(model);
        }
    }
}
