using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Organization.ValueObjects;

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
        result.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(GenericValidationError.TextTooLong);
    }
}
