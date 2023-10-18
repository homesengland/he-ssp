using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Queries;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetTaskListDataQueryHandler : IRequestHandler<GetTaskListDataQuery, GetTaskListDataQueryResponse>
{
    private readonly ILoanApplicationRepository _applicationRepository;

    private readonly ILoanUserContext _loanUserContext;

    public GetTaskListDataQueryHandler(ILoanApplicationRepository applicationRepository, ILoanUserContext loanUserContext)
    {
        _applicationRepository = applicationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<GetTaskListDataQueryResponse> Handle(GetTaskListDataQuery request, CancellationToken cancellationToken)
    {
        var loanApplication = await _applicationRepository.GetLoanApplication(request.Id, await _loanUserContext.GetSelectedAccount(), cancellationToken);

        return new GetTaskListDataQueryResponse(
            loanApplication.Id,
            loanApplication.ExternalStatus,
            loanApplication.CanBeSubmitted(),
            new Sections(
                MapToSectionStatus(loanApplication.ExternalStatus, loanApplication.CompanyStructure.Status),
                MapToSectionStatus(loanApplication.ExternalStatus, loanApplication.LegacyModel.Funding.State),
                MapToSectionStatus(loanApplication.ExternalStatus, loanApplication.Security.Status),
                loanApplication.LegacyModel.Projects
                    .Select(x => new ProjectSection(x.ProjectId, x.Name, MapToSectionStatus(loanApplication.ExternalStatus, x.Status)))
                    .ToArray()),
            loanApplication.LastModificationDate ?? loanApplication.CreatedOn ?? DateTime.MinValue,
            loanApplication.LastModifiedBy);
    }

    private SectionStatus MapToSectionStatus(ApplicationStatus status, SectionStatus sectionStatus)
    {
        return status switch
        {
            ApplicationStatus.Withdrawn => SectionStatus.Withdrawn,
            ApplicationStatus.ApplicationSubmitted => SectionStatus.Submitted,
            _ => sectionStatus,
        };
    }
}
