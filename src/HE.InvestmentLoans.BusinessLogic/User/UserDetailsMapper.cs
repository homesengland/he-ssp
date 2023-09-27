extern alias Org;

using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.User;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.InvestmentLoans.BusinessLogic.User;
public static class UserDetailsMapper
{
    public static UserDetailsViewModel MapToViewModel(UserDetails userDetailsEntity)
    {
        return new UserDetailsViewModel()
        {
            FirstName = userDetailsEntity.FirstName,
            LastName = userDetailsEntity.LastName,
            JobTitle = userDetailsEntity.JobTitle,
            TelephoneNumber = userDetailsEntity.TelephoneNumber,
            SecondaryTelephoneNumber = userDetailsEntity.SecondaryTelephoneNumber,
        };
    }

    public static ContactDto MapUserDetailsToContactDto(UserDetails userDetails)
    {
        return new ContactDto
        {
            firstName = userDetails.FirstName,
            lastName = userDetails.LastName,
            jobTitle = userDetails.JobTitle,
            email = userDetails.Email,
            phoneNumber = userDetails.TelephoneNumber,
            secondaryPhoneNumber = userDetails.SecondaryTelephoneNumber,
            isTermsAndConditionsAccepted = userDetails.IsTermsAndConditionsAccepted,
        };
    }

    public static UserDetails MapContactDtoToUserDetails(ContactDto contactDto)
    {
        return new UserDetails(
            contactDto.firstName,
            contactDto.lastName,
            contactDto.jobTitle,
            contactDto.email,
            contactDto.phoneNumber,
            contactDto.secondaryPhoneNumber,
            contactDto.isTermsAndConditionsAccepted);
    }
}
