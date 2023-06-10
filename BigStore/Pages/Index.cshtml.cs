using BigStore.DataAccess;
using BigStore.BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BigStore.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }

        public void OnGet()
        {
            Categories = _context.Categories.Where(x => x.ParentCategoryId == null).ToList();
            Products = _context.Products.Include(x => x.ProductImages).ToList();
        }
    }
}