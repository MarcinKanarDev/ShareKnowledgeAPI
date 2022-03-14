using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareKnowledgeAPI.Migrations
{
    public partial class removeBrainsProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brains",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Brains",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Brains",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Brains",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
