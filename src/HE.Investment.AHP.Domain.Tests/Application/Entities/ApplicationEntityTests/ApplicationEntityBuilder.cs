using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Factories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestData;
using Moq;
using ApplicationSection = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationEntityTests;

public class ApplicationEntityBuilder
{
    private readonly SiteId _siteId = new("site-1");

    private readonly AhpApplicationId _id = new("1");

    private readonly ApplicationName _name = new("zapytanko");

    private readonly ApplicationReferenceNumber _reference = new("REF123");

    private ApplicationStatus _status = ApplicationStatus.New;

    private ApplicationStatus? _previousStatus;

    private bool _wasSubmitted;

    private IList<ApplicationSection>? _sections;

    private IUserAccount _userAccount = UserAccountTestData.AdminUserAccountOne;

    public static ApplicationEntityBuilder New() => new();

    public ApplicationEntityBuilder WithSections(params ApplicationSection[] sections)
    {
        _sections = sections.ToList();
        return this;
    }

    public ApplicationEntityBuilder WithAllSectionsCompleted()
    {
        return WithSections(
            new ApplicationSection(SectionType.Scheme, SectionStatus.Completed),
            new ApplicationSection(SectionType.HomeTypes, SectionStatus.Completed),
            new ApplicationSection(SectionType.FinancialDetails, SectionStatus.Completed),
            new ApplicationSection(SectionType.DeliveryPhases, SectionStatus.Completed));
    }

    public ApplicationEntityBuilder WithApplicationStatus(ApplicationStatus status)
    {
        _status = status;
        return this;
    }

    public ApplicationEntityBuilder WithUserPermissions(bool canEditApplication, bool canSubmitApplication)
    {
        _userAccount = Mock.Of<IUserAccount>();
        Mock.Get(_userAccount).Setup(x => x.CanEditApplication).Returns(canEditApplication);
        Mock.Get(_userAccount).Setup(x => x.CanSubmitApplication).Returns(canSubmitApplication);

        return this;
    }

    public ApplicationEntityBuilder WithPreviousStatus(ApplicationStatus previousStatus)
    {
        _previousStatus = previousStatus;
        return this;
    }

    public ApplicationEntityBuilder WithWasSubmitted(bool wasSubmitted)
    {
        _wasSubmitted = wasSubmitted;
        return this;
    }

    public ApplicationEntity Build()
    {
        return new ApplicationEntity(
            _siteId,
            _id,
            _name,
            _status,
            new ApplicationTenure(Tenure.AffordableRent),
            new ApplicationStateFactory(_userAccount, _previousStatus, _wasSubmitted),
            _reference,
            new ApplicationSections(_sections ?? new List<ApplicationSection>()));
    }
}
