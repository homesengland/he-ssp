using HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.User.Queries;
using HE.InvestmentLoans.WWW.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Authorize]
[CheckProfileCompletion]
[Route("api/user")]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<GetUserAccountResponse> Index()
    {
        return await _mediator.Send(new GetUserAccountQuery());
    }

    [Route("dashboard")]
    public async Task<GetDashboardDataQueryResponse> Dashboard()
    {
        return await _mediator.Send(new GetDashboardDataQuery());
    }

    [Route("application-loan/{id}")]
    public async Task<GetLoanApplicationQueryResponse> ApplicationLoan(string id)
    {
        return await _mediator.Send(new GetLoanApplicationQuery(LoanApplicationId.From(id)));
    }

    [Route("organization-details")]
    public async Task<GetOrganizationBasicInformationQueryResponse> OrganizationDetails()
    {
        return await _mediator.Send(new GetOrganizationBasicInformationQuery());
    }
}
