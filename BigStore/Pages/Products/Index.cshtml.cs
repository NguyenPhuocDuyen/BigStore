using BigStore.DataAccess;
using BigStore.BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using BigStore.Utility;

namespace BigStore.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public IndexModel(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public PaginatedList<Product> ProductsPaging { get; set; }

        public List<Category>? Categories { get; set; }
        public Category? Category { get; set; }

        public string? CategorySlug { get; set; }
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public string? OrderBy { get; set; }
        public int? PageIndex { get; set; }

        public IActionResult OnGetAsync(string categorySlug,
            string search,
            string sortBy,
            int minPrice, int maxPrice,
            string orderBy,
            int pageIndex = 1)
        {
            // lấy ra danh sách sản phẩm
            var query = _dbContext.Products
                .Include(x => x.Category)
                .ThenInclude(x => x.ParentCategory)
                .ThenInclude(x => x.ParentCategory)
                .Include(x => x.ProductImages)
                .AsQueryable();

            // tìm kiếm sản phẩm theo search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            // lấy ra danh mục nếu tìm theo danh mục
            if (!string.IsNullOrEmpty(categorySlug))
            {
                // lấy danh mục được tìm
                var cateCurrent = _dbContext.Categories
                    .Include(x => x.ParentCategory)
                    .ThenInclude(x => x.CategoryChildren)
                    .Include(x => x.CategoryChildren)
                    .FirstOrDefault(x => x.Slug == categorySlug);

                // trả về trang chủ nếu không thấy danh mục
                if (cateCurrent is null)
                    return RedirectToPage("/Index");

                // ParentCategoryId == null đồng nghĩa là tìm bằng danh mục chính level 1
                if (cateCurrent.ParentCategoryId == null)
                {
                    // hiển thị cột loc danh mục là nó và các con của nó
                    Category = cateCurrent;
                    Categories = cateCurrent.CategoryChildren.ToList();
                }
                else
                {
                    // ParentCategoryId != null đồng nghĩa là tìm bằng danh mục phụ level 2
                    // hiển thị cột loc danh mục là cha nó và các con của cha nó (bao gồm chính nó)
                    Category = cateCurrent.ParentCategory;
                    Categories = cateCurrent.ParentCategory.CategoryChildren.ToList();
                }

                // lọc sản phẩm theo danh mục chính level 1, level 2 hoặc level 3
                // nhưng cột lọc danh mục chỉ hiển thị level 1 và 2
                // (lưu ý sản phẩm thuộc danh mục phụ level 3)
                query = query
                    .Where(x => x.Category.Slug == categorySlug
                    || x.Category.ParentCategory.Slug == categorySlug
                    || x.Category.ParentCategory.ParentCategory.Slug == categorySlug);
            }

            if (maxPrice != 0)
                if (minPrice < maxPrice)
                {
                    query = query.Where(x => x.Price > minPrice && x.Price < maxPrice);
                }

            switch (sortBy)
            {
                case "price":
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
                    break;
                case "star": break;
                case "pop": break;
                case "time":
                    query = query.OrderByDescending(x => x.UpdateAt);
                    break;
                case "sales": break;
                default: break;
            }
            //var pageSize = 24;
            var pageSize = _configuration.GetValue("PageSize", 24);
            ProductsPaging = PaginatedList<Product>.CreateAsync(query.AsNoTracking().ToList(), pageIndex, pageSize);

            CategorySlug = categorySlug;
            Search = search;
            SortBy = sortBy;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            OrderBy = orderBy;
            PageIndex = pageIndex;

            return Page();
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
