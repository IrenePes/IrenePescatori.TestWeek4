using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week4.EsempioEF2.Entities;

namespace Week4.EsempioEF2.EF
{
    public class Context : DbContext
    {
        public DbSet<Impiegato> Impiegati { get; set; }
        public DbSet<Azienda> Aziende { get; set; }
        
        public Context() : base() { }
        
        public Context(DbContextOptions<Context> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionStringSQL = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog=EsempioAziendaFluentAPI; Integrated Security=True; Connect Timeout=30; Encrypt=False; TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                optionsBuilder.UseSqlServer(connectionStringSQL);
            }
        }

        // Devo fare un override che contenga tutte le configurazioni
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Impiegato>(new ImpiegatoConfiguration());
            modelBuilder.ApplyConfiguration<Azienda>(new AziendaConfiguration());
        }
    }
}