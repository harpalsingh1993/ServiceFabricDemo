using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.ProductCatalog.Models;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System.Threading;

namespace ECommerce.ProductCatalog
{
    class ProductRepository : IProductRepository
    {
        private IReliableStateManager _stateManager;
        public ProductRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }
        async Task  IProductRepository.AddProduct(Product product)
        {
            var products = await this._stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");
            using (var tx = _stateManager.CreateTransaction())
            {
                await products.AddOrUpdateAsync(tx,product.Id,product,(id,value)=>  product);
                await tx.CommitAsync();
            }
        }

       async Task<IEnumerable<Product>> IProductRepository.GetAllProducts()
        {
            var products = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");
            var result = new List<Product>();
            using (var tx = _stateManager.CreateTransaction())
            {
                var allProducts = await products.CreateEnumerableAsync(tx,EnumerationMode.Unordered);
                using (var enumerator = allProducts.GetAsyncEnumerator())
                {
                    while(await enumerator.MoveNextAsync(cancellationToken: CancellationToken.None))
                    {
                        KeyValuePair<Guid, Product> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }
            return result;
        }
    }
}
