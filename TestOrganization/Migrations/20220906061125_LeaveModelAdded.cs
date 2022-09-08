using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestOrganization.Migrations
{
    public partial class LeaveModelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf1d44f1-afdf-475e-ae95-ab5e228f2be4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d35088bd-21d5-4bba-a719-53e00d9803f9");

            migrationBuilder.DeleteData(
                table: "Designations",
                keyColumn: "Id",
                keyValue: "2492692f-3e16-4071-a151-b26b6bb77925");

            migrationBuilder.DeleteData(
                table: "Designations",
                keyColumn: "Id",
                keyValue: "3bcf33ba-cbf0-4190-906b-8d44c53bec5d");

            migrationBuilder.DeleteData(
                table: "Designations",
                keyColumn: "Id",
                keyValue: "d63b1b97-a2ef-41cd-888a-acc99c70d33f");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "c49d225f-4da1-4580-962a-4f8c785f5995");

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeaveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.EmployeeId);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "08080318-ae16-4098-8390-1d374321befa", "39546796-9bb4-4a25-9c06-e4bf98987e03", "User", "USER" },
                    { "9fd1ca63-88c2-4e48-bd71-6d9fd01c61f7", "7cd82bc2-2f1c-4648-a3bd-3b9bcd584049", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Designations",
                columns: new[] { "Id", "CallName", "Name" },
                values: new object[,]
                {
                    { "3db25d33-b47f-49dc-8583-8a5a32c8d130", "Developer", "Developer" },
                    { "4be2bdf1-05d1-439f-b6b9-1e40cd9526ab", "PM", "Project Manager" },
                    { "f615e9dc-9ffc-4477-a1da-f1dbecd4773e", "TL", "Team Lead" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OrganizationId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "838bc12f-a63c-4690-a653-c056fd685575", 0, "49c7767c-def5-477a-92c8-28b2dc5c19b1", "superadmin@gmail.com", false, true, false, null, "SUPERADMIN@GMAIL.COM", "ADMIN", null, "AQAAAAEAACcQAAAAEKCMump2gqcmfjNgDs9op7PWQ5wB6f0K4m6sY1Cw1pvFbPYhSsYklwcNwkdW6VemZg==", null, false, "cc2b15d3-b7b7-49bd-99b1-5e282e694cda", false, "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "08080318-ae16-4098-8390-1d374321befa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fd1ca63-88c2-4e48-bd71-6d9fd01c61f7");

            migrationBuilder.DeleteData(
                table: "Designations",
                keyColumn: "Id",
                keyValue: "3db25d33-b47f-49dc-8583-8a5a32c8d130");

            migrationBuilder.DeleteData(
                table: "Designations",
                keyColumn: "Id",
                keyValue: "4be2bdf1-05d1-439f-b6b9-1e40cd9526ab");

            migrationBuilder.DeleteData(
                table: "Designations",
                keyColumn: "Id",
                keyValue: "f615e9dc-9ffc-4477-a1da-f1dbecd4773e");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "838bc12f-a63c-4690-a653-c056fd685575");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cf1d44f1-afdf-475e-ae95-ab5e228f2be4", "5e497bd5-d2e9-442d-9e4a-f0fcd2687f5e", "Admin", "ADMIN" },
                    { "d35088bd-21d5-4bba-a719-53e00d9803f9", "f3dac464-2129-42e9-9d0d-1d0bf7e702d8", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "Designations",
                columns: new[] { "Id", "CallName", "Name" },
                values: new object[,]
                {
                    { "2492692f-3e16-4071-a151-b26b6bb77925", "Developer", "Developer" },
                    { "3bcf33ba-cbf0-4190-906b-8d44c53bec5d", "TL", "Team Lead" },
                    { "d63b1b97-a2ef-41cd-888a-acc99c70d33f", "PM", "Project Manager" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OrganizationId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c49d225f-4da1-4580-962a-4f8c785f5995", 0, "769feaa5-4f0c-4540-bdc3-77f925f7fa49", "superadmin@gmail.com", false, true, false, null, "SUPERADMIN@GMAIL.COM", "ADMIN", null, "AQAAAAEAACcQAAAAEM63sSJOeukhK5PENghzNXAT7Ity24IAlPI4M2iEXIIE3KuaUE/OESQXTCvrOURFlA==", null, false, "f3a11996-e122-45ec-b019-8625cfd01276", false, "Admin" });
        }
    }
}
