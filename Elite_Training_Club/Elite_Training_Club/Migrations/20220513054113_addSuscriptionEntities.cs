using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elite_Training_Club.Migrations
{
    public partial class addSuscriptionEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionsPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionsId = table.Column<int>(type: "int", nullable: true),
                    PlanId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionsPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionsPlans_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubscriptionsPlans_Subscriptions_SubscriptionsId",
                        column: x => x.SubscriptionsId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Name",
                table: "Subscriptions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionsPlans_PlanId",
                table: "SubscriptionsPlans",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionsPlans_SubscriptionsId_PlanId",
                table: "SubscriptionsPlans",
                columns: new[] { "SubscriptionsId", "PlanId" },
                unique: true,
                filter: "[SubscriptionsId] IS NOT NULL AND [PlanId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionsPlans");

            migrationBuilder.DropTable(
                name: "Subscriptions");
        }
    }
}
