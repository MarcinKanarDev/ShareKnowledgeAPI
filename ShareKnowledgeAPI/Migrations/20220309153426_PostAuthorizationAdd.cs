using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareKnowledgeAPI.Migrations
{
    public partial class PostAuthorizationAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CreatedById",
                table: "Posts",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_CreatedById",
                table: "Posts",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_CreatedById",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CreatedById",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Posts");
        }
    }
}
