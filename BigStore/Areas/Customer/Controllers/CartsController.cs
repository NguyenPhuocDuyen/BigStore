using BigStore.BusinessObject;
using BigStore.BusinessObject.OtherModels;
using BigStore.DataAccess;
using BigStore.DataAccess.Repository.IRepository;
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
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<User> _userManager;

        public CartsController(ICartRepository cartRepository, UserManager<User> userManager)
        {
            _cartRepository = cartRepository;
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
            var cartItems = (await _cartRepository.GetAll())
                    .Where(c => c.UserId.Equals(userId))
                    .ToList();

            // cập nhật lại số lượng nếu số lượng trong shop không đủ trong giỏ hàng
            foreach (var item in cartItems)
            {
                if (item.Quantity > item.Product.Quantity)
                {
                    item.Quantity = item.Product.Quantity;
                }
                await _cartRepository.Update(item);
            }
            //await _cartRepository.UpdateRange(cartItems);

            //nhóm theo shop
            var listGroupShopAndItems = cartItems.GroupBy(x => x.Product?.Shop).ToList();

            return View(listGroupShopAndItems);
        }

        [HttpPost]
        public async Task<IActionResult> MakeOrder(string[] selectedItems)
        {
            // lấy các item trong giỏ hàng được check mua
            var cartItems = (await _cartRepository.GetAll())
                    .Where(c => selectedItems.Contains(c.Id.ToString()))
                    .ToList();

            //nhóm theo shop 
            var listItemsBuy = cartItems.GroupBy(x => x.Product?.Shop).ToList();

            return View(listItemsBuy);
        }
    }
}
