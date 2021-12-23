using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Week4.EsempioEF2.Entities;

namespace Week4.EsempioEF2.EF
{
    public class ImpiegatoConfiguration : IEntityTypeConfiguration<Impiegato>
    {
        public void Configure(EntityTypeBuilder<Impiegato> builder)
        {
            builder.ToTable("Impiegati");
            builder.HasKey(i => i.ImpiegatoID);
            builder.Property(i => i.Nome).IsRequired().HasMaxLength(50);
            builder.Property(i => i.Cognome).IsRequired().HasMaxLength(50);

            //RELAZIONI 
            // relazione Impiegati : Aziende = 1 : n
            builder.HasOne(i => i.Azienda).WithMany(a => a.Impiegati).HasForeignKey(i => i.AziendaID);
        }
    }
}