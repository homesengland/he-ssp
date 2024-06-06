using HE.Investments.Common.Contract.Pagination;

namespace HE.Investments.Organisation.Contract;

public record SearchOrganisationResult(string Phrase, PaginationResult<OrganisationDetails> Page);
