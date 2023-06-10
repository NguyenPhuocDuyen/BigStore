using BigStore.BusinessObject;
using BigStore.BusinessObject.OtherModels;
using BigStore.DataAccess;
using BigStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BigStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = RoleContent.Customer)]
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public CartsController(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //// lấy thông tin người dùng hiện tại
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is null)
                return NotFound();

            string userId = user.Id;

            // lấy các item trong giỏ hàng
            var cartItems = await _dbContext.Carts
                .Include(x => x.Product)
                    .ThenInclude(x => x.Shop)
                .Where(x => x.UserId.Equals(userId))
                .ToListAsync();

            var ketqua = cartItems.GroupBy(x => x.Product.Shop).ToList();

            return View(ketqua);
        }

        [HttpPost]
        public async Task<IActionResult> MakeOrder(string[] selectedItems)
        {
            var listCart = _dbContext.Carts.Where(x => selectedItems.Contains(x.Id.ToString())).ToList();
            return View(listCart);
        }
    }
}
