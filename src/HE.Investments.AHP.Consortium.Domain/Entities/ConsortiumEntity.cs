using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Contract.Events;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Errors;
using HE.Investments.Common.Extensions;
using HE.Investments.Organisation.ValueObjects;
using HE.Investments.Programme.Contract;

namespace HE.Investments.AHP.Consortium.Domain.Entities;

public class ConsortiumEntity : DomainEntity, IConsortiumEntity
{
    private readonly List<ConsortiumMember> _members;

    private readonly List<ConsortiumMember> _joinRequests = [];

    private readonly List<OrganisationId> _removeRequests = [];

    public ConsortiumEntity(ConsortiumId id, ConsortiumName name, ProgrammeId programmeId, ConsortiumMember leadPartner, IEnumerable<ConsortiumMember>? members)
    {
        Id = id;
        ProgrammeId = programmeId;
        LeadPartner = leadPartner;
        Name = name;
        _members = members?.ToList() ?? [];
    }

    public ConsortiumId Id { get; private set; }

    public ConsortiumName Name { get; }

    public ProgrammeId ProgrammeId { get; }

    public ConsortiumMember LeadPartner { get; }

    public IEnumerable<ConsortiumMember> ActiveMembers => _members.Where(x => x.Status is ConsortiumMemberStatus.Active);

    public IEnumerable<ConsortiumMember> Members => _members.Where(x => x.Status != ConsortiumMemberStatus.Inactive);

    public static async Task<ConsortiumEntity> New(Programme.Contract.Programme programme, ConsortiumMember leadPartner, IIsPartOfConsortium isPartOfConsortium)
    {
        if (await isPartOfConsortium.IsPartOfConsortiumForProgramme(programme.Id, leadPartner.Id))
        {
            OperationResult.ThrowValidationError("SelectedProgrammeId", "A consortium has already been added to this programme");
        }

        return new ConsortiumEntity(
            ConsortiumId.New(),
            ConsortiumName.GenerateName(programme.Name, leadPartner.OrganisationName),
            programme.Id,
            leadPartner,
            Enumerable.Empty<ConsortiumMember>());
    }

    public async Task AddMember(InvestmentsOrganisation organisation, IIsPartOfConsortium isPartOfConsortium, CancellationToken cancellationToken)
    {
        if (IsPartOfThisConsortium(organisation))
        {
            OperationResult.ThrowValidationError("SelectedMember", ConsortiumValidationErrors.IsPartOfThisConsortium);
        }

        if (await IsPartOfOtherConsortium(organisation, isPartOfConsortium, cancellationToken))
        {
            OperationResult.ThrowValidationError("SelectedMember", ConsortiumValidationErrors.IsPartOfOtherConsortium);
        }

        var member = new ConsortiumMember(organisation.Id, organisation.Name, ConsortiumMemberStatus.PendingAddition);
        _members.Add(member);
        _joinRequests.Add(member);

        Publish(new ConsortiumMemberChangedEvent(Id, organisation.Id));
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

            Publish(new ConsortiumMemberChangedEvent(Id, draftMember.Id));
        }

        return true;
    }

    public async Task RemoveMember(
        OrganisationId organisationId,
        bool? isConfirmed,
        IConsortiumPartnerStatusProvider consortiumPartnerStatusProvider,
        CancellationToken cancellationToken)
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
        var partnerStatus = await consortiumPartnerStatusProvider.GetConsortiumPartnerStatus(Id, member.Id, cancellationToken);
        if (partnerStatus == ConsortiumPartnerStatus.SitePartner)
        {
            OperationResult.ThrowValidationError(nameof(isConfirmed), ConsortiumValidationErrors.IsSitePartner);
        }

        if (partnerStatus == ConsortiumPartnerStatus.ApplicationPartner)
        {
            OperationResult.ThrowValidationError(nameof(isConfirmed), ConsortiumValidationErrors.IsApplicationPartner);
        }

        _removeRequests.Add(organisationId);
        _members.Remove(member);
        _members.Add(new ConsortiumMember(member.Id, member.OrganisationName, ConsortiumMemberStatus.PendingRemoval));

        Publish(new ConsortiumMemberChangedEvent(Id, organisationId));
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

    private bool IsPartOfThisConsortium(InvestmentsOrganisation organisation) =>
        organisation.Id == LeadPartner.Id || Members.Any(x => x.Id == organisation.Id);

    private async Task<bool> IsPartOfOtherConsortium(
        InvestmentsOrganisation organisation,
        IIsPartOfConsortium isPartOfConsortium,
        CancellationToken cancellationToken)
        => await isPartOfConsortium.IsPartOfConsortiumForProgramme(ProgrammeId, organisation.Id, cancellationToken);

    private ConsortiumMember GetMember(OrganisationId organisationId) => Members.SingleOrDefault(x => x.Id == organisationId) ??
                                                                         throw new NotFoundException(nameof(ConsortiumMember), organisationId);
}
