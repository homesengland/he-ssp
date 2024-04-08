using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.CRM.AHP.Plugins.Handlers.HomeType;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Tests.Plugins.Handlers;
using HE.Base.Common.Extensions;

namespace HE.CRM.AHP.Plugins.Tests.Handlers.HomeType
{
    [TestClass]
    public class CreateDocumentLocationOnCreateHandlerTests : CrmEntityHandlerTestBase<invln_HomeType, CreateDocumentLocationOnCreateHandler, DataverseContext>
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
            var applicationId = Guid.NewGuid();
            var homeTypeLocationId = Guid.NewGuid();
            var parentApplicationId = Guid.NewGuid();

            fakedContext.Initialize(new List<Entity>()
            {
                new invln_HomeType() { Id = homeTypeId},
                new SharePointDocumentLocation() { Id = parentApplicationId, RegardingObjectId = applicationId.ToEntityReference<invln_HomeType>()},
                new SharePointDocumentLocation() { Id = homeTypeLocationId , ParentSiteOrLocation = parentApplicationId.ToEntityReference<SharePointDocumentLocation>()}
            });

            Target = new invln_HomeType
            {
                Id = homeTypeId,
                invln_application = applicationId.ToEntityReference<SharePointDocumentLocation>()
            };

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();

            var documentLocation = fakedContext.CreateQuery<SharePointDocumentLocation>().Where(x => x.Name == "Home Type For Ahp Application");

            Assert.AreEqual(1, documentLocation.Count());
            ;
        }
    }
}
