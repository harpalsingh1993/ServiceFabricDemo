using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Models
{
    public class ApiBasketAddRequest
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("qunatity")]
        public int Qunatity { get; set; }

    }
}
