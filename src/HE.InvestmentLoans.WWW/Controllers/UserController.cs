using HE.InvestmentLoans.Contract.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IMediator mediator;

    public UserController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<GetUserDetailsResponse> Index()
    {
        return await mediator.Send(new GetUserDetailsQuery());
    }
}
