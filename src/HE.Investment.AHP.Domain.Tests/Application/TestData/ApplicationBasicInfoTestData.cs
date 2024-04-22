using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Factories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Programme;
using HE.Investment.AHP.Domain.Tests.Programme.TestData;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestData;

namespace HE.Investment.AHP.Domain.Tests.Application.TestData;

public static class ApplicationBasicInfoTestData
{
    public static readonly ApplicationBasicInfo AffordableRentInDraftState = CreateAffordableRentInDraftState();

    public static readonly ApplicationBasicInfo SharedOwnershipInDraftState = new(
        AhpApplicationId.From(GuidTestData.GuidOne),
        new SiteId("test-site-12312"),
        new ApplicationName(GuidTestData.GuidOne.ToString()),
        Tenure.SharedOwnership,
        ApplicationStatus.Draft,
        new ApplicationSections(new[]
        {
            new HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection(SectionType.Scheme),
            new HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection(SectionType.HomeTypes),
            new HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection(SectionType.FinancialDetails),
            new HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection(SectionType.DeliveryPhases),
        }),
        new AhpProgramme(ProgrammeDatesTestData.ProgrammeDates, MilestoneFramework.Default),
        new ApplicationStateFactory(UserAccountTestData.AdminUserAccountOne));

    public static ApplicationBasicInfo CreateAffordableRentInDraftState() => new(
        AhpApplicationId.From(GuidTestData.GuidTwo),
        new SiteId("test-site-12312"),
        new ApplicationName(GuidTestData.GuidTwo.ToString()),
        Tenure.AffordableRent,
        ApplicationStatus.Draft,
        new ApplicationSections(new[]
        {
            new HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection(SectionType.Scheme),
            new HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection(SectionType.HomeTypes),
            new HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection(SectionType.FinancialDetails),
            new HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection(SectionType.DeliveryPhases),
        }),
        new AhpProgramme(ProgrammeDatesTestData.ProgrammeDates, MilestoneFramework.Default),
        new ApplicationStateFactory(UserAccountTestData.AdminUserAccountOne));
}
