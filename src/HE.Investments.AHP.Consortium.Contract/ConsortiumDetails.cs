namespace HE.Investments.AHP.Consortium.Contract;

public record ConsortiumDetails(
    ConsortiumId ConsortiumId,
    ProgrammeSlim Programme,
    ConsortiumMemberDetails LeadPartner,
    bool IsDraft,
    IList<ConsortiumMemberDetails> Members);
