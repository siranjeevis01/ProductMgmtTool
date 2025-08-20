using System;
using System.Collections.Generic;
using ProductMgmt.Models;

namespace ProductMgmt.Models
{
    public class ProductAttributeValue
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int CategoryAttributeDefinitionId { get; set; }
        public CategoryAttributeDefinition? CategoryAttributeDefinition { get; set; }

        public string Value { get; set; } = string.Empty;
    }
}
