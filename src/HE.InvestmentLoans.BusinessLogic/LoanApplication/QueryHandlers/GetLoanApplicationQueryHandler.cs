using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.CompanyStructure.Queries;
using HE.InvestmentLoans.Contract.Funding.Queries;
using HE.InvestmentLoans.Contract.Security.Queries;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetLoanApplicationQueryHandler : IRequestHandler<GetLoanApplicationQuery, GetLoanApplicationQueryResponse>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly ILoanUserContext _loanUserContext;
    private readonly IMediator _mediator;
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;

    public GetLoanApplicationQueryHandler(ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext, IMediator mediator, IApplicationProjectsRepository applicationProjectsRepository)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _mediator = mediator;
        _applicationProjectsRepository = applicationProjectsRepository;
    }

    public async Task<GetLoanApplicationQueryResponse> Handle(GetLoanApplicationQuery request, CancellationToken cancellationToken)
    {
        var companyStructureResponse = await _mediator.Send(new GetCompanyStructureQuery(request.Id, CompanyStructureFieldsSet.GetAllFields), cancellationToken);
        companyStructureResponse.ViewModel.OrganisationMoreInformationFiles = (await _mediator.Send(new GetCompanyStructureFilesQuery(request.Id), cancellationToken)).Items;
        var securityResponse = await _mediator.Send(new GetSecurity(request.Id, SecurityFieldsSet.GetAllFields), cancellationToken);
        var fundingResponse = await _mediator.Send(new GetFundingQuery(request.Id, FundingFieldsSet.GetAllFields), cancellationToken);

        var selectedAccount = await _loanUserContext.GetSelectedAccount();
        var projects = await _applicationProjectsRepository.GetAllAsync(request.Id, selectedAccount, cancellationToken);
        var loanApplication = await _loanApplicationRepository.GetLoanApplication(request.Id, selectedAccount, cancellationToken);

        var viewModel = new LoanApplicationViewModel
        {
            ID = loanApplication.Id.Value,
            Status = loanApplication.ExternalStatus,
            Purpose = loanApplication.FundingReason,
            Company = companyStructureResponse.ViewModel,
            Funding = fundingResponse.ViewModel,
            Security = securityResponse.ViewModel,
            ReferenceNumber = loanApplication.ReferenceNumber,
            Projects = projects.Projects.Select(project => ProjectMapper.MapToViewModel(project, loanApplication.Id)),
        };

        return new GetLoanApplicationQueryResponse(viewModel);
    }
}
