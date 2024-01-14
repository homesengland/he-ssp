using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Common;

namespace HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;
internal static class PublicSectorGrantFundingMapper
{
    public static PublicSectorGrantFunding? MapFromCrm(SiteDetailsDto projectFromCrm)
    {
        if (!GrantFundingExistsIn(projectFromCrm))
        {
            return null;
        }

        var whoProvided = projectFromCrm.whoProvided.IsProvided() ? new ShortText(projectFromCrm.whoProvided) : null;
        var howMuch = projectFromCrm.howMuch.IsProvided() ? new Pounds(decimal.Parse(projectFromCrm.howMuch, CultureInfo.InvariantCulture)) : null;
        var nameOfGrantFund = projectFromCrm.nameOfGrantFund.IsProvided() ? new ShortText(projectFromCrm.nameOfGrantFund) : null;
        var reason = projectFromCrm.reason.IsProvided() ? new LongText(projectFromCrm.reason) : null;

        return new PublicSectorGrantFunding(whoProvided, howMuch, nameOfGrantFund, reason);
    }

    private static bool GrantFundingExistsIn(SiteDetailsDto projectFromCrm)
    {
        return projectFromCrm.whoProvided.IsProvided() || projectFromCrm.howMuch.IsProvided() || projectFromCrm.nameOfGrantFund.IsProvided() || projectFromCrm.reason.IsProvided();
    }
}
