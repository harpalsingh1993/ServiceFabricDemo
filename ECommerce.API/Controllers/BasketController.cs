using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Models;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors;
using UserActorService.Interfaces;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        [HttpGet("{userId}")]
        public async Task<ApiBasket> Get(string userId)
        {
            try
            {
                IUserActorService userActor = GetActor(userId);
                Dictionary<Guid, int> products = await userActor.GetBasket();
                return new ApiBasket()
                {
                    UserId = userId,
                    Items = products.Select(
                        p => new ApiBasketItem { ProductId = p.Key.ToString(), Quantity = p.Value }).ToArray()
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }
            

        }

        [HttpPost("{userId}")]
        public async Task Add(string userId,[FromBody] ApiBasketAddRequest request)
        {
            IUserActorService userActor = GetActor(userId);
            await userActor.AddToBasket(new Guid(request.ProductId),request.Qunatity);

        }

        [HttpDelete("{userId}")]
        public async Task Delete(string userId)
        {
            IUserActorService userActor = GetActor(userId);
            await userActor.ClearBasket();
        }

        private IUserActorService GetActor(string userId)
        {
            return ActorProxy.Create<IUserActorService>(
                new ActorId(userId),
                new Uri("fabric:/ECommerce/UserActorServiceActorService"));

        }
    }
}