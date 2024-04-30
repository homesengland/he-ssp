using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using OrganisationId = HE.Investments.Account.Shared.User.ValueObjects.OrganisationId;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public interface IApplicationSectionStatusChanger
{
    Task ChangeSectionStatus(
        AhpApplicationId applicationId,
        OrganisationId organisationId,
        SectionType sectionType,
        SectionStatus targetStatus,
        CancellationToken cancellationToken);
}
