using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Crm;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public class ApplicationSectionStatusChanger : IApplicationSectionStatusChanger
{
    private readonly IApplicationCrmContext _crmContext;

    public ApplicationSectionStatusChanger(IApplicationCrmContext crmContext)
    {
        _crmContext = crmContext;
    }

    public async Task ChangeSectionStatus(
        AhpApplicationId applicationId,
        UserAccount userAccount,
        SectionType sectionType,
        SectionStatus targetStatus,
        CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().ToGuidAsString();
        var dto = await _crmContext.GetOrganisationApplicationById(applicationId.Value, organisationId, cancellationToken);
        var dtoStatus = SectionStatusMapper.ToDto(targetStatus);
        var modificationTracker = new ModificationTracker();

        switch (sectionType)
        {
            case SectionType.Scheme:
                dto.schemeInformationSectionCompletionStatus = modificationTracker.Change(dto.schemeInformationSectionCompletionStatus, dtoStatus);
                break;
            case SectionType.HomeTypes:
                dto.homeTypesSectionCompletionStatus = modificationTracker.Change(dto.homeTypesSectionCompletionStatus, dtoStatus);
                break;
            case SectionType.FinancialDetails:
                dto.financialDetailsSectionCompletionStatus = modificationTracker.Change(dto.financialDetailsSectionCompletionStatus, dtoStatus);
                break;
            case SectionType.DeliveryPhases:
                dto.deliveryPhasesSectionCompletionStatus = modificationTracker.Change(dto.deliveryPhasesSectionCompletionStatus, dtoStatus);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null);
        }

        if (modificationTracker.IsModified)
        {
            await _crmContext.Save(dto, organisationId, userAccount.UserGlobalId.ToString(), cancellationToken);
        }
    }
}
