using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record DownloadDesignFileQuery(AhpApplicationId ApplicationId, string HomeTypeId, string FileId) : IRequest<DownloadedFile>;
