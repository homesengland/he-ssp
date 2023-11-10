using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.Common.Tests;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.Investments.Account.Shared.User;
using HE.Investments.TestsUtils;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;

public class LoanApplicationTestBuilder : TestEntityBuilderBase<LoanApplicationEntity>
{
    public LoanApplicationTestBuilder(LoanApplicationEntity item)
    {
        Item = item;
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
            DateTimeTestData.SeptemberDay20Year2023At0736.AddHours(2),
            "Anonymous",
            LoanApplicationSection.New(),
            LoanApplicationSection.New(),
            LoanApplicationSection.New(),
            ProjectsSection.Empty(),
            ReferenceNumberTestData.One));

    public static LoanApplicationTestBuilder NewSubmitted(UserAccount? userAccount = null) => new(
        new LoanApplicationEntity(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            LoanApplicationNameTestData.MyFirstApplication,
            userAccount ?? UserAccountTestData.UserAccountOne,
            ApplicationStatus.ApplicationSubmitted,
            FundingPurpose.BuildingNewHomes,
            DateTimeTestData.SeptemberDay20Year2023At0736,
            DateTimeTestData.SeptemberDay20Year2023At0736.AddHours(1),
            DateTimeTestData.SeptemberDay20Year2023At0736.AddHours(2),
            "Anonymous",
            LoanApplicationSectionTestData.CompletedSection,
            LoanApplicationSectionTestData.CompletedSection,
            LoanApplicationSectionTestData.CompletedSection,
            LoanApplicationSectionTestData.CompletedProjectsSection,
            ReferenceNumberTestData.One));

    public LoanApplicationTestBuilder WithCreatedOn(DateTime createdOn)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item, nameof(Item.CreatedOn), createdOn);
        return this;
    }

    public LoanApplicationTestBuilder WithAllCompletedSections()
    {
        return WithCompanyStructureSection(LoanApplicationSectionTestData.CompletedSection)
            .WithFundingSection(LoanApplicationSectionTestData.CompletedSection)
            .WithSecuritySection(LoanApplicationSectionTestData.CompletedSection)
            .WithProjectSection(LoanApplicationSectionTestData.CompletedProjectsSection);
    }

    public LoanApplicationTestBuilder WithCompanyStructureSection(LoanApplicationSection companyStructureSection)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item, nameof(Item.CompanyStructure), companyStructureSection);

        return this;
    }

    public LoanApplicationTestBuilder WithSecuritySection(LoanApplicationSection companyStructureSection)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item, nameof(Item.Security), companyStructureSection);

        return this;
    }

    public LoanApplicationTestBuilder WithFundingSection(LoanApplicationSection companyStructureSection)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item, nameof(Item.Funding), companyStructureSection);

        return this;
    }

    public LoanApplicationTestBuilder WithProjectSection(ProjectsSection projectsSection)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item, nameof(Item.ProjectsSection), projectsSection);

        return this;
    }
}
