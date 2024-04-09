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
    public class SetWhichNdssStandardsHaveBeenMetValueHandlerTests : CrmEntityHandlerTestBase<invln_HomeType, SetWhichNdssStandardsHaveBeenMetValueHandler, DataverseContext>
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
            var osvc = new OptionSetValueCollection
            {
                new OptionSetValue((int)invln_WhichNDSSstandardshavebeenmet.Bedroomareas),
                new OptionSetValue((int)invln_WhichNDSSstandardshavebeenmet.Bedroomwidths),
                new OptionSetValue((int)invln_WhichNDSSstandardshavebeenmet.Builtinstoragespacesize),
                new OptionSetValue((int)invln_WhichNDSSstandardshavebeenmet.Noneofthese),
            };

            Target = new invln_HomeType
            {
                Id = Guid.NewGuid(),
                invln_whichndssstandardshavebeenmet = osvc
            };

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var a = Target.invln_whichndssstandardshavebeenmet;
            Assert.AreEqual(1, a.Count);
            Assert.AreEqual((int)invln_WhichNDSSstandardshavebeenmet.Noneofthese, a.First().Value);
        }
    }
}
