using BigStore.Data;
using BigStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BigStore.Pages.Products
{
    public class DetailModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public DetailModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public Product? Product { get; set; }

        public IActionResult OnGet(string slug = "")
        {
            Product = _dbContext.Products.Include(x => x.ProductImages).FirstOrDefault(p => p.Slug == slug);
            if (Product is null)
                return RedirectToPage("/NotFound");

            return Page();
        }
    }
}
