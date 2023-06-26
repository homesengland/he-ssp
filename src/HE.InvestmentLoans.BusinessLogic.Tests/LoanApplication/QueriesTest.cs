using HE.InvestmentLoans.BusinessLogic._LoanApplication.Commands;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication
{
    [TestClass]
    public class QueriesTest: MediatorTestBase
    {
        

        [TestMethod]
        public async Task Select_Command()
        {
            var mediator = (IMediator)serviceProvider.GetService(typeof(IMediator));
            var model = await mediator.Send(new Create());
            var modelToCheck = await mediator.Send(new GetSingle() { Id= model.ID });
            Assert.AreEqual(model.ID, modelToCheck.ID);


        }



    }
}
