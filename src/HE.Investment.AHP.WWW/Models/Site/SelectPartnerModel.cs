using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.WWW.Models.Site;

public record SelectPartnerModel(string SiteId, string SiteName, PaginationResult<ConsortiumMemberDetails> ConsortiumMembers);