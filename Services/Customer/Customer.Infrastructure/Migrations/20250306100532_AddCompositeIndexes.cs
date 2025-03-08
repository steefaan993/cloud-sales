using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompositeIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SoftwareLicenses_Vendor_SoftwareName",
                table: "SoftwareLicenses",
                columns: new[] { "Vendor", "SoftwareName" });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Name",
                table: "Accounts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Name_Department",
                table: "Accounts",
                columns: new[] { "Name", "Department" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SoftwareLicenses_Vendor_SoftwareName",
                table: "SoftwareLicenses");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Name",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Name_Department",
                table: "Accounts");
        }
    }
}
