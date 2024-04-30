using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Crm;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using OrganisationId = HE.Investments.Account.Shared.User.ValueObjects.OrganisationId;

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
        OrganisationId organisationId,
        SectionType sectionType,
        SectionStatus targetStatus,
        CancellationToken cancellationToken)
    {
        var dto = await _crmContext.GetOrganisationApplicationById(applicationId.Value, organisationId.Value, cancellationToken);
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
            await _crmContext.Save(dto, organisationId.Value, cancellationToken);
        }
    }
}
