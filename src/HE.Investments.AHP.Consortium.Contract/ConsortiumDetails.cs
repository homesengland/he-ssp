namespace HE.Investments.AHP.Consortium.Contract;

public record ConsortiumDetails(
    ConsortiumId ConsortiumId,
    string ProgrammeName,
    OrganisationDetails LeadPartner,
    IList<OrganisationDetails> Members);
