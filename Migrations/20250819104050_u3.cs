using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductMgmtTool.Migrations
{
    /// <inheritdoc />
    public partial class u3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValues_CategoryAttributeDefinitions_Attribut~",
                table: "ProductAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_Products_SKU",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Slug",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "ValueBool",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "ValueDate",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "ValueDecimal",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "ValueInt",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "ValueJson",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "ValueString",
                table: "ProductAttributeValues");

            migrationBuilder.RenameColumn(
                name: "AttributeDefinitionId",
                table: "ProductAttributeValues",
                newName: "CategoryAttributeDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeValues_AttributeDefinitionId",
                table: "ProductAttributeValues",
                newName: "IX_ProductAttributeValues_CategoryAttributeDefinitionId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldMaxLength: 300)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "ProductAttributeValues",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValues_CategoryAttributeDefinitions_Category~",
                table: "ProductAttributeValues",
                column: "CategoryAttributeDefinitionId",
                principalTable: "CategoryAttributeDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValues_CategoryAttributeDefinitions_Category~",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "ProductAttributeValues");

            migrationBuilder.RenameColumn(
                name: "CategoryAttributeDefinitionId",
                table: "ProductAttributeValues",
                newName: "AttributeDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeValues_CategoryAttributeDefinitionId",
                table: "ProductAttributeValues",
                newName: "IX_ProductAttributeValues_AttributeDefinitionId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "Products",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductAttributeValues",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductAttributeValues",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ValueBool",
                table: "ProductAttributeValues",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValueDate",
                table: "ProductAttributeValues",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValueDecimal",
                table: "ProductAttributeValues",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ValueInt",
                table: "ProductAttributeValues",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValueJson",
                table: "ProductAttributeValues",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ValueString",
                table: "ProductAttributeValues",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                table: "Products",
                column: "SKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValues_CategoryAttributeDefinitions_Attribut~",
                table: "ProductAttributeValues",
                column: "AttributeDefinitionId",
                principalTable: "CategoryAttributeDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
