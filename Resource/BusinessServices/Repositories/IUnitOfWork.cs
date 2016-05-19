using DataModel.Models;

namespace BusinessServices.Repositories
{
    public interface IUnitOfWork
    {
        GenericRepository<Product> ProductRepository();
        void Save();
    }
}
