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
    public class CategoryDAO
    {
        public static async Task<List<Category>> GetCategories()
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var categories = await _context.Categories
                    .Include(x => x.ParentCategory)
                    .Include(x => x.CategoryChildren)
                    .ThenInclude(x => x.CategoryChildren)
                    .Where(x => x.ParentCategory == null)
                    .AsNoTracking()
                    .ToListAsync();
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<Category?> GetCategoryById(int id)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var category = await _context.Categories
                    .Include(x => x.ParentCategory)
                    .Include(x => x.CategoryChildren)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<Category?> GetCategoryBySlug(string slug)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var category = await _context.Categories
                    .Include(x => x.ParentCategory)
                    .FirstOrDefaultAsync(x => x.Slug == slug);
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task Add(Category category)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task Update(Category category)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task Remove(int id)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var cateDb = _context.Categories
                    .Include(x => x.ParentCategory)
                    .Include(x => x.CategoryChildren)
                    .FirstOrDefault(x => x.Id == id);
                if (cateDb != null)
                {
                    if (cateDb.CategoryChildren != null)
                    {
                        foreach (var childCategory in cateDb.CategoryChildren)
                        {
                            childCategory.ParentCategoryId = cateDb.ParentCategoryId;
                        }
                    }
                    _context.Categories.Remove(cateDb);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<Category>> GetChildCategories(int parentId)
        {
            try
            {
                using var _context = new ApplicationDbContext();
                var categories = await _context.Categories.AsNoTracking()
                                .Include(x => x.CategoryChildren)
                                .Where(x => x.ParentCategoryId == parentId)
                                .ToListAsync();
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
