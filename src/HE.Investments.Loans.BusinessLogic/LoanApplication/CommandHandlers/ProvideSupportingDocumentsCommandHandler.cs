using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.Contract.Application.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.CommandHandlers;

public class ProvideSupportingDocumentsCommandHandler : ApplicationBaseCommandHandler,
    IRequestHandler<ProvideSupportingDocumentsCommand, OperationResult>
{
    private readonly ILoansFileService<SupportingDocumentsParams> _fileService;

    private readonly ISupportingDocumentsFileFactory _fileFactory;

    private readonly IChangeApplicationStatus _changeApplicationStatus;

    private readonly IEventDispatcher _eventDispatcher;

    public ProvideSupportingDocumentsCommandHandler(
        ILoansFileService<SupportingDocumentsParams> fileService,
        ISupportingDocumentsFileFactory fileFactory,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        IChangeApplicationStatus changeApplicationStatus,
        IEventDispatcher eventDispatcher)
        : base(loanApplicationRepository, loanUserContext)
    {
        _fileFactory = fileFactory;
        _fileService = fileService;
        _changeApplicationStatus = changeApplicationStatus;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<OperationResult> Handle(ProvideSupportingDocumentsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async loanApplication =>
            {
                if (request.FormFiles == null)
                {
                    return;
                }

                using var files = request.FormFiles.Select(_fileFactory.Create).ToDisposableList();
                await loanApplication.UploadFiles(_fileService, files, _eventDispatcher, cancellationToken);
                await _changeApplicationStatus.ChangeApplicationStatus(request.LoanApplicationId, ApplicationStatus.UnderReview, cancellationToken);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
