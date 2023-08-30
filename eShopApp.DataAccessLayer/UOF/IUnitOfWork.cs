using eShopApp.DataAccessLayer.Repository.CartRepository;
using eShopApp.DataAccessLayer.Repository.CategoryRepository;
using eShopApp.DataAccessLayer.Repository.OrderDetailRepository;
using eShopApp.DataAccessLayer.Repository.OrderHeaderRepository;
using eShopApp.DataAccessLayer.Repository.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.DataAccessLayer.UOF
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICartRepository Cart { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }
        void Save();
    }
}
