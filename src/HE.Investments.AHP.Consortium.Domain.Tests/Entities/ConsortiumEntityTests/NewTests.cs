using FluentAssertions;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Exceptions;
using Moq;
using Xunit;

namespace HE.Investments.AHP.Consortium.Domain.Tests.Entities.ConsortiumEntityTests;

public class NewTests
{
    [Fact]
    public async Task ShouldCreateConsortiumEntity()
    {
        // given
        var programme = ProgrammeSlimTestData.AhpCmeProgramme;
        var consortiumMember = ConsortiumMemberTestData.CarqMember;
        var isPartOfConsortium = IsPartOfConsortiumBuilder.New().IsNotPartOfConsortium().Build();

        // when
        var result = await ConsortiumEntity.New(programme, consortiumMember, isPartOfConsortium);

        // then
        result.Name.Value.Should().NotBeEmpty();
        result.Programme.Should().Be(programme);
        result.LeadPartner.Should().Be(consortiumMember);
    }

    [Fact]
    public async Task ShouldThrowValidationErrorWhenConsortiumAlreadyExists()
    {
        // given
        var programme = ProgrammeSlimTestData.AhpCmeProgramme;
        var consortiumMember = ConsortiumMemberTestData.CarqMember;
        var isPartOfConsortium = IsPartOfConsortiumBuilder.New().IsPartOfConsortium().Build();

        // when
        Func<Task> act = async () => await ConsortiumEntity.New(programme, consortiumMember, isPartOfConsortium);

        // then
        (await act.Should().ThrowExactlyAsync<DomainValidationException>()).WithMessage("A consortium has already been added to this programme");
    }
}
