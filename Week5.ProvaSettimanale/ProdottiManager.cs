using System;
using System.Data;
using System.Data.SqlClient;

namespace Week5.ProvaSettimanale
{
    public class ProdottiManager
    {
        const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=Magazzino;Trusted_Connection=True;";

        public static void MostraProdotti()
        {
            using (SqlConnection conn = new(connectionString))
            {
                DataSet dsMagazzino = new DataSet();

                conn.Open();

                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Problemi di connessione...");

                SqlDataAdapter prodottiAdapter = new();

                SqlCommand selectProdotti = new SqlCommand("SELECT * FROM Prodotti", conn);

                prodottiAdapter.SelectCommand = selectProdotti;

                prodottiAdapter.Fill(dsMagazzino, "Prodotti");

                conn.Close();

                Console.WriteLine("--- Prodotti ---");
                Console.WriteLine($"{"ID",-5} {"CodiceProdotto",-15} {"Categoria",-20} " +
                    $"{"Descrizione",-50} {"PrezzoUnitario",-15} {"QuantitàDisponibile",-4}");
                foreach (DataRow row in dsMagazzino.Tables["Prodotti"].Rows)
                {
                    Console.WriteLine($"{row["ID"],-5} {row["CodiceProdotto"],-15} {row["Categoria"],-20} " +
                        $"{row["Descrizione"],-50} {row["PrezzoUnitario"],-15} {row["QuantitàDisponibile"],-4}");
                }

            }
        }

        public static void AggiungiProdotto()
        {
            using (SqlConnection conn = new(connectionString))
            {
                DataSet dsMagazzino = new DataSet();

                conn.Open();

                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Problemi di connessione...");

                SqlDataAdapter prodottiAdapter = new();

                SqlCommand selectProdotti = new SqlCommand("SELECT * FROM Prodotti", conn);

                prodottiAdapter.SelectCommand = selectProdotti;

                prodottiAdapter.InsertCommand = GetProdottoInsertCommand(conn);

                prodottiAdapter.Fill(dsMagazzino, "Prodotti");

                conn.Close();

                DataRow newRow = dsMagazzino.Tables["Prodotti"].NewRow();

                string codice = "";
                bool isUnique = true;
                do
                {
                    isUnique = true;
                    Console.WriteLine("Codice prodotto: ");
                    codice = Console.ReadLine();
                    foreach (DataRow row in dsMagazzino.Tables["Prodotti"].Rows)
                    {
                        if ((string)row["CodiceProdotto"] == codice)
                            isUnique = false;
                    }
                    if (!isUnique)
                        Console.WriteLine("Codice già presente, riprova");
                    else
                    {
                        newRow["CodiceProdotto"] = codice;
                    }
                } while (isUnique == false);

                bool continua = false;
                string categoria = "";
                do
                {
                    continua = false;
                    Console.WriteLine("Categoria(1 Alimentari/2 Sanitari/3 Cancelleria): ");
                    string scelta = Console.ReadLine();

                    switch (scelta)
                    {
                        case "1":
                            categoria = "Alimentari";
                            break;
                        case "2":
                            categoria = "Sanitari";
                            break;
                        case "3":
                            categoria = "Cancelleria";
                            break;
                        default:
                            continua = true;
                            break;
                    }
                } while (continua == true);
                
                Console.WriteLine("Descrizione: ");
                string descrizione = Console.ReadLine();

                string prezzoU = "";
                do
                {
                    Console.WriteLine("Prezzo Unitario (usare la virgola per separare le cifre decimali): ");
                    prezzoU = Console.ReadLine();

                } while(!(decimal.TryParse(prezzoU,out decimal prezzo))||prezzo<0);

                string quantita = "";
                do
                {
                    Console.WriteLine("Quantità disponibile: ");
                    quantita = Console.ReadLine();

                } while (!(int.TryParse(quantita, out int quanto)) || quanto <= 0);

                newRow["Categoria"] = categoria;
                newRow["Descrizione"] = descrizione;
                newRow["PrezzoUnitario"] = prezzoU;
                newRow["QuantitàDisponibile"] = quantita;

                dsMagazzino.Tables["Prodotti"].Rows.Add(newRow);

                prodottiAdapter.Update(dsMagazzino, "Prodotti");
            }
        }

