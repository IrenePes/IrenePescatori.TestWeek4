using Academy.Week4.EsercitazioneFinale;

bool esci = false;
do
{
    Console.WriteLine($"---------- Menu ----------");
    Console.WriteLine();
    Console.WriteLine("[ 1 ] - Inserisci nuova spesa\n");
    Console.WriteLine("[ 2 ] - Approva una spesa esistente\n");
    Console.WriteLine("[ 3 ] - Elimina una spesa esistente\n");
    Console.WriteLine("[ 4 ] - Mostra elenco delle spese approvate\n");
    Console.WriteLine("[ 5 ] - Mostra elenco delle spese di uno specifico utente\n");
    Console.WriteLine("[ 5 ] - Mostra il totale delle spese per categoria\n");
    Console.WriteLine("[ Q ] - ESCI\n");


    Console.Write("> ");
    string scelta = Console.ReadLine();
    Console.WriteLine();

    switch (scelta)
    {
        case "1":
            // Inserisci nuova spesa
            ConnectedMode.InserisciNuovaSpesa();
            //DisconnectedMode.InserisciSpesa();
            break;
        case "2":
            // Approva una spesa esistente
            //ConnectedMode.ApprovaSpesa();
            DisconnectedMode.ApprovaSpesa();
            break;
        case "3":
            // Elimina una spesa esistente
            //ConnectedMode.CancellaSpesa();
            DisconnectedMode.CancellaSpesa();
            break;
        case "4":
            // Mostra spese approvate
            ConnectedMode.MostraSpeseApprovate();
            break;
        case "5":
            // Mostra spese di un utente
            ConnectedMode.MostraSpeseDiUnUtente();
            break;
        case "6":
            // Mostra totale delle spese per categoria
            ConnectedMode.MostraTotSpesePerCategoria();
            break;
        case "Q":
            esci = true;
            break;
        default:
            Console.WriteLine("Comando sconosciuto");
            break;
    }

} while (!esci);

