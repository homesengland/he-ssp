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
                MapToSectionStatus(loanApplication.ExternalStatus, loanApplication.LegacyModel.Company.State),
                MapToSectionStatus(loanApplication.ExternalStatus, loanApplication.LegacyModel.Funding.State),
                MapToSectionStatus(loanApplication.ExternalStatus, loanApplication.LegacyModel.Security.State),
                loanApplication.LegacyModel.Sites
                    .Select(x => new ProjectSection(x.Id, x.Name, MapToSectionStatus(loanApplication.ExternalStatus, x.GetSectionStatus())))
                    .ToArray()),
            loanApplication.LastModificationDate ?? loanApplication.CreatedOn ?? DateTime.MinValue);
    }

    private SectionStatus MapToSectionStatus(ApplicationStatus status, SectionStatus sectionStatus)
    {
        if (status == ApplicationStatus.Withdrawn)
        {
            return SectionStatus.Withdrawn;
        }

        return sectionStatus;
    }
}
