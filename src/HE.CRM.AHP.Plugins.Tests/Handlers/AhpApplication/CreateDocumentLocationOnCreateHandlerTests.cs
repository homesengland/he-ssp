using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.CRM.AHP.Plugins.Handlers.AHPApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Tests.Plugins.Handlers;

namespace HE.CRM.AHP.Plugins.Tests.Handlers.AhpApplication
{
    [TestClass]
    public class CreateDocumentLocationOnCreateHandlerTests : CrmEntityHandlerTestBase<invln_scheme, CreateDocumentLocationOnCreateHandler, DataverseContext>
    {
        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();
        }

        [TestMethod]
        public void CanWork_Success()
        {
            Target = new invln_scheme
            {
                Id = Guid.NewGuid(),
            };
            Asset("Create", (int)StageEnum.PreOperation);

            var result = handler.CanWork();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Dowork_Success()
        {
            Guid invln_schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = invln_schemeId,
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new SharePointDocumentLocation()
                {
                    Id = Guid.NewGuid(),
                    Name = "AHP Application Documents"
                },
                new invln_scheme()
                {
                    Id = invln_schemeId
                }
            });

            Asset("Create", (int)StageEnum.PreOperation);
            handler.DoWork();
            var spdl1 = fakedContext.CreateQuery(SharePointDocumentLocation.EntityLogicalName).Select(e => e.ToEntity<SharePointDocumentLocation>()).Where(s => s.Name == "Documents on AHP Application" && s.RegardingObjectId != null).ToList().FirstOrDefault();
            var spdlHt = fakedContext.CreateQuery(SharePointDocumentLocation.EntityLogicalName).Select(e => e.ToEntity<SharePointDocumentLocation>()).Where(s => s.Name == "Home Types").First();
            Assert.IsNotNull(spdl1);
            Assert.IsNotNull(spdlHt);
        }

        [TestMethod]
        public void Dowork_Fail()
        {
            Guid invln_schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = invln_schemeId,
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new SharePointDocumentLocation() { Id = Guid.NewGuid(), Name = "AHP Application Document"},
                new invln_scheme() { Id = invln_schemeId}
            });

            Asset("Create", (int)StageEnum.PreOperation);
            try
            {
                handler.DoWork();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Document Location record for AHP Application does not exists", e.Message);
            }
        }
    }
}
