using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace купикота.рф.Migrations
{
    public partial class DEALSfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsConfirmedByOBuyer",
                table: "Deals",
                newName: "IsConfirmedByBuyer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsConfirmedByBuyer",
                table: "Deals",
                newName: "IsConfirmedByOBuyer");
        }
    }
}
