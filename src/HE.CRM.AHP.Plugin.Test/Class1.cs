using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.CRM.Plugins.Handlers.LoanApplications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.Base.Tests.Plugins.Handlers;

namespace HE.CRM.AHP.Plugin.Test
{
    [TestClass]
    public class ChangeExternalStatusOnInternalStatusChangeHandlerTests : CrmEntityHandlerTestBase<invln_scheme, ChangeExternalStatusOnInternalStatusChangeHandler, DataverseContext>
    {
        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();
        }
    }
}
