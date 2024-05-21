extern alias Org;

using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using InvestmentsOrganisation = Org::HE.Investments.Organisation.ValueObjects.InvestmentsOrganisation;

namespace HE.Investments.AHP.Consortium.Domain.Entities;

public class DraftConsortiumEntity : IConsortiumEntity
{
    public DraftConsortiumEntity(
        ConsortiumId id,
        ConsortiumName name,
        ProgrammeSlim programme,
        DraftConsortiumMember leadPartner,
        IList<DraftConsortiumMember> members)
    {
        Id = id;
        Name = name;
        Programme = programme;
        LeadPartner = leadPartner;
        Members = members;
    }

    public ConsortiumId Id { get; }

    public ConsortiumName Name { get; }

    public ProgrammeSlim Programme { get; }

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

        Members.Remove(GetMember(organisationId));
    }

    private bool IsPartOfThisConsortium(InvestmentsOrganisation organisation) =>
        organisation.Id == LeadPartner.Id || Members.Any(x => x.Id == organisation.Id);

    private async Task<bool> IsPartOfOtherConsortium(
        InvestmentsOrganisation organisation,
        IIsPartOfConsortium isPartOfConsortium,
        CancellationToken cancellationToken)
        => await isPartOfConsortium.IsPartOfConsortiumForProgramme(Programme.Id, organisation.Id, cancellationToken);

    private DraftConsortiumMember GetMember(OrganisationId organisationId) => Members.SingleOrDefault(x => x.Id == organisationId) ??
                                                                              throw new NotFoundException(nameof(DraftConsortiumMember), organisationId);
}
