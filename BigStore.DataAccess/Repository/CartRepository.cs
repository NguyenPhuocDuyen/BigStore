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
    public class CartRepository : ICartRepository
    {
        public async Task Add(Cart entity)
            => await CartDAO.Add(entity);

        public async Task Remove(Cart entity)
            => await CartDAO.Remove(entity);

        public async Task<List<Cart>> GetAll()
            => await CartDAO.GetAll();

        public async Task<Cart?> GetById(string id)
            => await CartDAO.GetById(id);

        public async Task Update(Cart entity)
            => await CartDAO.Update(entity);
    }
}
