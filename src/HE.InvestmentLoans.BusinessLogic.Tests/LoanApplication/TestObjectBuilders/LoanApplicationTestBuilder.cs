using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Tests;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.Investments.TestsUtils;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;

public class LoanApplicationTestBuilder : TestEntityBuilderBase<LoanApplicationEntity>
{
    public LoanApplicationTestBuilder(LoanApplicationEntity item)
    {
        Item = item;
        Item.LegacyModel = new LoanApplicationViewModel { ReferenceNumber = ReferenceNumberTestData.One, };
    }

    public static LoanApplicationTestBuilder NewDraft(UserAccount? userAccount = null) => new(
        new LoanApplicationEntity(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            LoanApplicationNameTestData.MyFirstApplication,
            userAccount ?? UserAccountTestData.UserAccountOne,
            ApplicationStatus.Draft,
            FundingPurpose.BuildingNewHomes,
            DateTimeTestData.SeptemberDay20Year2023At0736,
            DateTimeTestData.SeptemberDay20Year2023At0736.AddHours(1),
            new LoanApplicationSection(SectionStatus.NotStarted)));

    public static LoanApplicationTestBuilder NewSubmitted(UserAccount userAccount) => new(
        new LoanApplicationEntity(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            LoanApplicationNameTestData.MyFirstApplication,
            userAccount,
            ApplicationStatus.ApplicationSubmitted,
            FundingPurpose.BuildingNewHomes,
            DateTimeTestData.SeptemberDay20Year2023At0736,
            DateTimeTestData.SeptemberDay20Year2023At0736.AddHours(1),
            new LoanApplicationSection(SectionStatus.NotStarted)));

    public LoanApplicationTestBuilder WithCreatedOn(DateTime createdOn)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item, nameof(Item.CreatedOn), createdOn);
        return this;
    }

    public LoanApplicationTestBuilder WithCompanyStructureSection(LoanApplicationSection companyStructureSection)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item, nameof(Item.CompanyStructure), companyStructureSection);
        return this;
    }
}
