using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.CRM.AHP.Plugins.Handlers.HomeType;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Tests.Plugins.Handlers;

namespace HE.CRM.AHP.Plugins.Tests.Handlers.HomeType
{
    [TestClass]
    public class SetHappiPrinciplesValueHandlerTest : CrmEntityHandlerTestBase<invln_HomeType, SetHappiPrinciplesValueHandler, DataverseContext>
    {
        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();
        }

        [TestMethod]
        public void CanWork_Success()
        {
            Target = new invln_HomeType
            {
                Id = Guid.NewGuid(),
            };

            PreImage = new invln_HomeType
            {
                Id = Guid.NewGuid(),
            };

            Asset("Update", (int)StageEnum.PreOperation);

            var result = handler.CanWork();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoWork_Success()
        {
            var homeTypeId = Guid.NewGuid();

            var osvc = new OptionSetValueCollection
            {
                new OptionSetValue((int)invln_HAPPIprinciples.BBalconiesandoutdoorspace),
                new OptionSetValue((int)invln_HAPPIprinciples.KNone),
                new OptionSetValue((int)invln_HAPPIprinciples.FPlantstreesandthenaturalenvironment)
            };

            Target = new invln_HomeType
            {
                Id = homeTypeId,
                invln_happiprinciples = osvc
            };

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var a = Target.invln_happiprinciples;
            Assert.AreEqual(1, a.Count);
            Assert.AreEqual((int)invln_HAPPIprinciples.KNone, a.First().Value);
        }
    }
}
