using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEndAPI.Migrations
{
    /// <inheritdoc />
    public partial class DodavanjeTabeleHome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HomeID",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Home",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Home", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_HomeID",
                table: "Customers",
                column: "HomeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Home_HomeID",
                table: "Customers",
                column: "HomeID",
                principalTable: "Home",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Home_HomeID",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "Home");

            migrationBuilder.DropIndex(
                name: "IX_Customers_HomeID",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "HomeID",
                table: "Customers");
        }
    }
}
