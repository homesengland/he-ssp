extern alias Org;

using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Errors;
using HE.Investments.Common.Extensions;
using Org::HE.Investments.Organisation.ValueObjects;
using OrganisationId = HE.Investments.Account.Shared.User.ValueObjects.OrganisationId;

namespace HE.Investments.AHP.Consortium.Domain.Entities;

public class ConsortiumEntity
{
    private readonly IList<ConsortiumMember> _members;

    private readonly IList<ConsortiumMember> _joinRequests = new List<ConsortiumMember>();

    private readonly IList<OrganisationId> _removeRequests = new List<OrganisationId>();

    public ConsortiumEntity(ConsortiumId id, ConsortiumName name, ProgrammeSlim programme, ConsortiumMember leadPartner, IEnumerable<ConsortiumMember> members)
    {
        Id = id;
        Programme = programme;
        LeadPartner = leadPartner;
        Name = name;
        _members = members.ToList();
    }

    public ConsortiumId Id { get; private set; }

    public ConsortiumName Name { get; }

    public ProgrammeSlim Programme { get; }

    public ConsortiumMember LeadPartner { get; }

    public IEnumerable<ConsortiumMember> Members => _members;

    public static async Task<ConsortiumEntity> New(ProgrammeSlim programme, ConsortiumMember leadPartner, IIsPartOfConsortium isPartOfConsortium)
    {
        if (await isPartOfConsortium.IsPartOfConsortiumForProgramme(programme.Id, leadPartner.Id))
        {
            OperationResult.ThrowValidationError("SelectedProgrammeId", "A consortium has already been added to this programme");
        }

        return new ConsortiumEntity(
            ConsortiumId.New(),
            ConsortiumName.GenerateName(programme.Name, leadPartner.OrganisationName),
            programme,
            leadPartner,
            Enumerable.Empty<ConsortiumMember>());
    }

    public async Task AddMember(InvestmentsOrganisation organisation, IIsPartOfConsortium isPartOfConsortium)
    {
        if (await IsPartOfConsortium(organisation, isPartOfConsortium))
        {
            OperationResult.ThrowValidationError(
                "SelectedMember",
                "This organisation cannot be added to your consortium. Check you have selected the correct organisation. If it is correct, contact your Growth Manager");
        }

        var member = new ConsortiumMember(new OrganisationId(organisation.Id.Value), organisation.Name, ConsortiumMemberStatus.PendingAddition);
        _members.Add(member);
        _joinRequests.Add(member);
    }

    public void RemoveMember(OrganisationId organisationId, bool? isConfirmed)
    {
        if (isConfirmed.IsNotProvided())
        {
            OperationResult.ThrowValidationError(nameof(isConfirmed), "Select whether you want to remove this organisation from consortium");
        }

        // TODO: add validation when member does not exist
        var member = _members.Single(x => x.Id == organisationId);

        _removeRequests.Add(organisationId);
        _members.Remove(member);
    }

    public OrganisationId? PopJoinRequest()
    {
        if (_joinRequests.Any())
        {
            var result = _joinRequests[0];
            _joinRequests.RemoveAt(0);
            return result.Id;
        }

        return null;
    }

    public OrganisationId? PopRemoveRequest()
    {
        if (_removeRequests.Any())
        {
            var result = _removeRequests[0];
            _removeRequests.RemoveAt(0);
            return result;
        }

        return null;
    }

    public void SetId(ConsortiumId newId)
    {
        if (!Id.IsNew)
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
    }

    private async Task<bool> IsPartOfConsortium(InvestmentsOrganisation organisation, IIsPartOfConsortium isPartOfConsortium)
    {
        if (organisation.Id.Value == LeadPartner.Id.ToString() || _members.Any(x => x.Id.ToString() == organisation.Id.Value))
        {
            return true;
        }

        return await isPartOfConsortium.IsPartOfConsortiumForProgramme(Programme.Id, new OrganisationId(organisation.Id.Value));
    }
}
