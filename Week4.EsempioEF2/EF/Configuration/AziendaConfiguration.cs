using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Week4.EsempioEF2.Entities;

namespace Week4.EsempioEF2.EF
{
    public class AziendaConfiguration : IEntityTypeConfiguration<Azienda>
    {
        public void Configure(EntityTypeBuilder<Azienda> builder)
        {
            builder.ToTable("Aziende"); // Specifico il nome alla tabella
            builder.HasKey(a => a.AziendaID); // Specifico il campo che voglio come PK 
            // Specifico che il campo Nome è obbligatorio
            // builder.Property("Nome").IsRequired();
            builder.Property(a => a.Nome).IsRequired();

            // RELAZIONI
            // relazione Aziende : Impiegati = n : 1
            builder.HasMany(a => a.Impiegati).WithOne(i => i.Azienda).HasForeignKey(i => i.AziendaID);
        }
    }
}