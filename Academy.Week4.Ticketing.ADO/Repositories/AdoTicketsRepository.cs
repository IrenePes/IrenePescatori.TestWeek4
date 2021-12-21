using Academy.Week4.Ticketing.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.Ticketing.ADO.Repositories
{
    public class AdoTicketsRepository : ITicketsRepository
    {
        static string connectionStringSQL = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Ticketing;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public DataSet FetchAll()
        {
            DataSet ticketDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                SqlDataAdapter ticketsAdapter = new SqlDataAdapter();
                ticketsAdapter.SelectCommand = new SqlCommand("Select * from Tickets order by Data desc", connessione);
                ticketsAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                ticketsAdapter.Fill(ticketDS, "Tickets");

                connessione.Close();
                Console.WriteLine("Connessione chiusa");
                Console.WriteLine("\n");

                //foreach (DataTable table in ticketDS.Tables)
                //{
                //    Console.WriteLine($"{table.TableName} - {table.Rows.Count}");
                //}


                //Console.WriteLine("\n");
                //Console.WriteLine("-------Tickets Columns-------");
                //foreach (DataColumn colonna in ticketDS.Tables["Tickets"].Columns)
                //{
                //    Console.WriteLine($"{colonna.ColumnName} - {colonna.DataType}");
                //}


                //Console.WriteLine("\n");
                //Console.WriteLine("-------Tickets Rows-------");
                //foreach (DataRow riga in ticketDS.Tables["Tickets"].Rows)
                //{
                //    Console.WriteLine($"{riga["ID"]} - {riga["Descrizione"]} - {riga["Data"]} - {riga["Utente"]} - {riga["Stato"]}");
                //}
                //Console.WriteLine("\n");
                return ticketDS;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
                return null;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
                return null;
            }

            finally
            {
                connessione.Close();
            }


        }

        public void InsertNew(string descrizione, DateTime data, string utente, string stato)
        {
            DataSet ticketDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                SqlDataAdapter ticketsAdapter = new SqlDataAdapter();
                ticketsAdapter.SelectCommand = new SqlCommand("Select * from Tickets", connessione);
                ticketsAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                ticketsAdapter.Fill(ticketDS, "Tickets");

                connessione.Close();
                Console.WriteLine("Connessione chiusa");
                Console.WriteLine("\n");

                DataRow nuovaRiga = ticketDS.Tables["Tickets"].NewRow();
                nuovaRiga["Descrizione"] = descrizione;
                nuovaRiga["Data"] = data;
                nuovaRiga["Utente"] = utente;
                nuovaRiga["Stato"] = stato;

                
                ticketDS.Tables["Tickets"].Rows.Add(nuovaRiga);

                ticketsAdapter.InsertCommand = GenerateInsertCommand(connessione);

                ticketsAdapter.Update(ticketDS, "Tickets");
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

        public SqlCommand GenerateInsertCommand(SqlConnection connessione)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connessione;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into Tickets values(@descrizione, @data, @utente, @stato)";

            cmd.Parameters.Add(new SqlParameter("@descrizione", SqlDbType.NVarChar, 500, "Descrizione"));
            cmd.Parameters.Add(new SqlParameter("@data", SqlDbType.DateTime, 100, "Data"));
            cmd.Parameters.Add(new SqlParameter("@utente", SqlDbType.NVarChar, 100, "Utente"));
            cmd.Parameters.Add(new SqlParameter("@stato", SqlDbType.NVarChar, 10, "Stato"));

            return cmd;
        }

        private static SqlCommand GeneraDeleteCommand(SqlConnection connessione)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connessione;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from Tickets where ID = @id";

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "ID"));

            return cmd;
        }

        public void DeleteById(int idDaEliminare)
        {
            DataSet ticketDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                SqlDataAdapter ticketsAdapter = new SqlDataAdapter();
                ticketsAdapter.SelectCommand = new SqlCommand("Select * from Tickets", connessione);
                ticketsAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                ticketsAdapter.Fill(ticketDS, "Tickets");

                connessione.Close();
                Console.WriteLine("Connessione chiusa");
                Console.WriteLine("\n");

                DataRow rigaDaEliminare = ticketDS.Tables["Tickets"].Rows.Find(idDaEliminare);
                if (rigaDaEliminare != null)
                {
                    rigaDaEliminare.Delete();
                }
                
                ticketsAdapter.DeleteCommand = GeneraDeleteCommand(connessione);

                ticketsAdapter.Update(ticketDS, "Tickets");
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

        public List<int> FetchAllId()
        {
            List<int> idPossibili = new List<int>();
            DataSet ticketDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();

                SqlDataAdapter ticketsAdapter = new SqlDataAdapter();
                ticketsAdapter.SelectCommand = new SqlCommand("Select * from Tickets", connessione);
                ticketsAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                ticketsAdapter.Fill(ticketDS, "Tickets");

                connessione.Close();

                foreach(DataRow riga in ticketDS.Tables["Tickets"].Rows)
                {
                    idPossibili.Add((int)riga[0]);
                }
                return idPossibili;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
                return null;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
                return null;
            }

            finally
            {
                connessione.Close();
            }
        }
    }
}
