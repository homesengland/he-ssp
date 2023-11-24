using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Common;

namespace HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;
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
