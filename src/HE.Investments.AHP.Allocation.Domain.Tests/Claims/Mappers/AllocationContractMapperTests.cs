using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.AHP.Allocation.Domain.Claims.Mappers;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Mappers;

public class AllocationContractMapperTests : TestBase<AllocationContractMapper>
{
    [Fact]
    public void ShouldReturnCorrectAllocationDetails_WhenDataIsValid()
    {
        // given
        var allocation = AllocationEntityTestBuilder.New()
            .WithListOfPhaseClaims([PhaseEntityTestBuilder.New().Build()])
            .Build();

        var phaseMapper = CreateAndRegisterDependencyMock<IPhaseContractMapper>();
        phaseMapper.Setup(x => x.Map(It.IsAny<PhaseEntity>()))
            .Returns<PhaseEntity>(x => new Phase(x.Id, x.Name.Value, null!, "1", BuildActivityType.Conversion, []));

        // when
        var result = TestCandidate.Map(allocation, new PaginationRequest(1));

        // then
        result.AllocationBasicInfo.Id.Should().Be(allocation.BasicInfo.Id);
        result.AllocationBasicInfo.Name.Should().Be(allocation.BasicInfo.Name.Value);
        result.AllocationBasicInfo.ReferenceNumber.Should().Be(allocation.BasicInfo.ReferenceNumber.Value);
        result.AllocationBasicInfo.LocalAuthority.Should().Be(allocation.BasicInfo.LocalAuthority.Name);
        result.AllocationBasicInfo.ProgrammeName.Should().Be(allocation.BasicInfo.Programme.ShortName);
        result.AllocationBasicInfo.Tenure.Should().Be(allocation.BasicInfo.Tenure);
        result.PhaseList.Items.Count.Should().Be(1);
    }
}
