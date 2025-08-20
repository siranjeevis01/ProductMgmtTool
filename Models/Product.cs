using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;      
using System.ComponentModel.DataAnnotations.Schema; 
using ProductMgmt.Models; 

namespace ProductMgmt.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        [Required]
        public string SKU { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue>();
    }
}
