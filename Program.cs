using System;
using System.Linq;

var df = "influd.csv".Open().Read();

var query = df.Where(c => c.IsCovid)
            .GroupBy(c => c.NumberOfVaccines)
            .OrderBy(g => g.Key)
            .Select(g => new {
                numVaccines = g.Key,
                letality = g.Average(c => c.IsDead ? 1.0 : 0.0)
            });

foreach(var group in query)
{
    Console.WriteLine($"Num vacs: {group.numVaccines} Letality: {Math.Round(group.letality * 100, 2)}%");
}

