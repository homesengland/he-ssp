using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Tests.FluentAssertions;
using ApplicationSection = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationEntityTests;

public class SubmitTests
{
    [Fact]
    public void ShouldThrowException_WhenNotAllSectionsAreCompleted()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.Draft)
            .WithSections(
                new ApplicationSection(SectionType.Scheme, SectionStatus.Completed),
                new ApplicationSection(SectionType.HomeTypes, SectionStatus.Completed),
                new ApplicationSection(SectionType.FinancialDetails, SectionStatus.Completed),
                new ApplicationSection(SectionType.DeliveryPhases, SectionStatus.InProgress))
            .Build();
        var representationAndWarranties = RepresentationsAndWarranties.Confirmed();

        // when
        var submit = () => testCandidate.Submit(representationAndWarranties);

        // then
        submit.Should().Throw<DomainValidationException>().WithSingleError("Cannot submit application with at least one not completed section.");
    }

    [Fact]
    public void ShouldThrowException_WhenSectionsAreCompletedButUserIsNotPermittedToSubmit()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.Draft)
            .WithUserPermissions(canEditApplication: true, canSubmitApplication: false)
            .WithAllSectionsCompleted()
            .Build();
        var representationAndWarranties = RepresentationsAndWarranties.Confirmed();

        // when
        var submit = () => testCandidate.Submit(representationAndWarranties);

        // then
        submit.Should().Throw<DomainException>();
    }

    [Fact]
    public void ShouldThrowException_WhenSectionsAreCompletedButStatusIsNotValidForSubmit()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.Withdrawn)
            .WithUserPermissions(canEditApplication: true, canSubmitApplication: true)
            .WithAllSectionsCompleted()
            .Build();
        var representationAndWarranties = RepresentationsAndWarranties.Confirmed();

        // when
        var submit = () => testCandidate.Submit(representationAndWarranties);

        // then
        submit.Should().Throw<DomainException>();
    }

    [Fact]
    public void ShouldChangeStatusToSubmitted_WhenSectionsAreCompletedAndTransitionIsAllowed()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.Draft)
            .WithUserPermissions(canEditApplication: false, canSubmitApplication: true)
            .WithAllSectionsCompleted()
            .Build();
        var representationAndWarranties = RepresentationsAndWarranties.Confirmed();

        // when
        testCandidate.Submit(representationAndWarranties);

        // then
        testCandidate.Status.Should().Be(ApplicationStatus.ApplicationSubmitted);
        testCandidate.ChangeStatusReason.Should().BeNull();
        testCandidate.RepresentationsAndWarranties.Should().Be(representationAndWarranties);
        testCandidate.IsStatusModified.Should().BeTrue();
    }
}
