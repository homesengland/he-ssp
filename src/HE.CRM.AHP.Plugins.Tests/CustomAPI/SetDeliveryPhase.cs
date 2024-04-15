using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using FakeXrmEasy;
using HE.Base.Common.Extensions;
using HE.CRM.AHP.Plugins.Plugins.CustomApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Tests.CustomAPI
{
    [TestClass]
    public class SetDeliveryPhase
    {
        private XrmFakedContext fakedContext;

        private XrmFakedPluginExecutionContext pluginContext;

        [TestInitialize]
        public void Initialize()
        {
            fakedContext = new XrmFakedContext();
            pluginContext = fakedContext.GetDefaultPluginContext();
        }

        [TestMethod]
        public void Test_1()
        {

            var invitedContactId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var inviterContactId = Guid.NewGuid();
            var systemUserId = Guid.NewGuid();

            fakedContext.Initialize(new List<Entity>()
            {

            });
            pluginContext.InputParameters = new ParameterCollection
            {
            };
            fakedContext.ExecutePluginWithConfigurations<SetDeliveryPhasePlugin>(pluginContext, "", "");
        }
    }
}
