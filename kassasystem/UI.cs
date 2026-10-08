// Klassen UI: allt som handlar om användaren – meny, inmatning och utskrift.
public static class UI
{
    public static void VisaHuvudmeny()
    {
        Console.WriteLine();
        Console.WriteLine("KASSASYSTEM");
        Console.WriteLine("1. Ny kund");
        Console.WriteLine("2. Admin");
        Console.WriteLine("0. Avsluta");
        Console.Write("Val: ");
    }

    // En kund: lägg till varor tills användaren skriver PAY
    public static void NyKund(List<Produkt> produkter, string kvittoMapp)
    {
        Forsaljning forsaljning = new Forsaljning();

        Console.WriteLine();
        Console.WriteLine("Skriv <produktid> <antal>, t.ex. 300 2 eller 300 1,5. Skriv PAY för att betala.");

        while (true)
        {
            Console.Write("> ");
            string? inmatning = Console.ReadLine();

            if (inmatning == null)
            {
                return;
            }

            if (inmatning.Trim().ToUpper() == "PAY")
            {
                Betala(forsaljning, kvittoMapp);
                return;
            }

            LaggTillVara(forsaljning, produkter, inmatning);
        }
    }

    private static void LaggTillVara(Forsaljning forsaljning, List<Produkt> produkter, string inmatning)
    {
        // Dela upp "300 2" i två delar vid mellanslaget
        string[] delar = inmatning.Trim().Split(' ');
        if (delar.Length != 2)
        {
            Console.WriteLine("Fel format. Skriv produktid, ett mellanslag och antal, t.ex. 300 2");
            return;
        }

        Produkt? produkt = Produkt.Hitta(produkter, delar[0]);
        if (produkt == null)
        {
            Console.WriteLine($"Produkt-ID {delar[0]} finns inte.");
            return;
        }

        if (!decimal.TryParse(delar[1], out decimal antal))
        {
            Console.WriteLine($"{delar[1]} är inte ett giltigt antal.");
            return;
        }

        if (!forsaljning.LaggTill(produkt, antal))
        {
            Console.WriteLine($"Felaktigt antal för {produkt.Namn}.");
            return;
        }

        Console.WriteLine($"{produkt.Namn} {antal} {produkt.HamtaEnhet()} = {produkt.BeraknaPris(antal):F2} kr");
    }

    private static void Betala(Forsaljning forsaljning, string kvittoMapp)
    {
        if (forsaljning.Rader.Count == 0)
        {
            Console.WriteLine("Inga varor. Försäljningen avbryts.");
            return;
        }

        DateTime nu = DateTime.Now;
        string kvittoText = forsaljning.SkapaKvittoText(nu);
        Console.WriteLine();
        Console.WriteLine(kvittoText);

        try
        {
            string sokvag = Filhantering.SparaKvitto(kvittoMapp, kvittoText, nu);
            Console.WriteLine($"Kvittot sparades i {sokvag}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Kvittot kunde inte sparas: {ex.Message}");
        }
    }
}
