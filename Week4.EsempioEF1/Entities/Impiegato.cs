using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week4.EsempioEF1.Entities
{
    public class Impiegato
    {
        public  int ImpiegatoID { get; set; }
        public  string Nome { get; set; }
        public  string Cognome { get; set; }
        public  DateTime DataNascita { get; set; }

        public int AziendaID { get; set; }
        public  Azienda Azienda { get; set; }  // navigation property
    }
}
