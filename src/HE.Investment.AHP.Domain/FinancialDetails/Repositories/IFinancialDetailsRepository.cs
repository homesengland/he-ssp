using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Repositories;

public interface IFinancialDetailsRepository
{
    Task<FinancialDetailsEntity> GetById(ApplicationId id, UserAccount userAccount, CancellationToken cancellationToken);

    Task<FinancialDetailsEntity> Save(FinancialDetailsEntity financialDetails, OrganisationId organisationId, CancellationToken cancellationToken);
}
