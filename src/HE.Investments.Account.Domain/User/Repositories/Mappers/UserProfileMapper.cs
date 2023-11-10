using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Extensions;

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
            contactDto.firstName.IsProvided() ? new FirstName(contactDto.firstName) : null,
            contactDto.lastName.IsProvided() ? new LastName(contactDto.lastName) : null,
            contactDto.jobTitle.IsProvided() ? new JobTitle(contactDto.jobTitle) : null,
            contactDto.email,
            contactDto.phoneNumber.IsProvided() ? new TelephoneNumber(contactDto.phoneNumber) : null,
            contactDto.secondaryPhoneNumber.IsProvided() ? new SecondaryTelephoneNumber(contactDto.secondaryPhoneNumber) : null,
            contactDto.isTermsAndConditionsAccepted);
    }
}
