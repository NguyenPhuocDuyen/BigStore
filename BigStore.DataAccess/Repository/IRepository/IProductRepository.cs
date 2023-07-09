using BigStore.BusinessObject;

namespace BigStore.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetByShopId(string shopId);
        Task<Product?> GetBySlug(string slug);
    }
}
