using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Commands;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Routing;
using MediatR;
using Moq;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding;

public class FlowTests : MediatorTestBase
{
    [Theory]
    [InlineData(FundingWorkflow.State.Index, FundingWorkflow.State.GDV)]
    [InlineData(FundingWorkflow.State.GDV, FundingWorkflow.State.TotalCosts)]
    [InlineData(FundingWorkflow.State.TotalCosts, FundingWorkflow.State.AbnormalCosts)]
    [InlineData(FundingWorkflow.State.AbnormalCosts, FundingWorkflow.State.PrivateSectorFunding)]
    [InlineData(FundingWorkflow.State.PrivateSectorFunding, FundingWorkflow.State.Refinance)]
    [InlineData(FundingWorkflow.State.Refinance, FundingWorkflow.State.AdditionalProjects)]
    [InlineData(FundingWorkflow.State.AdditionalProjects, FundingWorkflow.State.CheckAnswers)]
    public async Task Workflow_Continue_Test(FundingWorkflow.State begin, FundingWorkflow.State expected)
    {
        var mediator = (IMediator)ServiceProvider.GetService(typeof(IMediator));

        var model = await mediator.Send(new Create());

        model.Funding.State = begin;

        var workflow = new FundingWorkflow(model, mediator);

        workflow.NextState(Trigger.Continue);

        Assert.AreEqual(model.Funding.State, expected);
    }

    [Fact]
    public async Task Complete_workflow_when_answers_are_confirmed()
    {
        var mediator = (IMediator)ServiceProvider.GetService(typeof(IMediator));

        var model = await mediator.Send(new Create());

        model.Funding.State = FundingWorkflow.State.CheckAnswers;
        model.Funding.CheckAnswers = "Yes";

        var workflow = new FundingWorkflow(model, mediator);

        workflow.NextState(Trigger.Continue);

        Assert.AreEqual(model.Funding.State, FundingWorkflow.State.Complete);
    }

    [Fact]
    public async Task Do_not_change_workflow_state_when_answers_are_not_confirmed()
    {
        var mediator = (IMediator)ServiceProvider.GetService(typeof(IMediator));

        var model = await mediator.Send(new Create());

        model.Funding.State = FundingWorkflow.State.CheckAnswers;
        model.Funding.CheckAnswers = "No";

        var workflow = new FundingWorkflow(model, mediator);

        workflow.NextState(Trigger.Continue);

        Assert.AreEqual(model.Funding.State, FundingWorkflow.State.CheckAnswers);
    }

    [Theory]
    [InlineData(FundingWorkflow.State.GDV, FundingWorkflow.State.Index)]
    [InlineData(FundingWorkflow.State.TotalCosts, FundingWorkflow.State.GDV)]
    [InlineData(FundingWorkflow.State.AbnormalCosts, FundingWorkflow.State.TotalCosts)]
    [InlineData(FundingWorkflow.State.PrivateSectorFunding, FundingWorkflow.State.AbnormalCosts)]
    [InlineData(FundingWorkflow.State.Refinance, FundingWorkflow.State.PrivateSectorFunding)]
    [InlineData(FundingWorkflow.State.AdditionalProjects, FundingWorkflow.State.Refinance)]
    [InlineData(FundingWorkflow.State.CheckAnswers, FundingWorkflow.State.AdditionalProjects)]
    [InlineData(FundingWorkflow.State.Complete, FundingWorkflow.State.CheckAnswers)]
    public async Task Workflow_Back_Test(FundingWorkflow.State begin, FundingWorkflow.State expected)
    {
        var mediator = (IMediator)ServiceProvider.GetService(typeof(IMediator));

        var model = await mediator.Send(new Create());

        model.Funding.State = begin;

        var workflow = new FundingWorkflow(model, mediator);

        workflow.NextState(Trigger.Back);

        Assert.AreEqual(model.Funding.State, expected);
    }

    [Theory]
    [InlineData(FundingWorkflow.State.GDV)]
    [InlineData(FundingWorkflow.State.TotalCosts)]
    [InlineData(FundingWorkflow.State.AbnormalCosts)]
    [InlineData(FundingWorkflow.State.PrivateSectorFunding)]
    [InlineData(FundingWorkflow.State.Refinance)]
    [InlineData(FundingWorkflow.State.AdditionalProjects)]
    public async Task Go_back_to_check_answers_on_change(FundingWorkflow.State begin)
    {
        var mediator = (IMediator)ServiceProvider.GetService(typeof(IMediator));

        var model = await mediator.Send(new Create());

        model.Funding.State = begin;

        var workflow = new FundingWorkflow(model, mediator);

        workflow.NextState(Trigger.Change);

        Assert.AreEqual(model.Funding.State, FundingWorkflow.State.CheckAnswers);
    }

    [Theory]
    [InlineData(FundingWorkflow.State.Index)]
    [InlineData(FundingWorkflow.State.GDV)]
    [InlineData(FundingWorkflow.State.TotalCosts)]
    [InlineData(FundingWorkflow.State.AbnormalCosts)]
    [InlineData(FundingWorkflow.State.PrivateSectorFunding)]
    [InlineData(FundingWorkflow.State.Refinance)]
    [InlineData(FundingWorkflow.State.AdditionalProjects)]
    public void Send_update_on_continue(FundingWorkflow.State initialState)
    {
        var mediatorMock = new Mock<IMediator>();

        var model = new LoanApplicationViewModel
        {
            Funding = new FundingViewModel
            {
                State = initialState,
            },
        };

        var workflow = new FundingWorkflow(model, mediatorMock.Object);

        workflow.NextState(Trigger.Continue);

        mediatorMock.Verify(c => c.Send(It.IsAny<Update>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(FundingWorkflow.State.GDV)]
    [InlineData(FundingWorkflow.State.TotalCosts)]
    [InlineData(FundingWorkflow.State.AbnormalCosts)]
    [InlineData(FundingWorkflow.State.PrivateSectorFunding)]
    [InlineData(FundingWorkflow.State.Refinance)]
    [InlineData(FundingWorkflow.State.AdditionalProjects)]
    public void Send_update_on_back(FundingWorkflow.State initialState)
    {
        var mediatorMock = new Mock<IMediator>();

        var model = new LoanApplicationViewModel
        {
            Funding = new FundingViewModel
            {
                State = initialState,
            },
        };

        var workflow = new FundingWorkflow(model, mediatorMock.Object);

        workflow.NextState(Trigger.Back);

        mediatorMock.Verify(c => c.Send(It.IsAny<Update>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(FundingWorkflow.State.GDV)]
    [InlineData(FundingWorkflow.State.TotalCosts)]
    [InlineData(FundingWorkflow.State.AbnormalCosts)]
    [InlineData(FundingWorkflow.State.PrivateSectorFunding)]
    [InlineData(FundingWorkflow.State.Refinance)]
    [InlineData(FundingWorkflow.State.AdditionalProjects)]
    public void Send_update_on_change(FundingWorkflow.State initialState)
    {
        var mediatorMock = new Mock<IMediator>();

        var model = new LoanApplicationViewModel
        {
            Funding = new FundingViewModel
            {
                State = initialState,
            },
        };

        var workflow = new FundingWorkflow(model, mediatorMock.Object);

        workflow.NextState(Trigger.Change);

        mediatorMock.Verify(c => c.Send(It.IsAny<Update>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
