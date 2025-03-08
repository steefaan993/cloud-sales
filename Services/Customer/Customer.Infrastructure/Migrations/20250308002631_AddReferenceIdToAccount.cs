using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReferenceIdToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionTerm",
                table: "SoftwareLicenses");

            migrationBuilder.AddColumn<Guid>(
                name: "ReferenceId",
                table: "SoftwareLicenses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareLicenses_ReferenceId",
                table: "SoftwareLicenses",
                column: "ReferenceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SoftwareLicenses_ReferenceId",
                table: "SoftwareLicenses");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "SoftwareLicenses");

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionTerm",
                table: "SoftwareLicenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