        private static SqlCommand GetProdottoInsertCommand(SqlConnection conn)
        {
            SqlCommand insertCommand = new SqlCommand();

            insertCommand.Connection = conn;
            insertCommand.CommandText = "INSERT INTO Prodotti " +
                "VALUES(@codice, @categoria, @descrizione, @prezzoU, @quantita)";

            insertCommand.CommandType = System.Data.CommandType.Text;

            insertCommand.Parameters.Add(
                new SqlParameter(
                    "@codice",
                    SqlDbType.NVarChar,
                    10,
                    "CodiceProdotto"
                )
            );

            insertCommand.Parameters.Add(
                new SqlParameter(
                    "@categoria",
                    SqlDbType.NVarChar,
                    20,
                    "Categoria"
                )
            );

            insertCommand.Parameters.Add(
                new SqlParameter(
                    "@descrizione",
                    SqlDbType.NVarChar,
                    500,
                    "Descrizione"
                )
            );

            insertCommand.Parameters.Add(
                new SqlParameter(
                    "@prezzoU",
                    SqlDbType.Decimal,
                    10,
                    "PrezzoUnitario"
                )
            );

            insertCommand.Parameters.Add(
                new SqlParameter(
                    "@quantita",
                    SqlDbType.Int,
                    10,
                    "QuantitàDisponibile"
                )
            );

            return insertCommand;
        }

        public static void EliminaProdotto()
        {
            using (SqlConnection conn = new(connectionString))
            {
                DataSet dsMagazzino = new DataSet();

                conn.Open();

                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Problemi di connessione...");

                SqlDataAdapter prodottiAdapter = new();

                prodottiAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                SqlCommand selectAutori = new SqlCommand("SELECT * FROM Prodotti", conn);

                prodottiAdapter.SelectCommand = selectAutori;

                prodottiAdapter.DeleteCommand = GetProdottoDeleteCommand(conn);

                prodottiAdapter.Fill(dsMagazzino, "Prodotti");

                conn.Close();

                MostraProdotti();
                Console.WriteLine("ID del prodotto da eliminare: ");
                string id = Console.ReadLine();

                DataRow rowToChange = dsMagazzino.Tables["Prodotti"].Rows.Find(id);
                if (rowToChange != null)
                {
                    rowToChange.Delete();

                    prodottiAdapter.Update(dsMagazzino, "Prodotti");
                }
            }
        }

        private static SqlCommand GetProdottoDeleteCommand(SqlConnection conn)
        {
            SqlCommand deleteCommand = new SqlCommand();

            deleteCommand.Connection = conn;
            deleteCommand.CommandText = "DELETE FROM Prodotti WHERE Id = @id";

            deleteCommand.CommandType = System.Data.CommandType.Text;

            deleteCommand.Parameters.Add(
                new SqlParameter(
                    "@id",
                    SqlDbType.Int,
                    10,
                    "ID"
                )
            );

            return deleteCommand;
        }

        public static void ModificaProdotto()
        {
            using (SqlConnection conn = new(connectionString))
            {
                DataSet dsMagazzino = new DataSet();

                conn.Open();

                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Problemi di connessione...");

                SqlDataAdapter prodottiAdapter = new();

                prodottiAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                SqlCommand selectProdotti = new SqlCommand("SELECT * FROM Prodotti", conn);

                prodottiAdapter.SelectCommand = selectProdotti;

                prodottiAdapter.UpdateCommand = GetProdottoUpdateCommand(conn);

                prodottiAdapter.Fill(dsMagazzino, "Prodotti");

                conn.Close();

                MostraProdotti();
                Console.WriteLine("ID del prodotto da modificare: ");
                string id = Console.ReadLine();

                DataRow rowToChange = dsMagazzino.Tables["Prodotti"].Rows.Find(id);

                if (rowToChange != null)
                {
                    string codice = "";
                    bool isUnique = true;
                    do
                    {
                        isUnique = true;
                        Console.WriteLine("Codice prodotto: ");
                        codice = Console.ReadLine();
                        if ((string)rowToChange["CodiceProdotto"] != codice)
                        {
                            foreach (DataRow row in dsMagazzino.Tables["Prodotti"].Rows)
                            {

                                if ((string)row["CodiceProdotto"] == codice)
                                    isUnique = false;
                            }
                            if (!isUnique)
                                Console.WriteLine("Codice già presente, riprova");
                            else
                            {
                                rowToChange["CodiceProdotto"] = codice;
                            }
                        }   
                    } while (isUnique == false);

                    bool continua = false;
                    string categoria = "";
                    do
                    {
                        continua = false;
                        Console.WriteLine("Categoria(1 Alimentari /2 Sanitari /3 Cancelleria): ");
                        string scelta = Console.ReadLine();

                        switch (scelta)
                        {
                            case "1":
                                categoria = "Alimentari";
                                break;
                            case "2":
                                categoria = "Sanitari";
                                break;
                            case "3":
                                categoria = "Cancelleria";
                                break;
                            default:
                                continua = true;
                                break;
                        }
                    } while (continua == true);

                    Console.WriteLine("Descrizione: ");
                    string descrizione = Console.ReadLine();

                    string prezzoU = "";
                    do
                    {
                        Console.WriteLine("Prezzo Unitario (usare la virgola per separare le cifre decimali): ");
                        prezzoU = Console.ReadLine();

                    } while (!(decimal.TryParse(prezzoU, out decimal prezzo)) || prezzo <= 0);

                    string quantita = "";
                    do
                    {
                        Console.WriteLine("Quantità disponibile: ");
                        quantita = Console.ReadLine();

                    } while (!(int.TryParse(quantita, out int quanto)) || quanto <= 0);

                    rowToChange["ID"] = id;
                    rowToChange["Categoria"] = categoria;
                    rowToChange["Descrizione"] = descrizione;
                    rowToChange["PrezzoUnitario"] = prezzoU;
                    rowToChange["QuantitàDisponibile"] = quantita;

                    prodottiAdapter.Update(dsMagazzino, "Prodotti");
                }
            }
        }

