using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.User.Queries;

namespace HE.InvestmentLoans.BusinessLogic.User.QueryHandlers;
public static class UserViewModelMapper
{
    public static GetUserDetailsResponse Map(UserDetails userDetailsEntity)
    {
        return new GetUserDetailsResponse(
            userDetailsEntity.FirstName,
            userDetailsEntity.Surname,
            userDetailsEntity.JobTitle,
            userDetailsEntity.TelephoneNumber,
            userDetailsEntity.SecondaryTelephoneNumber);
    }
}
