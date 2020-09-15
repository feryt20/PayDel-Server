using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PayDel.Data.Migrations.DbCon
{
    public partial class ConcurrencyToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Photos",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Photos");
        }
    }
}
