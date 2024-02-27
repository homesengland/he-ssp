using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.CompanyStructure.Commands;
using HE.Investments.Loans.Contract.Documents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;

public class UploadOrganisationMoreInformationFileCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<UploadOrganisationMoreInformationFileCommand, OperationResult<UploadedFile?>>
{
    private readonly ILoansFileService<LoanApplicationId> _fileService;

    private readonly ICompanyStructureFileFactory _fileFactory;

    public UploadOrganisationMoreInformationFileCommandHandler(
        ILoansFileService<LoanApplicationId> fileService,
        ICompanyStructureFileFactory fileFactory,
        ICompanyStructureRepository companyStructureRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<CompanyStructureBaseCommandHandler> logger)
        : base(companyStructureRepository, loanApplicationRepository, loanUserContext, logger)
    {
        _fileService = fileService;
        _fileFactory = fileFactory;
    }

    public async Task<OperationResult<UploadedFile?>> Handle(UploadOrganisationMoreInformationFileCommand request, CancellationToken cancellationToken)
    {
        UploadedFile? uploadedFile = null;
        var result = await Perform(
            async companyStructure =>
            {
                await using var file = _fileFactory.Create(request.File);
                uploadedFile = (await companyStructure.UploadFiles(_fileService, new[] { file }, cancellationToken)).Single();
            },
            request.LoanApplicationId,
            cancellationToken);

        return result.HasValidationErrors ? new OperationResult<UploadedFile?>(result.Errors, null) : new OperationResult<UploadedFile?>(uploadedFile);
    }
}
