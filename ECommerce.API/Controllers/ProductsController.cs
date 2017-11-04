using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerce.ProductCatalog.Models;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;
using ECommerce.API.Models;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductCatalogService productCatalogService;

        public ProductsController()
        {
            productCatalogService = ServiceProxy.Create<IProductCatalogService>(new Uri("fabric:/ECommerce/ECommerce.ProductCatalog"), new ServicePartitionKey(0));
        }
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<APIProduct>> Get()
        {
            IEnumerable<Product> allProducts =await productCatalogService.GetAllProducts();

            return allProducts.Select(p=> new APIProduct
            {
                Id=p.Id,
                Name=p.Name
            });
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]APIProduct value)
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "SmartPhone"
            };

            await productCatalogService.AddProduct(product);

        }
    }
}
