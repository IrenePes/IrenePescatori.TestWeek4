using Academy.Week4.Ticketing.ADO.Repositories;
using Academy.Week4.Ticketing.Core.BusinessLayer;
using Academy.Week4.Ticketing.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.Ticketing.Client
{
    public class Menu
    {

        private static readonly IBusinessLayer mainBL = new BusinessLayer(new AdoTicketsRepository());

        internal static void Start()
        {
            Console.WriteLine("Benvenuto!");

            char choice;

            do
            {
                Console.WriteLine("[1] Visualizza tutti i tickets" +
                    "\n[2] Inserisci un nuovo ticket" +
                    "\n[3] Elimina un ticket" +
                    "\n[Q] Esci");

                choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (choice)
                {
                    case '1':
                        FetchAllTickets();
                        break;
                    case '2':
                        InsertNewTicket();
                        break;
                    case '3':
                        DeleteTicket();
                        break;
                    case 'Q':
                        Console.WriteLine("\nArrivederci");
                        return;
                    default:
                        Console.WriteLine("Scelta non disponibile. Riprova!");
                        break;
                }

            } while (!(choice == 'Q'));
        }

        public static void DeleteTicket()
        {
            Console.WriteLine("Inserisci l'ID del ticket da eliminare");
            List<int> idPossibili = mainBL.FetchAllId();

            foreach(int id in idPossibili)
                Console.WriteLine(id);

            int idDaEliminare;

            while (!int.TryParse(Console.ReadLine(), out idDaEliminare))
            {
                Console.WriteLine("Inserisci un ID valido");
            }

            while (!idPossibili.Contains(idDaEliminare))
            {
                Console.WriteLine("Inserisci un ID valido");
                while (!int.TryParse(Console.ReadLine(), out idDaEliminare))
                    Console.WriteLine("Inserisci un ID valido");
            }

            mainBL.DeleteTicketById(idDaEliminare);
        }

        public static void InsertNewTicket()
        {
            Console.WriteLine("Inserisci la descrizione del nuovo ticket da inserire");
            string descrizione = Console.ReadLine();

            DateTime data;
            Console.WriteLine("Inserisci una data:");

            while (!DateTime.TryParse(Console.ReadLine(), out data))
            {
                Console.WriteLine("Inserisci un formato corretto di data!");
            }

            Console.WriteLine("Inserisci il nome utente:");
            string utente = Console.ReadLine();

            Console.WriteLine("Inserisci lo stato del ticket:");
            string stato = Console.ReadLine();

            while (stato != "New" && stato != "OnGoing" && stato != "Resolved")
            {
                Console.WriteLine("Stato inserito non corretto");
                stato = Console.ReadLine();
            }

            mainBL.InsertNewTicket(descrizione, data, utente, stato);
        }

        public static void FetchAllTickets()
        {
            mainBL.FetchAllTickets();
        }
    }
}
