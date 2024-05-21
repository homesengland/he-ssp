using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.WWW.Models.Scheme;

public record SelectPartnerModel(string ApplicationId, string ApplicationName, PaginationResult<ConsortiumMemberDetails> ConsortiumMembers);
