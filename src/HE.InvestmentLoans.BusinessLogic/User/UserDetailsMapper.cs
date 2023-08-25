using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.User;

namespace HE.InvestmentLoans.BusinessLogic.User;
public static class UserDetailsMapper
{
    public static UserDetailsViewModel MapToViewModel(UserDetails userDetailsEntity)
    {
        return new UserDetailsViewModel()
        {
            FirstName = userDetailsEntity.FirstName,
            Surname = userDetailsEntity.Surname,
            JobTitle = userDetailsEntity.JobTitle,
            TelephoneNumber = userDetailsEntity.TelephoneNumber,
            SecondaryTelephoneNumber = userDetailsEntity.SecondaryTelephoneNumber,
            IsTermsAndConditionsAccepted = userDetailsEntity.IsTermsAndConditionsAccepted == true ? CommonResponse.Checked : null,
        };
    }
}
