using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Contract.Services.Interfaces;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Utils;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideMoreInformationAboutOrganizationCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideMoreInformationAboutOrganizationCommand, OperationResult>
{
    private readonly IDocumentServiceConfig _config;

    private readonly IHttpDocumentService _documentService;

    private readonly IDateTimeProvider _dateTime;

    private readonly INotificationService _notificationService;

    public ProvideMoreInformationAboutOrganizationCommandHandler(
                ICompanyStructureRepository repository,
                ILoanUserContext loanUserContext,
                IDocumentServiceConfig config,
                ILogger<CompanyStructureBaseCommandHandler> logger,
                IHttpDocumentService documentService,
                IDateTimeProvider dateTime,
                INotificationService notificationService)
        : base(repository, loanUserContext, logger)
    {
        _config = config;
        _documentService = documentService;
        _dateTime = dateTime;
        _notificationService = notificationService;
    }

    public async Task<OperationResult> Handle(ProvideMoreInformationAboutOrganizationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async (companyStructure, userAccount) =>
            {
                companyStructure.ProvideMoreInformation(
                    request.OrganisationMoreInformation.IsProvided() ? new OrganisationMoreInformation(request.OrganisationMoreInformation!) : null);

                if (request.FormFiles == null)
                {
                    return;
                }

                var filesCount = request.OrganisationMoreInformationFiles?.Count + request.FormFiles.Count;
                companyStructure.ProvideFilesWithMoreInformation(new OrganisationMoreInformationFiles(filesCount));

                var uploadedFilesNotify = string.Empty;
                foreach (var formFile in request.FormFiles)
                {
                    var file = new FileData(formFile);
                    companyStructure.ProvideFileWithMoreInformation(new OrganisationMoreInformationFile(file.Name, file.Data, _config.MaxFileSizeInMegabytes));

                    await _documentService.UploadAsync(new FileUploadModel()
                    {
                        ListTitle = _config.ListTitle,
                        FolderPath = "0000000_DA2123DAE440EE11BDF3002248C653E1",
                        File = file,
                        Metadata = JsonSerializer.Serialize(new FileMetadata
                        {
                            CreateDate = _dateTime.Now,
                            Creator = userAccount.AccountName,
                        }),
                        Overwrite = true,
                    });

                    uploadedFilesNotify += $"<div>{file.Name} successfully uploaded</div>";
                }

                if (!string.IsNullOrEmpty(uploadedFilesNotify))
                {
                    _notificationService.NotifySuccess(NotificationBodyType.Html, uploadedFilesNotify);
                }
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
