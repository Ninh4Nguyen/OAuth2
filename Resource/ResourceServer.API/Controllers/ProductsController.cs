using BusinessEntities;
using BusinessServices.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace ResourceServer.API.Controllers
{
    [RoutePrefix("api/products")]
    [Authorize]
    public class ProductsController : ApiController
    {
        private IProductServices _productServices;
        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
        }
        [Route("")]
        [Authorize(Roles = "admin, manager")]
        public IEnumerable<ProductEntity> GetAll()
        {
            return _productServices.GetAll();
        }
    }
}
