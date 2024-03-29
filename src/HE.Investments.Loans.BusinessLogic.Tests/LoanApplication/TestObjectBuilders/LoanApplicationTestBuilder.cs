using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Entities;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Common.Tests;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.TestsUtils;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;

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
            ReferenceNumberTestData.One,
            null));

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
            ReferenceNumberTestData.One,
            null));

    public static LoanApplicationTestBuilder NewWithOtherApplicationStatus(ApplicationStatus applicationStatus, UserAccount? userAccount = null) => new(
        new LoanApplicationEntity(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            LoanApplicationNameTestData.MyFirstApplication,
            userAccount ?? UserAccountTestData.UserAccountOne,
            applicationStatus,
            FundingPurpose.BuildingNewHomes,
            DateTimeTestData.SeptemberDay20Year2023At0736,
            DateTimeTestData.SeptemberDay20Year2023At0736.AddHours(1),
            DateTimeTestData.SeptemberDay20Year2023At0736.AddHours(2),
            "Anonymous",
            LoanApplicationSectionTestData.CompletedSection,
            LoanApplicationSectionTestData.CompletedSection,
            LoanApplicationSectionTestData.CompletedSection,
            LoanApplicationSectionTestData.CompletedProjectsSection,
            ReferenceNumberTestData.One,
            null));

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
