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
using BigStore.DataAccess.Repository.IRepository;

namespace BigStore.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = RoleContent.Seller)]
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly ICategoryRepository _category;
        private readonly IProductRepository _product;
        private readonly IProductImageRepository _productImage;

        public ProductsController(IConfiguration configuration,
            UserManager<User> userManager,
            ICategoryRepository category,
            IProductRepository product,
            IProductImageRepository productImage)
        {
            _configuration = configuration;
            _userManager = userManager;
            _category = category;
            _product = product;
            _productImage = productImage;
        }

        // GET: Seller/Products
        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.ShopId == null)
                return NotFound();

            var products = await _product.GetByShopId(user.ShopId);
            var pageSize = _configuration.GetValue("PageSize", 24);
            PaginatedList<Product> pagingProduts = PaginatedList<Product>.CreateAsync(products, pageIndex, pageSize);
            return View(pagingProduts);
        }

        // GET: Seller/Products/Details/5
        public async Task<IActionResult> Details(string? slug)
        {
            if (slug == null) return NotFound();
            var product = await _product.GetBySlug(slug);
            if (product == null) return NotFound();
            return View(product);
        }

        // GET: Seller/Products/Create
        public async Task<IActionResult> CreateAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.ShopId == null)
                return NotFound();

            ViewData["CategoryId"] = await RenderSelectListCategories(null);
            return View();
        }

        // POST: Seller/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Name,Description,Price,Quantity")] Product product, IFormFileCollection ThumbnailFiles)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.ShopId == null)
                return NotFound();

            //Generate slug
            product.Slug = Slug.GenerateSlug(product.Name);
            var productSlug = await _product.GetBySlug(product.Slug);
            if (productSlug != null)
                ModelState.AddModelError(string.Empty, "Sản phẩm bị trùng slug. Hãy đặt tên khác");

            if (ThumbnailFiles.Count == 0)
                ModelState.AddModelError(string.Empty, "Hãy thêm ảnh.");

            if (ModelState.IsValid)
            {
                product.ShopId = user.ShopId;
                if (product.CategoryId == "-1") product.CategoryId = null;
                product.CreatedAt = DateTime.UtcNow;
                product.UpdatedAt = DateTime.UtcNow;
                product.IsDeleted = false;
                product.ProductImages = new List<ProductImage>();

                // thêm ảnh cho sản phẩm
                foreach (var file in ThumbnailFiles)
                {
                    ProductImage productImage = new()
                    {
                        Product = product,
                        ImageUrl = await Image.GetPathImageSaveAsync(file, "products")
                    };
                    product.ProductImages.Add(productImage);
                }

                await _product.Add(product);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = await RenderSelectListCategories(product.CategoryId);

            return View(product);
        }

        // GET: Seller/Products/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null) return NotFound();

            var product = await _product.GetById((string)id);
            if (product == null) return NotFound();

            ViewData["CategoryId"] = await RenderSelectListCategories(product.CategoryId);
            return View(product);
        }

        // POST: Seller/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,CategoryId,ShopId,Name,Description,Price,Quantity")] Product product, IFormFileCollection ThumbnailFiles)
        {
            if (id != product.Id) return NotFound();

            var productDb = await _product.GetById(id);
            if (productDb == null) return NotFound();

            //Generate slug
            product.Slug = Slug.GenerateSlug(product.Name);
            var productSlug = await _product.GetBySlug(product.Slug);
            if (productSlug != null && productSlug.Id != id)
                ModelState.AddModelError(string.Empty, "Sản phẩm bị trùng slug. Hãy đặt tên khác");

            if (ModelState.IsValid)
            {
                productDb.CategoryId = product.CategoryId;
                if (productDb.CategoryId == "-1") productDb.CategoryId = null;
                productDb.Name = product.Name;
                productDb.Description = product.Description;
                productDb.Price = product.Price;
                productDb.Quantity = product.Quantity;
                productDb.Slug = product.Slug;
                productDb.UpdatedAt = DateTime.UtcNow;
                
                if (ThumbnailFiles.Count > 0)
                {
                    foreach (var img in productDb.ProductImages)
                    {
                        // xoá ảnh
                        img.IsDeleted = true;
                        img.UpdatedAt = DateTime.UtcNow;
                         await _productImage.Update(img);
                        //await _product.RemoveImagesOfProduct(productDb.Id);
                    }
                    productDb.ProductImages = new List<ProductImage>();
                    foreach (var item in ThumbnailFiles)
                    {
                        ProductImage productImage = new()
                        {
                            Product = productDb,
                            ImageUrl = await Image.GetPathImageSaveAsync(item, "products")
                        };
                        productDb.ProductImages.Add(productImage);
                    }
                }

                try
                {
                    await _product.Update(productDb);
                }
                catch (Exception ex)
                {
                    if (!await ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw new Exception(ex.Message); 
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = await RenderSelectListCategories(product.CategoryId);
            return View(product);
        }

        // GET: Seller/Products/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null) return NotFound();
            var product = await _product.GetById((string)id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: Seller/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = await _product.GetById(id);
            if (product == null) return NotFound();

            product.IsDeleted = true;
            product.UpdatedAt = DateTime.UtcNow;
            await _product.Update(product);
            //await _product.Remove(product);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductExists(string id)
        {
            return await _product.GetById(id) is not null;
        }

        private async Task<SelectList?> RenderSelectListCategories(string? idSelect)
        {
            var categories = await _category.GetAll();
            var items = SelectItem.CreateSelectItemsHasNoParent(categories);
            return new SelectList(items, "Id", "Title", idSelect);
        }
    }
}
