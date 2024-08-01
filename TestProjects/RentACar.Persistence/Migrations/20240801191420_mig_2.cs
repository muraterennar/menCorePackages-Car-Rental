using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACar.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtpAuthentiacators_Users_UserId",
                table: "OtpAuthentiacators");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOperationCliams_OperationCliams_OperationClaimId",
                table: "UserOperationCliams");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOperationCliams_Users_UserId",
                table: "UserOperationCliams");

            migrationBuilder.DropIndex(
                name: "IX_EmailAuthenticators_UserId",
                table: "EmailAuthenticators");

            migrationBuilder.DropIndex(
                name: "IX_Cars_ModelId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOperationCliams",
                table: "UserOperationCliams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OtpAuthentiacators",
                table: "OtpAuthentiacators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OperationCliams",
                table: "OperationCliams");

            migrationBuilder.RenameTable(
                name: "UserOperationCliams",
                newName: "UserOperationClaims");

            migrationBuilder.RenameTable(
                name: "OtpAuthentiacators",
                newName: "OtpAuthenticators");

            migrationBuilder.RenameTable(
                name: "OperationCliams",
                newName: "OperationClaims");

            migrationBuilder.RenameIndex(
                name: "IX_UserOperationCliams_UserId",
                table: "UserOperationClaims",
                newName: "IX_UserOperationClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserOperationCliams_OperationClaimId",
                table: "UserOperationClaims",
                newName: "IX_UserOperationClaims_OperationClaimId");

            migrationBuilder.RenameIndex(
                name: "IX_OtpAuthentiacators_UserId",
                table: "OtpAuthenticators",
                newName: "IX_OtpAuthenticators_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OperationClaims",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOperationClaims",
                table: "UserOperationClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OtpAuthenticators",
                table: "OtpAuthenticators",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OperationClaims",
                table: "OperationClaims",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PasswordResets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UK_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Users_Name",
                table: "Users",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "UK_UserLogins_Name",
                table: "UserLogins",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_RefreshTokens_Id",
                table: "RefreshTokens",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_EmailAuthenticators_UserId",
                table: "EmailAuthenticators",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Cars_Id",
                table: "Cars",
                column: "ModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_UserOperationClaims_Name",
                table: "UserOperationClaims",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_OtpAuthenticators_Id",
                table: "OtpAuthenticators",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_OperationClaims_Id",
                table: "OperationClaims",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResets_UserId",
                table: "PasswordResets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UK_PasswordResets_Id",
                table: "PasswordResets",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OtpAuthenticators_Users_UserId",
                table: "OtpAuthenticators",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOperationClaims_OperationClaims_OperationClaimId",
                table: "UserOperationClaims",
                column: "OperationClaimId",
                principalTable: "OperationClaims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOperationClaims_Users_UserId",
                table: "UserOperationClaims",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtpAuthenticators_Users_UserId",
                table: "OtpAuthenticators");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOperationClaims_OperationClaims_OperationClaimId",
                table: "UserOperationClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOperationClaims_Users_UserId",
                table: "UserOperationClaims");

            migrationBuilder.DropTable(
                name: "PasswordResets");

            migrationBuilder.DropIndex(
                name: "UK_Users_Id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "UK_Users_Name",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "UK_UserLogins_Name",
                table: "UserLogins");

            migrationBuilder.DropIndex(
                name: "UK_RefreshTokens_Id",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "UK_EmailAuthenticators_UserId",
                table: "EmailAuthenticators");

            migrationBuilder.DropIndex(
                name: "UK_Cars_Id",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOperationClaims",
                table: "UserOperationClaims");

            migrationBuilder.DropIndex(
                name: "UK_UserOperationClaims_Name",
                table: "UserOperationClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OtpAuthenticators",
                table: "OtpAuthenticators");

            migrationBuilder.DropIndex(
                name: "UK_OtpAuthenticators_Id",
                table: "OtpAuthenticators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OperationClaims",
                table: "OperationClaims");

            migrationBuilder.DropIndex(
                name: "UK_OperationClaims_Id",
                table: "OperationClaims");

            migrationBuilder.RenameTable(
                name: "UserOperationClaims",
                newName: "UserOperationCliams");

            migrationBuilder.RenameTable(
                name: "OtpAuthenticators",
                newName: "OtpAuthentiacators");

            migrationBuilder.RenameTable(
                name: "OperationClaims",
                newName: "OperationCliams");

            migrationBuilder.RenameIndex(
                name: "IX_UserOperationClaims_UserId",
                table: "UserOperationCliams",
                newName: "IX_UserOperationCliams_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserOperationClaims_OperationClaimId",
                table: "UserOperationCliams",
                newName: "IX_UserOperationCliams_OperationClaimId");

            migrationBuilder.RenameIndex(
                name: "IX_OtpAuthenticators_UserId",
                table: "OtpAuthentiacators",
                newName: "IX_OtpAuthentiacators_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OperationCliams",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOperationCliams",
                table: "UserOperationCliams",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OtpAuthentiacators",
                table: "OtpAuthentiacators",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OperationCliams",
                table: "OperationCliams",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAuthenticators_UserId",
                table: "EmailAuthenticators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ModelId",
                table: "Cars",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_OtpAuthentiacators_Users_UserId",
                table: "OtpAuthentiacators",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOperationCliams_OperationCliams_OperationClaimId",
                table: "UserOperationCliams",
                column: "OperationClaimId",
                principalTable: "OperationCliams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOperationCliams_Users_UserId",
                table: "UserOperationCliams",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
