using System.ComponentModel;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared;
using HE.Investments.Account.WWW.Models.UserOrganisation;
using HE.Investments.Account.WWW.Routing;
using HE.Investments.Common.Contract;

namespace HE.Investments.Account.WWW.Utils;

public class Programmes : IProgrammes
{
    private readonly ProgrammeUrlConfig _programmeUrlConfig;

    private readonly Dictionary<ProgrammeType, ProgrammeModel> _programmes;

    public Programmes(ProgrammeUrlConfig programmeUrlConfig, IAccountUserContext accountUserContext)
    {
        _programmeUrlConfig = programmeUrlConfig;
        SelectedOrganisationId = accountUserContext.GetSelectedAccount().Result.SelectedOrganisationId();
        _programmes = BuildProgrammeModels();
    }

    private OrganisationId SelectedOrganisationId { get; }

    public ProgrammeModel GetProgramme(ProgrammeType programmeType)
    {
        return _programmes.TryGetValue(programmeType, out var programmeModel)
            ? programmeModel
            : throw new InvalidEnumArgumentException($"Programme for {programmeType} does not exist.");
    }

    public string GetUrl(ProgrammeType programmeType, HeApplianceId id)
    {
        if (programmeType == ProgrammeType.Ahp)
        {
            return $"{_programmeUrlConfig.Ahp}/{SelectedOrganisationId}/project/{id}";
        }

        if (programmeType == ProgrammeType.Loans)
        {
            return $"{_programmeUrlConfig.Loans}/{SelectedOrganisationId}/application/{id.ToGuidAsString()}";
        }

        throw new InvalidEnumArgumentException($"Programme for {programmeType} does not exist.");
    }

    private Dictionary<ProgrammeType, ProgrammeModel> BuildProgrammeModels()
    {
        return new Dictionary<ProgrammeType, ProgrammeModel>
        {
            {
                ProgrammeType.Loans, new(
                    ProgrammeType.Loans,
                    "Levelling Up Home Building Fund",
                    "Applications",
                    "Start a new Levelling Up Home Building Fund application. This will not affect any of your previous applications.",
                    "View all applications",
                    $"{_programmeUrlConfig.Loans}/{SelectedOrganisationId}/dashboard",
                    5)
            },
            {
                ProgrammeType.Ahp, new(
                    ProgrammeType.Ahp,
                    "Affordable Homes Programme Continuous Market Engagement 2021-2026",
                    null,
                    "Start a new Affordable Homes Programme application. This will not affect any of your previous applications.",
                    "View all projects",
                    $"{_programmeUrlConfig.Ahp}/{SelectedOrganisationId}/projects",
                    3)
            },
        };
    }
}
