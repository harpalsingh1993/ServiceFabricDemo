using ECommerce.ProductCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ProductCatalog
{
    interface IProductRepository
    {
        //reason to return task is most of the SF service called asynchronosly.
        Task<IEnumerable<Product>> GetAllProducts();
        Task AddProduct(Product product);
    }
}
