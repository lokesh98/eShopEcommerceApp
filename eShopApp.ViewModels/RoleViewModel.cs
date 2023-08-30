using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.ViewModels
{
    public class RoleViewModel
    {
        public string RoleId { get;set; }
        [Required]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
