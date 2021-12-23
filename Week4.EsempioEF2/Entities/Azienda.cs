using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week4.EsempioEF2.Entities
{
    public class Azienda
    {
        public int AziendaID { get; set; }
        public string Nome { get; set; }
        public int AnnoFondazione { get; set; }
        public List<Impiegato> Impiegati { get; set; } = new List<Impiegato>();
    }
}
