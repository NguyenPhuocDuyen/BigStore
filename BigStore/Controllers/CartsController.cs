using BigStore.DataAccess;
using BigStore.BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BigStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public CartsController(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("hi");
        }

        // POST api/<CartsController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Cart cart)
        {
            // lấy thông tin người dùng hiện tại
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is null)
                return NotFound();
            //string userId = "6b8bbf76-705d-4d1a-99e5-9b18e6067c67";
            string userId = user.Id;

            // lấy thông tin sản phẩm hiên tại
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == cart.ProductId);
            if (product is null)
                return NotFound();
            // lấy thông tin cart cũ nếu có tồn tại
            var oldCart = await _dbContext.Carts.FirstOrDefaultAsync(x => x.UserId == userId
                && x.ProductId == cart.ProductId);

            // biến check số lượng sản phẩm hợp lệ
            bool quanlityIsValid = true;

            //Nếu không có thì tạo mới, có thì thêm số lượng
            if (oldCart is null)
            {
                oldCart = new Cart
                {
                    UserId = userId,
                    ProductId = cart.ProductId,
                    Quantity = cart.Quantity,
                };
                // Vượt quá số lượng sản phẩm hiện có được thêm vào
                if (cart.Quantity > product.Quantity)
                {
                    quanlityIsValid = false;
                    oldCart.Quantity = product.Quantity;
                }
                await _dbContext.Carts.AddAsync(oldCart);
            }
            else
            {
                // đã tồn tại cart cũ cộng số lượng
                oldCart.Quantity += cart.Quantity;

                // vượt quá số lượng sản phẩm hiện có được thêm vào
                quanlityIsValid = oldCart.Quantity <= product.Quantity;

                // vẫn cộng max số lượng cho user
                if (!quanlityIsValid) oldCart.Quantity = product.Quantity;
            }

            try
            {
                await _dbContext.SaveChangesAsync();

                if (!quanlityIsValid) return Ok("Vượt quá số lượng, vẫn cộng max");

                return NoContent();
            }
            catch { return BadRequest(); }
        }

        // DELETE api/<CartsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // tìm và xoá sản phẩm
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.Id == id);
            if (cart is null) return NotFound();

            _dbContext.Carts.Remove(cart);
            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch { return BadRequest(); }
        }
    }
}
