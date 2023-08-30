using eShopApp.DataAccessLayer.Data;
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
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _context;

        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICartRepository Cart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }

        public IOrderDetailRepository OrderDetail { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
            Cart = new CartRepository(context);
            OrderHeader = new OrderHeaderRepository(context);
            OrderDetail = new OrderDetailRepository(context);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool flag)
        {
            if (flag)
            {
                _context.Dispose();
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
