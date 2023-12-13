using System.Runtime.CompilerServices;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.TestsUtils;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.TestObjectBuilders;

public class ExpectedContributionsToSchemeBuilder
{
    private readonly ExpectedContributionsToScheme _item;

    private ExpectedContributionsToSchemeBuilder(Tenure tenure)
    {
        _item = new(tenure);
    }

    private ExpectedContributionsToSchemeBuilder(ExpectedContributionsToScheme item)
    {
        _item = item;
    }

    public static ExpectedContributionsToSchemeBuilder New(Tenure tenure = Tenure.AffordableRent) => new(tenure);

    public static ExpectedContributionsToSchemeBuilder NewWithAllValuesAs50()
    {
        var item = new ExpectedContributionsToScheme(
            new ExpectedContributionValue(ExpectedContributionFields.RentalIncomeBorrowing, 50),
            new ExpectedContributionValue(ExpectedContributionFields.SaleOfHomesOnThisScheme, 50),
            new ExpectedContributionValue(ExpectedContributionFields.SaleOfHomesOnOtherSchemes, 50),
            new ExpectedContributionValue(ExpectedContributionFields.OwnResources, 50),
            new ExpectedContributionValue(ExpectedContributionFields.RcgfContribution, 50),
            new ExpectedContributionValue(ExpectedContributionFields.OtherCapitalSources, 50),
            new ExpectedContributionValue(ExpectedContributionFields.InitialSalesOfSharedHomes, 50),
            new ExpectedContributionValue(ExpectedContributionFields.HomesTransferValue, 50),
            Tenure.SharedOwnership);

        return new ExpectedContributionsToSchemeBuilder(item);
    }

    public ExpectedContributionsToSchemeBuilder WithOwnResources(decimal ownResources)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.OwnResources),
            new ExpectedContributionValue(ExpectedContributionFields.OwnResources, ownResources));

        return this;
    }

    public ExpectedContributionsToSchemeBuilder WithSharedOwnershipSales(decimal sharedOwnershipSales)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.SharedOwnershipSales),
            new ExpectedContributionValue(ExpectedContributionFields.InitialSalesOfSharedHomes, sharedOwnershipSales));

        return this;
    }

    public ExpectedContributionsToScheme Build() => _item;
}
