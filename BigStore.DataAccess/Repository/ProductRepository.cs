using BigStore.BusinessObject;
using BigStore.DataAccess.DAO;
using BigStore.DataAccess.Repository.IRepository;

namespace BigStore.DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        public async Task Add(Product entity)
            => await ProductDAO.Add(entity);

        public async Task<List<Product>> GetAll()
            => await ProductDAO.GetAll();

        public async Task<Product?> GetById(string id)
            => await ProductDAO.GetById(id);

        public async Task<List<Product>> GetByShopId(string shopId)
            => await ProductDAO.GetByShopId(shopId);

        public async Task<Product?> GetBySlug(string slug)
            => await ProductDAO.GetBySlug(slug);

        public async Task Remove(Product entity)
            => await ProductDAO.Remove(entity);

        public async Task Update(Product entity)
            => await ProductDAO.Update(entity);
    }
}
