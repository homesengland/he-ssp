using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using ApplicationSection = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationEntityTests;

public class ApplicationEntitySubmitTests
{
    [Fact]
    public void ShouldSubmitApplication()
    {
        // given
        var entity = ApplicationEntityBuilder
            .New()
            .WithSections(new ApplicationSection(SectionType.Scheme, SectionStatus.Completed), new ApplicationSection(SectionType.DeliveryPhases, SectionStatus.Completed))
            .Build();

        // when
        var result = () => entity.Submit();

        // then
        result.Should().NotThrow<DomainValidationException>();
        entity.IsModified.Should().BeTrue();
        entity.Status.Should().Be(ApplicationStatus.ApplicationSubmitted);
    }

    [Fact]
    public void ShouldThrowException_WhenSectionsNotCompleted()
    {
        // given
        var entity = ApplicationEntityBuilder
            .New()
            .WithSections(new ApplicationSection(SectionType.Scheme, SectionStatus.InProgress), new ApplicationSection(SectionType.DeliveryPhases, SectionStatus.Completed))
            .Build();

        // when
        var result = () => entity.Submit();

        // then
        result.Should().Throw<DomainValidationException>();
        entity.IsModified.Should().BeFalse();
        entity.Status.Should().Be(ApplicationStatus.New);
    }
}
