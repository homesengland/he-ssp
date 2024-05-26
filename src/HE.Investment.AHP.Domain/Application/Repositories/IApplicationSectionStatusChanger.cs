using HE.Investment.AHP.Contract.Application;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public interface IApplicationSectionStatusChanger
{
    Task ChangeSectionStatus(
        AhpApplicationId applicationId,
        UserAccount userAccount,
        SectionType sectionType,
        SectionStatus targetStatus,
        CancellationToken cancellationToken);
}
