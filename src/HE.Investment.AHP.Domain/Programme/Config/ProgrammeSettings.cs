using Microsoft.Extensions.Configuration;

namespace HE.Investment.AHP.Domain.Programme.Config;

public class ProgrammeSettings : IProgrammeSettings
{
    public ProgrammeSettings(IConfiguration configuration)
    {
        AhpProgrammeId = configuration.GetValue<string>("AppConfiguration:AhpProgrammeId") ??
                         throw new ArgumentException("Config AppConfiguration:AhpProgrammeId is missing");
    }

    public string AhpProgrammeId { get; }
}
