using FluentAssertions;
using HE.Investments.Common.Contract;
using Xunit;

namespace HE.Investments.Common.Tests.Domain;

public class ShortGuidTests
{
    [Theory]
    [InlineData("3OIZO4IyEe6BeQAiSABKBg", "3b19e2dc-3282-ee11-8179-002248004a06")]
    [InlineData("WxhaYzDuQU6If-MkHjb6XA", "635a185b-ee30-4e41-887f-e3241e36fa5c")]
    [InlineData("G0f7eLiEN0qArOEfpL7rHQ", "78fb471b-84b8-4a37-80ac-e11fa4beeb1d")]
    [InlineData("KKmHHM3Dp0yFWSkYuzcKtA", "1c87a928-c3cd-4ca7-8559-2918bb370ab4")]
    [InlineData("evvCVvfTRE6qcLYFD_t81w", "56c2fb7a-d3f7-4e44-aa70-b6050ffb7cd7")]
    public void ShouldDecodeGuid(string shortGuid, string expectedGuid)
    {
        // when
        var guid = ShortGuid.ToGuid(shortGuid);

        // then
        guid.Should().Be(new Guid(expectedGuid));
    }

    [Theory]
    [InlineData("3b19e2dc-3282-ee11-8179-002248004a06", "3OIZO4IyEe6BeQAiSABKBg")]
    [InlineData("635a185b-ee30-4e41-887f-e3241e36fa5c", "WxhaYzDuQU6If-MkHjb6XA")]
    [InlineData("78fb471b-84b8-4a37-80ac-e11fa4beeb1d", "G0f7eLiEN0qArOEfpL7rHQ")]
    [InlineData("1c87a928-c3cd-4ca7-8559-2918bb370ab4", "KKmHHM3Dp0yFWSkYuzcKtA")]
    [InlineData("56c2fb7a-d3f7-4e44-aa70-b6050ffb7cd7", "evvCVvfTRE6qcLYFD_t81w")]
    public void ShouldEncodeGuid(string normalGuid, string expectedShortGuid)
    {
        // when
        var shortGuid = ShortGuid.FromGuid(new Guid(normalGuid));

        // then
        shortGuid.Value.Should().Be(expectedShortGuid);
    }

    [Theory]
    [InlineData("3b19e2dc-3282-ee11-8179-002248004a06", "3OIZO4IyEe6BeQAiSABKBg")]
    [InlineData("635a185b-ee30-4e41-887f-e3241e36fa5c", "WxhaYzDuQU6If-MkHjb6XA")]
    [InlineData("78fb471b-84b8-4a37-80ac-e11fa4beeb1d", "G0f7eLiEN0qArOEfpL7rHQ")]
    [InlineData("1c87a928-c3cd-4ca7-8559-2918bb370ab4", "KKmHHM3Dp0yFWSkYuzcKtA")]
    [InlineData("56c2fb7a-d3f7-4e44-aa70-b6050ffb7cd7", "evvCVvfTRE6qcLYFD_t81w")]
    public void ShouldEncodeString(string normalGuid, string expectedShortGuid)
    {
        // when
        var shortGuid = ShortGuid.FromString(normalGuid);

        // then
        shortGuid.Value.Should().Be(expectedShortGuid);
    }

    [Theory]
    [InlineData("3OIZO4IyEe6BeQAiSABKBg", "3b19e2dc-3282-ee11-8179-002248004a06")]
    [InlineData("WxhaYzDuQU6If-MkHjb6XA", "635a185b-ee30-4e41-887f-e3241e36fa5c")]
    [InlineData("G0f7eLiEN0qArOEfpL7rHQ", "78fb471b-84b8-4a37-80ac-e11fa4beeb1d")]
    [InlineData("KKmHHM3Dp0yFWSkYuzcKtA", "1c87a928-c3cd-4ca7-8559-2918bb370ab4")]
    [InlineData("evvCVvfTRE6qcLYFD_t81w", "56c2fb7a-d3f7-4e44-aa70-b6050ffb7cd7")]
    public void ShouldDecodeString(string shortGuid, string expectedGuid)
    {
        // when
        var guid = ShortGuid.ToGuidAsString(shortGuid);

        // then
        guid.Should().Be(expectedGuid);
    }
}
