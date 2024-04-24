using HE.Investments.Common.Contract.Pagination;

namespace HE.Investments.AHP.Consortium.Contract;

public record SearchOrganisationResult(string Phrase, PaginationResult<OrganisationDetails> Page);
