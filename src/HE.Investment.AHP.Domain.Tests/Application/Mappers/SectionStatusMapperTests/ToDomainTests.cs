using FluentAssertions;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Tests.Application.Mappers.SectionStatusMapperTests;

public class ToDomainTests
{
    [Theory]
    [InlineData((int)invln_ahpsectioncompletionstatusset.Notstarted, SectionStatus.NotStarted)]
    [InlineData((int)invln_ahpsectioncompletionstatusset.InProgress, SectionStatus.InProgress)]
    [InlineData((int)invln_ahpsectioncompletionstatusset.Completed, SectionStatus.Completed)]
    public void ShouldReturnExpectedSectionStatus_WhenValueIsProvided(int value, SectionStatus expectedStatus)
    {
        var result = SectionStatusMapper.ToDomain(value);

        result.Should().Be(expectedStatus);
    }

    [Fact]
    public void ShouldReturnStatusNotStarted_WhenValueIsNull()
    {
        var result = SectionStatusMapper.ToDomain(null);

        result.Should().Be(SectionStatus.NotStarted);
    }

    [Theory]
    [InlineData(ApplicationStatus.Withdrawn, SectionStatus.Withdrawn)]
    [InlineData(ApplicationStatus.OnHold, SectionStatus.OnHold)]
    public void ShouldReturnSectionStatusWithdrawn_WhenApplicationStatusIsWithdrawn(ApplicationStatus applicationStatus, SectionStatus expectedSectionStatus)
    {
        var result = SectionStatusMapper.ToDomain(null, applicationStatus);

        result.Should().Be(expectedSectionStatus);
    }
}
