using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.Ticketing.Core.Interfaces
{
    public interface ITicketsRepository
    {
        DataSet FetchAll();
        void InsertNew(string descrizione, DateTime data, string utente, string stato);
        void DeleteById(int idDaEliminare);
        List<int> FetchAllId();
    }
}
