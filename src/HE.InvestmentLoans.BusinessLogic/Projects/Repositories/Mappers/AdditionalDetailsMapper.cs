using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.Generic;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories.Mappers;
internal static class AdditionalDetailsMapper
{
    public static AdditionalDetails? MapFromCrm(SiteDetailsDto projectFromCrm, DateTime now)
    {
        return AdditionalDetailsExistsIn(projectFromCrm) ?
            new AdditionalDetails(
                projectFromCrm.dateOfPurchase.IsProvided() ? new PurchaseDate(new ProjectDate(projectFromCrm.dateOfPurchase!.Value), now) : null!,
                projectFromCrm.siteCost.IsProvided() ? new Pounds(decimal.Parse(projectFromCrm.siteCost, CultureInfo.InvariantCulture)) : null!,
                projectFromCrm.currentValue.IsProvided() ? new Pounds(decimal.Parse(projectFromCrm.currentValue, CultureInfo.InvariantCulture)) : null!,
                SourceOfValuationMapper.FromString(projectFromCrm.valuationSource)!.Value) :
                null;
    }

    private static bool AdditionalDetailsExistsIn(SiteDetailsDto projectFromCrm)
    {
        return projectFromCrm.dateOfPurchase.IsProvided() && projectFromCrm.siteCost.IsProvided() && projectFromCrm.currentValue.IsProvided() && projectFromCrm.valuationSource.IsProvided();
    }
}
