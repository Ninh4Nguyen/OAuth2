using BusinessEntities;
using System.Collections.Generic;

namespace BusinessServices.Services
{
    public interface IProductServices
    {
        ProductEntity GetById(int id);
        IEnumerable<ProductEntity> GetAll();
    }
}
