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
        public async Task Add(Category entity)
            => await CategoryDAO.Add(entity);

        public async Task<List<Category>> GetAll()
            => await CategoryDAO.GetAll();

        public async Task<Category?> GetById(string id)
            => await CategoryDAO.GetById(id);

        public async Task<Category?> GetBySlug(string slug)
            => await CategoryDAO.GetBySlug(slug);

        public async Task<List<Category>> GetChildren(string parentId)
            => await CategoryDAO.GetChildren(parentId);

        public async Task Remove(Category entity)
            => await CategoryDAO.Remove(entity);

        public async Task Update(Category entity)
            => await CategoryDAO.Update(entity);
    }
}
