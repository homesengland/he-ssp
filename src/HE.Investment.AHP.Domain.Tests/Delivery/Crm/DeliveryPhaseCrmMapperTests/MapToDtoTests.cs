using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.Crm;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Crm.DeliveryPhaseCrmMapperTests;

// TODO: implement more tests here
public class MapToDtoTests : TestBase<DeliveryPhaseCrmMapper>
{
    [Fact]
    public void ShouldMapDeliveryPhaseBasicProperties()
    {
        // given
        var entity = new DeliveryPhaseEntityBuilder().Build();

        // when
        var result = TestCandidate.MapToDto(entity);

        // then
        result.Should().NotBeNull();
    }
}
