using BigStore.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigStore.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories();
        Task<Category?> GetCategoryById(int id);
        Task<Category?> GetCategoryBySlug(string slug);
        Task Add(Category category);
        Task Update(Category category);
        Task Remove(int id);

        Task<List<Category>> GetChildCategories(int parentId);
    }
}
