using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Organisation.ValueObjects;
using HE.Investments.Programme.Contract;

namespace HE.Investments.AHP.Consortium.Domain.Entities;

public class DraftConsortiumEntity : IConsortiumEntity
{
    public DraftConsortiumEntity(
        ConsortiumId id,
        ConsortiumName name,
        ProgrammeId programmeId,
        DraftConsortiumMember leadPartner,
        IList<DraftConsortiumMember> members)
    {
        Id = id;
        Name = name;
        ProgrammeId = programmeId;
        LeadPartner = leadPartner;
        Members = members;
    }

    public ConsortiumId Id { get; }

    public ConsortiumName Name { get; }

    public ProgrammeId ProgrammeId { get; }

    public DraftConsortiumMember LeadPartner { get; }

    public IList<DraftConsortiumMember> Members { get; }

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

        Members.Add(new DraftConsortiumMember(organisation.Id, organisation.Name));
    }

    public Task RemoveMember(
        OrganisationId organisationId,
        bool? isConfirmed,
        IConsortiumPartnerStatusProvider consortiumPartnerStatusProvider,
        CancellationToken cancellationToken)
    {
        if (isConfirmed.IsNotProvided())
        {
            OperationResult.ThrowValidationError(nameof(isConfirmed), ConsortiumValidationErrors.RemoveConfirmationNotSelected);
        }

        if (isConfirmed == true)
        {
            Members.Remove(GetMember(organisationId));
        }

        return Task.CompletedTask;
    }

    private bool IsPartOfThisConsortium(InvestmentsOrganisation organisation) =>
        organisation.Id == LeadPartner.Id || Members.Any(x => x.Id == organisation.Id);

    private async Task<bool> IsPartOfOtherConsortium(
        InvestmentsOrganisation organisation,
        IIsPartOfConsortium isPartOfConsortium,
        CancellationToken cancellationToken)
        => await isPartOfConsortium.IsPartOfConsortiumForProgramme(ProgrammeId, organisation.Id, cancellationToken);

    private DraftConsortiumMember GetMember(OrganisationId organisationId) => Members.SingleOrDefault(x => x.Id == organisationId) ??
                                                                              throw new NotFoundException(nameof(DraftConsortiumMember), organisationId);
}
