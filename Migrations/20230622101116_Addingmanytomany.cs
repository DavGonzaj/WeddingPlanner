using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingPlanner.Migrations
{
    public partial class Addingmanytomany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserWeddingSignup",
                columns: table => new
                {
                    UserWeddingSignupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WeddingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWeddingSignup", x => x.UserWeddingSignupId);
                    table.ForeignKey(
                        name: "FK_UserWeddingSignup_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWeddingSignup_Weddings_WeddingId",
                        column: x => x.WeddingId,
                        principalTable: "Weddings",
                        principalColumn: "WeddingId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserWeddingSignup_UserId",
                table: "UserWeddingSignup",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWeddingSignup_WeddingId",
                table: "UserWeddingSignup",
                column: "WeddingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWeddingSignup");
        }
    }
}
