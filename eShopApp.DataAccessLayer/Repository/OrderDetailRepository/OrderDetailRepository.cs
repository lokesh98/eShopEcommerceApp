using eShopApp.DataAccessLayer.Data;
using eShopApp.DataAccessLayer.Repository.CategoryRepository;
using eShopApp.DataAccessLayer.Repository.GenericRepository;
using eShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.DataAccessLayer.Repository.OrderDetailRepository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context= context;
        }
        public void Update(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
        }
    }
}
