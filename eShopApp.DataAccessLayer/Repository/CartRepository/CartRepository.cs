using eShopApp.DataAccessLayer.Data;
using eShopApp.DataAccessLayer.Repository.GenericRepository;
using eShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.DataAccessLayer.Repository.CartRepository
{
    public class CartRepository : GenericRepository<CartItem>, ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context= context;
        }

        public void IncrementCartItems(CartItem entity, int counter)
        {
            var _cart =_context.CartItems.FirstOrDefault(x=>x.CartId == entity.CartId);
            if (_cart != null)
            {
                _cart.Count += counter;
            }
        }

        public void Update(CartItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
