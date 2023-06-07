using BigStore.DataAccess;
using BigStore.BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;

namespace BigStore.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public List<Product> Products { get; set; } = new();

        public List<Category>? Categories { get; set; }
        public Category? CategorySlug { get; set; }
        public string? Search { get; set; }

        public void OnGet(string categorySlug, string search, string sortBy, int minPrice, int maxPrice, string orderBy)
        {

            var query = _dbContext.Products.Include(x => x.Category).Include(x => x.ProductImages).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                Search = search;
                query = query.Where(x => x.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(categorySlug))
            {
                CategorySlug = _dbContext.Categories.FirstOrDefault(x => x.Slug.Equals(categorySlug));
                Categories = _dbContext.Categories
                    .Include(x => x.ParentCategory)
                    .ThenInclude(x => x.ParentCategory)
                    .Where(x => x.ParentCategory.Slug.Equals(categorySlug) && x.ParentCategory.ParentCategoryId == null)
                    .ToList();

                query = query.Where(x => x.Category.Slug == categorySlug);
            }

            switch (sortBy)
            {
                case "price":
                    if (maxPrice != 0)
                        if (minPrice < maxPrice)
                        {
                            query = query.Where(x => x.Price > minPrice && x.Price < maxPrice);
                        }
                    break;
                case "star": break;
                case "pop": break;
                case "time":
                    query = query.OrderByDescending(x => x.UpdateAt);
                    break;
                case "sales": break;
                default: break;
            }

            switch (orderBy)
            {
                case "asc":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "desc":
                    query = query.OrderByDescending(x => x.Price);
                    break;
                default: break;
            }

            Products = query.ToList();
        }

        // Phương thức chuyển đổi chữ có dấu thành không dấu bỏ ký tự đặc biệt
        //private static string RemoveDiacriticsAndSpecialCharacters(string text)
        //{
        //    string normalized = text.ToLower().Trim().Normalize(NormalizationForm.FormKD);
        //    Regex regex = new("[^\\p{L}\\p{Nd}\\s#@!&$%]");
        //    string result = regex.Replace(normalized, "");

        //    // Replace "đ" with "d"
        //    result = result.Replace("đ", "d");

        //    return result;
        //}
    }
}
