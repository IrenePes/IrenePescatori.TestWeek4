using System.ComponentModel.DataAnnotations;

namespace Week4.EsempioEF1.Entities
{
    public class Azienda
    {
        public int AziendaID { get; set; }

        [MaxLength(50)]
        public string Nome { get; set; }
        public int AnnoFondazione { get; set; }
        public List<Impiegato> Impiegati { get; set; } = new List<Impiegato>();
    }
}