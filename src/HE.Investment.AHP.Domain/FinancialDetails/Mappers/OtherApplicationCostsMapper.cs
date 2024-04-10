using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Mappers;

public static class OtherApplicationCostsMapper
{
    public static OtherApplicationCosts MapToOtherApplicationCosts(AhpApplicationDto application)
    {
        return new OtherApplicationCosts(
            application.expectedOnWorks.IsProvided() ? ExpectedWorksCosts.FromCrm(application.expectedOnWorks!.Value) : null,
            application.expectedOnCosts.IsProvided() ? ExpectedOnCosts.FromCrm(application.expectedOnCosts!.Value) : null);
    }
}
