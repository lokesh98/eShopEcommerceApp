using eShopApp.DataAccessLayer.Repository.GenericRepository;
using eShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.DataAccessLayer.Repository.ProductRepository
{
    public interface IProductRepository: IGenericRepository<Product>
    {
        void Update(Product product);
    }
}
