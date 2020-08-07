using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Groupement_Citoyen.Models
{
    public class GroupementCitoyenDbContext : IdentityDbContext<Utilisateur>
    {
        public GroupementCitoyenDbContext(DbContextOptions<GroupementCitoyenDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder Builder)
        {
            base.OnModelCreating(Builder);
            Builder.Entity<Commande>().Property(t => t.DateAchat).HasDefaultValueSql("GetDate()");
            Builder.Entity<Produit>().Property(p => p.Id).ValueGeneratedOnAdd();
        }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<DetailsCommande> DetailsCommandes { get; set; }
        public DbSet<Commande> Commandes { get; set; }

    }
}
