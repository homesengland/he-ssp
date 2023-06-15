using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace HE.InvestmentLoans.WWW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IMediator mediator;

        public HomeController(ILogger<HomeController> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        public IActionResult Index()
        {
            var x = this.User.Claims.ToList();
            // var list = _mediator.Send(new BusinessLogic._Account.GetAll());
            return Redirect("application");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
