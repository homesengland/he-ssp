using HE.Investments.AHP.Consortium.Contract.Enums;

namespace HE.Investments.AHP.Consortium.Contract;

public record ConsortiumByMemberRole(ConsortiumId ConsortiumId, ProgrammeSlim Programme, string LeadPartnerName, ConsortiumMembershipRole MembershipRole);
