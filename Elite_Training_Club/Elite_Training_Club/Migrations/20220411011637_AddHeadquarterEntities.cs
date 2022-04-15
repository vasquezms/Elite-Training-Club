using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elite_Training_Club.Migrations
{
    public partial class AddHeadquarterEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Headquarters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headquarters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Headquarters_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Headquarters_CityId",
                table: "Headquarters",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Headquarters_Name_CityId",
                table: "Headquarters",
                columns: new[] { "Name", "CityId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Headquarters");
        }
    }
}
