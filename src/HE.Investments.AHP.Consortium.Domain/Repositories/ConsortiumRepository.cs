using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Crm;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investments.AHP.Consortium.Domain.Repositories;

public class ConsortiumRepository : IConsortiumRepository
{
    private readonly IConsortiumCrmContext _crmContext;

    public ConsortiumRepository(IConsortiumCrmContext crmContext)
    {
        _crmContext = crmContext;
    }

    public async Task<ConsortiumEntity> GetConsortium(ConsortiumId consortiumId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var consortiumDto = await _crmContext.GetConsortium(
            consortiumId.ToString(),
            userAccount.SelectedOrganisationId().ToString(),
            cancellationToken);

        if (!string.IsNullOrWhiteSpace(consortiumDto.id))
        {
            return new ConsortiumEntity(
                consortiumId,
                new ConsortiumName(consortiumDto.name),
                new ProgrammeSlim(new ProgrammeId(consortiumDto.programmeId), "AHP CME"),
                new ConsortiumMember(new OrganisationId(consortiumDto.leadPartnerId), consortiumDto.leadPartnerName));
        }

        throw new NotFoundException("Consortium", consortiumId.Value);
    }

    public async Task<ConsortiumEntity> Save(ConsortiumEntity consortiumEntity, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (consortiumEntity.Id.IsNew)
        {
            var consortiumId = await _crmContext.CreateConsortium(
                userAccount.UserGlobalId.ToString(),
                consortiumEntity.Programme.Id.ToString(),
                consortiumEntity.Name.ToString(),
                consortiumEntity.LeadPartner.Id.ToString(),
                cancellationToken);

            consortiumEntity.SetId(new ConsortiumId(consortiumId));
        }

        return consortiumEntity;
    }

    public async Task<bool> IsPartOfConsortiumForProgramme(ProgrammeId programmeId, OrganisationId organisationId, CancellationToken cancellationToken = default)
    {
        return await _crmContext.IsConsortiumExistForProgrammeAndOrganisation(programmeId.ToString(), organisationId.ToString(), cancellationToken);
    }
}
