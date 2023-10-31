using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.Common.Extensions;
using HE.Investments.Account.Domain.User.Entities;
using HE.Investments.Account.Domain.User.ValueObjects;

namespace HE.Investments.Account.Domain.User.Repositories.Mappers;

public static class UserProfileMapper
{
    public static ContactDto MapUserDetailsToContactDto(UserProfileDetails userProfileDetails)
    {
        return new ContactDto
        {
            firstName = userProfileDetails.FirstName?.ToString(),
            lastName = userProfileDetails.LastName?.ToString(),
            jobTitle = userProfileDetails.JobTitle?.ToString(),
            email = userProfileDetails.Email,
            phoneNumber = userProfileDetails.TelephoneNumber?.ToString(),
            secondaryPhoneNumber = userProfileDetails.SecondaryTelephoneNumber?.ToString(),
            isTermsAndConditionsAccepted = userProfileDetails.IsTermsAndConditionsAccepted,
        };
    }

    public static UserProfileDetails MapContactDtoToUserDetails(ContactDto contactDto)
    {
        return new UserProfileDetails(
            new FirstName(contactDto.firstName),
            new LastName(contactDto.lastName),
            new JobTitle(contactDto.jobTitle),
            contactDto.email,
            new TelephoneNumber(contactDto.phoneNumber),
            contactDto.secondaryPhoneNumber.IsProvided() ? new SecondaryTelephoneNumber(contactDto.secondaryPhoneNumber) : null,
            contactDto.isTermsAndConditionsAccepted);
    }
}
