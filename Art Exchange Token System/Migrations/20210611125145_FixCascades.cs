using Microsoft.EntityFrameworkCore.Migrations;

namespace Art_Exchange_Token_System.Migrations
{
    public partial class FixCascades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtTradeOffers_PendingArtTrades_PendingArtTradeId",
                table: "ArtTradeOffers");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtTradeOffers_PendingArtTrades_PendingArtTradeId",
                table: "ArtTradeOffers",
                column: "PendingArtTradeId",
                principalTable: "PendingArtTrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtTradeOffers_PendingArtTrades_PendingArtTradeId",
                table: "ArtTradeOffers");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtTradeOffers_PendingArtTrades_PendingArtTradeId",
                table: "ArtTradeOffers",
                column: "PendingArtTradeId",
                principalTable: "PendingArtTrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
