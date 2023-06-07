using BigStore.DataAccess;
using BigStore.DataAccess.DbInitializer;
using BigStore.BusinessObject;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BigStore.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/database-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbManageController(ApplicationDbContext dbContext, 
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetDb()
        {
            return View();
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpPost]
        public async Task<IActionResult> ResetDbAsync()
        {
            //var success = await _dbContext.Database.EnsureDeletedAsync();
            //StatusMessage = success ? "Xoá Database thành công" : "Không xoá được Database";
            await _dbContext.Database.EnsureDeletedAsync();
            new DbInitializer(_dbContext, _userManager, _roleManager).Initialize();
            StatusMessage = "Reset database thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MigrateAsync()
        {
            await _dbContext.Database.MigrateAsync();
            StatusMessage = "Cập nhật database thành công";

            return RedirectToAction(nameof(Index));
        }
    }
}
