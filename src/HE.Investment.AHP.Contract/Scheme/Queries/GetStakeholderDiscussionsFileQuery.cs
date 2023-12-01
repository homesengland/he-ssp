using HE.Investment.AHP.Contract.Common;
using MediatR;

namespace HE.Investment.AHP.Contract.Scheme.Queries;

public record GetStakeholderDiscussionsFileQuery(string ApplicationId, string FileId) : IRequest<FileWithContent>;
