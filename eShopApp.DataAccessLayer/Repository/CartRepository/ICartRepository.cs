using eShopApp.DataAccessLayer.Repository.GenericRepository;
using eShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.DataAccessLayer.Repository.CartRepository
{
    public interface ICartRepository :IGenericRepository<CartItem>
    {
        void Update(CartItem entity);
        void IncrementCartItems(CartItem entity, int counter);
    }
}
