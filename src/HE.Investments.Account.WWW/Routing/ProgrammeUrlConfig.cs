namespace HE.Investments.Account.WWW.Routing;

public class ProgrammeUrlConfig
{
    public IDictionary<string, string> ProgrammeUrl { get; set; }

    public string Loans => GetProgrammeUrlOrDefault(nameof(Loans));

    public string Ahp => GetProgrammeUrlOrDefault(nameof(Ahp));

    public string FrontDoor => GetProgrammeUrlOrDefault(nameof(FrontDoor));

    private string GetProgrammeUrlOrDefault(string programmeName)
    {
        return ProgrammeUrl.TryGetValue(programmeName, out var programmeUrl) ? programmeUrl : string.Empty;
    }
}
