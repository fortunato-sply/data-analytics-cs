using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

public static class MyExtensionMethods
{
    public static IEnumerable<string> Open(this string path)
    {
        var stream = new StreamReader(path);

        while (!stream.EndOfStream)
            yield return stream.ReadLine();
        
        stream.Close();
    } 

    public static IEnumerable<CovidCase> Read(this IEnumerable<string> coll)
    {
        var it = coll.GetEnumerator();

        var header = it.MoveNext() ? it.Current.Replace("\"", "").Split(";").ToList() : null;
        if (header == null)
            throw new Exception("CSV vazio");

        string[][] allVacs = new string[][]
        {
            new string[] { "CORONAVAC" },
            new string[] { "ASTRAZENECA" },
            new string[] { "PFIZER" },
            new string[] { "SINOVAC" },
            new string[] { "JANSSEN" },
            new string[] { "INDIA" },
            new string[] { "DESCONHECIDO" }
        }

        while (it.MoveNext())
        {
            var data = it.Current.Replace("\"", "").Split(";");

            CovidCase newCovidCase = new CovidCase();
            newCovidCase.IsCovid = data[header.IndexOf("CLASSI_FIN")] == "5";
            newCovidCase.IsDead = data[header.IndexOf("EVOLUCAO")] == "2";
            
            if (data[header.IndexOf("FAB_COV_1")] != "")
            {
                var vac = allVacs.FirstOrDefault(vacs =>
                    vacs.Any(el => data[header.IndexOf("FAB_COV_1")]
                        .Contains(el, StringComparison.OrdinalIgnoreCase)));

                if (vac != null)
                {
                    switch (vac[0])
                    {
                        case "CORONAVAC":
                            newCovidCase.Vaccine = Vaccines.Coronavac;
                            break;
                        
                        case "ASTRAZENECA":
                            newCovidCase.Vaccine = Vaccines.Astrazeneca;
                            break;

                        case "PFIZER":
                            newCovidCase.Vaccine = Vaccines.Pfizer;
                            break;

                        case "SINOVAC":
                            newCovidCase.Vaccine = Vaccines.Sinovac;
                            break;

                        case "JANSSEN":
                            newCovidCase.Vaccine = Vaccines.Janssen;
                            break;

                        case "INDIA":
                            newCovidCase.Vaccine = Vaccines.India;
                            break;
                    }
                }
            }

            if (data[header.IndexOf("DOSE_1_COV")] != "")
                newCovidCase.NumberOfVaccines++;
            if (data[header.IndexOf("DOSE_2_COV")] != "")
                newCovidCase.NumberOfVaccines++;
            if (data[header.IndexOf("DOSE_REF")] != "")
                newCovidCase.NumberOfVaccines++;

            yield return newCovidCase;
        }
    }
}