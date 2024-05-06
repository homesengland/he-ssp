using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Contract;

public record ConsortiumMemberDetails(OrganisationId OrganisationId, ConsortiumMemberStatus Status, OrganisationDetails Details);
