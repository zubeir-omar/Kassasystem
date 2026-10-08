// ===================== DEL 1: LÄS PRODUKTER =====================
// produkter.txt kopieras till samma mapp som programmet (se Properties på filen)

string mapp = AppContext.BaseDirectory;
string produktFil = Path.Combine(mapp, "produkter.txt");
List<Produkt> produkter;

try
{
    produkter = Filhantering.LasProdukter(produktFil);
}
catch (Exception ex)
{
    Console.WriteLine($"Kunde inte läsa produktfilen: {ex.Message}");
    return;
}

Console.WriteLine($"{produkter.Count} produkter inlästa.");


// ===================== DEL 2: HUVUDMENYN =====================

bool kor = true;
while (kor)
{
    UI.VisaHuvudmeny();
    string? val = Console.ReadLine();

    switch (val)
    {
        case "1":
            UI.NyKund(produkter, mapp);
            break;
        case "2":
            Console.WriteLine("Admin är inte implementerat.");
            break;
        case "0":
            kor = false;
            break;
        default:
            Console.WriteLine("Ogiltigt val. Ange 1, 2 eller 0.");
            break;
    }
}

Console.WriteLine("Programmet avslutas.");
