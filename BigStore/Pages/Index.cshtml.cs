using BigStore.DataAccess;
using BigStore.BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BigStore.DataAccess.Repository.IRepository;

namespace BigStore.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public IndexModel(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }

        public async Task OnGet()
        {
            Categories = await _categoryRepository.GetCategories();
            Products = (await _productRepository.GetProducts())
                .OrderByDescending(x => x.UpdateAt).ToList();
        }
    }
}