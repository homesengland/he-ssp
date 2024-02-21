using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Mappers;

public static class OtherApplicationCostsMappers
{
    public static OtherApplicationCosts MapToOtherApplicationCosts(AhpApplicationDto application)
    {
        return new OtherApplicationCosts(
            application.expectedOnWorks.IsProvided() ? new ExpectedWorksCosts(application.expectedOnWorks!.Value) : null,
            application.expectedOnCosts.IsProvided() ? new ExpectedOnCosts(application.expectedOnCosts!.Value) : null);
    }
}
