using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Contract.Cache;

public record AhpConsortiumBasicInfo(ConsortiumId ConsortiumId, bool IsLeadPartner, IList<OrganisationId> ActiveMembers);
