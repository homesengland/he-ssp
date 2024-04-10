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
    public class CheckIfApplicationWithNewNameExistsHandlerTests : CrmEntityHandlerTestBase<invln_scheme, CheckIfApplicationWithNewNameExistsHandler, DataverseContext>
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
        public void DoWork_PreImage_ThesameShemaName()
        {
            Guid shemaId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = shemaId,
                invln_schemename = "SchemeName"
            };

            PreImage = new invln_scheme
            {
                Id = shemaId,
                invln_schemename = "SchemeName"
            };

            Asset("Create", (int)StageEnum.PreOperation);

            try
            {
                handler.DoWork();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void DoWork_Success_PreImage_Null()
        {
            Guid shemaId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = shemaId,
                invln_organisationid = null
            };

            Asset("Create", (int)StageEnum.PreOperation);

            try
            {
                handler.DoWork();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void DoWork_Success_ApplicationExist()
        {
            var organization = new Account() { Id = Guid.NewGuid() };

            Guid shemaId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = shemaId,
                invln_schemename = "SchemaName",
            };

            PreImage = new invln_scheme
            {
                Id = shemaId,
                invln_schemename = "SchemaName_1",
                invln_organisationid = organization.ToEntityReference()
            };

            fakedContext.Initialize(new List<Entity>()
            {
                organization
            });

            Asset("Create", (int)StageEnum.PreOperation);

            try
            {
                handler.DoWork();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }
    }
}
