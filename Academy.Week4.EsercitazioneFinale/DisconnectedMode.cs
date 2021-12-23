using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.EsercitazioneFinale
{
    public class DisconnectedMode
    {
        static string connectionStringSQL = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GestioneSpese;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        internal static void InserisciSpesa()
        {
            DataSet speseDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            Console.WriteLine("----DISCONNECTED MODE----");
            try
            {
                connessione.Open();

                var speseAdapter = InizializzaAdapter(connessione);
                speseAdapter.Fill(speseDS, "Spese");

                connessione.Close();

                Console.WriteLine("---- Inserire una nuova spesa ----");
                Console.Write("Data: ");
                DateTime data;
                while (!DateTime.TryParse(Console.ReadLine(), out data))
                {
                    Console.WriteLine("Inserisci un formato corretto di data");
                }
                Console.Write("Descrizione: ");
                string descrizione = Console.ReadLine();
                Console.Write("Categoria: ");
                List<int> categoriePossibili = new List<int>();
                foreach (DataRow item in speseDS.Tables["Categorie"].Rows)
                {
                    Console.WriteLine($"{item["Id"]}");
                    categoriePossibili.Add((int)item["id"]);
                }
                int categoria;
                while (!int.TryParse(Console.ReadLine(), out categoria))
                {
                    Console.WriteLine("Inserisci una categoria valida!");
                }
                while (!categoriePossibili.Contains(categoria))
                {
                    Console.WriteLine("Inserisci una categoria valida!");
                    while (!int.TryParse(Console.ReadLine(), out categoria))
                        Console.WriteLine("Inserisci una categoria valida!");
                }
                Console.Write("Utente: ");
                string utente = Console.ReadLine();
                Console.Write("Importo: ");
                decimal importo;
                while (!decimal.TryParse(Console.ReadLine(), out importo))
                {
                    Console.WriteLine("Inserisci un formato corretto di importo");
                }


                DataRow nuovaRiga = speseDS.Tables["Spese"].NewRow();
                nuovaRiga["Data"] = data;
                nuovaRiga["CategoriaId"] = categoria;
                nuovaRiga["Descrizione"] = descrizione;
                nuovaRiga["Utente"] = utente;
                nuovaRiga["Importo"] = importo;
                nuovaRiga["Approvato"] = 0;


                speseDS.Tables["Spese"].Rows.Add(nuovaRiga);

                speseAdapter.Update(speseDS, "Spese");
                Console.WriteLine("Database aggiornato");
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

        internal static void CancellaSpesa()
        {
            Console.WriteLine("----DISCONNECTED MODE----");
            DataSet speseDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                var speseAdapter = InizializzaAdapter(connessione);
                speseAdapter.Fill(speseDS, "Spese");
                connessione.Close();

                Console.Write("Inserisci id della spesa da eliminare:");
                List<int> idPossibili = new List<int>();
                foreach (DataRow item in speseDS.Tables["Spese"].Rows)
                {
                    Console.WriteLine($"{item["Id"]}");
                    idPossibili.Add((int)item["id"]);
                }
                int idDaEliminare;
                while (!int.TryParse(Console.ReadLine(), out idDaEliminare))
                {
                    Console.WriteLine("Inserisci un id valido!");
                }
                while (!idPossibili.Contains(idDaEliminare))
                {
                    Console.WriteLine("Inserisci un id valido!");
                    while (!int.TryParse(Console.ReadLine(), out idDaEliminare))
                        Console.WriteLine("Inserisci un id valido!");
                }

                DataRow rigaDaEliminare = speseDS.Tables["Spese"].Rows.Find(idDaEliminare);
                if (rigaDaEliminare != null)
                {
                    rigaDaEliminare.Delete();
                }

                speseAdapter.Update(speseDS, "Spese");
                Console.WriteLine("Database aggiornato");


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

        internal static void ApprovaSpesa()
        {
            Console.WriteLine("----DISCONNECTED MODE----");
            DataSet speseDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                var speseAdapter = InizializzaAdapter(connessione);
                speseAdapter.Fill(speseDS, "Spese");
                connessione.Close();

                Console.WriteLine("Inserisci id della spesa da approvare:");
                List<int> idPossibili = IdSpeseDaApprovare(speseDS, connessione);
                Console.Write("> ");
                int idDaApprovare;
                while (!int.TryParse(Console.ReadLine(), out idDaApprovare))
                {
                    Console.WriteLine("Inserisci un id valido!");
                    Console.Write("> ");
                }
                while (!idPossibili.Contains(idDaApprovare))
                {
                    Console.WriteLine("Inserisci un id valido!");
                    Console.Write("> ");
                    while (!int.TryParse(Console.ReadLine(), out idDaApprovare))
                    {
                        Console.WriteLine("Inserisci un id valido!");
                        Console.Write("> ");
                    }
                
                }

                DataRow rigaDaAggiornare = speseDS.Tables["Spese"].Rows.Find(idDaApprovare);
                if (rigaDaAggiornare != null)
                {
                    rigaDaAggiornare["Approvato"] = 1;
                }

                speseAdapter.Update(speseDS, "Spese");
                Console.WriteLine("Database aggiornato");

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

        private static List<int> IdSpeseDaApprovare(DataSet speseDS, SqlConnection connessione)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand("Select * from Spese where Approvato = 0", connessione);
            adapter.Fill(speseDS, "SpeseFiltro");
            List<int> risultato = new List<int>();
            foreach (DataRow item in speseDS.Tables["SpeseFiltro"].Rows)
            {
                risultato.Add((int)item["Id"]);
                Console.WriteLine($"{item["Id"]}");
            }
            return risultato;
            
        }

        private static SqlDataAdapter InizializzaAdapter(SqlConnection connessione)
        {
            SqlDataAdapter speseAdapter = new SqlDataAdapter();

            speseAdapter.SelectCommand = new SqlCommand("Select * from Spese", connessione);
            speseAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            speseAdapter.InsertCommand = GeneraInsertCommand(connessione);
            speseAdapter.UpdateCommand = GeneraUpdateCommand(connessione);
            speseAdapter.DeleteCommand = GeneraDeleteCommand(connessione);

            return speseAdapter;
        }

        private static SqlCommand GeneraInsertCommand(SqlConnection connessione)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connessione;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Insert into Spese values (@data, @categoriaId, @descr, @utente, @importo, @appr)";

            cmd.Parameters.Add(new SqlParameter("@data", SqlDbType.DateTime, 100, "Data"));
            cmd.Parameters.Add(new SqlParameter("@categoriaId", SqlDbType.Int, 0, "CategoriaId"));
            cmd.Parameters.Add(new SqlParameter("@descr", SqlDbType.NVarChar, 500, "Descrizione"));
            cmd.Parameters.Add(new SqlParameter("@utente", SqlDbType.NVarChar, 100, "Utente"));
            cmd.Parameters.Add(new SqlParameter("@importo", SqlDbType.Decimal, 0, "Importo"));
            cmd.Parameters.Add(new SqlParameter("@appr", SqlDbType.Bit, 2, "Approvato"));

            return cmd;
        }
        private static SqlCommand GeneraDeleteCommand(SqlConnection connessione)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connessione;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Delete from Spese where ID=@id";

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "Id"));

            return cmd;
        }

        private static SqlCommand GeneraUpdateCommand(SqlConnection connessione)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connessione;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Update Spese set Data=@data, CategoriaId = @categoriaId, Descrizione = @descr, Utente = @utente, Importo = @importo, Approvato = @approvato where ID=@id";

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "Id"));
            cmd.Parameters.Add(new SqlParameter("@data", SqlDbType.DateTime, 0, "Data"));
            cmd.Parameters.Add(new SqlParameter("@categoriaId", SqlDbType.Int, 0, "CategoriaId"));
            cmd.Parameters.Add(new SqlParameter("@descr", SqlDbType.NVarChar, 500, "Descrizione"));
            cmd.Parameters.Add(new SqlParameter("@utente", SqlDbType.NVarChar, 100, "Utente"));
            cmd.Parameters.Add(new SqlParameter("@importo", SqlDbType.Decimal, 0, "Importo"));
            cmd.Parameters.Add(new SqlParameter("@approvato", SqlDbType.Bit, 2, "Approvato"));

            return cmd;
        }
    }
}