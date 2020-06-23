using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportsStore.Models {
    public class Order {
        [BindNever]
        public long OrderId { get; set; }

        [Required]
        public string Name { get; set; }
        public IEnumerable<CartLine> Products { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public Payment Payment { get; set; }

        [BindNever]
        public bool Shipped { get; set; } = false;
    }
}