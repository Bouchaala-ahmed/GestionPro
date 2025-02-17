using Microsoft.EntityFrameworkCore;

namespace GestionProAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employe> Employes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Ressource> Ressources { get; set; }
        public DbSet<Projet> Projets { get; set; }
    }
}

