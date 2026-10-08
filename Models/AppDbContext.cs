using Microsoft.EntityFrameworkCore;
using Pharmacie2.Services;

namespace Pharmacie2.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Vente> ventes { get; set; }
        public DbSet<LigneVente> LigneVentes { get; set; }
        public DbSet<Produit> produits { get; set; }
        public DbSet<Fournisseur> fournisseur { get; set; }
        public DbSet<Commande> commandes { get; set; }
        public DbSet<LigneCommande> LigneCommandes { get; set; }
        public DbSet<Mutuel> mutuels { get; set; }
        public DbSet<Paiement> paiement { get; set; }
        public DbSet<SessionCaisse> SessionsCaisse { get; set; }
        public DbSet<MutuelPaiement> MutuelPaiements { get; set; }
        public DbSet<DepenseAnnexe> DepensesAnnexes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={CheminsApp.CheminBase}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vente>()
                .HasOne(v => v.Mutuel).WithMany()
                .HasForeignKey(v => v.MutuelId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Vente>()
                .HasOne(v => v.User).WithMany()
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Paiement>()
                .HasOne(p => p.User).WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Produit>()
                .HasOne(p => p.Fournisseur)
                .WithMany(f => f.Produits)
                .HasForeignKey(p => p.FournisseurId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<SessionCaisse>()
                .HasOne(s => s.User).WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<MutuelPaiement>()
                .HasOne(mp => mp.Mutuel).WithMany()
                .HasForeignKey(mp => mp.MutuelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MutuelPaiement>()
                .HasOne(mp => mp.User).WithMany()
                .HasForeignKey(mp => mp.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // ← AJOUT : relation DepenseAnnexe → User
            modelBuilder.Entity<DepenseAnnexe>()
                .HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull); // NO ACTION en base (identique à l'existant)
        }
    }
}