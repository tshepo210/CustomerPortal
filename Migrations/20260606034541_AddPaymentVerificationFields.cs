using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentVerificationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SwiftReference",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerifiedById",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_VerifiedById",
                table: "Payments",
                column: "VerifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_VerifiedById",
                table: "Payments",
                column: "VerifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_VerifiedById",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_VerifiedById",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SwiftReference",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "VerifiedById",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
