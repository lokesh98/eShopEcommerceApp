using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.ViewModels
{
    public class AdminUserViewModel
    {
        public string Id { get; set; }
        public string? CurrentUserRole { get; set; }
        public UserRegistrationViewModel UserDetails { get; set; }
        public List<string>? Roles { get; set; }

        public IEnumerable<SelectListItem>? UserRoles { get; set; }
    }
}
