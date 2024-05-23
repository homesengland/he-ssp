using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Contract;

public record ConsortiumByMemberRole(ConsortiumId ConsortiumId, ProgrammeSlim Programme, string LeadPartnerName, ConsortiumMembershipRole MembershipRole);
