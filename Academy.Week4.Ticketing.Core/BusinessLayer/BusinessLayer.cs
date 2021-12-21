using Academy.Week4.Ticketing.Core.Interfaces;
using Academy.Week4.Ticketing.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.Ticketing.Core.BusinessLayer
{
    public class BusinessLayer : IBusinessLayer
    {
        private readonly ITicketsRepository _ticketsRepository;
            
        public BusinessLayer(ITicketsRepository ticketsRepo)
            {
                _ticketsRepository = ticketsRepo;
            }

        public void DeleteTicketById(int idDaEliminare)
        {
            _ticketsRepository.DeleteById(idDaEliminare);
        }

        public List<int> FetchAllId()
        {
            return _ticketsRepository.FetchAllId();
        }

        public void FetchAllTickets()
        {
            _ticketsRepository.FetchAll();
        }

        public void InsertNewTicket(string descrizione, DateTime data, string utente, string stato)
        {
            _ticketsRepository.InsertNew(descrizione, data, utente, stato);
        }
    }
}
