// Klassen Forsaljning är ett köp för EN kund.
// Den håller alla rader, räknar ut totalen och skapar kvittotexten.

public class Forsaljning
{
    public List<Forsaljningsrad> Rader { get; } = new List<Forsaljningsrad>();

    // Lägger till en rad om antalet är giltigt. Returnerar false annars.
    public bool LaggTill(Produkt produkt, decimal antal)
    {
        if (!produkt.ArGiltigtAntal(antal))
        {
            return false;
        }

        Rader.Add(new Forsaljningsrad(produkt, antal));
        return true;
    }

    public decimal BeraknaTotal()
    {
        decimal total = 0;
        foreach (Forsaljningsrad rad in Rader)
        {
            total += rad.BeraknaRadpris();
        }
        return total;
    }
    // så att testerna kan använda ett fast datum.
    public string SkapaKvittoText(DateTime datum)
    {
        string nl = Environment.NewLine;
        string text = "========================================" + nl +
                      "KVITTO" + nl +
                      datum.ToString("yyyy-MM-dd HH:mm:ss") + nl +
                      "----------------------------------------" + nl;

        foreach (Forsaljningsrad rad in Rader)
        {
            string enhet = rad.Produkt.HamtaEnhet();
            text += rad.Produkt.Namn + nl;
            text += $"  {rad.Antal} {enhet} x {rad.Produkt.Pris:F2} kr/{enhet} = {rad.BeraknaRadpris():F2} kr" + nl;
        }

        text += "----------------------------------------" + nl +
                $"TOTAL: {BeraknaTotal():F2} kr" + nl +
                "========================================" + nl;
        return text;
    }
}

//En rad på kvitto: Vilken produkt och hur många/hur mycket.
public class Forsaljningsrad
{
    public Produkt Produkt { get; }
    public decimal Antal { get; }

    public Forsaljningsrad(Produkt produkt, decimal antal)
    {
        Produkt = produkt;
        Antal = antal;
    }
    public decimal BeraknaRadpris()
    {
        return Produkt.BeraknaPris(Antal);
    }
}