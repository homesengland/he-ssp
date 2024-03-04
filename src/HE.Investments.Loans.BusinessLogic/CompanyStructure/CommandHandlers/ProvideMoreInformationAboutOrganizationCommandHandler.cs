using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.CompanyStructure.Commands;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideMoreInformationAboutOrganizationCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideMoreInformationAboutOrganizationCommand, OperationResult>
{
    private readonly ILoansFileService<LoanApplicationId> _fileService;

    private readonly ICompanyStructureFileFactory _fileFactory;

    public ProvideMoreInformationAboutOrganizationCommandHandler(
        ILoansFileService<LoanApplicationId> fileService,
        ICompanyStructureFileFactory fileFactory,
        ICompanyStructureRepository companyStructureRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
        : base(companyStructureRepository, loanApplicationRepository, loanUserContext)
    {
        _fileFactory = fileFactory;
        _fileService = fileService;
    }

    public async Task<OperationResult> Handle(ProvideMoreInformationAboutOrganizationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async companyStructure =>
            {
                companyStructure.ProvideMoreInformation(
                    request.OrganisationMoreInformation.IsProvided() ? new OrganisationMoreInformation(request.OrganisationMoreInformation!) : null);

                if (request.FormFiles == null)
                {
                    return;
                }

                using var files = request.FormFiles.Select(_fileFactory.Create).ToDisposableList();
                await companyStructure.UploadFiles(_fileService, files, cancellationToken);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
