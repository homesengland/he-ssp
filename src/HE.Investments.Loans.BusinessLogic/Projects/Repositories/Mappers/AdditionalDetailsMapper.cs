using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Common;

namespace HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;
internal static class AdditionalDetailsMapper
{
    public static AdditionalDetails? MapFromCrm(SiteDetailsDto projectFromCrm)
    {
        if (!AdditionalDetailsExistsIn(projectFromCrm))
        {
            return null;
        }

        var dateOfPurchase = projectFromCrm.dateOfPurchase.IsProvided() ? new PurchaseDate(projectFromCrm.dateOfPurchase!.Value) : null!;
        var siteCost = projectFromCrm.siteCost.IsProvided() ? new Pounds(decimal.Parse(projectFromCrm.siteCost, CultureInfo.InvariantCulture)) : null!;
        var currentValue = projectFromCrm.currentValue.IsProvided() ? new Pounds(decimal.Parse(projectFromCrm.currentValue, CultureInfo.InvariantCulture)) : null!;
        var valuationSource = SourceOfValuationMapper.FromString(projectFromCrm.valuationSource)!.Value;

        return new AdditionalDetails(dateOfPurchase, siteCost, currentValue, valuationSource);
    }

    private static bool AdditionalDetailsExistsIn(SiteDetailsDto projectFromCrm)
    {
        return projectFromCrm.dateOfPurchase.IsProvided() && projectFromCrm.siteCost.IsProvided() && projectFromCrm.currentValue.IsProvided() && projectFromCrm.valuationSource.IsProvided();
    }
}
