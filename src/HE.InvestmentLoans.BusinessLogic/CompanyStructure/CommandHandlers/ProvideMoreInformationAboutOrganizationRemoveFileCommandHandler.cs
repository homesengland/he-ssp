using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideMoreInformationAboutOrganizationRemoveFileCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideMoreInformationAboutOrganizationRemoveFileCommand, OperationResult>
{
    private readonly IHttpDocumentService _documentService;
    private readonly IDocumentServiceConfig _config;

    public ProvideMoreInformationAboutOrganizationRemoveFileCommandHandler(
                ICompanyStructureRepository repository,
                ILoanUserContext loanUserContext,
                ILogger<CompanyStructureBaseCommandHandler> logger,
                IHttpDocumentService documentService,
                IDocumentServiceConfig config)
        : base(repository, loanUserContext, logger)
    {
        _documentService = documentService;
        _config = config;
    }

    public async Task<OperationResult> Handle(ProvideMoreInformationAboutOrganizationRemoveFileCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async companyStructure => await _documentService.DeleteAsync(
                    _config.ListAlias,
                    request.FolderPath,
                    request.FileName),
            request.LoanApplicationId,
            cancellationToken);
    }
}
