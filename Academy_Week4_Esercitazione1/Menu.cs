using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy_Week4_Esercitazione1
{
    static class Menu
    {
        static string connectionStringSql = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Ticketing;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    

        public static void StampaListaTickets()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSql);
            try
            {
                connessione.Open();

                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");


                string query = "select * from Tickets order by Data desc";

                SqlCommand cmd = new SqlCommand(query, connessione);

                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("---------Tickets---------");
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var descrizione = reader.GetString(1);
                    var data = (DateTime)reader["Data"];
                    var utente = reader.GetString(3);
                    var stato = reader.GetString(4);

                    Console.WriteLine($"{id} - {descrizione} - {data} - {utente} - {stato}");
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }

            finally
            {
                connessione.Close();
                Console.WriteLine("Connessione chiusa");
            }
        }

        public static void InserisciTicket()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSql);
            try
            {
                connessione.Open();

                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                Console.WriteLine("Inserisci una descrizione per il nuovo ticket:");
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

                while(stato != "New" && stato != "OnGoing" && stato != "Resolved")
                {
                    Console.WriteLine("Stato inserito non corretto");
                    stato = Console.ReadLine();
                }
                
                string insertSql = $"insert into Tickets values('{descrizione}', '{data}', '{utente}', '{stato}')";

                SqlCommand insertCMD = connessione.CreateCommand();

                insertCMD.CommandText = insertSql;

                int righeInserite = insertCMD.ExecuteNonQuery();

                if (righeInserite >= 1)
                    Console.WriteLine($"{righeInserite} riga/righe inserita/e correttamente");
                else
                    Console.WriteLine("Ops... qualcosa non torna!");

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }

            finally
            {
                connessione.Close();
                Console.WriteLine("Connessione chiusa");
            }
        }

        public static void EliminaTicketById()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSql);
            try
            {
                connessione.Open();

                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                Console.WriteLine("Inserisci l'ID del ticket da rimuovere:");

                string query = "select * from Tickets";

                SqlCommand cmd = new SqlCommand(query, connessione);

                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("---------Id tickets---------");
                
                List<int> listaId = new List<int>();

                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    listaId.Add(id);
                    Console.WriteLine($"{id}");
                }

                connessione.Close();

                connessione.Open();

                int idDaEliminare;

                while (!int.TryParse(Console.ReadLine(), out idDaEliminare))
                    Console.WriteLine("Inserisci un ID valido!");

                while (!listaId.Contains(idDaEliminare))
                {
                    Console.WriteLine("Inserisci un ID valido!");
                    while (!int.TryParse(Console.ReadLine(), out idDaEliminare))
                        Console.WriteLine("Inserisci un ID valido!");
                }


                string deleteSql = "delete from Tickets where ID = @id";

                SqlCommand deleteCommand = connessione.CreateCommand();
                deleteCommand.CommandText = deleteSql;
                deleteCommand.Parameters.AddWithValue("@id", idDaEliminare);

                int righeEliminate = deleteCommand.ExecuteNonQuery();

                if (righeEliminate >= 1)
                    Console.WriteLine($"{righeEliminate} riga/righe eliminata/e correttamente");
                else
                    Console.WriteLine("Ops... qualcosa non torna!");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }

            finally
            {
                connessione.Close();
                Console.WriteLine("Connessione chiusa");
            }
        }
    }
}
