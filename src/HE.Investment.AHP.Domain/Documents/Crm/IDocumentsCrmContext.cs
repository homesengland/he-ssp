using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Documents.Crm;

public interface IDocumentsCrmContext
{
    Task<string> GetDocumentLocation(ApplicationId applicationId, CancellationToken cancellationToken);
}
