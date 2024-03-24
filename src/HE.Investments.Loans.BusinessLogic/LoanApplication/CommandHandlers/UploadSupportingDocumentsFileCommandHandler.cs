using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.Contract.Application.Commands;
using HE.Investments.Loans.Contract.Documents;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.CommandHandlers;

public class UploadSupportingDocumentsFileCommandHandler : ApplicationBaseCommandHandler,
    IRequestHandler<UploadSupportingDocumentsFileCommand, OperationResult<UploadedFile?>>
{
    private readonly ILoansFileService<SupportingDocumentsParams> _fileService;

    private readonly ISupportingDocumentsFileFactory _fileFactory;

    public UploadSupportingDocumentsFileCommandHandler(
        ILoansFileService<SupportingDocumentsParams> fileService,
        ISupportingDocumentsFileFactory fileFactory,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
        : base(loanApplicationRepository, loanUserContext)
    {
        _fileService = fileService;
        _fileFactory = fileFactory;
    }

    public async Task<OperationResult<UploadedFile?>> Handle(UploadSupportingDocumentsFileCommand request, CancellationToken cancellationToken)
    {
        UploadedFile? uploadedFile = null;
        var result = await Perform(
            async loanApplication =>
            {
                await using var file = _fileFactory.Create(request.File);
                uploadedFile = (await loanApplication.UploadFiles(_fileService, new[] { file }, cancellationToken)).Single();
            },
            request.LoanApplicationId,
            cancellationToken);

        return result.HasValidationErrors ? new OperationResult<UploadedFile?>(result.Errors, null) : new OperationResult<UploadedFile?>(uploadedFile);
    }
}
