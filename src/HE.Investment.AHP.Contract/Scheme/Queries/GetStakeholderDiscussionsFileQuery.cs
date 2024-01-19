using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common;
using HE.Investments.Common.Contract;
using MediatR;

namespace HE.Investment.AHP.Contract.Scheme.Queries;

public record GetStakeholderDiscussionsFileQuery(AhpApplicationId ApplicationId, FileId FileId) : IRequest<DownloadedFile>;
