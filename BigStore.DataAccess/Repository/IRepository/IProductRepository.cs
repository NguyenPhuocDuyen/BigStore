using BigStore.BusinessObject;

namespace BigStore.DataAccess.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();
        Task<List<Product>> GetProductsOfShopByUserId(string userId);
        Task<Product?> GetProductById(int id);
        Task<Product?> GetProductBySlug(string slug);
        Task Add(Product product);
        Task Update(Product product);
        Task Remove(int id);

        Task RemoveImagesOfProduct(int id);
    }
}
