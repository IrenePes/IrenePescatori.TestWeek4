﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.Ticketing.Core.Interfaces
{
    public interface IBusinessLayer
    {
        void FetchAllTickets();
        void InsertNewTicket(string descrizione, DateTime data, string utente, string stato);
        void DeleteTicketById(int idDaEliminare);
        List<int> FetchAllId();
    }
}
