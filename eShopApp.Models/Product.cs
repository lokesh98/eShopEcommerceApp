using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [Column(TypeName="varchar(200)")]
        public string ProductName { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string? ProductDescription { get; set; }
        public double Price { get; set; }
        [ValidateNever]
        public string ProductImage { get; set; }
        public int StockQty { get; set; }
        public int CategoryId { get; set; } //FK
        [ValidateNever]
        public virtual Category Category { get; set; }
    }
}
