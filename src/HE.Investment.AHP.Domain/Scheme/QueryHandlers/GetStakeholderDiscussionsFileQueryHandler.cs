using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Loans.Common.Exceptions;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.QueryHandlers;

public class GetStakeholderDiscussionsFileQueryHandler : IRequestHandler<GetStakeholderDiscussionsFileQuery, FileWithContent>
{
    private readonly ISchemeRepository _repository;

    public GetStakeholderDiscussionsFileQueryHandler(ISchemeRepository repository)
    {
        _repository = repository;
    }

    public async Task<FileWithContent> Handle(GetStakeholderDiscussionsFileQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByApplicationId(new(request.ApplicationId), cancellationToken);
        var file = entity.StakeholderDiscussionsFiles.UploadedFiles.FirstOrDefault(f => f.Id == new FileId(request.FileId));

        if (file == null)
        {
            throw new NotFoundException($"Cannot find file with id {request.FileId}.");
        }

        return new FileWithContent(file.Name.Value, 1, new MemoryStream(new byte[] { 0x20 }));
    }
}
