using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BigStore.DataAccess;
using BigStore.BusinessObject;
using Microsoft.AspNetCore.Identity;
using BigStore.BusinessObject.OtherModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using BigStore.Utility;

namespace BigStore.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = RoleContent.Seller)]
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ProductsController(IConfiguration configuration, ApplicationDbContext context, UserManager<User> userManager)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
        }

        // GET: Seller/Products
        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            var user = await _userManager.GetUserAsync(User);

            var products = _context.Products.Include(p => p.Category)
                .Include(p => p.Shop)
                .Where(p => p.Shop.UserId == user.Id)
                .AsNoTracking()
                .ToList();

            var pageSize = _configuration.GetValue("PageSize", 24);
            PaginatedList<Product> pagingProduts = PaginatedList<Product>.CreateAsync(products, pageIndex, pageSize);

            return View(pagingProduts);
        }

        // GET: Seller/Products/Details/5
        public async Task<IActionResult> Details(string? slug)
        {
            if (slug == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Shop)
                .FirstOrDefaultAsync(m => m.Slug == slug);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Seller/Products/Create
        public async Task<IActionResult> CreateAsync()
        {
            var qr = (from c in _context.Categories select c)
                   .Include(x => x.ParentCategory)
                   .Include(x => x.CategoryChildren);

            var categories = (await qr.ToListAsync())
                            .Where(x => x.ParentCategory == null)
                            .ToList();

            categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });

            var items = new List<Category>();
            SelectItem.CreateSelectItems(categories, items, 0);

            ViewData["CategoryId"] = new SelectList(items, "Id", "Title");

            return View();
        }

        // POST: Seller/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Name,Description,Price,Quantity,Slug")] Product product)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                product.ShopId = (int)user.ShopId;

                if (product.CategoryId == -1) product.CategoryId = null;

                product.CreateAt = DateTime.Now;
                product.UpdateAt = DateTime.Now;
                product.IsDelete = false;

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", product.CategoryId);

            return View(product);
        }

        // GET: Seller/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var qr = (from c in _context.Categories select c)
                   .Include(x => x.ParentCategory)
                   .Include(x => x.CategoryChildren);

            var categories = (await qr.ToListAsync())
                            .Where(x => x.ParentCategory == null)
                            .ToList();

            categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });

            var items = new List<Category>();
            SelectItem.CreateSelectItems(categories, items, 0);

            ViewData["CategoryId"] = new SelectList(items, "Id", "Title", product.CategoryId);
            return View(product);
        }

        // POST: Seller/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,ShopId,Name,Description,Price,Quantity,Slug,CreateAt,UpdateAt,IsDelete")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", product.CategoryId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "ShopName", product.ShopId);
            return View(product);
        }

        // GET: Seller/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Shop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Seller/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
