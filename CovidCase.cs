public class CovidCase
{
    public bool IsDead { get; set; }
    public bool IsCovid { get; set; }
    public int NumberOfVaccines { get; set; } = 0;
    public Vaccines Vaccine { get; set; } = Vaccines.Desconhecido;
}

public enum Vaccines
{
    Coronavac,
    Astrazeneca,
    Pfizer,
    Sinovac,
    Janssen,
    India,
    Desconhecido
}