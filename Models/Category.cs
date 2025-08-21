using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductMgmt.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection<CategoryAttributeDefinition> AttributeDefinitions { get; set; } 
            = new List<CategoryAttributeDefinition>();

        public ICollection<Product> Products { get; set; } 
            = new List<Product>();
    }
}
