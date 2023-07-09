using BigStore.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigStore.DataAccess.DAO
{
    internal class DAO<T>
    {
        internal static async Task Add(T cart)
        {
            try
            {
                using var context = new ApplicationDbContext();
                if (cart != null)
                {
                    await context.AddAsync(cart);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task Update(T cart)
        {
            try
            {
                using var context = new ApplicationDbContext();
                if (cart != null)
                {
                    context.Update(cart);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task Remove(T cart)
        {
            try
            {
                using var context = new ApplicationDbContext();
                if (cart != null)
                {
                    context.Remove(cart);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
