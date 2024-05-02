using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.AHP.Consortium.Contract.Enums;

namespace HE.Investments.AHP.Consortium.Contract;

public record ConsortiumMemberDetails(OrganisationId OrganisationId, ConsortiumMemberStatus Status, OrganisationDetails Details);
