extern alias Org;

using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.User;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.InvestmentLoans.BusinessLogic.User;
public static class UserDetailsMapper
{
    public static UserDetailsViewModel MapToViewModel(UserDetails userDetailsEntity)
    {
        return new UserDetailsViewModel()
        {
            FirstName = userDetailsEntity.FirstName?.ToString(),
            LastName = userDetailsEntity.LastName?.ToString(),
            JobTitle = userDetailsEntity.JobTitle?.ToString(),
            TelephoneNumber = userDetailsEntity.TelephoneNumber?.ToString(),
            SecondaryTelephoneNumber = userDetailsEntity.SecondaryTelephoneNumber?.ToString(),
        };
    }

    public static ContactDto MapUserDetailsToContactDto(UserDetails userDetails)
    {
        return new ContactDto
        {
            firstName = userDetails.FirstName?.ToString(),
            lastName = userDetails.LastName?.ToString(),
            jobTitle = userDetails.JobTitle?.ToString(),
            email = userDetails.Email,
            phoneNumber = userDetails.TelephoneNumber?.ToString(),
            secondaryPhoneNumber = userDetails.SecondaryTelephoneNumber?.ToString(),
            isTermsAndConditionsAccepted = userDetails.IsTermsAndConditionsAccepted,
        };
    }

    public static UserDetails MapContactDtoToUserDetails(ContactDto contactDto)
    {
        return new UserDetails(
            FirstName.FromString(contactDto.firstName),
            LastName.FromString(contactDto.lastName),
            JobTitle.FromString(contactDto.jobTitle),
            contactDto.email,
            TelephoneNumber.FromString(contactDto.phoneNumber),
            SecondaryTelephoneNumber.FromString(contactDto.secondaryPhoneNumber),
            contactDto.isTermsAndConditionsAccepted);
    }
}
