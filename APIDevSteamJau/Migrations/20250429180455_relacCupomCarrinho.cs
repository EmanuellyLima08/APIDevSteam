using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIDevSteamJau.Migrations
{
    /// <inheritdoc />
    public partial class relacCupomCarrinho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensCarrinhos_Carrinhos_CarrinhoId",
                table: "ItensCarrinhos");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensCarrinhos_Jogos_JogoId",
                table: "ItensCarrinhos");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensCarrinhos_Carrinhos_CarrinhoId",
                table: "ItensCarrinhos",
                column: "CarrinhoId",
                principalTable: "Carrinhos",
                principalColumn: "CarrinhoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensCarrinhos_Jogos_JogoId",
                table: "ItensCarrinhos",
                column: "JogoId",
                principalTable: "Jogos",
                principalColumn: "JogoId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensCarrinhos_Carrinhos_CarrinhoId",
                table: "ItensCarrinhos");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensCarrinhos_Jogos_JogoId",
                table: "ItensCarrinhos");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensCarrinhos_Carrinhos_CarrinhoId",
                table: "ItensCarrinhos",
                column: "CarrinhoId",
                principalTable: "Carrinhos",
                principalColumn: "CarrinhoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensCarrinhos_Jogos_JogoId",
                table: "ItensCarrinhos",
                column: "JogoId",
                principalTable: "Jogos",
                principalColumn: "JogoId");
        }
    }
}
