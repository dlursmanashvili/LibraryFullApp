using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "DeletedAt", "IsDeleted", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { "2c5e174e-3b0e-446f-86af-483d56fd7210", "11d16551-a780-477d-8b58-6091a7269cb3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "RootAdmin", "ROOTADMIN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "DeletedAt", "IsDeleted", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[] { "847aeaad-ec0e-4b85-b20e-1828fae116c9", "5fb7f941-9d65-4354-96ef-e201532c2f1f", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user", "USER", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "ConfirmationId", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsOorganisation", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PNumber", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0f8fad5b-d9cb-429f-a165-70867528350e", 0, "3bc03145-cc9e-4d45-9d5f-e38aa27a7a01", null, "SuperAdmin@gmail.com", false, "admin", true, false, "admin", false, null, "SUPERADMIN@GMAIL.COM", "SUPERADMIN@GMAIL.COM", "admin", "AQAAAAEAACcQAAAAEL9PxquaUB30HvHfKoBvT5a6X/YrmS7efzjNH6b+AoFZTLYBGDxjsO8xKOUI5iz7fQ==", null, false, "d14d334e-0893-403d-81d3-486d2700aafa", false, "SuperAdmin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2c5e174e-3b0e-446f-86af-483d56fd7210", "0f8fad5b-d9cb-429f-a165-70867528350e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "847aeaad-ec0e-4b85-b20e-1828fae116c9");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2c5e174e-3b0e-446f-86af-483d56fd7210", "0f8fad5b-d9cb-429f-a165-70867528350e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0f8fad5b-d9cb-429f-a165-70867528350e");
        }
    }
}
