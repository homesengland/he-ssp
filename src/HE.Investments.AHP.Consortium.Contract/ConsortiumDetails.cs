using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Contract;

public record ConsortiumDetails(
    ConsortiumId ConsortiumId,
    Programme.Contract.Programme Programme,
    ConsortiumMemberDetails LeadPartner,
    bool IsDraft,
    IList<ConsortiumMemberDetails> Members);
