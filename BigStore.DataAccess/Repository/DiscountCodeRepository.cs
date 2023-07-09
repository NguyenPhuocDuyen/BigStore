using BigStore.BusinessObject;
using BigStore.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigStore.DataAccess.Repository
{
    public class DiscountCodeRepository : IDiscountCodeRepository
    {
        public Task Add(DiscountCode entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<DiscountCode>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DiscountCode?> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task Remove(DiscountCode entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(DiscountCode entity)
        {
            throw new NotImplementedException();
        }
    }
}
