using eShopApp.DataAccessLayer.Repository.GenericRepository;
using eShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.DataAccessLayer.Repository.CategoryRepository
{
    public interface ICategoryRepository: IGenericRepository<Category>
    {
        void Update(Category category);
    }
}
