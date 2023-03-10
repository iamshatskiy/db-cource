using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace купикота.рф.Migrations
{
    public partial class new_t : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Cat_Photos",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Cat_Photos",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
