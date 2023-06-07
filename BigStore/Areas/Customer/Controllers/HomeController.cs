using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BigStore.DataAccess;
using BigStore.BusinessObject;
using Microsoft.AspNetCore.Identity;
using BigStore.Utility;
using Microsoft.AspNetCore.Authorization;
using BigStore.BusinessObject.OtherModels;

namespace BigStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = RoleContent.Customer)]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Customer/Home
        public IActionResult Index()
        {
            return View();
        }

        // GET: Customer/Home/Create
        public async Task<IActionResult> CreateAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user?.ShopId != null)
                return RedirectToAction("Index", "Shop", new { area = "seller" });

            return View();
        }

        // POST: Customer/Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShopName,Description,Phone,Address")] Shop shop, IFormFile? ThumbnailFile)
        {
            var user = await _userManager.GetUserAsync(User);

            // Lấy thông tin id người dùng từ đối tượng IdentityUser
            shop.UserId = user.Id;

            if (ModelState.IsValid)
            {
                shop.CreateAt = DateTime.Now;
                shop.UpdateAt = DateTime.Now;

                //save images default
                shop.ImageUrl = await Image.GetPathImageSaveAsync(ThumbnailFile, "shops");
                _userManager.AddToRoleAsync(user, RoleContent.Seller).GetAwaiter().GetResult();

                _context.Add(shop);
                user.Shop = shop;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(shop);
        }
    }
}
