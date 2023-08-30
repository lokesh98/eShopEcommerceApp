using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.Utility.DataSeeding
{
    public class DbInitializer : IDbInitializer
    {
        public readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(RoleManager<IdentityRole> roleManager)
        {
            _roleManager=roleManager;
        }
        public void Initialize()
        {
            if (!_roleManager.RoleExistsAsync(UserRoles.AdminUser).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole() { Name = UserRoles.AdminUser }).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole() { Name = UserRoles.Customer }).GetAwaiter().GetResult();
            }
        }
    }
}
