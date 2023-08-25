using HE.InvestmentLoans.Contract.User.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.User.Entities;

public record UserAccount(UserGlobalId UserGlobalId, string UserEmail, Guid AccountId, string AccountName);