        private static SqlCommand GetProdottoUpdateCommand(SqlConnection conn)
        {
            SqlCommand updateCommand = new SqlCommand();

            updateCommand.Connection = conn;
            updateCommand.CommandText = "UPDATE Prodotti " +
                "SET CodiceProdotto = @codice, Categoria = @categoria, Descrizione = @descrizione, " +
                "PrezzoUnitario = @prezzoU, QuantitàDisponibile = @quantita " +
                "WHERE Id = @id";

            updateCommand.CommandType = System.Data.CommandType.Text;

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@id",
                    SqlDbType.Int,
                    10,
                    "ID"
                )
            );

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@codice",
                    SqlDbType.NVarChar,
                    10,
                    "CodiceProdotto"
                )
            );

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@categoria",
                    SqlDbType.NVarChar,
                    20,
                    "Categoria"
                )
            );

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@descrizione",
                    SqlDbType.NVarChar,
                    500,
                    "Descrizione"
                )
            );

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@prezzoU",
                    SqlDbType.Decimal,
                    10,
                    "PrezzoUnitario"
                )
            );

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@quantita",
                    SqlDbType.Int,
                    10,
                    "QuantitàDisponibile"
                )
            );

            return updateCommand;
        }

        public static void ProdottiQuantitaLimitata()
        {
            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();

                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Problemi di connessione...");

                SqlCommand leggi = new("SELECT * FROM Prodotti WHERE QuantitàDisponibile<10 ", conn);
                leggi.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = leggi.ExecuteReader();

                Console.WriteLine();
                Console.WriteLine("{0,-18}{1,-20}{2,-50}{3,-20}", "Codice prodotto", "Categoria", "Descrizione", "Quantità");
                Console.WriteLine(new String('-', 100));

                while (reader.Read())
                {
                    Console.WriteLine(
                        "{0,-18}{1,-20}{2,-50}{3,-20}",
                        reader["CodiceProdotto"],
                        reader["Categoria"],
                        reader["Descrizione"],
                        reader["QuantitàDisponibile"]
                    );
                }

                Console.WriteLine(new String('-', 100));
                Console.WriteLine();

                conn.Close();
            }
        }

        public static void QuantitaProdottiPerCategoria()
        {
            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();

                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Problemi di connessione...");

                SqlCommand leggi = new("SELECT Categoria, COUNT(*) as NumeroProdotti FROM Prodotti GROUP BY Categoria ", conn);
                leggi.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = leggi.ExecuteReader();

                Console.WriteLine();
                Console.WriteLine("{0,-20}{1,-10}", "Categoria", "Numero Prodotti");
                Console.WriteLine(new String('-', 40));

                while (reader.Read())
                {
                    Console.WriteLine(
                        "{0,-20}{1,-10}",
                        reader["Categoria"],
                        reader["NumeroProdotti"]
                    );
                }

                Console.WriteLine(new String('-', 40));
                Console.WriteLine();

                conn.Close();
            }
        }
    }
}