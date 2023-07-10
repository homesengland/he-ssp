using HE.InvestmentLoans.BusinessLogic._LoanApplication.Commands;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Queries;
using HE.InvestmentLoans.BusinessLogic.Application.Repositories;
using HE.InvestmentLoans.Common.Authorization;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xrm.Sdk;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication;

[TestClass]
public class CommandsTest : MediatorTestBase
{
    public override void AddAditionalServices(ServiceCollection collection)
    {
        var orgService = new Mock<IOrganizationService>();
        collection.AddTransient(x => new Mock<IOrganizationService>().Object);
        collection.AddTransient(x => new Mock<ILoanApplicationRepository>().Object);
        collection.AddTransient(x => new Mock<ILoanUserContext>().Object);

        base.AddAditionalServices(collection);
    }

    [TestMethod]
    public void DICheck()
    {
        var mediator = ServiceProvider.GetService(typeof(IMediator));
        Assert.IsNotNull(mediator);
    }

    [TestMethod]
    public async Task Create_Command_Execute_CheckAsync()
    {
        var mediator = (IMediator)ServiceProvider.GetService(typeof(IMediator));
        var model = await mediator.Send(new Create());
        Assert.IsNotNull(model);
    }

    [TestMethod]
    public async Task Create_Command_Execute_CheckIDAsync()
    {
        var mediator = (IMediator)ServiceProvider.GetService(typeof(IMediator));
        var model = await mediator.Send(new Create());
        Assert.IsNotNull(model);
        Assert.AreNotEqual(model.ID, Guid.Empty, model.ID.ToString());
    }

    [TestMethod]
    public async Task Update_Command_Execute_CheckIDAsync()
    {
        var mediator = (IMediator)ServiceProvider.GetService(typeof(IMediator));
        var model = await mediator.Send(new Create());
        Assert.IsNotNull(model);
        Assert.AreNotEqual(model.ID, Guid.Empty, model.ID.ToString());
        model.Purpose = Enums.FundingPurpose.Other;
        await mediator.Send(new Update() { Model = model });
        var modelToCheck = await mediator.Send(new GetSingle() { Id = model.ID });
        Assert.IsNotNull(modelToCheck);
        Assert.AreEqual(model.Purpose, modelToCheck.Purpose);
    }
}
