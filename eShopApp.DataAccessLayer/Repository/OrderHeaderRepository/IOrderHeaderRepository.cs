using eShopApp.DataAccessLayer.Repository.GenericRepository;
using eShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.DataAccessLayer.Repository.OrderHeaderRepository
{
    public interface IOrderHeaderRepository: IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
    }
}
