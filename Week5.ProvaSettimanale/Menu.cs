using System;

namespace Week5.ProvaSettimanale
{
    public class Menu
    {

        public static void Start()
        {
            Console.WriteLine("-----Prova Week5-----");

            bool continua = true;

            do
            {
                Console.WriteLine();
                Console.WriteLine("Gestione Magazzino");
                Console.WriteLine("[1] Mostra prodotti");
                Console.WriteLine("[2] Aggiungi un prodotto");
                Console.WriteLine("[3] Elimina un prodotto");
                Console.WriteLine("[4] Modifica i dati di un prodotto");
                Console.WriteLine("[5] Elenco prodotti con giacenza limitata");
                Console.WriteLine("[6] Numero di prodotti per ogni categoria");
                Console.WriteLine("[q] Esci");
                string scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                        ProdottiManager.MostraProdotti();
                        break;
                    case "2":
                        ProdottiManager.AggiungiProdotto();
                        break;
                    case "3":
                        ProdottiManager.EliminaProdotto();
                        break;
                    case "4":
                        ProdottiManager.ModificaProdotto();
                        break;
                    case "5":
                        ProdottiManager.ProdottiQuantitaLimitata();
                        break;
                    case "6":
                        ProdottiManager.QuantitaProdottiPerCategoria();
                        break;
                    case "q":
                        continua = false;
                        break;
                    default:
                        Console.WriteLine("Scelta non valida!");
                        break;
                }

            } while (continua == true);

            Console.WriteLine("-----Arrivederci-----");
        }
    }
}