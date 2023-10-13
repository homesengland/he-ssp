using System.Globalization;
using System.Text.Json;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Utils.Constants;
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

public class ProvideMoreInformationAboutOrganizationFileCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideMoreInformationAboutOrganizationFileCommand, OperationResult>
{
    private readonly IDocumentServiceConfig _config;

    private readonly IHttpDocumentService _documentService;

    public ProvideMoreInformationAboutOrganizationFileCommandHandler(
                ICompanyStructureRepository repository,
                ILoanUserContext loanUserContext,
                IAppConfig appConfig,
                ILogger<CompanyStructureBaseCommandHandler> logger,
                IHttpDocumentService documentService,
                IDocumentServiceConfig config)
        : base(repository, loanUserContext, logger)
    {
        _config = config;
        _documentService = documentService;
    }

    public async Task<OperationResult> Handle(ProvideMoreInformationAboutOrganizationFileCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async (companyStructure, userAccount) =>
            {
                companyStructure.ProvideFilesWithMoreInformation(new OrganisationMoreInformationFiles(request.OrganisationMoreInformationFiles?.Count + 1));

                var file = new FileData(request.FormFile);
                companyStructure.ProvideFileWithMoreInformation(new OrganisationMoreInformationFile(file, _config.MaxFileSizeInMegabytes));

                await _documentService.UploadAsync(new FileUploadModel()
                {
                    ListTitle = _config.ListTitle,
                    FolderPath = "0000000_DA2123DAE440EE11BDF3002248C653E1",
                    File = file,
                    Metadata = JsonSerializer.Serialize(new FileMetadata
                    {
                        CreateDate = DateTime.Now,
                        Creator = userAccount.AccountName,
                    }),
                    Overwrite = true,
                });
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
