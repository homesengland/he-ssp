using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.CompanyStructure.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.QueryHandlers;

public class GetCompanyStructureFilesQueryHandler : IRequestHandler<GetCompanyStructureFilesQuery, IList<FileModel>>
{
    private readonly ILoansFileService<LoanApplicationId> _fileService;

    public GetCompanyStructureFilesQueryHandler(ILoansFileService<LoanApplicationId> fileService)
    {
        _fileService = fileService;
    }

    public async Task<IList<FileModel>> Handle(GetCompanyStructureFilesQuery request, CancellationToken cancellationToken)
    {
        var result = await _fileService.GetFiles(request.LoanApplicationId, cancellationToken);

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
