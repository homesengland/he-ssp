using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.Contract.Common;
using HE.Investments.Loans.Contract.CompanyStructure.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.QueryHandlers;

public class GetCompanyStructureFileQueryHandler : IRequestHandler<GetCompanyStructureFileQuery, DownloadedFile>
{
    private readonly ILoansFileService<CompanyStructureFileParams> _fileService;

    public GetCompanyStructureFileQueryHandler(ILoansFileService<CompanyStructureFileParams> fileService)
    {
        _fileService = fileService;
    }

    public async Task<DownloadedFile> Handle(GetCompanyStructureFileQuery request, CancellationToken cancellationToken)
    {
        var result = await _fileService.DownloadFile(request.FileId, new CompanyStructureFileParams(request.LoanApplicationId), cancellationToken);
        return new DownloadedFile(result.Name, result.Content);
    }
}
