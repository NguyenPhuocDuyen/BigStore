using BigStore.BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace BigStore.DataAccess.DAO
{
    public class ProductDAO
    {
        public static async Task<List<Product>> GetProducts()
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var products = await _context.Products.Include(p => p.Category)
                    .Include(p => p.Shop)
                    .Include(p => p.ProductImages)
                    .AsNoTracking()
                    .ToListAsync();
                return products;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public static async Task<List<Product>> GetProductsOfShopByUserId(string userId)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var products = await _context.Products.Include(p => p.Category)
                    .Include(p => p.Shop)
                    .Where(p => p.Shop.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync();
                return products;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public static async Task<Product?> GetProductBySlug(string slug)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Shop)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Slug == slug);
                return product;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public static async Task<Product?> GetProductById(int id)
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

        public static async Task Add(Product product)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public static async Task Update(Product product)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                _context.Products.Update(product);
                //_context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public static async Task Remove(int id)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var product = _context.Products.FirstOrDefault(x => x.Id == id);
                if (product != null)
                {
                    product.IsDelete = true;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public static async Task RemoveImagesOfProduct(int id)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var productImages = _context.ProductImages.Where(x => x.ProductId == id);
                _context.ProductImages.RemoveRange(productImages);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
