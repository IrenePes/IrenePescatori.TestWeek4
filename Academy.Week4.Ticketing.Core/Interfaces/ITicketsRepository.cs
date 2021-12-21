using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.Ticketing.Core.Interfaces
{
    public interface ITicketsRepository
    {
        void FetchAll();
        void InsertNew(string descrizione, DateTime data, string utente, string stato);
        void DeleteById(int idDaEliminare);
        List<int> FetchAllId();
    }
}
