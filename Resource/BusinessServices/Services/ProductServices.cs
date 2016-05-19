using AutoMapper;
using BusinessEntities;
using BusinessServices.Repositories;
using DataModel.Models;
using System.Collections.Generic;
using System.Linq;

namespace BusinessServices.Services
{
    public class ProductServices : IProductServices
    {
        private IUnitOfWork _unitOfWork;
        public ProductServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ProductEntity GetById(int id)
        {
            var product = _unitOfWork.ProductRepository().GetById(id);
            if(product != null)
            {
                Mapper.CreateMap<Product, ProductEntity>();
                var productEntity = Mapper.Map<Product, ProductEntity>(product);
                return productEntity;
            }
            return null;
        }
        public IEnumerable<ProductEntity> GetAll()
        {
            List<Product> products = _unitOfWork.ProductRepository().GetAll().ToList();
            if(products.Any())
            {
                Mapper.CreateMap<Product, ProductEntity>();
                var productsEntity = Mapper.Map<List<Product>, List<ProductEntity>>(products);
                return productsEntity;
            }
            return null;
        }
    }
}
