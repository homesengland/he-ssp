using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.Contract.Application.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetSupportingDocumentsFilesQueryHandler : IRequestHandler<GetSupportingDocumentsFilesQuery, IList<FileModel>>
{
    private readonly ILoansFileService<SupportingDocumentsParams> _fileService;

    public GetSupportingDocumentsFilesQueryHandler(ILoansFileService<SupportingDocumentsParams> fileService)
    {
        _fileService = fileService;
    }

    public async Task<IList<FileModel>> Handle(GetSupportingDocumentsFilesQuery request, CancellationToken cancellationToken)
    {
        var result = await _fileService.GetFiles(SupportingDocumentsParams.New(request.LoanApplicationId), cancellationToken);

        return result
            .OrderBy(x => x.UploadedOn)
            .Select(x => new FileModel(
                x.Id?.Value ?? string.Empty,
                x.Name,
                x.UploadedOn,
                x.UploadedBy ?? string.Empty,
                x.Id.IsProvided(),
                string.Empty,
                string.Empty))
            .ToList();
    }
}
