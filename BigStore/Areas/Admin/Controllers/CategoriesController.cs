using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BigStore.BusinessObject;
using Microsoft.AspNetCore.Authorization;
using BigStore.BusinessObject.OtherModels;
using BigStore.Utility;
using BigStore.DataAccess.Repository.IRepository;
using BigStore.DataAccess;
using BigStore.Dtos.CategoryDto;
using AutoMapper;

namespace BigStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/category/[action]/{id?}")]
    [Authorize(Roles = RoleContent.Admin)]
    public class CategoriesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _category;
        private readonly IConfiguration _configuration;

        public CategoriesController(IMapper mapper,
            ICategoryRepository category, 
            IConfiguration configuration)
        {
            _mapper = mapper;
            _category = category;
            _configuration = configuration;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index(int pageIndex)
        {
            var categories = await _category.GetAll();

            var pageSize = _configuration.GetValue("PageSize", 24);
            var pagingCategories = PaginatedList<Category>.CreateAsync(categories, pageIndex, pageSize);
            return View(pagingCategories);
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(string slug)
        {
            var category = await _category.GetBySlug(slug);
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
        public async Task<IActionResult> Create([Bind("ParentCategoryId,Title,Description")] CategoryCreateDto category, IFormFile? ThumbnailFile)
        {
            Category newCate = _mapper.Map<Category>(category);

            //Generate slug
            newCate.Slug = Slug.GenerateSlug(newCate.Title);
            var cateSlug = await _category.GetBySlug(newCate.Slug);
            if (cateSlug != null)
                ModelState.AddModelError(string.Empty, "Danh mục bị trùng slug. Hãy đặt tên khác");

            if (ModelState.IsValid)
            {
                newCate.ImageUrl = await Image.GetPathImageSaveAsync(ThumbnailFile, "categories");
                if (newCate.ParentCategoryId == "-1") newCate.ParentCategoryId = null;
                newCate.CreatedAt = DateTime.UtcNow;
                newCate.UpdatedAt = DateTime.UtcNow;
                newCate.IsDeleted = false;

                try
                {
                    await _category.Add(newCate);
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

        // GET: Admin/Categories/Edit/...
        public async Task<IActionResult> Edit(string id)
        {
            var category = await _category.GetById(id);
            if (category == null)
                return NotFound();

            var editCate = _mapper.Map<CategoryUpdateDto>(category);

            ViewData["ParentCategoryId"] = await RenderSelectListCategories(category.ParentCategoryId);
            return View(editCate);
        }

        // POST: Admin/Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,ParentCategoryId,Title,Description")] CategoryUpdateDto category, IFormFile? ThumbnailFile)
        {
            if (id != category.Id)
                return NotFound();

            var existingCategory = await _category.GetById(category.Id);
            if (existingCategory == null)
                return NotFound();

            bool canUpdate = true;

            if (category.ParentCategoryId == category.Id)
            {
                canUpdate = false;
                ModelState.AddModelError(string.Empty, "Phải chọn danh mục cha khác");
            }

            if (canUpdate && category.ParentCategoryId != null && category.ParentCategoryId != "-1")
            {
                var childCates = await _category.GetChildren(category.Id);

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
            existingCategory.Slug = Slug.GenerateSlug(category.Title);
            var cateSlug = await _category.GetBySlug(existingCategory.Slug);
            if (cateSlug != null && cateSlug.Id != category.Id)
                ModelState.AddModelError(string.Empty, "Danh mục bị trùng slug. Hãy đặt tên khác");

            if (ModelState.IsValid && canUpdate)
            {
                try
                {
                    existingCategory.ParentCategoryId = category.ParentCategoryId;
                    if (existingCategory.ParentCategoryId == "-1") existingCategory.ParentCategoryId = null;

                    //save images
                    // Giữ nguyên giá trị cũ của ImageUrl nếu không có file được tải lên
                    existingCategory.ImageUrl = await Image.GetPathImageSaveAsync(ThumbnailFile, "categories", existingCategory.ImageUrl);
                    existingCategory.Title = category.Title;
                    existingCategory.Description = category.Description;
                    existingCategory.UpdatedAt = DateTime.UtcNow;

                    await _category.Update(existingCategory);
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
        public async Task<IActionResult> Delete(string id, bool deleteFailed = false)
        {
            if (deleteFailed)
            {
                ViewBag.ErrorMessage = "Xóa thất bại vì còn danh mục con. Hãy thử lại sau.";
            }

            if (id == null) return NotFound();
            var category = await _category.GetById(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var category = await _category.GetById(id);

            if (category == null)
                return NotFound();

            if (category.CategoryChildren.Any())
            {
                return RedirectToAction(nameof(Delete), "Categories" , new { id = id, deleteFailed = true });
            }

            category.IsDeleted = true;
            category.UpdatedAt = DateTime.UtcNow;
            await _category.Update(category);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoryExists(string id)
        {
            var category = await _category.GetById(id);
            return category is not null;
        }

        private async Task<SelectList?> RenderSelectListCategories(string? idSelect)
        {
            var categories = await _category.GetAll();
            var items = SelectItem.CreateSelectItemsHasNoParent(categories);
            return new SelectList(items, "Id", "Title", idSelect);
        }
    }
}
