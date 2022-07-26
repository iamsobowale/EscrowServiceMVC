using Microsoft.EntityFrameworkCore.Migrations;

namespace EscrowService.Migrations
{
    public partial class thirdr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaidOut",
                table: "TransactionTypes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaidOut",
                table: "TransactionTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
