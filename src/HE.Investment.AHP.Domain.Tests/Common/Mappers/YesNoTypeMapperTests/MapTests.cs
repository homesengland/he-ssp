using FluentAssertions;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Domain.Tests.Common.Mappers.YesNoTypeMapperTests;

public class MapTests
{
    [Fact]
    public void ShouldReturnNull_WhenValueIsNotProvided()
    {
        // given & when
        var result = YesNoTypeMapper.Map((YesNoType?)null);

        // then
        result.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnTrue_WhenValueIsYes()
    {
        // given & when
        var result = YesNoTypeMapper.Map(YesNoType.Yes);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenValueIsNo()
    {
        // given & when
        var result = YesNoTypeMapper.Map(YesNoType.No);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnNull_WhenValueIsUndefined()
    {
        // given & when
        var result = YesNoTypeMapper.Map(YesNoType.Undefined);

        // then
        result.Should().BeNull();
    }
}
