using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Contract;

public record ConsortiumByMemberRole(ConsortiumId ConsortiumId, Programme.Contract.Programme Programme, string LeadPartnerName, ConsortiumMembershipRole MembershipRole);
