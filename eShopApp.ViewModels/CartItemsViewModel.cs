using eShopApp.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.ViewModels
{
    public class CartItemsViewModel
    {
        public Guid CartId { get; set; }
        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; }

        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [ValidateNever]
        public IEnumerable<CartItem> ListOfCartItems { get; set; }
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
    }
}
