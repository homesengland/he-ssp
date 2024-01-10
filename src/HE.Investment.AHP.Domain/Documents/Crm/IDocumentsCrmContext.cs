using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Domain.Documents.Crm;

public interface IDocumentsCrmContext
{
    Task<string> GetDocumentLocation(AhpApplicationId applicationId, CancellationToken cancellationToken);
}
