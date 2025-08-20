using ProductMgmt.Data;
using ProductMgmt.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductMgmt.Services
{
    public class ProductValidationService
    {
        private readonly ApplicationDbContext _context;

        public ProductValidationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public (bool IsValid, Dictionary<string, string> Errors) ValidateProduct(Product product, Dictionary<int, string> attributeValues)
        {
            var errors = new Dictionary<string, string>();

            // 1. Get the category and its attributes
            var category = _context.Categories
                .Include(c => c.AttributeDefinitions)
                .FirstOrDefault(c => c.Id == product.CategoryId);

            if (category == null)
            {
                errors.Add("Category", "Selected category is invalid.");
                return (false, errors);
            }

            var attributes = category.AttributeDefinitions.ToList();

            // 2. Check required attributes
            foreach (var attr in attributes.Where(a => a.IsRequired))
            {
                if (!attributeValues.ContainsKey(attr.Id) || string.IsNullOrWhiteSpace(attributeValues[attr.Id]))
                {
                    errors.Add($"attr_{attr.Id}", $"The field '{attr.Name}' is required.");
                }
            }

            // 3. Validate data types
            foreach (var kvp in attributeValues)
            {
                var attr = attributes.FirstOrDefault(a => a.Id == kvp.Key);
                if (attr == null) continue;

                var value = kvp.Value;
                if (string.IsNullOrWhiteSpace(value)) continue;

                switch (attr.DataType.ToLower())
                {
                    case "number":
                        if (!decimal.TryParse(value, out _))
                            errors.Add($"attr_{attr.Id}", $"The value for '{attr.Name}' must be a number.");
                        break;
                    case "date":
                        if (!DateTime.TryParse(value, out _))
                            errors.Add($"attr_{attr.Id}", $"The value for '{attr.Name}' must be a valid date.");
                        break;
                    case "bool":
                        if (!bool.TryParse(value, out _) && value != "1" && value != "0")
                            errors.Add($"attr_{attr.Id}", $"The value for '{attr.Name}' must be 'true' or 'false'.");
                        break;
                    // string needs no validation
                }
            }

            return (!errors.Any(), errors);
        }
    }
}
