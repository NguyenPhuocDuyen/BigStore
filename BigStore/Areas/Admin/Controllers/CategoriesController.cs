using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BigStore.BusinessObject;
using Microsoft.AspNetCore.Authorization;
using BigStore.BusinessObject.OtherModels;
using BigStore.Utility;
using BigStore.DataAccess.Repository.IRepository;
using BigStore.DataAccess;

namespace BigStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/category/[action]/{id?}")]
    [Authorize(Roles = RoleContent.Admin)]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public CategoriesController(ICategoryRepository categoryRepository, IConfiguration configuration, ApplicationDbContext context)
        {
            _categoryRepository = categoryRepository;
            _configuration = configuration;
            _context = context;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index(int pageIndex)
        {
            var categories = await _categoryRepository.GetCategories();

            var pageSize = _configuration.GetValue("PageSize", 24);
            var pagingCategories = PaginatedList<Category>.CreateAsync(categories, pageIndex, pageSize);
            return View(pagingCategories);
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(string slug)
        {
            var category = await _categoryRepository.GetCategoryBySlug(slug);
            if (category == null) return NotFound();
            return View(category);
        }

        // GET: Admin/Categories/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["ParentCategoryId"] = await RenderSelectListCategories(null);
            return View();
        }

        // POST: Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParentCategoryId,Title,Description")] Category category, IFormFile? ThumbnailFile)
        {
            //Generate slug
            category.Slug ??= Slug.GenerateSlug(category.Title);
            var cateSlug = await _categoryRepository.GetCategoryBySlug(category.Slug);
            if (cateSlug != null)
                ModelState.AddModelError(string.Empty, "Danh mục bị trùng slug. Hãy đặt tên khác");

            if (ModelState.IsValid)
            {
                category.ImageUrl = await Image.GetPathImageSaveAsync(ThumbnailFile, "categories");
                if (category.ParentCategoryId == -1) category.ParentCategoryId = null;
                category.CreateAt = DateTime.UtcNow;
                category.UpdateAt = DateTime.UtcNow;
                category.IsDelete = false;
                try
                {
                    await _categoryRepository.Add(category);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra!");
                }

            }

            ViewData["ParentCategoryId"] = await RenderSelectListCategories(category.ParentCategoryId);
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryRepository.GetCategoryById((int)id);
            if (category == null)
                return NotFound();

            ViewData["ParentCategoryId"] = await RenderSelectListCategories(category.ParentCategoryId);
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParentCategoryId,Title,Description")] Category category, IFormFile? ThumbnailFile)
        {
            if (id != category.Id)
                return NotFound();

            bool canUpdate = true;

            if (category.ParentCategoryId == category.Id)
            {
                canUpdate = false;
                ModelState.AddModelError(string.Empty, "Phải chọn danh mục cha khác");
            }

            if (canUpdate && category.ParentCategoryId != null && category.ParentCategoryId != -1)
            {
                var childCates = await _categoryRepository.GetChildCategories(category.Id);

                Func<List<Category>, bool> checkCateIds = null;
                checkCateIds = (cates) =>
                {
                    foreach (var cate in cates)
                    {
                        if (cate.Id == category.ParentCategoryId)
                        {
                            canUpdate = false;
                            ModelState.AddModelError(string.Empty, "Phải chọn danh mục cha khác");
                            return true;
                        }
                        if (cate.CategoryChildren != null)
                        {
                            return checkCateIds(cate.CategoryChildren.ToList());
                        }
                    }
                    return false;
                };
                checkCateIds(childCates.ToList());
            }

            //Generate slug
            category.Slug ??= Slug.GenerateSlug(category.Title);
            var cateSlug = await _categoryRepository.GetCategoryBySlug(category.Slug);
            if (cateSlug != null && cateSlug.Id != category.Id)
                ModelState.AddModelError(string.Empty, "Danh mục bị trùng slug. Hãy đặt tên khác");

            if (ModelState.IsValid && canUpdate)
            {
                try
                {
                    //save images
                    // Giữ nguyên giá trị cũ của ImageUrl nếu không có file được tải lên
                    var existingCategory = await _categoryRepository.GetCategoryById(category.Id);
                    category.ImageUrl = await Image.GetPathImageSaveAsync(ThumbnailFile, "categories", existingCategory.ImageUrl);

                    //Generate slug
                    category.Slug ??= Slug.GenerateSlug(category.Title);

                    if (category.ParentCategoryId == -1) category.ParentCategoryId = null;
                    category.UpdateAt = DateTime.UtcNow;

                    await _categoryRepository.Update(category);
                    //_context.Entry(category).State = EntityState.Modified;
                    ////_context.Update(category);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CategoryExists(category.Id))
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

            ViewData["ParentCategoryId"] = await RenderSelectListCategories(category.ParentCategoryId);
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var category = await _categoryRepository.GetCategoryById((int)id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);

            if (category == null)
                return NotFound();

            await _categoryRepository.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoryExists(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            return category is not null;
        }

        private async Task<SelectList?> RenderSelectListCategories(int? idSelect)
        {
            var categories = await _categoryRepository.GetCategories();
            var items = SelectItem.CreateSelectItemsHasNoParent(categories);
            return new SelectList(items, "Id", "Title", idSelect);
        }
    }
}
