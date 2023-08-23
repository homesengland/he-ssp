using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.User;

namespace HE.InvestmentLoans.BusinessLogic.User.QueryHandlers;
public static class UserViewModelMapper
{
    public static UserDetailsViewModel? Map(UserDetails? userDetailsEntity)
    {
        return userDetailsEntity != null ? new UserDetailsViewModel
        {
            FirstName = userDetailsEntity.FirstName,
            Surname = userDetailsEntity.Surname,
            JobTitle = userDetailsEntity.JobTitle,
            TelephoneNumber = userDetailsEntity.TelephoneNumber,
            SecondaryTelephoneNumber = userDetailsEntity.SecondaryTelephoneNumber,
        }
        : null;
    }
}
