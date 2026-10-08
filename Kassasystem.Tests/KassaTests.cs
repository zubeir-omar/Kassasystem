namespace Kassasystem.Tests;

public class KassaTests
{
    // ---------- Produkt: prisberäkning och antal ----------

    [Fact]
    public void BeraknaPris_TreKaffe_Blir17970()
    {
        StyckProdukt kaffe = new StyckProdukt("302", "Kaffe", 59.90m);

        Assert.Equal(179.70m, kaffe.BeraknaPris(3));
    }

    [Fact]
    public void BeraknaPris_TvaMjolk_Blir3180()
    {
        StyckProdukt mjolk = new StyckProdukt("301", "Mjölk", 15.90m);

        Assert.Equal(31.80m, mjolk.BeraknaPris(2));
    }

    [Fact]
    public void BeraknaPris_HalvannatKiloBananer_Blir4485()
    {
        KiloProdukt bananer = new KiloProdukt("300", "Bananer", 29.90m);

        Assert.Equal(44.85m, bananer.BeraknaPris(1.5m));
    }

    [Fact]
    public void StyckProdukt_DecimalAntal_ArInteGiltigt()
    {
        StyckProdukt mjolk = new StyckProdukt("301", "Mjölk", 15.90m);

        Assert.False(mjolk.ArGiltigtAntal(1.5m));
    }

    [Fact]
    public void KiloProdukt_DecimalAntal_ArGiltigt()
    {
        KiloProdukt bananer = new KiloProdukt("300", "Bananer", 29.90m);

        Assert.True(bananer.ArGiltigtAntal(1.5m));
    }

    [Fact]
    public void NollEllerNegativtAntal_ArInteGiltigt()
    {
        StyckProdukt mjolk = new StyckProdukt("301", "Mjölk", 15.90m);

        Assert.False(mjolk.ArGiltigtAntal(0));
        Assert.False(mjolk.ArGiltigtAntal(-1));
    }

    [Fact]
    public void Hitta_IdSomInteFinns_BlirNull()
    {
        List<Produkt> produkter = new List<Produkt>();
        produkter.Add(new StyckProdukt("301", "Mjölk", 15.90m));

        Assert.Null(Produkt.Hitta(produkter, "999"));
        Assert.NotNull(Produkt.Hitta(produkter, "301"));
    }

    // ---------- Forsaljning: totalsumma och kvitto ----------

    [Fact]
    public void BeraknaTotal_SummerarAllaRader()
    {
        Forsaljning forsaljning = new Forsaljning();
        forsaljning.LaggTill(new StyckProdukt("301", "Mjölk", 15.90m), 2);      // 31,80
        forsaljning.LaggTill(new KiloProdukt("300", "Bananer", 29.90m), 1.5m);  // 44,85

        Assert.Equal(76.65m, forsaljning.BeraknaTotal());
    }

    [Fact]
    public void LaggTill_FelaktigtAntal_LaggsInteTill()
    {
        Forsaljning forsaljning = new Forsaljning();

        bool ok = forsaljning.LaggTill(new StyckProdukt("301", "Mjölk", 15.90m), 1.5m);

        Assert.False(ok);
        Assert.Empty(forsaljning.Rader);
    }

    [Fact]
    public void SkapaKvittoText_InnehallerDatumProduktOchTotal()
    {
        Forsaljning forsaljning = new Forsaljning();
        forsaljning.LaggTill(new StyckProdukt("301", "Mjölk", 15.90m), 2);

        string text = forsaljning.SkapaKvittoText(new DateTime(2026, 9, 7, 14, 30, 0));

        Assert.Contains("2026-09-07 14:30:00", text);
        Assert.Contains("Mjölk", text);
        Assert.Contains("TOTAL:", text);
    }

    // ---------- Filhantering ----------

    [Fact]
    public void TolkaRad_KiloRad_BlirKiloProdukt()
    {
        Produkt? produkt = Filhantering.TolkaRad("300;Bananer;29,90;Kg");

        Assert.IsType<KiloProdukt>(produkt);
        Assert.Equal(29.90m, produkt.Pris);
    }

    [Fact]
    public void TolkaRad_StyckRad_BlirStyckProdukt()
    {
        Produkt? produkt = Filhantering.TolkaRad("301;Mjölk;15,90;Styck");

        Assert.IsType<StyckProdukt>(produkt);
    }

    [Fact]
    public void TolkaRad_FelaktigRad_BlirNull()
    {
        Assert.Null(Filhantering.TolkaRad("300;Bananer;abc;Kg")); // bokstäver i pris
        Assert.Null(Filhantering.TolkaRad("300;Bananer;29,90"));  // del saknas
    }

    [Fact]
    public void SkapaKvittoFilnamn_RattFormat()
    {
        Assert.Equal("RECEIPT_20260907.txt", Filhantering.SkapaKvittoFilnamn(new DateTime(2026, 9, 7)));
    }

    [Fact]
    public void SparaKvitto_TvaKvittonSammaDag_BadaFinnsIFilen()
    {
        string mapp = Path.GetTempPath();
        DateTime datum = new DateTime(2000, 1, 1);
        string sokvag = Path.Combine(mapp, Filhantering.SkapaKvittoFilnamn(datum));
        File.Delete(sokvag); // börja med tom fil

        Filhantering.SparaKvitto(mapp, "Kvitto ett", datum);
        Filhantering.SparaKvitto(mapp, "Kvitto två", datum);

        string innehall = File.ReadAllText(sokvag);
        Assert.Contains("Kvitto ett", innehall);
        Assert.Contains("Kvitto två", innehall);

        File.Delete(sokvag);
    }
}