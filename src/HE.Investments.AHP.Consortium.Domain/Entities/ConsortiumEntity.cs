extern alias Org;

using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Errors;
using HE.Investments.Common.Extensions;
using Org::HE.Investments.Organisation.ValueObjects;

namespace HE.Investments.AHP.Consortium.Domain.Entities;

public class ConsortiumEntity : IConsortiumEntity
{
    private readonly List<ConsortiumMember> _members;

    private readonly List<ConsortiumMember> _joinRequests = [];

    private readonly List<OrganisationId> _removeRequests = [];

    public ConsortiumEntity(ConsortiumId id, ConsortiumName name, ProgrammeSlim programme, ConsortiumMember leadPartner, IEnumerable<ConsortiumMember>? members)
    {
        Id = id;
        Programme = programme;
        LeadPartner = leadPartner;
        Name = name;
        _members = members?.ToList() ?? [];
    }

    public ConsortiumId Id { get; private set; }

    public ConsortiumName Name { get; }

    public ProgrammeSlim Programme { get; }

    public ConsortiumMember LeadPartner { get; }

    public IEnumerable<ConsortiumMember> ActiveMembers => _members.Where(x => x.Status is ConsortiumMemberStatus.Active);

    public IEnumerable<ConsortiumMember> Members => _members.Where(x => x.Status != ConsortiumMemberStatus.Inactive);

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

    public async Task AddMember(InvestmentsOrganisation organisation, IIsPartOfConsortium isPartOfConsortium, CancellationToken cancellationToken)
    {
        if (await IsPartOfConsortium(organisation, isPartOfConsortium, cancellationToken))
        {
            OperationResult.ThrowValidationError("SelectedMember", ConsortiumValidationErrors.IsAlreadyPartOfConsortium);
        }

        var member = new ConsortiumMember(organisation.Id, organisation.Name, ConsortiumMemberStatus.PendingAddition);
        _members.Add(member);
        _joinRequests.Add(member);
    }

    public bool AddMembersFromDraft(DraftConsortiumEntity draftConsortium, AreAllMembersAdded? requestAreAllMembersAdded)
    {
        if (draftConsortium.Id != Id || draftConsortium.LeadPartner.Id != LeadPartner.Id)
        {
            throw new InvalidOperationException("Draft consortium members cannot be added to consortium because consortium details does not match.");
        }

        if (requestAreAllMembersAdded is null or AreAllMembersAdded.Undefined)
        {
            OperationResult.ThrowValidationError(nameof(AreAllMembersAdded), "Select whether you have you added all members to this consortium");
        }

        if (requestAreAllMembersAdded == AreAllMembersAdded.No)
        {
            return false;
        }

        foreach (var draftMember in draftConsortium.Members)
        {
            var member = new ConsortiumMember(draftMember.Id, draftMember.OrganisationName, ConsortiumMemberStatus.PendingAddition);
            _members.Add(member);
            _joinRequests.Add(member);
        }

        return true;
    }

    public void RemoveMember(OrganisationId organisationId, bool? isConfirmed)
    {
        if (isConfirmed.IsNotProvided())
        {
            OperationResult.ThrowValidationError(nameof(isConfirmed), ConsortiumValidationErrors.RemoveConfirmationNotSelected);
        }

        if (isConfirmed == false)
        {
            return;
        }

        var member = GetMember(organisationId);

        _removeRequests.Add(organisationId);
        _members.Remove(member);
        _members.Add(new ConsortiumMember(member.Id, member.OrganisationName, ConsortiumMemberStatus.PendingRemoval));
    }

    public OrganisationId? PopJoinRequest() => _joinRequests.PopItem()?.Id;

    public OrganisationId? PopRemoveRequest() => _removeRequests.PopItem();

    public void SetId(ConsortiumId newId)
    {
        if (!Id.IsNew)
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
    }

    private async Task<bool> IsPartOfConsortium(
        InvestmentsOrganisation organisation,
        IIsPartOfConsortium isPartOfConsortium,
        CancellationToken cancellationToken)
    {
        if (organisation.Id == LeadPartner.Id || Members.Any(x => x.Id == organisation.Id))
        {
            return true;
        }

        return await isPartOfConsortium.IsPartOfConsortiumForProgramme(Programme.Id, organisation.Id, cancellationToken);
    }

    private ConsortiumMember GetMember(OrganisationId organisationId) => Members.SingleOrDefault(x => x.Id == organisationId) ??
                                                                         throw new NotFoundException(nameof(ConsortiumMember), organisationId);
}
