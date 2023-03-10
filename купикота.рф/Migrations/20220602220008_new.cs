using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace купикота.рф.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "Feedbacks",
                nullable: false,
                oldClrType: typeof(sbyte));

            migrationBuilder.AlterColumn<int>(
                name: "IsConfirmedByOWner",
                table: "Deals",
                nullable: false,
                oldClrType: typeof(sbyte));

            migrationBuilder.AlterColumn<int>(
                name: "IsConfirmedByBuyer",
                table: "Deals",
                nullable: false,
                oldClrType: typeof(sbyte));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<sbyte>(
                name: "Rate",
                table: "Feedbacks",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<sbyte>(
                name: "IsConfirmedByOWner",
                table: "Deals",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<sbyte>(
                name: "IsConfirmedByBuyer",
                table: "Deals",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
