using BigStore.BusinessObject.OtherModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BigStore.Areas.Seller.Controllers
{
    [Area(RoleContent.Seller)]
    [Route("seller/shop/[action]/{id?}")]
    [Authorize(Roles = RoleContent.Seller)]
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
