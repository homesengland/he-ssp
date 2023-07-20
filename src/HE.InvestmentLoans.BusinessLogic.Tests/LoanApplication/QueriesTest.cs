using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Commands;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Queries;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication;

[TestClass]
public class QueriesTest : MediatorTestBase
{
    [TestMethod]
    public async Task Select_Command()
    {
        var mediator = (IMediator)ServiceProvider.GetService(typeof(IMediator));
        var model = await mediator.Send(new Create());
        var modelToCheck = await mediator.Send(new GetSingle() { Id = model.ID });
        Assert.AreEqual(model.ID, modelToCheck.ID);
    }
}
