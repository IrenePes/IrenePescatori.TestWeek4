using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.Ticketing.Core.Entites
{
    public class Tickets
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        public DateTime Data { get; set; }
        public string Utente { get; set; }
        public string Stato { get; set; }

        //public StatoEnum Stato { get; set; }

    }
//    public enum StatoEnum
//    {
//        New = 1,
//        OnStage = 2,
//        Resolved = 3
//    }
}
