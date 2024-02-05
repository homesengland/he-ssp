using FluentAssertions;
using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.Organisation.ValueObjects;

public class OrganisationPhoneNumberTests
{
    private static readonly string PhoneNumber = "868 976 222";

    public static OrganisationPhoneNumber CreatePhoneNumber(string? phoneNumber = null)
    {
        return new OrganisationPhoneNumber(
            phoneNumber ?? PhoneNumber);
    }

    [Fact]
    public void ShouldCreate()
    {
        // given
        var organisationPhoneNumber = "123 456 786";

        // & when
        var phoneNumber = new OrganisationPhoneNumber(organisationPhoneNumber);

        // then
        phoneNumber.PhoneNumber.Should().Be(organisationPhoneNumber);
    }

    [Fact]
    public void ShouldThrowException_WhenProvidedNameIsToLong()
    {
        // given
        var organisationPhoneNumber = TestData.StringLenght101;

        // when
        var result = () => CreatePhoneNumber(organisationPhoneNumber);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == "The Phone number must be 100 characters or less");
    }
}
