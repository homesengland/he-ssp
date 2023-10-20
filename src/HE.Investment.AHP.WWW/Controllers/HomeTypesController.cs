using HE.Investment.AHP.BusinessLogic.HomeTypes;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.InvestmentLoans.Common.Routing;
using HE.Investments.Common.WWW.Routing;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("home-types")]
public class HomeTypesController : WorkflowController<HomeTypesWorkflow.State>
{
    [HttpGet("index")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("back")]
    public Task<IActionResult> Back(HomeTypesWorkflow.State currentPage)
    {
        return Back(currentPage, new object());
    }

    [HttpGet("type-of-housing")]
    public IActionResult TypeOfHousing()
    {
        return View(new TypeOfHousingModel("Scheme name"));
    }

    [HttpPost("type-of-housing")]
    public IActionResult TypeOfHousing(TypeOfHousingModel model, CancellationToken cancellationToken)
    {
        return View(model);
    }

    protected override Task<IStateRouting<HomeTypesWorkflow.State>> Routing(HomeTypesWorkflow.State currentState)
    {
        return Task.FromResult<IStateRouting<HomeTypesWorkflow.State>>(new HomeTypesWorkflow(currentState));
    }
}
