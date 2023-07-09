using BigStore.BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace BigStore.DataAccess.DAO
{
    internal class ProductDAO : DAO<Product>
    {
        internal static async Task<List<Product>> GetAll()
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var products = await _context.Products.Include(p => p.Category)
                    .Include(p => p.Shop)
                    .Include(p => p.ProductImages)
                    .ToListAsync();
                return products;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task<List<Product>> GetByShopId(string shopId)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var products = await _context.Products.Include(p => p.Category)
                    .Include(p => p.Shop)
                    .Where(p => p.Shop.Id == shopId)
                    .ToListAsync();
                return products;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task<Product?> GetBySlug(string slug)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Shop)
                    .FirstOrDefaultAsync(x => x.Slug == slug);
                return product;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task<Product?> GetById(string id)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Shop)
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(x => x.Id == id);
                return product;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
