using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddResellers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Customers");

            migrationBuilder.AddColumn<decimal>(
                name: "BaseDiscountRate",
                table: "Customers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionPercentage",
                table: "Customers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Customers",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ResellerId",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ResellerId",
                table: "Customers",
                column: "ResellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Customers_ResellerId",
                table: "Customers",
                column: "ResellerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Customers_ResellerId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_ResellerId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BaseDiscountRate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CommissionPercentage",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ResellerId",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
