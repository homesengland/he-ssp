using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Utils.Pagination;

namespace HE.Investment.AHP.WWW.Models.Application;

public record ApplicationsListModel(string OrganisationName, PaginationResult<ApplicationBasicDetails> Result, bool IsReadOnly);
