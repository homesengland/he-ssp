using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Domain;
using MediatR;
using ApplicationStatus = HE.InvestmentLoans.Contract.Application.Enums.ApplicationStatus;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetTaskListDataQueryHandler : IRequestHandler<GetTaskListDataQuery, GetTaskListDataQueryResponse>
{
    private readonly ILoanApplicationRepository _applicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    public GetTaskListDataQueryHandler(ILoanApplicationRepository applicationRepository, IAccountUserContext loanUserContext)
    {
        _applicationRepository = applicationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<GetTaskListDataQueryResponse> Handle(GetTaskListDataQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var loanApplication = await _applicationRepository.GetLoanApplication(request.Id, userAccount, cancellationToken);

        return new GetTaskListDataQueryResponse(
            loanApplication.Id,
            loanApplication.Name,
            userAccount.AccountName,
            loanApplication.ExternalStatus,
            loanApplication.CanBeSubmitted(),
            loanApplication.WasSubmitted(),
            new Sections(
                MapToSectionStatus(loanApplication.ExternalStatus, loanApplication.CompanyStructure.Status),
                MapToSectionStatus(loanApplication.ExternalStatus, loanApplication.Funding.Status),
                MapToSectionStatus(loanApplication.ExternalStatus, loanApplication.Security.Status),
                loanApplication.ProjectsSection.Projects
                    .Select(x => new ProjectSection(x.Id.Value, x.Name.Value, MapToSectionStatus(loanApplication.ExternalStatus, x.Status)))
                    .ToArray()),
            loanApplication.LastModificationDate ?? loanApplication.CreatedOn ?? DateTime.MinValue,
            loanApplication.SubmittedOn ?? DateTime.MinValue,
            loanApplication.LastModifiedBy);
    }

    private SectionStatus MapToSectionStatus(ApplicationStatus status, SectionStatus sectionStatus)
    {
        return status switch
        {
            ApplicationStatus.Withdrawn => SectionStatus.Withdrawn,
            _ => sectionStatus,
        };
    }
}
