using ProductMgmt.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductMgmt.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate(); // Apply migrations if not applied

            // Seed Categories
            if (!context.Categories.Any())
            {
                var smartphones = new Category
                {
                    Name = "Smartphones",
                    Slug = "smartphones",
                    Description = "All types of smartphones",
                    CreatedAt = DateTime.UtcNow
                };

                var dresses = new Category
                {
                    Name = "Dresses",
                    Slug = "dresses",
                    Description = "Fashion dresses for all occasions",
                    CreatedAt = DateTime.UtcNow
                };

                context.Categories.AddRange(smartphones, dresses);
                context.SaveChanges();
            }

            // Seed Category Attributes
            if (!context.CategoryAttributeDefinitions.Any())
            {
                var smartphoneCategory = context.Categories.First(c => c.Slug == "smartphones");
                var dressCategory = context.Categories.First(c => c.Slug == "dresses");

                var smartphoneAttributes = new List<CategoryAttributeDefinition>
                {
                    new() { Name = "RAM", DataType = "string", IsRequired = true, DisplayOrder = 1, CategoryId = smartphoneCategory.Id, CreatedAt = DateTime.UtcNow },
                    new() { Name = "Storage", DataType = "string", IsRequired = true, DisplayOrder = 2, CategoryId = smartphoneCategory.Id, CreatedAt = DateTime.UtcNow },
                    new() { Name = "OS", DataType = "string", IsRequired = true, DisplayOrder = 3, CategoryId = smartphoneCategory.Id, CreatedAt = DateTime.UtcNow },
                    new() { Name = "Battery", DataType = "string", IsRequired = false, DisplayOrder = 4, CategoryId = smartphoneCategory.Id, CreatedAt = DateTime.UtcNow },
                };

                var dressAttributes = new List<CategoryAttributeDefinition>
                {
                    new() { Name = "Size", DataType = "string", IsRequired = true, DisplayOrder = 1, CategoryId = dressCategory.Id, CreatedAt = DateTime.UtcNow },
                    new() { Name = "Color", DataType = "string", IsRequired = true, DisplayOrder = 2, CategoryId = dressCategory.Id, CreatedAt = DateTime.UtcNow },
                    new() { Name = "Material", DataType = "string", IsRequired = false, DisplayOrder = 3, CategoryId = dressCategory.Id, CreatedAt = DateTime.UtcNow },
                };

                context.CategoryAttributeDefinitions.AddRange(smartphoneAttributes);
                context.CategoryAttributeDefinitions.AddRange(dressAttributes);
                context.SaveChanges();
            }

            // Seed Products
            if (!context.Products.Any())
            {
                var smartphoneCategory = context.Categories.First(c => c.Slug == "smartphones");
                var dressCategory = context.Categories.First(c => c.Slug == "dresses");

                var products = new List<Product>
                {
                    new() { Name = "iPhone 15", SKU = "IP15", Price = 999.99m, CategoryId = smartphoneCategory.Id, CreatedAt = DateTime.UtcNow },
                    new() { Name = "Samsung Galaxy S25", SKU = "SGS25", Price = 899.99m, CategoryId = smartphoneCategory.Id, CreatedAt = DateTime.UtcNow },
                    new() { Name = "Red Dress", SKU = "RD01", Price = 79.99m, CategoryId = dressCategory.Id, CreatedAt = DateTime.UtcNow },
                    new() { Name = "Blue Dress", SKU = "BD01", Price = 69.99m, CategoryId = dressCategory.Id, CreatedAt = DateTime.UtcNow }
                };

                context.Products.AddRange(products);
                context.SaveChanges();

                // Seed Product Attribute Values
                var iphone = context.Products.First(p => p.SKU == "IP15");
                var s25 = context.Products.First(p => p.SKU == "SGS25");
                var redDress = context.Products.First(p => p.SKU == "RD01");
                var blueDress = context.Products.First(p => p.SKU == "BD01");

                var iphoneAttributes = context.CategoryAttributeDefinitions.Where(a => a.CategoryId == smartphoneCategory.Id).ToList();
                var dressAttributes = context.CategoryAttributeDefinitions.Where(a => a.CategoryId == dressCategory.Id).ToList();

                var productAttributeValues = new List<ProductAttributeValue>
                {
                    // iPhone 15
                    new() { ProductId = iphone.Id, CategoryAttributeDefinitionId = iphoneAttributes.First(a => a.Name == "RAM").Id, Value = "8GB" },
                    new() { ProductId = iphone.Id, CategoryAttributeDefinitionId = iphoneAttributes.First(a => a.Name == "Storage").Id, Value = "256GB" },
                    new() { ProductId = iphone.Id, CategoryAttributeDefinitionId = iphoneAttributes.First(a => a.Name == "OS").Id, Value = "iOS 19" },
                    new() { ProductId = iphone.Id, CategoryAttributeDefinitionId = iphoneAttributes.First(a => a.Name == "Battery").Id, Value = "3200mAh" },

                    // Samsung Galaxy S25
                    new() { ProductId = s25.Id, CategoryAttributeDefinitionId = iphoneAttributes.First(a => a.Name == "RAM").Id, Value = "12GB" },
                    new() { ProductId = s25.Id, CategoryAttributeDefinitionId = iphoneAttributes.First(a => a.Name == "Storage").Id, Value = "512GB" },
                    new() { ProductId = s25.Id, CategoryAttributeDefinitionId = iphoneAttributes.First(a => a.Name == "OS").Id, Value = "Android 15" },
                    new() { ProductId = s25.Id, CategoryAttributeDefinitionId = iphoneAttributes.First(a => a.Name == "Battery").Id, Value = "4500mAh" },

                    // Red Dress
                    new() { ProductId = redDress.Id, CategoryAttributeDefinitionId = dressAttributes.First(a => a.Name == "Size").Id, Value = "M" },
                    new() { ProductId = redDress.Id, CategoryAttributeDefinitionId = dressAttributes.First(a => a.Name == "Color").Id, Value = "Red" },
                    new() { ProductId = redDress.Id, CategoryAttributeDefinitionId = dressAttributes.First(a => a.Name == "Material").Id, Value = "Cotton" },

                    // Blue Dress
                    new() { ProductId = blueDress.Id, CategoryAttributeDefinitionId = dressAttributes.First(a => a.Name == "Size").Id, Value = "L" },
                    new() { ProductId = blueDress.Id, CategoryAttributeDefinitionId = dressAttributes.First(a => a.Name == "Color").Id, Value = "Blue" },
                    new() { ProductId = blueDress.Id, CategoryAttributeDefinitionId = dressAttributes.First(a => a.Name == "Material").Id, Value = "Silk" },
                };

                context.ProductAttributeValues.AddRange(productAttributeValues);
                context.SaveChanges();
            }
        }
    }
}
