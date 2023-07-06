using HE.InvestmentLoans.Contract.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<GetUserDetailsResponse> Index()
    {
        return await _mediator.Send(new GetUserDetailsQuery());
    }
}
