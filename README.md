# Product Management Tool

An internal ASP.NET Core MVC tool for managing a dynamic e-commerce product catalog with category-specific attributes.

---

## Design Justification

### Core Problem:
Products from different categories have **different attributes**. For example:
- Smartphones: RAM, Storage, OS
- Dresses: Size, Color, Material

Hardcoding these attributes in the Product table would break normalization and require frequent schema changes.

### Solution: **Entity-Attribute-Value (EAV) Pattern**
- **Scalability:** Add new categories without changing the DB schema.
- **Flexibility:** Each category has its own attributes via `CategoryAttributeDefinitions`.
- **Normalization:** Data is stored in separate tables:
  - `Category`
  - `CategoryAttributeDefinition`
  - `Product`
  - `ProductAttributeValue`
- **Trade-off:** Fetching a product with all attributes requires joins across tables.

---

## Database Structure

- **Category**
  - Id (PK)
  - Name
  - Slug
  - Description
  - CreatedAt
  - UpdatedAt

- **CategoryAttributeDefinition**
  - Id (PK)
  - Name
  - DataType (`string`, `number`, `date`, `bool`)
  - IsRequired
  - DisplayOrder
  - CategoryId (FK)

- **Product**
  - Id (PK)
  - Name
  - SKU
  - Price
  - CategoryId (FK)
  - CreatedAt
  - UpdatedAt

- **ProductAttributeValue**
  - Id (PK)
  - ProductId (FK)
  - CategoryAttributeDefinitionId (FK)
  - Value

---

## How to Run

1. Install **.NET 9.0 SDK** and **MySQL**.
2. Create a MySQL database: `ProductMgmtDb`.
3. Update `appsettings.json` connection string:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "server=localhost;database=ProductMgmtDb;user=root;password=YourPassword"
   }

4. Apply migrations:
   dotnet ef database update
5. Run the app:
   dotnet run
6. Navigate to https://localhost:5000 or To find in Terminal

- **Usage Flow**
  - Create Categories: Go to Categories → Create.
  - Define Attributes: On category details page, add attributes like RAM, Color, Size.
  - Create Products: Go to Products → Create. Select category, dynamic attribute form appears.
  - Edit/Details/Delete: Manage both categories and attributes.

- **Features**
  - Full CRUD for Products, Categories, Attributes.
  - Dynamic attribute forms per category.
  - Slug generation and timestamps.
  - Validation via ProductValidationService.
  - Normalized EAV schema for scalability.

- **Screenshots**
  - ./Docs/screenshot-dashboard.png
  - ./Docs/screenshot-product-creation.png
  - ./Docs/screenshot-category-creation.png

