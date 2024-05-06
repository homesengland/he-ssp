extern alias Org;

using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using InvestmentsOrganisation = Org::HE.Investments.Organisation.ValueObjects.InvestmentsOrganisation;

namespace HE.Investments.AHP.Consortium.Domain.Entities;

public class DraftConsortiumEntity : IConsortiumEntity
{
    public DraftConsortiumEntity(string id, string name, string programmeId, string programmeName, DraftConsortiumMember leadPartner, IList<DraftConsortiumMember> members)
    {
        Id = id;
        Name = name;
        ProgrammeId = programmeId;
        ProgrammeName = programmeName;
        LeadPartner = leadPartner;
        Members = members;
    }

    public string Id { get; }

    public string Name { get; }

    public string ProgrammeId { get; }

    public string ProgrammeName { get; }

    public DraftConsortiumMember LeadPartner { get; }

    public IList<DraftConsortiumMember> Members { get; }

    public async Task AddMember(InvestmentsOrganisation organisation, IIsPartOfConsortium isPartOfConsortium, CancellationToken cancellationToken)
    {
        if (await IsPartOfConsortium(organisation, isPartOfConsortium, cancellationToken))
        {
            OperationResult.ThrowValidationError(
                "SelectedMember",
                "This organisation cannot be added to your consortium. Check you have selected the correct organisation. If it is correct, contact your Growth Manager");
        }

        Members.Add(new DraftConsortiumMember(organisation.Id, organisation.Name));
    }

    public void RemoveMember(OrganisationId organisationId, bool? isConfirmed)
    {
        if (isConfirmed.IsNotProvided())
        {
            OperationResult.ThrowValidationError(nameof(isConfirmed), "Select whether you want to remove this organisation from consortium");
        }

        if (isConfirmed == false)
        {
            return;
        }

        // TODO: add validation when member does not exist
        var member = Members.Single(x => x.Id == organisationId);

        Members.Remove(member);
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

        return await isPartOfConsortium.IsPartOfConsortiumForProgramme(new ProgrammeId(ProgrammeId), organisation.Id, cancellationToken);
    }
}
