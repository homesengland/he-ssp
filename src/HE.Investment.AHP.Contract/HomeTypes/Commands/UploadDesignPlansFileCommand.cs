using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using UploadedFile = HE.Investment.AHP.Contract.Common.UploadedFile;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record UploadDesignPlansFileCommand(AhpApplicationId ApplicationId, string HomeTypeId, FileToUpload File)
    : IRequest<OperationResult<UploadedFile?>>;
