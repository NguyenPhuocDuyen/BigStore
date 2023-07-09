using BigStore.BusinessObject;
using BigStore.Utility;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigStore.DataAccess.DAO
{
    internal class CategoryDAO : DAO<Category>
    {
        internal static async Task<List<Category>> GetAll()
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var categories = await _context.Categories
                    .Include(x => x.ParentCategory)
                    .Include(x => x.CategoryChildren)
                        .ThenInclude(x => x.CategoryChildren)
                    .Where(x => x.ParentCategory == null)
                    .ToListAsync();
                return categories;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task<Category?> GetById(string id)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var category = await _context.Categories
                    .Include(x => x.ParentCategory)
                    .Include(x => x.CategoryChildren)
                        .ThenInclude(x => x.CategoryChildren)
                    .FirstOrDefaultAsync(x => x.Id == id);
                return category;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task<Category?> GetBySlug(string slug)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var category = await _context.Categories
                    .Include(x => x.ParentCategory)
                    .Include(x => x.CategoryChildren)
                        .ThenInclude(x => x.CategoryChildren)
                    .FirstOrDefaultAsync(x => x.Slug == slug);
                return category;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        internal static async Task<List<Category>> GetChildren(string parentId)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var children = await _context.Categories
                                .Include(x => x.ParentCategory)
                                .Include(x => x.CategoryChildren)
                                    .ThenInclude(x => x.CategoryChildren)
                                .Where(x => x.ParentCategoryId == parentId)
                                .ToListAsync();
                return children;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
