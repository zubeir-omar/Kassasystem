//Klassen produkt beskriver EN produkt i butiken.
// Den är abstrakt = man kan inte skapa en "Produkt" direkt,
// bara en StyckProdukt eller en KiloProdukt (längre ner i filen).
public abstract class Produkt
{
    // ---------- Egenskaper ----------
    public string Id { get; }
    public string Namn { get; }
    public decimal Pris { get; }

    // ---------- Konstruktor ----------
    public Produkt(string id, string namn, decimal pris)
    {
        Id = id;
        Namn = namn;
        Pris = pris;
    }

    // ---------- Metoder ----------

    // Abstrakta metoder: varje produkttyp skriver sin egen version (polymorfism)
    public abstract bool ArGiltigtAntal(decimal antal);
    public abstract string HamtaEnhet();

    // Samma beräkning för alla produkter
    public decimal BeraknaPris(decimal antal)
    {
        return Math.Round(Pris * antal, 2);
    }

    // Letar upp en produkt med rätt ID i en lista. Returnerar null om den inte finns.
    public static Produkt? Hitta(List<Produkt> produkter, string id)
    {
        foreach (Produkt produkt in produkter)
        {
            if (produkt.Id == id)
            {
                return produkt;
            }
        }
        return null;
    }
}

// Säljs per styck, t.ex. mjölk. Bara heltal är tillåtna.
public class StyckProdukt : Produkt
{
    public StyckProdukt(string id, string namn, decimal pris) : base(id, namn, pris)
    {
    }

    public override bool ArGiltigtAntal(decimal antal)
    {
        // Math.Floor tar bort decimalerna. Är talet oförändrat var det ett heltal.
        return antal > 0 && antal == Math.Floor(antal);
    }

    public override string HamtaEnhet()
    {
        return "st";
    }
}

// Säljs per kilo, t.ex. bananer. Decimaler är tillåtna (1.5 kg).
public class KiloProdukt : Produkt
{
    public KiloProdukt(string id, string namn, decimal pris) : base(id, namn, pris)
    {
    }

    public override bool ArGiltigtAntal(decimal antal)
    {
        return antal > 0;
    }

    public override string HamtaEnhet()
    {
        return "kg";
    }
}
