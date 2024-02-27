using HE.Investments.Account.Shared;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.ViewModel;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.CompanyStructure.Queries;
using HE.Investments.Loans.Contract.Funding.Queries;
using HE.Investments.Loans.Contract.Security.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetLoanApplicationQueryHandler : IRequestHandler<GetLoanApplicationQuery, GetLoanApplicationQueryResponse>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly IAccountUserContext _loanUserContext;
    private readonly IMediator _mediator;
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;

    public GetLoanApplicationQueryHandler(ILoanApplicationRepository loanApplicationRepository, IAccountUserContext loanUserContext, IMediator mediator, IApplicationProjectsRepository applicationProjectsRepository)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _mediator = mediator;
        _applicationProjectsRepository = applicationProjectsRepository;
    }

    public async Task<GetLoanApplicationQueryResponse> Handle(GetLoanApplicationQuery request, CancellationToken cancellationToken)
    {
        var companyStructureResponse = await _mediator.Send(new GetCompanyStructureQuery(request.Id, CompanyStructureFieldsSet.GetAllFields), cancellationToken);
        if (request.LoadFiles)
        {
            companyStructureResponse.ViewModel.OrganisationMoreInformationFiles = await _mediator.Send(new GetCompanyStructureFilesQuery(request.Id), cancellationToken);
        }

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
            WasSubmittedPreviously = loanApplication.WasSubmitted(),
        };

        return new GetLoanApplicationQueryResponse(viewModel);
    }
}
