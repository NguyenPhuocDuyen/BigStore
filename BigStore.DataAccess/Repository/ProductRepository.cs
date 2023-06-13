using BigStore.BusinessObject;
using BigStore.DataAccess.DAO;
using BigStore.DataAccess.Repository.IRepository;

namespace BigStore.DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        public async Task<List<Product>> GetProducts() => await ProductDAO.GetProducts();

        public async Task<List<Product>> GetProductsOfShopByUserId(string userId)
            => await ProductDAO.GetProductsOfShopByUserId(userId);

        public async Task<Product?> GetProductById(int id)
            => await ProductDAO.GetProductById(id);

        public async Task<Product?> GetProductBySlug(string slug)
            => await ProductDAO.GetProductBySlug(slug);

        public async Task Add(Product product) 
            => await ProductDAO.Add(product);

        public Task Update(Product product)
            => ProductDAO.Update(product);

        public Task Remove(int id)
            => ProductDAO.Remove(id);

        public Task RemoveImagesOfProduct(int id)
            => ProductDAO.RemoveImagesOfProduct(id);
    }
}
