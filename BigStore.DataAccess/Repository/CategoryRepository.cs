using BigStore.BusinessObject;
using BigStore.DataAccess.DAO;
using BigStore.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigStore.DataAccess.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public async Task<List<Category>> GetCategories() => await CategoryDAO.GetCategories();
        public async Task<Category?> GetCategoryById(int id) => await CategoryDAO.GetCategoryById(id);
        public async Task<Category?> GetCategoryBySlug(string slug) => await CategoryDAO.GetCategoryBySlug(slug);
        public async Task Add(Category category) => await CategoryDAO.Add(category);
        public async Task Update(Category category) => await CategoryDAO.Update(category);
        public async Task Remove(int id) => await CategoryDAO.Remove(id);

        public async Task<List<Category>> GetChildCategories(int parentId) => await CategoryDAO.GetChildCategories(parentId);
    }
}
