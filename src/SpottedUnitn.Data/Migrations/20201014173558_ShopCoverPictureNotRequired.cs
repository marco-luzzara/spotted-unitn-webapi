using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpottedUnitn.Data.Migrations
{
    public partial class ShopCoverPictureNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "CoverPicture",
                table: "ShopCoverPicture",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "CoverPicture",
                table: "ShopCoverPicture",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }
    }
}
