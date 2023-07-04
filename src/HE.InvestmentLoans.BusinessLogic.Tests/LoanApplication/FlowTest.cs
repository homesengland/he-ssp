using HE.InvestmentLoans.BusinessLogic._LoanApplication.Commands;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Queries;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xrm.Sdk;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication
{
    [TestClass]
    public class FlowTest : MediatorTestBase
    {
        public override void AddAditionalServices(ServiceCollection collection)
        {
            var orgService = new Mock<IOrganizationService>();
            collection.AddTransient<IOrganizationService>(x => (new Mock<IOrganizationService>()).Object);
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
            var mediator = (IMediator)serviceProvider.GetService(typeof(IMediator));

            var model = await mediator.Send(new Create());

            model.State = begin;
            model.Purpose = Enums.FundingPurpose.BuildingNewHomes;

            var workflow = new LoanApplicationWorkflow(model, mediator);

            workflow.NextState(Routing.Trigger.Continue);

            Assert.AreEqual(model.State, expcected);
        }


        [TestMethod]
        [DataRow(LoanApplicationWorkflow.State.AboutLoan, LoanApplicationWorkflow.State.Index)]
        [DataRow(LoanApplicationWorkflow.State.CheckYourDetails, LoanApplicationWorkflow.State.AboutLoan)]
        [DataRow(LoanApplicationWorkflow.State.LoanPurpose, LoanApplicationWorkflow.State.CheckYourDetails)]
        [DataRow(LoanApplicationWorkflow.State.TaskList, LoanApplicationWorkflow.State.LoanPurpose)]
        public async Task Workflow_Back_Test(LoanApplicationWorkflow.State begin, LoanApplicationWorkflow.State expcected)
        {
            var mediator = (IMediator)serviceProvider.GetService(typeof(IMediator));
            var model = await mediator.Send(new Create());
            model.State = begin;
            model.Purpose = Enums.FundingPurpose.BuildingNewHomes;
            LoanApplicationWorkflow workflow = new LoanApplicationWorkflow(model, mediator);
            workflow.NextState(Routing.Trigger.Back);
            Assert.AreEqual(model.State, expcected);
        }

        [TestMethod]
        [DataRow(LoanApplicationWorkflow.State.Index, LoanApplicationWorkflow.State.AboutLoan)]
        [DataRow(LoanApplicationWorkflow.State.AboutLoan, LoanApplicationWorkflow.State.CheckYourDetails)]
        [DataRow(LoanApplicationWorkflow.State.CheckYourDetails, LoanApplicationWorkflow.State.LoanPurpose)]
        [DataRow(LoanApplicationWorkflow.State.LoanPurpose, LoanApplicationWorkflow.State.Ineligible)]

        public async Task Workflow_Continue_Ineligible_Test(LoanApplicationWorkflow.State begin, LoanApplicationWorkflow.State expcected)
        {
            var mediator = (IMediator)serviceProvider.GetService(typeof(IMediator));
            var model = await mediator.Send(new Create());
            model.State = begin;
            model.Purpose = Enums.FundingPurpose.Other;
            LoanApplicationWorkflow workflow = new LoanApplicationWorkflow(model, mediator);
            workflow.NextState(Routing.Trigger.Continue);
            Assert.AreEqual(model.State, expcected);
        }


        [TestMethod]
        [DataRow(LoanApplicationWorkflow.State.AboutLoan, LoanApplicationWorkflow.State.Index)]
        [DataRow(LoanApplicationWorkflow.State.CheckYourDetails, LoanApplicationWorkflow.State.AboutLoan)]
        [DataRow(LoanApplicationWorkflow.State.LoanPurpose, LoanApplicationWorkflow.State.CheckYourDetails)]
        [DataRow(LoanApplicationWorkflow.State.Ineligible, LoanApplicationWorkflow.State.LoanPurpose)]
        public async Task Workflow_Back_Ineligible_Test(LoanApplicationWorkflow.State begin, LoanApplicationWorkflow.State expcected)
        {
            var mediator = (IMediator)serviceProvider.GetService(typeof(IMediator));
            var model = await mediator.Send(new Create());
            model.State = begin;
            model.Purpose = Enums.FundingPurpose.Other;
            LoanApplicationWorkflow workflow = new LoanApplicationWorkflow(model, mediator);
            workflow.NextState(Routing.Trigger.Back);
            Assert.AreEqual(model.State, expcected);
        }


    }
}
