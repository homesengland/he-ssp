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

    [Fact]
    public void ShouldReturnSectionStatusWithdrawn_WhenApplicationStatusIsWithdrawn()
    {
        var result = SectionStatusMapper.ToDomain(null, ApplicationStatus.Withdrawn);

        result.Should().Be(SectionStatus.Withdrawn);
    }
}
