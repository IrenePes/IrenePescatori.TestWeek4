using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyD_ADOEF.ConsoleApp
{
    static class AdoNetDemo
    {
        // usiamo la stringa di connessione del DB a cui voglio connettermi (RICORDA @)
        static string connectionStringSQL = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = Zio; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void ConnectionDemo()
        {
            // stiamo creando un oggetto SqlConnection passando la stringa di connessione
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);

            connessione.Open();

            if (connessione.State == System.Data.ConnectionState.Open)
                Console.WriteLine("Connessi al DB");
            else
                Console.WriteLine("NON connessi al DB");

            connessione.Close();

        }

        // READ 
        public static void DataReaderDemo()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();

                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                // voglio leggere tutti i dati contenuti nella tabella "Ingrediente"
                // memorizzo in una stringa la query che vorrei eseguire
                string query = "select * from Ingrediente";

                // istanziare SqlCommand (1)
                SqlCommand comando = new SqlCommand();
                comando.Connection = connessione;
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = query;

                // istanziare SqlCommand (2)
                SqlCommand comando2 = new SqlCommand(query, connessione);

                // istanziare SqlCommand (3)
                SqlCommand comando3 = connessione.CreateCommand();
                comando3.CommandText = query;

                // creo l'oggetto che conterra' la tabella risultato della query effettuata
                SqlDataReader reader = comando.ExecuteReader();

                Console.SetCursorPosition((Console.WindowWidth - "------ Ingredienti ------".Length) / 2, Console.CursorTop);
                Console.WriteLine("------ Ingredienti ------");

                while (reader.Read())
                {
                    // lettura tipizzata del dato
                    //var idIngrediente = reader.GetInt32(0);
                    //var nome = reader.GetString(1);
                    //var descrizione = reader.GetString(2);
                    //var unitaDiMisura = reader.GetString(3);

                    // altro modo per recuperare i dati dal DB
                    var idIngrediente = (int)reader["IdIngrediente"]; //sto specificando il nome del campo, non la sua posizione
                    var nome = (string)reader["Nome"];
                    var descrizione = (string)reader["Descrizione"];
                    var unitaDiMisura = (string)reader["UnitaDiMisura"];


                    Console.WriteLine($"{idIngrediente} - {nome} - {descrizione} - {unitaDiMisura}");
                }
                // i catch concatenati devono essere ordinati dal più specifico al più generico
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

        // INSERT 
        public static void InsertDemo()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();

                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                //chiediamo all'utente di inserire nome e descrizione del nuovo ingrediente
                string nomeIngrediente = "Cocco";
                string descrizioneIngrediente = "Latte di cocco";
                string udmIngrediente = "ml";

                string insertSql = $"insert into Ingrediente values(29, '{nomeIngrediente}', '{descrizioneIngrediente}', '{udmIngrediente}')";
                //string insertSql = "insert into Ingrediente values(28, 'Cocco', 'Latte di cocco', 'ml')";

                SqlCommand insertCommand = connessione.CreateCommand();
                insertCommand.CommandText = insertSql;

                int righeInserite = insertCommand.ExecuteNonQuery();

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

        // INSERT CON PARAMETRI
        public static void InsertConParametriDemo()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();

                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                // chiedere all'utente di inserire nome, descrizione, unità di misura
                int IdIngrediente = 30;
                string nomeIngrediente = "peperoncino";
                string descrizioneIngrediente = "peperoncino piccantisssssssimo";
                string udm = "g";

                string insertSQL = "insert into Ingrediente values(@IdIngrediente, @nomeIngrediente, @descrizioneIngrediente, @udmIngrediente)";

                SqlCommand insertCommand = connessione.CreateCommand();
                insertCommand.CommandText = insertSQL;
                insertCommand.Parameters.AddWithValue("@nomeIngrediente", nomeIngrediente);
                insertCommand.Parameters.AddWithValue("@IdIngrediente", IdIngrediente);
                insertCommand.Parameters.AddWithValue("@descrizioneIngrediente", descrizioneIngrediente);
                insertCommand.Parameters.AddWithValue("@udmIngrediente", udm);

                // altro modo per aggiungere parametri
                //int IdIngrediente2 = 31;
                //string nomeIngrediente2 = "Liquirizia";
                //string descrizioneIngrediente2 = "Bastoncino di legno profumato";
                //string udm2 = "g";

                //SqlParameter idParametro = new SqlParameter();
                //idParametro.ParameterName = "@idIngrediente";
                //idParametro.Value = IdIngrediente2;
                //idParametro.DbType = System.Data.DbType.Int32;
                //insertCommand.Parameters.Add(idParametro);

                //SqlParameter udmParametro = new SqlParameter();
                //udmParametro.ParameterName = "@udmIngrediente";
                //udmParametro.Value = udm2;
                //udmParametro.DbType = System.Data.DbType.String;
                //udmParametro.Size = 10;
                //insertCommand.Parameters.Add(udmParametro);

                //SqlParameter nomeParametro = new SqlParameter();
                //nomeParametro.ParameterName = "@nomeIngrediente";
                //nomeParametro.Value = nomeIngrediente2;
                //nomeParametro.DbType = System.Data.DbType.String;
                //insertCommand.Parameters.Add(nomeParametro);

                //SqlParameter descParametro = new SqlParameter();
                //descParametro.ParameterName = "@descrizioneIngrediente";
                //descParametro.Value = descrizioneIngrediente2;
                //descParametro.DbType = System.Data.DbType.String;
                //insertCommand.Parameters.Add(descParametro);

                int righeInserite = insertCommand.ExecuteNonQuery();

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

        // DELETE CON PARAMETRI

        public static void DeleteConParametriDemo()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();

                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                //chiedo all'utente quale id vuole cancellare 
                int idDaEliminare = 30;

                string deleteSQL = "delete from Ingrediente where IdIngrediente = @id";

                SqlCommand deleteCommand = connessione.CreateCommand();
                deleteCommand.CommandText = deleteSQL;
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

         // STORED PROCEDURES
         public static void StoreProcedureDemo()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();

                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                SqlCommand spInsertCocktail = connessione.CreateCommand();
                spInsertCocktail.CommandType = System.Data.CommandType.StoredProcedure;
                spInsertCocktail.CommandText = "InserisciCocktail";

                // parametri
                // chiedo all'utente quale cocktail vuole inserire e tutti i dati necessari
                int idCocktail = 10;
                string nomeCocktail = "Cocktail Renata";
                int tempoPreparazione = 5;
                int numPersone = 10;
                string libro = "Cocktails classici";
                string procedimento = "Mescola tutto";

                
                spInsertCocktail.Parameters.AddWithValue("idCocktail", idCocktail);
                spInsertCocktail.Parameters.AddWithValue("@nomeCocktail", nomeCocktail);
                spInsertCocktail.Parameters.AddWithValue("@tempoPreparazione", tempoPreparazione);
                spInsertCocktail.Parameters.AddWithValue("@numeroPersone", numPersone);
                spInsertCocktail.Parameters.AddWithValue("@libro", libro);
                spInsertCocktail.Parameters.AddWithValue("@procedimento", procedimento);

                int risultato = spInsertCocktail.ExecuteNonQuery();

                if (risultato >= 1)
                    Console.WriteLine($"{risultato} riga/righe inserita/e/aggiornata/e correttamente");
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
        public static void RisultatiMultipli()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();

                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                string sqlStatement = "select * from Ingrediente; select * from Cocktail;";

                SqlCommand readerCommand = new SqlCommand();
                readerCommand.Connection = connessione;
                readerCommand.CommandType = System.Data.CommandType.Text;
                readerCommand.CommandText = sqlStatement;

                SqlDataReader reader = readerCommand.ExecuteReader();

                int idx = 0;
                while(reader.HasRows)
                {
                    Console.WriteLine($"------Result set {idx + 1}------");
                    while(reader.Read())
                    {
                        Console.WriteLine($"{reader["Nome"]}");
                    }
                    reader.NextResult();
                    idx++;
                    Console.WriteLine("------------------------\n");
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

        // TRANSACTION

        public static void TransactionDemo()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            SqlTransaction transaction = null;
            try
            {
                connessione.Open();

                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                transaction = connessione.BeginTransaction();

                SqlCommand insertIngrediente = connessione.CreateCommand();
                insertIngrediente.CommandText = "Insert into Ingrediente values(30, 'Peperoncino', 'Peperoncino super piccante', 'g')";
                insertIngrediente.Transaction = transaction;
                int result = insertIngrediente.ExecuteNonQuery();

                SqlCommand insertLibro = connessione.CreateCommand();
                insertLibro.CommandText = "Insert into Libro values(4, 'Nuovi cocktails')";
                insertLibro.Transaction = transaction;
                result = insertLibro.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
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
