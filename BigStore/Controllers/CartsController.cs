using BigStore.DataAccess;
using BigStore.BusinessObject;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BigStore.BusinessObject.OtherModels;

namespace BigStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public CartsController(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        // POST api/<CartsController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Cart cart)
        {
            //// lấy thông tin người dùng hiện tại
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is null)
                return NotFound(new ApiResponse { Message = "Không tìm thấy người dùng" });

            string userId = user.Id;

            // lấy thông tin sản phẩm hiên tại
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == cart.ProductId);
            if (product is null)
                return NotFound(new ApiResponse { Message = "Không tìm thấy sản phẩm" });

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

                if (!quanlityIsValid)
                    return Ok(new ApiResponse { Success = true, Message = "Vượt quá số lượng, vẫn cộng max" });

                return NoContent();
            }
            catch { return BadRequest(new ApiResponse { Message = "Xung đột dữ liệu" }); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int? id, [FromBody] Cart cart)
        {
            if (id is null)
                return BadRequest(new ApiResponse { Message = "Thiếu id" });

            //// lấy thông tin người dùng hiện tại
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is null)
                return NotFound(new ApiResponse { Message = "Không tìm thấy người dùng" });

            var cartDb = await _dbContext.Carts.Include(x => x.Product).FirstOrDefaultAsync(c => c.Id == id && c.UserId == user.Id);

            // biến check số lượng sản phẩm hợp lệ
            bool quanlityIsValid = true;

            cartDb.Quantity = cart.Quantity;

            if (cartDb.Quantity > cartDb.Product.Quantity)
            {
                cartDb.Quantity = cartDb.Product.Quantity;
                quanlityIsValid = false;
            }

            try
            {
                await _dbContext.SaveChangesAsync();

                if (!quanlityIsValid)
                    return Ok(new ApiResponse { Success = true, Message = "Vượt quá số lượng, vẫn cộng max" });

                return NoContent();
            }
            catch { return BadRequest(new ApiResponse { Message = "Xung đột dữ liệu" }); }
        }

        // DELETE api/<CartsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //// lấy thông tin người dùng hiện tại
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is null)
                return NotFound(new ApiResponse { Message = "Không tìm thấy người dùng" });

            // tìm và xoá sản phẩm
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.Id == id && c.UserId == user.Id);
            if (cart is null) return NotFound(new ApiResponse { Message = "Không tìm thấy sản phẩm" });

            _dbContext.Carts.Remove(cart);
            try
            {
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch { return BadRequest(new ApiResponse { Message = "Xung đột dữ liệu" }); }
        }
    }
}
