using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common;
using HE.Investments.Common.Contract;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record DownloadDesignFileQuery(AhpApplicationId ApplicationId, string HomeTypeId, FileId FileId) : IRequest<DownloadedFile>;
