using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaumKantin.Repository.Migrations
{
    public partial class MigDataAdding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreateDate", "IdentityId", "ImageId", "Name", "Phone", "RoomId", "Surname", "UpdateDate", "UserTypeEnum" },
                values: new object[,]
                {
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 12, "Ahmet", "6852574", null, "Aydın", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)2 },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 14, "Kerem", "6852574", null, "Yıldırım", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)2 }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "CreateDate", "Floor", "Number", "UpdateDate" },
                values: new object[,]
                {
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "209", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "306", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
