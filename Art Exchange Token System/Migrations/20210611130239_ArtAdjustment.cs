using Microsoft.EntityFrameworkCore.Migrations;

namespace Art_Exchange_Token_System.Migrations
{
    public partial class ArtAdjustment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtData_ArtTradeOffers_ArtTradeOfferId",
                table: "ArtData");

            migrationBuilder.DropIndex(
                name: "IX_ArtData_ArtTradeOfferId",
                table: "ArtData");

            migrationBuilder.DropColumn(
                name: "ArtTradeOfferId",
                table: "ArtData");

            migrationBuilder.CreateTable(
                name: "ArtDataArtTradeOffer",
                columns: table => new
                {
                    ArtTradeOffersId = table.Column<long>(type: "bigint", nullable: false),
                    OferredArtDatasId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtDataArtTradeOffer", x => new { x.ArtTradeOffersId, x.OferredArtDatasId });
                    table.ForeignKey(
                        name: "FK_ArtDataArtTradeOffer_ArtData_OferredArtDatasId",
                        column: x => x.OferredArtDatasId,
                        principalTable: "ArtData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtDataArtTradeOffer_ArtTradeOffers_ArtTradeOffersId",
                        column: x => x.ArtTradeOffersId,
                        principalTable: "ArtTradeOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtDataArtTradeOffer_OferredArtDatasId",
                table: "ArtDataArtTradeOffer",
                column: "OferredArtDatasId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtDataArtTradeOffer");

            migrationBuilder.AddColumn<long>(
                name: "ArtTradeOfferId",
                table: "ArtData",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtData_ArtTradeOfferId",
                table: "ArtData",
                column: "ArtTradeOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtData_ArtTradeOffers_ArtTradeOfferId",
                table: "ArtData",
                column: "ArtTradeOfferId",
                principalTable: "ArtTradeOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
