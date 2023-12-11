using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Utils.Pagination;
using HE.Investments.Common.WWW.TagHelpers.Pagination;

namespace HE.Investment.AHP.WWW.Models.Application;

public record ApplicationsListModel(string OrganisationName, PaginationResult<ApplicationBasicDetails> Result);
