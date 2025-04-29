using APIDevSteamJau.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APIDevSteamJau.Data
{
    public class APIContext : IdentityDbContext<Usuario>
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        { }

        // DbSet
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<JogoMidia> JogosMidia { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<JogoCategoria> JogosCategorias { get; set; }
        public DbSet<Carrinho> Carrinhos { get; set; }
        public DbSet<ItemCarrinho> ItensCarrinhos { get; set; }
        public DbSet<Cupom> Cupons { get; set; }
        public DbSet<CupomCarrinho> CuponsCarrinhos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Tabelas
            builder.Entity<Jogo>().ToTable("Jogos");
            builder.Entity<JogoMidia>().ToTable("JogosMidia");
            builder.Entity<Categoria>().ToTable("Categorias");
            builder.Entity<JogoCategoria>().ToTable("JogosCategorias");
            builder.Entity<Carrinho>().ToTable("Carrinhos");
            builder.Entity<ItemCarrinho>().ToTable("ItensCarrinhos");
            builder.Entity<Cupom>().ToTable("Cupons");
            builder.Entity<CupomCarrinho>().ToTable("CuponsCarrinhos");

            // Relacionamento entre Carrinho e ItemCarrinho
            builder.Entity<Carrinho>()
                .HasMany(c => c.Itens)  // Um carrinho tem muitos itens
                .WithOne(i => i.Carrinho)  // Cada item pertence a um carrinho
                .HasForeignKey(i => i.CarrinhoId)  // Chave estrangeira
                .OnDelete(DeleteBehavior.Cascade);  // Se o carrinho for excluído, seus itens serão removidos

            // Relacionamento entre ItemCarrinho e Jogo
            builder.Entity<ItemCarrinho>()
                .HasOne(i => i.Jogo)  // Cada item está associado a um jogo
                .WithMany()  // Um jogo pode ter muitos itens de carrinho
                .HasForeignKey(i => i.JogoId)  // Chave estrangeira
                .OnDelete(DeleteBehavior.SetNull);  // Caso o jogo seja removido, o campo JogoId será nulo
        }
    }
}
