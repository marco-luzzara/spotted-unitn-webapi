using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpottedUnitn.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    LinkToSite = table.Column<string>(nullable: true, defaultValue: ""),
                    Description = table.Column<string>(nullable: false),
                    Location_Address = table.Column<string>(nullable: true),
                    Location_City = table.Column<string>(nullable: true),
                    Location_Province = table.Column<string>(nullable: true),
                    Location_PostalCode = table.Column<string>(maxLength: 16, nullable: true),
                    Location_Latitude = table.Column<float>(nullable: true),
                    Location_Longitude = table.Column<float>(nullable: true),
                    CoverPicture = table.Column<byte[]>(nullable: true, defaultValue: new byte[] {  }),
                    Discount = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Credentials_Mail = table.Column<string>(type: "NVARCHAR(320)", maxLength: 320, nullable: true),
                    Credentials_HashedPwd = table.Column<string>(type: "VARCHAR(72)", maxLength: 72, nullable: true),
                    ProfilePhoto = table.Column<byte[]>(nullable: false),
                    SubscriptionDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex("IX_Credentials_Email", "Users", "Credentials_Mail", unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_Credentials_Email", table: "Users");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
