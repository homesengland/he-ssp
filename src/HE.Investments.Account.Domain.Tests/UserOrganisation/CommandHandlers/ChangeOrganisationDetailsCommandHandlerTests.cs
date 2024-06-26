using FluentAssertions;
using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Account.Domain.Tests.Organisation.TestObjectBuilder;
using HE.Investments.Account.Domain.UserOrganisation.CommandHandlers;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.UserOrganisation.CommandHandlers;

public class ChangeOrganisationDetailsCommandHandlerTests : TestBase<ChangeOrganisationDetailsCommandHandler>
{
    [Fact]
    public async Task ReturnSuccessOperationResult_WhenAllOrganisationDetailsAreProvidedCorrectly()
    {
        // given
        AccountUserContextTestBuilder
            .New()
            .IsLinkedWithOrganization()
            .Register(this);

        var organizationDetailsDto = OrganizationServiceMockTestBuilder
            .New()
            .Register(this)
            .OrganizationDetailsDtoMock;

        var organisationRepository = OrganizationRepositoryTestBuilder
            .New()
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(
            new ChangeOrganisationDetailsCommand(
                organizationDetailsDto.registeredCompanyName,
                organizationDetailsDto.organisationPhoneNumber,
                organizationDetailsDto.addressLine1,
                organizationDetailsDto.addressLine2,
                organizationDetailsDto.city,
                organizationDetailsDto.county,
                organizationDetailsDto.postalcode),
            CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        organisationRepository.Verify(c => c.Save(It.IsAny<OrganisationId>(), It.IsAny<OrganisationEntity>(), CancellationToken.None));
    }
}
