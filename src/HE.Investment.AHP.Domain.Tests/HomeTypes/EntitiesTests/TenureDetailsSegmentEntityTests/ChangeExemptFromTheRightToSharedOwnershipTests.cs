using FluentAssertions;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.EntitiesTests.TenureDetailsSegmentEntityTests;

public class ChangeExemptFromTheRightToSharedOwnershipTests
{
    [Theory]
    [InlineData(YesNoType.No)]
    [InlineData(YesNoType.Undefined)]
    public void ShouldClearExemptJustification_WhenAnswerIs(YesNoType answer)
    {
        // given
        var testCandidate = new TenureDetailsTestDataBuilder()
            .WithExemptFromTheRightToSharedOwnership(YesNoType.Yes)
            .WithExemptionJustification("some justification")
            .Build();

        // when
        testCandidate.ChangeExemptFromTheRightToSharedOwnership(answer);

        // then
        testCandidate.IsModified.Should().BeTrue();
        testCandidate.ExemptionJustification.Should().BeNull();
    }

    [Fact]
    public void ShouldNotClearExemptJustification_WhenAnswerIsAlreadyYes()
    {
        // given
        var testCandidate = new TenureDetailsTestDataBuilder()
            .WithExemptFromTheRightToSharedOwnership(YesNoType.Yes)
            .WithExemptionJustification("some justification")
            .Build();

        // when
        testCandidate.ChangeExemptFromTheRightToSharedOwnership(YesNoType.Yes);

        // then
        testCandidate.IsModified.Should().BeFalse();
        testCandidate.ExemptionJustification.Should().Be(new MoreInformation("some justification"));
    }
}
