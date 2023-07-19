using HE.InvestmentLoans.BusinessLogic.Application.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Commands;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Workflow;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Contract.Application.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.PowerPlatform.Dataverse.Client;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication;

[TestClass]
public class FlowTest : MediatorTestBase
{
    public override void AddAditionalServices(ServiceCollection collection)
    {
        collection.AddTransient(x => new Mock<IOrganizationServiceAsync2>().Object);
        collection.AddTransient(x => new Mock<ILoanApplicationRepository>().Object);
        collection.AddTransient(x => new Mock<ILoanUserContext>().Object);
        base.AddAditionalServices(collection);
    }

    [TestMethod]
    [DataRow(LoanApplicationWorkflow.State.Index, LoanApplicationWorkflow.State.AboutLoan)]
    [DataRow(LoanApplicationWorkflow.State.AboutLoan, LoanApplicationWorkflow.State.CheckYourDetails)]
    [DataRow(LoanApplicationWorkflow.State.CheckYourDetails, LoanApplicationWorkflow.State.LoanPurpose)]
    [DataRow(LoanApplicationWorkflow.State.LoanPurpose, LoanApplicationWorkflow.State.TaskList)]
    [DataRow(LoanApplicationWorkflow.State.TaskList, LoanApplicationWorkflow.State.CheckAnswers)]
    [DataRow(LoanApplicationWorkflow.State.CheckAnswers, LoanApplicationWorkflow.State.ApplicationSubmitted)]
    public async Task Workflow_Continue_Test(LoanApplicationWorkflow.State begin, LoanApplicationWorkflow.State expcected)
    {
        var mediator = (IMediator)ServiceProvider.GetService(typeof(IMediator));

        var model = await mediator.Send(new Create());

        model.State = begin;
        model.Purpose = FundingPurpose.BuildingNewHomes;

        var workflow = new LoanApplicationWorkflow(model, mediator);

        workflow.NextState(Trigger.Continue);

        Assert.AreEqual(model.State, expcected);
    }

    [TestMethod]
    [DataRow(LoanApplicationWorkflow.State.AboutLoan, LoanApplicationWorkflow.State.Index)]
    [DataRow(LoanApplicationWorkflow.State.CheckYourDetails, LoanApplicationWorkflow.State.AboutLoan)]
    [DataRow(LoanApplicationWorkflow.State.LoanPurpose, LoanApplicationWorkflow.State.CheckYourDetails)]
    [DataRow(LoanApplicationWorkflow.State.TaskList, LoanApplicationWorkflow.State.LoanPurpose)]
    public async Task Workflow_Back_Test(LoanApplicationWorkflow.State begin, LoanApplicationWorkflow.State expcected)
    {
        var mediator = (IMediator)ServiceProvider.GetService(typeof(IMediator));
        var model = await mediator.Send(new Create());
        model.State = begin;
        model.Purpose = FundingPurpose.BuildingNewHomes;
        var workflow = new LoanApplicationWorkflow(model, mediator);
        workflow.NextState(Trigger.Back);
        Assert.AreEqual(model.State, expcected);
    }

    [TestMethod]
    [DataRow(LoanApplicationWorkflow.State.Index, LoanApplicationWorkflow.State.AboutLoan)]
    [DataRow(LoanApplicationWorkflow.State.AboutLoan, LoanApplicationWorkflow.State.CheckYourDetails)]
    [DataRow(LoanApplicationWorkflow.State.CheckYourDetails, LoanApplicationWorkflow.State.LoanPurpose)]
    [DataRow(LoanApplicationWorkflow.State.LoanPurpose, LoanApplicationWorkflow.State.Ineligible)]

    public async Task Workflow_Continue_Ineligible_Test(LoanApplicationWorkflow.State begin, LoanApplicationWorkflow.State expcected)
    {
        var mediator = (IMediator)ServiceProvider.GetService(typeof(IMediator));
        var model = await mediator.Send(new Create());
        model.State = begin;
        model.Purpose = FundingPurpose.Other;
        var workflow = new LoanApplicationWorkflow(model, mediator);
        workflow.NextState(Trigger.Continue);
        Assert.AreEqual(model.State, expcected);
    }

    [TestMethod]
    [DataRow(LoanApplicationWorkflow.State.AboutLoan, LoanApplicationWorkflow.State.Index)]
    [DataRow(LoanApplicationWorkflow.State.CheckYourDetails, LoanApplicationWorkflow.State.AboutLoan)]
    [DataRow(LoanApplicationWorkflow.State.LoanPurpose, LoanApplicationWorkflow.State.CheckYourDetails)]
    [DataRow(LoanApplicationWorkflow.State.Ineligible, LoanApplicationWorkflow.State.LoanPurpose)]
    public async Task Workflow_Back_Ineligible_Test(LoanApplicationWorkflow.State begin, LoanApplicationWorkflow.State expcected)
    {
        var mediator = (IMediator)ServiceProvider.GetService(typeof(IMediator));
        var model = await mediator.Send(new Create());
        model.State = begin;
        model.Purpose = FundingPurpose.Other;
        var workflow = new LoanApplicationWorkflow(model, mediator);
        workflow.NextState(Trigger.Back);
        Assert.AreEqual(model.State, expcected);
    }
}
