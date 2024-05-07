using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Domain.Crm;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Mappers;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;
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
            var members = consortiumDto.members.Select(x =>
                new ConsortiumMember(OrganisationId.From(x.id), x.name, ConsortiumMemberStatusMapper.ToDomain(x.status)));

            return new ConsortiumEntity(
                consortiumId,
                new ConsortiumName(consortiumDto.name),
                new ProgrammeSlim(ProgrammeId.From(consortiumDto.programmeId), "AHP CME"),
                new ConsortiumMember(OrganisationId.From(consortiumDto.leadPartnerId), consortiumDto.leadPartnerName, ConsortiumMemberStatus.Active),
                members);
        }

        throw new NotFoundException("Consortium", consortiumId.Value);
    }

    public async Task<IList<ConsortiumEntity>> GetConsortiumsListByMemberId(OrganisationId organisationId, CancellationToken cancellationToken)
    {
        var consortiumsListDto = await _crmContext.GetConsortiumsListByMemberId(
            organisationId.ToGuidAsString(),
            cancellationToken);

        return consortiumsListDto.Select(x => new ConsortiumEntity(
            ConsortiumId.From(x.id),
            new ConsortiumName(x.name),
            new ProgrammeSlim(ProgrammeId.From(x.programmeId), x.programmeName),
            new ConsortiumMember(OrganisationId.From(x.leadPartnerId), x.leadPartnerName, ConsortiumMemberStatus.Active),
            x.members.Select(y => new ConsortiumMember(OrganisationId.From(y.id), y.name, ConsortiumMemberStatusMapper.ToDomain(y.status))))).ToList();
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

            consortiumEntity.SetId(ConsortiumId.From(consortiumId));
        }

        await SaveConsortiumMemberRequests(consortiumEntity, userAccount, cancellationToken);

        return consortiumEntity;
    }

    public async Task<bool> IsPartOfConsortiumForProgramme(ProgrammeId programmeId, OrganisationId organisationId, CancellationToken cancellationToken = default)
    {
        return await _crmContext.IsConsortiumExistForProgrammeAndOrganisation(programmeId.ToString(), organisationId.ToString(), cancellationToken);
    }

    private async Task SaveConsortiumMemberRequests(
        ConsortiumEntity consortiumEntity,
        UserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var joinRequest = consortiumEntity.PopJoinRequest();
        while (joinRequest != null)
        {
            await _crmContext.CreateJoinConsortiumRequest(
                consortiumEntity.Id.Value,
                joinRequest.Value,
                userAccount.UserGlobalId.ToString(),
                cancellationToken);
            joinRequest = consortiumEntity.PopJoinRequest();
        }

        var removeRequest = consortiumEntity.PopRemoveRequest();
        while (removeRequest != null)
        {
            await _crmContext.CreateRemoveFromConsortiumRequest(
                consortiumEntity.Id.Value,
                removeRequest.Value,
                userAccount.UserGlobalId.ToString(),
                cancellationToken);
            removeRequest = consortiumEntity.PopRemoveRequest();
        }
    }
}
