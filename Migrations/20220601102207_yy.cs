using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EscrowService.Migrations
{
    public partial class yy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReferenceNumber",
                table: "Payments",
                type: "text",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(16)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "ReferenceNumber",
                table: "Payments",
                type: "varbinary(16)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
