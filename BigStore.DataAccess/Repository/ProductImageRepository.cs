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
    public class ProductImageRepository : IProductImageRepository
    {
        public async Task Add(ProductImage entity)
            => await ProductImageDAO.Add(entity);

        public async Task<List<ProductImage>> GetAll()
            => await ProductImageDAO.GetAll();

        public async Task<ProductImage?> GetById(string id)
            => await ProductImageDAO.GetById(id);

        public async Task Remove(ProductImage entity)
            => await ProductImageDAO.Remove(entity);

        public async Task Update(ProductImage entity)
            => await ProductImageDAO.Update(entity);
    }
}
