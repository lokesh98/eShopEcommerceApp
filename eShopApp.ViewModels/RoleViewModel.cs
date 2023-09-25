using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        [ValidateNever]
        public string RoleId { get;set; }
        [Required]
        public string RoleName { get; set; }
        [ValidateNever]
        public List<string> Users { get; set; }
    }
}
