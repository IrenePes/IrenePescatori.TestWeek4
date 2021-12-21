using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyD_ADOEF.Disconnected
{
    public static class AdoNetDemoDisconnected
    {
        // recupero la stringa di connessione
        static string connectionStringSQL = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = Zio; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // creo il metodo per popolare il DataSet
        public static void FillDataSet()
        {
            DataSet zioDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if(connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                // creo un metodo per inizializzare il DataSet al quale passo un DS e la connessione
                InizializzaDaSetEAdapter(zioDS, connessione);

                connessione.Close();
                Console.WriteLine("Connessione chiusa");

                // da qua in poi lavoro in modalità disconnessa ---> sono offline

                foreach (DataTable table in zioDS.Tables)
                {
                    Console.WriteLine($"{table.TableName} - {table.Rows.Count}");
                }



                Console.WriteLine("Come è fatta la tabella 'Ingrediente' del mio dataset");
                foreach(DataColumn colonna in zioDS.Tables["Ingrediente"].Columns)
                {
                    Console.WriteLine($"{colonna.ColumnName} - {colonna.DataType}");
                }



                Console.WriteLine("Di seguito le constraint sulla tabella 'Ingrediente' del mio dataset");
                foreach(Constraint vincolo in zioDS.Tables["Ingrediente"].Constraints)
                {
                    Console.WriteLine($"{vincolo.ConstraintName} - {vincolo.ExtendedProperties}");
                }


                Console.WriteLine("---------- Ingredienti ----------");
                foreach(DataRow riga in zioDS.Tables["Ingrediente"].Rows)
                {
                    Console.WriteLine($"{riga["IdIngrediente"]} - {riga["Nome"]} - {riga["Descrizione"]} - {riga["UnitaDiMisura"]}");
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

        private static SqlDataAdapter InizializzaDaSetEAdapter(DataSet cocktailsZioDS, SqlConnection connessione)
        {
            SqlDataAdapter zioAdapter = new SqlDataAdapter();
            //FILL
            zioAdapter.SelectCommand = new SqlCommand("Select * from Ingrediente", connessione);
            //serve per recuperare le PK automaticamente, evita di doverle definire a mano
            zioAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //INSERT
            //zioAdapter.InsertCommand = new SqlCommand("insert into Ingrediente values(@id, @nome, @descrizione, @udm)", connessione);
            //zioAdapter.InsertCommand.Parameters.AddWithValue("@nome", "Nome");
            //zioAdapter.InsertCommand.Parameters.AddWithValue("@descrizione", "Descrizione");
            //zioAdapter.InsertCommand.Parameters.AddWithValue("@udm", "UnitaDiMisura");
            //zioAdapter.InsertCommand.Parameters.AddWithValue("@id", "IdIngrediente");
            zioAdapter.InsertCommand = GeneraInsertCommand(connessione);
            //UPDATE    
            zioAdapter.UpdateCommand = GeneraUpdateCommand(connessione);
            //DELETE
            zioAdapter.DeleteCommand = GeneraDeleteCommand(connessione);


            zioAdapter.Fill(cocktailsZioDS, "Ingrediente");

            return zioAdapter;
        }

        private static SqlCommand GeneraDeleteCommand(SqlConnection connessione)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connessione;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from Ingrediente where IdIngrediente = @id";

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "IdIngrediente"));

            return cmd;
        }

        private static SqlCommand GeneraUpdateCommand(SqlConnection connessione)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connessione;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Update Ingrediente set IdIngrediente = @id, Nome = @nome, Descrizione = @descrizione, UnitaDiMisura = @udm where IdIngrediente = @newId";

            cmd.Parameters.Add(new SqlParameter("@newId", SqlDbType.Int, 0, "IdIngrediente"));
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "IdIngrediente"));
            cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.NVarChar, 30, "Nome"));
            cmd.Parameters.Add(new SqlParameter("@descrizione", SqlDbType.NVarChar, 50, "Descrizione"));
            cmd.Parameters.Add(new SqlParameter("@udm", SqlDbType.NVarChar, 10, "UnitaDiMisura"));

            return cmd;
        }

        private static SqlCommand GeneraInsertCommand(SqlConnection connessione)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connessione;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into Ingrediente values(@id, @nome, @descrizione, @udm)";

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "IdIngrediente"));
            cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.NVarChar, 30, "Nome"));
            cmd.Parameters.Add(new SqlParameter("@descrizione", SqlDbType.NVarChar, 50, "Descrizione"));
            cmd.Parameters.Add(new SqlParameter("@udm", SqlDbType.NVarChar, 10, "UnitaDiMisura"));

            return cmd;
        }

        public static void InsertRowDemo()
        {
            DataSet zioDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                
                var zioAdapter = InizializzaDaSetEAdapter(zioDS, connessione);

                connessione.Close();
                Console.WriteLine("Connessione chiusa");

                DataRow nuovaRiga = zioDS.Tables["Ingrediente"].NewRow();
                nuovaRiga["IdIngrediente"] = 28;
                nuovaRiga["Nome"] = "Panna";
                nuovaRiga["Descrizione"] = "Panna montata";
                nuovaRiga["UnitaDiMisura"] = "ml";

                // ho aggiunto una nuova riga al dataset
                zioDS.Tables["Ingrediente"].Rows.Add(nuovaRiga);
                // mi manca di riconciliare questa aggiunta al DB
                zioAdapter.Update(zioDS, "Ingrediente");
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


        public static void UpdateRowDemo()
        {
            DataSet zioDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");


                var zioAdapter = InizializzaDaSetEAdapter(zioDS, connessione);

                connessione.Close();
                Console.WriteLine("Connessione chiusa");

                // Find cerca la riga la cui PK è 31
                DataRow rigaDaAggiornare = zioDS.Tables["Ingrediente"].Rows.Find(31);
                if(rigaDaAggiornare != null)
                {
                    rigaDaAggiornare["Descrizione"] = "Nuova panna montata";
                }

                // riconcilio e quindi salvo la modifica sul DB
                zioAdapter.Update(zioDS, "Ingrediente");
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


        public static void DeleteRowDemo()
        {

            DataSet zioDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");


                var zioAdapter = InizializzaDaSetEAdapter(zioDS, connessione);

                connessione.Close();
                Console.WriteLine("Connessione chiusa");


                DataRow rigaDaEliminare = zioDS.Tables["Ingrediente"].Rows.Find(31);
                if (rigaDaEliminare != null)
                {
                    rigaDaEliminare.Delete();
                }

                // riconcilio e quindi salvo la modifica sul DB
                zioAdapter.Update(zioDS, "Ingrediente");
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


        public static void MultiTabelleDemo()
        {
            DataSet zioDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                SqlDataAdapter libroAdapter = new SqlDataAdapter();
                libroAdapter.SelectCommand = new SqlCommand("Select * from Libro", connessione);
                libroAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                libroAdapter.Fill(zioDS, "Libro");

                SqlDataAdapter cocktailAdapter = new SqlDataAdapter();
                cocktailAdapter.SelectCommand = new SqlCommand("Select * from Cocktail", connessione);
                cocktailAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                cocktailAdapter.Fill(zioDS, "Cocktail");

                connessione.Close();
                Console.WriteLine("Connessione chiusa");

                foreach(DataTable tabella in zioDS.Tables)
                {
                    Console.WriteLine($"{tabella.TableName} - {tabella.Rows.Count}");
                }
                Console.WriteLine("----------Libro----------");
                foreach(DataColumn colonna in zioDS.Tables["Libro"].Columns)
                {
                    Console.WriteLine($"{colonna.ColumnName} - {colonna.DataType}");
                }
                Console.WriteLine("----------Cocktail----------");
                foreach (DataColumn colonna in zioDS.Tables["Cocktail"].Columns)
                {
                    Console.WriteLine($"{colonna.ColumnName} - {colonna.DataType}");
                }
                Console.WriteLine("----------Libro----------");
                foreach (DataRow riga in zioDS.Tables["Libro"].Rows)
                {
                    Console.WriteLine($"{riga["IdLibro"]} - {riga["Titolo"]}");
                }
                Console.WriteLine("----------Cocktail----------");
                foreach (DataRow riga in zioDS.Tables["Cocktail"].Rows)
                {
                    Console.WriteLine($"{riga["IdCocktail"]} - {riga["Nome"]} - {riga["TempoPreparazione"]} - {riga["NumeroPersone"]} - {riga["Procedimento"]}");
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


        public static void ConstraintDemo()
        {

            DataSet zioDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("NON connessi al DB");

                SqlDataAdapter libroAdapter = new SqlDataAdapter();
                libroAdapter.SelectCommand = new SqlCommand("Select * from Libro", connessione);
                libroAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                libroAdapter.Fill(zioDS, "Libro");

                SqlDataAdapter cocktailAdapter = new SqlDataAdapter();
                cocktailAdapter.SelectCommand = new SqlCommand("Select * from Cocktail", connessione);
                cocktailAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                cocktailAdapter.Fill(zioDS, "Cocktail");

                // definiamo i vincoli
                // UNIQUE
                DataTable tabellaLibro = zioDS.Tables["Libro"];
                UniqueConstraint titoloUnique = new UniqueConstraint(tabellaLibro.Columns["Titolo"]);
                tabellaLibro.Constraints.Add(titoloUnique);

                //FOREIGN KEY
                DataColumn colonnaPadre = zioDS.Tables["Libro"].Columns["IdLibro"];
                DataColumn colonnaFiglio = zioDS.Tables["Cocktail"].Columns["IdLibro"];
                ForeignKeyConstraint fkLibro = new ForeignKeyConstraint("FK_LibroCocktail", colonnaPadre, colonnaFiglio);
                zioDS.Tables["Cocktail"].Constraints.Add(fkLibro);


                connessione.Close();
                Console.WriteLine("Connessione chiusa");

                foreach(DataTable tabella in zioDS.Tables)
                {
                    Console.WriteLine($"------{tabella.TableName} - {tabella.Rows.Count}------");
                    foreach(Constraint vincolo in tabella.Constraints)
                    {
                        Console.WriteLine($"{vincolo.ConstraintName} - ");
                        if(vincolo is UniqueConstraint)
                        {
                            //Stampa proprietà del vincolo Unique
                            StampaProprietaVincoloUnique(vincolo);
                        }
                        else if(vincolo is ForeignKeyConstraint)
                        {
                            // Stampa proprietà del vincolo FK
                            StampaProprietaVincoloFK(vincolo);
                        }
                        Console.WriteLine(" ");
                    }
                    Console.WriteLine("---------------------------------\n");
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

        private static void StampaProprietaVincoloFK(Constraint vincolo)
        {
           ForeignKeyConstraint vincoloFK = (ForeignKeyConstraint)vincolo;

            DataColumn[] arrayDiColonne = vincoloFK.Columns;

            for (int i = 0; i < arrayDiColonne.Length; i++)
            {
                Console.WriteLine($"Nome colonna: {arrayDiColonne[i].ColumnName}");
            }
        }

        private static void StampaProprietaVincoloUnique(Constraint vincolo)
        {
            UniqueConstraint vincoloUnique = (UniqueConstraint)vincolo;

            DataColumn[] arrayDiColonne = vincoloUnique.Columns;

            for(int i = 0; i < arrayDiColonne.Length; i++)
            {
                Console.WriteLine($"Nome colonna: {arrayDiColonne[i].ColumnName}");
            }
        }
    }
}
