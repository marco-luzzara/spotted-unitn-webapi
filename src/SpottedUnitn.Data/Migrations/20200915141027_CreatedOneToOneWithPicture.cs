using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpottedUnitn.Data.Migrations
{
    public partial class CreatedOneToOneWithPicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "CoverPicture",
                table: "ShopCoverPicture",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldDefaultValue: new byte[] {  });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "CoverPicture",
                table: "ShopCoverPicture",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[] {  },
                oldClrType: typeof(byte[]));
        }
    }
}
