// Klassen Filhanetering: allt som läser från och skriver till fil.
// Skriver aldrig till konsolen.
public static class Filhantering
{ 
    // Läser alla produkter från filen. Felaktiga rader hoppas över.
    // Om filen saknas kastas ett fel som fångas i Program.cs
       public static List<Produkt> LasProdukter(string sokvag)
    {
        List<Produkt> produkter = new List<Produkt>();
        string[] rader = File.ReadAllLines(sokvag);

        foreach (string rad in rader)
        {
            Produkt? produkt = TolkaRad(rad);
            if (produkt != null)
            {
                produkter.Add(produkt);
            }
        }

        return produkter;
    }

    // Gör om en rad, t.ex. "300;Bananer;29,90;Kg", till en produkt.
    // Returnerar null om raden är felaktig.
    public static Produkt? TolkaRad(string rad)
    {
        string[] delar = rad.Split(';');
        if (delar.Length != 4)
        {
            return null;
        }

        string id = delar[0];
        string namn = delar[1];
        string typ = delar[3].ToLower();

        bool okPris = decimal.TryParse(delar[2], out decimal pris);
        if (!okPris || pris < 0)
        {
            return null;
        }

        switch (typ)
        {
            case "kg":
                return new KiloProdukt(id, namn, pris);
            case "styck":
                return new StyckProdukt(id, namn, pris);
            default:
                return null;
        }
    }

    // ---------- Kvitton ----------

    public static string SkapaKvittoFilnamn(DateTime datum)
    {
        return "RECEIPT_" + datum.ToString("yyyyMMdd") + ".txt";
    }

    // AppendAllText lägger till i slutet av filen, så gamla kvitton skrivs inte över.
    // Returnerar sökvägen till filen.
    public static string SparaKvitto(string mapp, string kvittoText, DateTime datum)
    {
        string sokvag = Path.Combine(mapp, SkapaKvittoFilnamn(datum));
        File.AppendAllText(sokvag, kvittoText + Environment.NewLine);
        return sokvag;
    }
}
