using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.EsercitazioneFinale
{
    public class ConnectedMode
    {
        static string connectionStringSQL = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GestioneSpese;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public static void ApprovaSpesa()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            Console.WriteLine("----CONNECTED MODE----");
            try
            {
                connessione.Open();

                Console.WriteLine("Inserisci l'ID della spesa da approvare:");

                string query = "Select * from Spese where Approvato = 0";

                SqlCommand cmd = new SqlCommand(query, connessione);

                SqlDataReader reader = cmd.ExecuteReader();

                List<int> listaId = new List<int>();

                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    listaId.Add(id);
                    Console.WriteLine($"{id}");
                }

                connessione.Close();

                connessione.Open();

                int idDaApprovare;
                Console.Write("> ");
                while (!int.TryParse(Console.ReadLine(), out idDaApprovare))
                {
                    Console.WriteLine("Inserisci un ID valido!");
                    Console.Write("> ");
                }
                while (!listaId.Contains(idDaApprovare))
                {
                    Console.WriteLine("Inserisci un ID valido!");
                    Console.Write("> ");
                    while (!int.TryParse(Console.ReadLine(), out idDaApprovare))
                    {
                        Console.WriteLine("Inserisci un ID valido!");
                        Console.Write("> ");
                    }
                }


                string updateSql = "update Spese set Approvato = 1 where Id = @id";

                SqlCommand updateCommand = connessione.CreateCommand();
                updateCommand.CommandText = updateSql;
                updateCommand.Parameters.AddWithValue("@id", idDaApprovare);

                int righeAggiornate = updateCommand.ExecuteNonQuery();

                if (righeAggiornate >= 1)
                    Console.WriteLine($"{righeAggiornate} riga/righe aggiornata/e correttamente");
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
            }
        }

        internal static void InserisciNuovaSpesa()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            Console.WriteLine("----CONNECTED MODE----");
            try
            {
                connessione.Open();

                Console.WriteLine("---- Inserire una nuova spesa ----");
                Console.Write("Data: ");
                DateTime data;
                while (!DateTime.TryParse(Console.ReadLine(), out data))
                {
                    Console.WriteLine("Inserisci un formato corretto di data");
                }
                Console.Write("Descrizione: ");
                string descrizione = Console.ReadLine();
                Console.WriteLine("Categoria id: ");
                string query0 = "select Id from Categorie";
                SqlCommand cmd0 = new SqlCommand(query0, connessione);
                SqlDataReader reader0 = cmd0.ExecuteReader();
                List<int> listaId = new List<int>();
                while (reader0.Read())
                {
                    var id = reader0.GetInt32(0);
                    listaId.Add(id);
                    Console.WriteLine($"{id}");
                }
                connessione.Close();
                connessione.Open();
                int categoria;
                Console.Write("> ");
                while (!int.TryParse(Console.ReadLine(), out categoria))
                {
                    Console.WriteLine("Inserisci una categoria valida!");
                    Console.Write("> ");
                }
                while (!listaId.Contains(categoria))
                {
                    Console.WriteLine("Inserisci una categoria valida!");
                    Console.Write("> ");
                    while (!int.TryParse(Console.ReadLine(), out categoria))
                    {
                        Console.WriteLine("Inserisci una categoria valida!");
                        Console.Write("> ");
                    }
                }

                Console.Write("Utente: ");
                string utente = Console.ReadLine();
                Console.Write("Importo: ");
                decimal importo;
                while (!decimal.TryParse(Console.ReadLine(), out importo))
                {
                    Console.WriteLine("Inserisci un formato corretto di importo");
                }

                string insertSql = $"insert into Spese values('{data}', {categoria}, '{descrizione}', '{utente}', {importo}, 0)";

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
            }
        }

        public static void MostraSpeseDiUnUtente()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            Console.WriteLine("----CONNECTED MODE----");
            try
            {
                connessione.Open();

                Console.WriteLine("Inserisci il nome dell'utente di cui visualizzare le spese:");

                string query = "Select distinct Utente from Spese";

                SqlCommand cmd = new SqlCommand(query, connessione);

                SqlDataReader reader = cmd.ExecuteReader();

                List<string> listaUtenti = new List<string>();

                while (reader.Read())
                {
                    var utente = reader.GetString(4);
                    listaUtenti.Add(utente);
                    Console.WriteLine($"{utente}");
                }

                connessione.Close();

                connessione.Open();

                string utenteScelto = Console.ReadLine();
                Console.Write("> ");
                while (!listaUtenti.Contains(utenteScelto))
                {
                    Console.WriteLine("Inserisci un nome utente valido!");
                    Console.Write("> ");
                }

                string query2 = "select * from Spese where Utente = @utente";

                SqlCommand cmd2 = connessione.CreateCommand();
                cmd2.CommandText = query2;
                cmd2.Parameters.AddWithValue("@utente", utenteScelto);

                SqlDataReader reader2 = cmd2.ExecuteReader();

                Console.WriteLine($"---------Spese effettuate dall'utente {utenteScelto}---------");
                while (reader2.Read())
                {
                    var id = reader2.GetInt32(0);
                    var data = reader2.GetDateTime(1);
                    var categoriaId = reader2.GetInt32(2);
                    var descrizione = reader2.GetString(3);
                    var utente = reader2.GetString(4);
                    var importo = reader2.GetDecimal(5);
                    var approvato = reader2.GetBoolean(6);

                    Console.WriteLine($"{id} - {data} - {categoriaId} - {descrizione} - {importo}€ - {approvato}");
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
            }
        }

        public static void CancellaSpesa()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            Console.WriteLine("----CONNECTED MODE----");
            try
            {
                connessione.Open();

                Console.WriteLine("Inserisci l'ID della spesa da eliminare:");

                string query = "select * from Spese";

                SqlCommand cmd = new SqlCommand(query, connessione);

                SqlDataReader reader = cmd.ExecuteReader();

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
                Console.Write("> ");
                while (!int.TryParse(Console.ReadLine(), out idDaEliminare))
                {
                    Console.WriteLine("Inserisci un ID valido!");
                    Console.Write("> ");
                }
                while (!listaId.Contains(idDaEliminare))
                {
                    Console.WriteLine("Inserisci un ID valido!");
                    Console.Write("> ");
                    while (!int.TryParse(Console.ReadLine(), out idDaEliminare))
                    {
                        Console.WriteLine("Inserisci un ID valido!");
                        Console.Write("> ");
                    }
                }


                string deleteSql = "delete from Spese where Id = @id";

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
            }
        }

        public static void MostraSpeseApprovate()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            Console.WriteLine("----CONNECTED MODE----");
            try
            {
                connessione.Open();

                string query = "select * from Spese where Approvato = 1";

                SqlCommand cmd = new SqlCommand(query, connessione);

                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("---------Spese approvate---------");
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var data = reader.GetDateTime(1);
                    var categoriaId = reader.GetInt32(2);
                    var descrizione = reader.GetString(3);
                    var utente = reader.GetString(4);
                    var importo = reader.GetDecimal(5);
                    var approvato = reader.GetBoolean(6);

                    Console.WriteLine($"{id} - {data} - {categoriaId} - {descrizione} - {utente} - {importo}€ - {approvato}");
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
            }
        }

        public static void MostraTotSpesePerCategoria()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            Console.WriteLine("----CONNECTED MODE----");
            try
            {
                connessione.Open();

                string query = "Select c.Id, c.Categoria, sum(s.Importo) from Spese s join Categorie c on s.CategoriaId = c.Id group by c.Id, c.Categoria";

                SqlCommand cmd = new SqlCommand(query, connessione);

                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("---------Totale spese per categoria---------");
                while (reader.Read())
                {
                    var categoria = reader.GetString(1);
                    var spesaTot = reader.GetDecimal(2);
                    Console.WriteLine($"{categoria} - {spesaTot}€");
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
            }

        }

    }
}
