using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpottedUnitn.Data.Migrations
{
    public partial class SplitTableForImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePhoto",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CoverPicture",
                table: "Shops");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Shops",
                type: "VARCHAR(15)",
                maxLength: 15,
                nullable: true,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ShopCoverPicture",
                columns: table => new
                {
                    ShopId = table.Column<int>(nullable: false),
                    CoverPicture = table.Column<byte[]>(nullable: false, defaultValue: new byte[] {  })
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopCoverPicture", x => x.ShopId);
                    table.ForeignKey(
                        name: "FK_ShopCoverPicture_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfilePhoto",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ProfilePhoto = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfilePhoto", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserProfilePhoto_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopCoverPicture");

            migrationBuilder.DropTable(
                name: "UserProfilePhoto");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Shops");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePhoto",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[] {  });

            migrationBuilder.AddColumn<byte[]>(
                name: "CoverPicture",
                table: "Shops",
                type: "varbinary(max)",
                nullable: true,
                defaultValue: new byte[] {  });
        }
    }
}
