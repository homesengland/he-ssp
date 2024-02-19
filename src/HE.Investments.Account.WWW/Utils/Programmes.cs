using System.ComponentModel;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared;
using HE.Investments.Account.WWW.Models.UserOrganisation;
using HE.Investments.Account.WWW.Routing;
using NuGet.Common;

namespace HE.Investments.Account.WWW.Utils;

public class Programmes : IProgrammes
{
    private readonly ProgrammeUrlConfig _programmeUrlConfig;

    private readonly AsyncLazy<IDictionary<ProgrammeType, ProgrammeModel>> _programmes;

    public Programmes(ProgrammeUrlConfig programmeUrlConfig, IAccountAccessContext accountAccessContext)
    {
        _programmeUrlConfig = programmeUrlConfig;
        _programmes = new AsyncLazy<IDictionary<ProgrammeType, ProgrammeModel>>(async () =>
        {
            var canEditApplication = await accountAccessContext.CanEditApplication();
            return BuildProgrammeModels(canEditApplication);
        });
    }

    public async Task<ProgrammeModel> GetProgramme(ProgrammeType programmeType)
    {
        var programmes = await _programmes;
        return programmes.TryGetValue(programmeType, out var programmeModel)
            ? programmeModel
            : throw new InvalidEnumArgumentException($"Programme for {programmeType} does not exist.");
    }

    public string GetApplicationUrl(ProgrammeType programmeType, HeApplicationId applicationId)
    {
        if (programmeType == ProgrammeType.Ahp)
        {
            return $"{_programmeUrlConfig.Ahp}/application/{applicationId}";
        }

        if (programmeType == ProgrammeType.Loans)
        {
            return $"{_programmeUrlConfig.Loans}/application/{applicationId}";
        }

        throw new InvalidEnumArgumentException($"Programme for {programmeType} does not exist.");
    }

    private IDictionary<ProgrammeType, ProgrammeModel> BuildProgrammeModels(bool canCreateApplication)
    {
        return new Dictionary<ProgrammeType, ProgrammeModel>
        {
            {
                ProgrammeType.Loans, new(
                    ProgrammeType.Loans,
                    "Levelling Up Home Building Fund",
                    "Start a new Levelling Up Home Building Fund application. This will not affect any of your previous applications.",
                    $"{_programmeUrlConfig.Loans}/application",
                    $"{_programmeUrlConfig.Loans}/dashboard",
                    canCreateApplication)
            },
            {
                ProgrammeType.Ahp, new(
                    ProgrammeType.Ahp,
                    "Affordable Homes Programme 2021-2026 Continuous Market Engagement",
                    "Start a new Affordable Homes Programme application. This will not affect any of your previous applications.",
                    $"{_programmeUrlConfig.Ahp}/application/start",
                    $"{_programmeUrlConfig.Ahp}/application",
                    canCreateApplication)
            },
        };
    }
}
