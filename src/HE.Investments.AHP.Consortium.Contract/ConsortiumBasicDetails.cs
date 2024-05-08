using System.Collections.ObjectModel;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Contract;

public record ConsortiumBasicDetails(
    ConsortiumId Id,
    ProgrammeId ProgrammeId,
    OrganisationId LeadPartnerId,
    ReadOnlyCollection<OrganisationId> ActiveMembers);
