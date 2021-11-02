using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace metjelentes
{
    class Metadat
    {
        public string Varos;
        public DateTime Ido;
        public string Szelirany;
        public int Szelerosseg;
        public int Homerseklet;

        public Metadat(string varos, DateTime ido, string szelirany, int szelerosseg, int homerseklet)
        {
            Varos = varos;
            Ido = ido;
            Szelirany = szelirany;
            Szelerosseg = szelerosseg;
            Homerseklet = homerseklet;
        }
    }
    class Program
    {
        static List<Metadat> Beolvasas(string adat)
        {
            List<Metadat> adatok = new List<Metadat>();
            using (StreamReader sr = new StreamReader(adat))
            {
                while (!sr.EndOfStream)
                {
                    string sor = sr.ReadLine();
                    string[] tmp = sor.Split(' ');
                    string szeladat = tmp[2];
                    string szelirany = szeladat.Substring(0, 3); // Substring(): Egy addot string egy részét adja vissza adott karakterindextől megadott hosszúságban
                    string szelerosseg = szeladat.Substring(3);
                    string ido = tmp[1];
                    int ora = Convert.ToInt32(ido.Substring(0, 2));
                    int perc = Convert.ToInt32(ido.Substring(2));
                    Metadat ujadat = new Metadat(tmp[0], new DateTime(2020,5,18,ora,perc,00), szelirany, Convert.ToInt32(szelerosseg), Convert.ToInt32(tmp[3]));
                    adatok.Add(ujadat);
                }
            }
            return adatok;
        }

        // TODO: DateTime szerinti sortolás (bónusz)
        static void UtolsoAdat(List<Metadat> adat)
        {
            Console.WriteLine("2. feladat");
            Console.Write("Adja meg egy település kódját! Település: ");
            string bekertKod = Console.ReadLine();
            for (int i = adat.Count - 1; i > -1; i--)
            {
                if (bekertKod == adat[i].Varos)
                {
                    Console.WriteLine($"Az utolsó mérési adat a megadott településről {adat[i].Ido.Hour + ":" + adat[i].Ido.Minute}-kor érkezett.");
                    break;
                }
            }
        }
        static void LegHomerseklet(List<Metadat> adat)
        {
            int maxHo = int.MinValue;
            Metadat maxAdat = null;
            int minHo = int.MaxValue;
            Metadat minAdat = null;
            for (int i = 0; i < adat.Count; i++)
            {
                if (adat[i].Homerseklet > maxHo)
                {
                    maxHo = adat[i].Homerseklet;
                    maxAdat = adat[i];
                }
                if (adat[i].Homerseklet < minHo)
                {
                    minHo = adat[i].Homerseklet;
                    minAdat = adat[i];
                }
            }
            Console.WriteLine("3. feladat");
            Console.WriteLine($"A legalacsonyabb hőmérséklet: {minAdat.Varos} {minAdat.Ido.Hour}:{minAdat.Ido.Minute} {minAdat.Homerseklet} fok");
            Console.WriteLine($"A legmagasabb hőmérséklet: {maxAdat.Varos} {maxAdat.Ido.Hour}:{maxAdat.Ido.Minute} {maxAdat.Homerseklet} fok");
        }
        static void Szelcsend(List<Metadat> adat)
        {
            Console.WriteLine("4. feladat");
            int csendSzamlalo = 0;
            for (int i = 0; i < adat.Count; i++)
            {
                if (adat[i].Szelerosseg == 0 && adat[i].Szelirany == "000")
                {
                    Console.WriteLine($"{adat[i].Varos} {adat[i].Ido.ToString("HH:mm")}");
                    csendSzamlalo++;
                }
            }
            if (csendSzamlalo == 0)
            {
                Console.WriteLine("Nem volt szélcsend a mérések idején.");
            }
        }
        static void KozepHo(List<Metadat> adat)
        {
            // Kulonbozo varos azonositok kivalogatasa string listaba
            List<string> kulonbozoVarosok = adat.Select(x => x.Varos).Distinct().ToList();
            Dictionary<string, int> varosonkentiMeresekSzama = new Dictionary<string, int>();
            List<int> meresOrak = new List<int> { 1, 7, 13, 19 };

            // 
            //kulonbozoVarosok.ForEach(x => varosonkentiMeresekSzama.Add(x, adat.Count(y => y.Varos == x && meresOrak.Contains(y.Ido.Hour))));

            foreach (string varos in kulonbozoVarosok)
            {
                for (int i = 0; i < adat.Count; i++)
                {
                    if (varos == adat[i].Varos && meresOrak.Contains(adat[i].Ido.Hour))
                    {
                        if (varosonkentiMeresekSzama.ContainsKey(varos))
                        {
                            varosonkentiMeresekSzama[varos]++;
                        }
                        else
                        {
                            varosonkentiMeresekSzama.Add(varos, 1);
                        }
                    }
                }
            }

            foreach (var varos in kulonbozoVarosok)
            {
                int osszHomerseklet = 0;
                string atlagHomerseklet = ""; // STRING!!! KIVÉTELES ESET (N.A)
                int meresDb = 0;
                for (int j = 0; j < adat.Count; j++)
                {
                    if (varos == adat[j].Varos && meresOrak.Contains(adat[j].Ido.Hour))
                    {
                        osszHomerseklet += adat[j].Homerseklet;
                        meresDb++;
                    }
                }

                //atlagHomerseklet = meresOrak.All(x => adat.Any(y => y.Varos == varos && y.Ido.Hour == x)) ? $"{osszHomerseklet / varosonkentiMeresekSzama[varos]}" : "NA";
                #region
                bool ora1 = false;
                bool ora7 = false;
                bool ora13 = false;
                bool ora19 = false;
                for (int j = 0; j < adat.Count; j++)
                {
                    if (adat[j].Varos == varos)
                    {
                        if (adat[j].Ido.Hour == 1)
                        {
                            ora1 = true;
                        }
                        else if (adat[j].Ido.Hour == 7)
                        {
                            ora7 = true;
                        }
                        else if (adat[j].Ido.Hour == 13)
                        {
                            ora13 = true;
                        }
                        else if (adat[j].Ido.Hour == 19)
                        {
                            ora19 = true;
                        }
                    }
                }

                if (ora1 && ora7 && ora13 && ora19)
                {
                    atlagHomerseklet = $"Középhőmérséklet: {osszHomerseklet / varosonkentiMeresekSzama[varos]}";
                }
                else
                {
                    atlagHomerseklet = "N.A";
                }
                #endregion

                int max = int.MinValue;
                int min = int.MaxValue;
                for (int j = 0; j < adat.Count; j++)
                {
                    if (varos == adat[j].Varos && adat[j].Homerseklet > max)
                    {
                        max = adat[j].Homerseklet;
                    }
                    if (varos == adat[j].Varos && adat[j].Homerseklet < min)
                    {
                        min = adat[j].Homerseklet;
                    }
                }

                int ingadozas = max - min;

                Console.WriteLine($"{varos} {atlagHomerseklet} Hőmérséklet-ingadozás: {ingadozas}");
            }
        }

        static void TelepulesenkentiStatisztika(List<Metadat> adat)
        {
            List<string> kulonbozoVarosok = adat.Select(x => x.Varos).Distinct().ToList();

            for (int i = 0; i < kulonbozoVarosok.Count; i++)
            {
                using (StreamWriter sw = new StreamWriter($"{kulonbozoVarosok[i]}.txt"))
                {
                    sw.WriteLine(kulonbozoVarosok[i]);
                    for (int j = 0; j < adat.Count; j++)
                    {
                        if (adat[j].Varos == kulonbozoVarosok[i])
                        {
                            sw.Write(adat[j].Ido.ToString("HH:mm"));
                            sw.Write(" ");
                            for (int k = 0; k < adat[j].Szelerosseg; k++)
                            {
                                sw.Write("#");
                            }

                            sw.WriteLine();
                        }
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            List<Metadat> adatok = Beolvasas("tavirathu13.txt");
            UtolsoAdat(adatok);
            LegHomerseklet(adatok);
            Szelcsend(adatok);
            KozepHo(adatok);
            TelepulesenkentiStatisztika(adatok);
            Console.ReadKey();
        }
    }
}
