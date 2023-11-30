using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.Common.Contract;
using MediatR;
using ContractApplication = HE.Investment.AHP.Contract.Application.Application;

namespace HE.Investment.AHP.Domain.Application.QueryHandlers;

public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, ContractApplication>
{
    private readonly IApplicationRepository _repository;

    public GetApplicationQueryHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContractApplication> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
    {
        var application = await _repository.GetById(new(request.ApplicationId), cancellationToken);

        return new ContractApplication(
            application.Id.Value,
            application.Name.Name,
            application.Tenure?.Value ?? default,
            application.Status,
            application.ReferenceNumber.Value,
            application.LastModified != null ? new ModificationDetails(application.LastModified.FirstName, application.LastModified.LastName, application.LastModified.ChangedOn) : null,
            new List<ApplicationSection>
            {
                new(SectionType.Scheme, application.Sections.SchemeStatus),
                new(SectionType.HomeTypes, application.Sections.HomeTypesStatus),
                new(SectionType.FinancialDetails, application.Sections.FinancialDetailsStatus),
                new(SectionType.DeliveryPhases, application.Sections.DeliveryPhasesStatus),
            });
    }
}
