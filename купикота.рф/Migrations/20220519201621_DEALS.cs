using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace купикота.рф.Migrations
{
    public partial class DEALS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AdvertId = table.Column<int>(nullable: false),
                    BuyerId = table.Column<string>(nullable: true),
                    IsConfirmedByOBuyer = table.Column<sbyte>(nullable: false),
                    IsConfirmedByOWner = table.Column<sbyte>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deals_Cat_Adverts_AdvertId",
                        column: x => x.AdvertId,
                        principalTable: "Cat_Adverts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deals_AspNetUsers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deals_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deals_AdvertId",
                table: "Deals",
                column: "AdvertId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_BuyerId",
                table: "Deals",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_OwnerId",
                table: "Deals",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deals");
        }
    }
}
