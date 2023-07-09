using BigStore.BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace BigStore.DataAccess.DAO
{
    internal class CartDAO : DAO<Cart>
    {
        internal static async Task<List<Cart>> GetAll()
        {
            try
            {
                using var context = new ApplicationDbContext();
                var carts = await context.Carts
                        .Include(c => c.Product)
                            .ThenInclude(p => p.Shop)
                        .Include(c => c.Product)
                            .ThenInclude(p => p.ProductImages)
                        .ToListAsync();
                return carts;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task<Cart?> GetById(string id)
        {
            try
            {
                using var context = new ApplicationDbContext();
                var cart = await context.Carts
                        .Include(c => c.Product)
                            .ThenInclude(p => p.Shop)
                        .Include(c => c.Product)
                            .ThenInclude(p => p.ProductImages)
                        .FirstOrDefaultAsync(x => x.Id == id);
                return cart;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
