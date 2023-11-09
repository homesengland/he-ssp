using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Contract.Common;
using HE.Investments.Common.Extensions;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories.Mappers;
internal class PublicSectorGrantFundingMapper
{
    public static PublicSectorGrantFunding? MapFromCrm(SiteDetailsDto projectFromCrm)
    {
        return GrantFundingExistsIn(projectFromCrm) ?
            new PublicSectorGrantFunding(
                projectFromCrm.whoProvided.IsProvided() ? new ShortText(projectFromCrm.whoProvided) : null,
                projectFromCrm.howMuch.IsProvided() ? new Pounds(decimal.Parse(projectFromCrm.howMuch, CultureInfo.InvariantCulture)) : null,
                projectFromCrm.nameOfGrantFund.IsProvided() ? new ShortText(projectFromCrm.nameOfGrantFund) : null,
                projectFromCrm.reason.IsProvided() ? new LongText(projectFromCrm.reason) : null) :
                null;
    }

    private static bool GrantFundingExistsIn(SiteDetailsDto projectFromCrm)
    {
        return projectFromCrm.whoProvided.IsProvided() || projectFromCrm.howMuch.IsProvided() || projectFromCrm.nameOfGrantFund.IsProvided() || projectFromCrm.reason.IsProvided();
    }
}
