using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week4.EsempioEF1.Entities;

namespace Week4.EsempioEF1.EF
{
    public class Context : DbContext
    {
        public DbSet<Impiegato> Impiegati { get; set; }
        public DbSet<Azienda> Aziende { get; set; }
        // Costruttore base per il padre
        public Context() : base() { }
        // Costruttore per il padre con opzioni (ci servirà per specificare la stringa di connessione)
        public Context(DbContextOptions<Context> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionStringSQL = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog=EsempioAziendaEF; Integrated Security=True; Connect Timeout=30; Encrypt=False; TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                optionsBuilder.UseSqlServer(connectionStringSQL);
            }
        }
    }
}
