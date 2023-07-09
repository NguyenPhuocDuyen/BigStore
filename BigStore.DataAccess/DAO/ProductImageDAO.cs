using BigStore.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigStore.DataAccess.DAO
{
    internal class ProductImageDAO : DAO<ProductImage>
    {
        internal static async Task<List<ProductImage>> GetAll()
        {
            try
            {
                using var context = new ApplicationDbContext();
                var images = await context.ProductImages
                    .ToListAsync();
                return images;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task<ProductImage?> GetById(string id)
        {
            try
            {
                using var context = new ApplicationDbContext();
                var image = await context.ProductImages
                        .FirstOrDefaultAsync(x => x.Id == id);
                return image;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
