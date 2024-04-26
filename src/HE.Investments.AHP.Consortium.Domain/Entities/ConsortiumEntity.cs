using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Errors;

namespace HE.Investments.AHP.Consortium.Domain.Entities;

public class ConsortiumEntity
{
    public ConsortiumEntity(ConsortiumId id, ConsortiumName name, ProgrammeSlim programme, ConsortiumMember leadPartner)
    {
        Id = id;
        Programme = programme;
        LeadPartner = leadPartner;
        Name = name;
    }

    public ConsortiumId Id { get; private set; }

    public ConsortiumName Name { get; }

    public ProgrammeSlim Programme { get; }

    public ConsortiumMember LeadPartner { get; }

    public static async Task<ConsortiumEntity> New(ProgrammeSlim programme, ConsortiumMember leadPartner, IIsPartOfConsortium isPartOfConsortium)
    {
        if (await isPartOfConsortium.IsPartOfConsortiumForProgramme(programme.Id, leadPartner.Id))
        {
            OperationResult.ThrowValidationError("SelectedProgrammeId", "A consortium has already been added to this programme");
        }

        return new ConsortiumEntity(ConsortiumId.New(), ConsortiumName.GenerateName(programme.Name, leadPartner.OrganisationName), programme, leadPartner);
    }

    public void SetId(ConsortiumId newId)
    {
        if (!Id.IsNew)
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
    }
}
