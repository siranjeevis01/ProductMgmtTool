using System;
using System.ComponentModel.DataAnnotations;

namespace ProductMgmt.Models
{
    public class CategoryAttributeDefinition
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public bool IsRequired { get; set; }

        public int DisplayOrder { get; set; }

        [Required(ErrorMessage = "Data Type is required")]
        public string DataType { get; set; } = "string";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
