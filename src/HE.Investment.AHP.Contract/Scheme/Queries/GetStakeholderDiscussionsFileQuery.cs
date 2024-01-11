using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common;
using MediatR;

namespace HE.Investment.AHP.Contract.Scheme.Queries;

public record GetStakeholderDiscussionsFileQuery(AhpApplicationId ApplicationId, string FileId) : IRequest<DownloadedFile>;
