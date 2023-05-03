using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BigStore.Data;
using BigStore.Models;
using Microsoft.AspNetCore.Authorization;
using BigStore.Models.OtherModels;

namespace BigStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/category/[action]/{id?}")]
    [Authorize(Roles = RoleContent.Admin)]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            var qr = (from c in _context.Categories select c)
                    .Include(x => x.ParentCategory)
                    .Include(x => x.CategoryChildren);

            var categories = (await qr.ToListAsync())
                            .Where(x => x.ParentCategory == null)
                            .ToList();

            return View(categories);
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        private void CreateSelectItems(List<Category> source, List<Category> des, int level)
        {
            string prefix = String.Concat(Enumerable.Repeat("—", level));
            foreach (var category in source)
            {
                des.Add(new Category
                {
                    Id = category.Id,
                    Title = prefix + " " + category.Title
            });
                if(category.CategoryChildren?.Count > 0)
                {
                    CreateSelectItems(category.CategoryChildren.ToList(), des, level + 1);
                }
            }
        }

        // GET: Admin/Categories/Create
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
            CreateSelectItems(categories, items, 0);

            ViewData["ParentCategoryId"] = new SelectList(items, "Id", "Title");
            return View();
        }

        // POST: Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParentCategoryId,Title,Description,Slug,ImageUrl")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentCategoryId == -1) category.ParentCategoryId = null;
                category.CreateAt = DateTime.Now;
                category.UpdateAt = DateTime.Now;
                category.IsDelete = false;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
            CreateSelectItems(categories, items, 0);

            ViewData["ParentCategoryId"] = new SelectList(items, "Id", "Title", category.ParentCategoryId);
            return View(category); 
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
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
            CreateSelectItems(categories, items, 0);

            ViewData["ParentCategoryId"] = new SelectList(items, "Id", "Title", category.ParentCategoryId);
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParentCategoryId,Title,Description,Slug,ImageUrl,CreateAt,UpdateAt,IsDelete")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            
            if (category.ParentCategoryId == category.Id)
            {
                ModelState.AddModelError(string.Empty, "Phải chọn danh mục cha khác");
            }    

            if (ModelState.IsValid && category.ParentCategoryId != category.Id)
            {
                try
                {
                    if (category.ParentCategoryId == -1) category.ParentCategoryId = null;
                    category.UpdateAt = DateTime.Now;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            CreateSelectItems(categories, items, 0);

            ViewData["ParentCategoryId"] = new SelectList(items, "Id", "Title", category.ParentCategoryId);
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }

            var category = await _context.Categories
                        .Include(x => x.CategoryChildren)
                        .FirstOrDefaultAsync(x => x.Id == id);

            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            foreach (var childCategory in category.CategoryChildren)
            {
                childCategory.ParentCategoryId = category.ParentCategoryId;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
