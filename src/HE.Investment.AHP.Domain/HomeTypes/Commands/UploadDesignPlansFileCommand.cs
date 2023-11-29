using HE.Investment.AHP.Domain.Common;
using HE.Investments.Common.Validators;
using MediatR;
using UploadedFile = HE.Investment.AHP.Contract.Common.UploadedFile;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record UploadDesignPlansFileCommand(string ApplicationId, string HomeTypeId, FileToUpload File)
    : IRequest<OperationResult<UploadedFile?>>;
