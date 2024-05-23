using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using FakeXrmEasy.Extensions;
using HE.CRM.AHP.Plugins.Handlers.AHPApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Tests.Plugins.Handlers;

namespace HE.CRM.AHP.Plugins.Tests.Handlers.AhpApplication
{
    [TestClass]
    public class GrantCalculateHandlerTests : CrmEntityHandlerTestBase<invln_scheme, GrantCalculateHandler, DataverseContext>
    {
        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();
        }

        [TestMethod]
        public void CanWork_SubmitApplication_Work()
        {
            var ahpApplicationPreImage = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            var ahpApplicationTarget = ahpApplicationPreImage.Clone<invln_scheme>();
            ahpApplicationTarget.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApplicationSubmitted);

            PreImage = ahpApplicationPreImage;
            Target = ahpApplicationTarget;

            fakedContext.Initialize(ahpApplicationTarget);

            Asset("Update", (int)StageEnum.PreOperation);

            // Act

            var canWork = handler.CanWork();

            // Assert

            Assert.AreEqual(true, canWork);
        }

        [TestMethod]
        public void CanWork_RejectApplication_SkipWork()
        {
            var ahpApplicationPreImage = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApplicationSubmitted)
            };

            var ahpApplicationTarget = ahpApplicationPreImage.Clone<invln_scheme>();
            ahpApplicationTarget.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Rejected);

            PreImage = ahpApplicationPreImage;
            Target = ahpApplicationTarget;

            fakedContext.Initialize(ahpApplicationTarget);

            Asset("Update", (int)StageEnum.PreOperation);

            // Act

            var canWork = handler.CanWork();

            // Assert

            Assert.AreEqual(false, canWork);
        }

        [TestMethod]
        public void CanWork_UpdateFundingRequiredOnDraftApplication_Work()
        {
            var ahpApplicationPreImage = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                invln_fundingrequired = new Money(10000),
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            var ahpApplicationTarget = ahpApplicationPreImage.Clone<invln_scheme>();
            ahpApplicationTarget.invln_fundingrequired = new Money(12000);

            PreImage = ahpApplicationPreImage;
            Target = ahpApplicationTarget;

            fakedContext.Initialize(ahpApplicationTarget);

            Asset("Update", (int)StageEnum.PreOperation);

            // Act

            var canWork = handler.CanWork();

            // Assert

            Assert.AreEqual(false, canWork);
        }

        [TestMethod]
        public void CanWork_UpdateFundingRequiredOnSubmitedApplication_Work()
        {
            var ahpApplicationPreImage = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                invln_fundingrequired = new Money(10000),
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApplicationSubmitted)
            };

            var ahpApplicationTarget = ahpApplicationPreImage.Clone<invln_scheme>();
            ahpApplicationTarget.invln_fundingrequired = new Money(12000);

            PreImage = ahpApplicationPreImage;
            Target = ahpApplicationTarget;

            fakedContext.Initialize(ahpApplicationTarget);

            Asset("Update", (int)StageEnum.PreOperation);

            // Act

            var canWork = handler.CanWork();

            // Assert

            Assert.AreEqual(true, canWork);
        }

        [TestMethod]
        public void DoWork_AllCorrectData_GrantCalculateSuccess()
        {
            // Arrange

            var eastofEnglandRegion = new OptionSetValue((int)he_GovernmentOfficeRegion.EastofEngland);
            var northEastRegion = new OptionSetValue((int)he_GovernmentOfficeRegion.NorthEast);

            var tenureAffordablerent = new OptionSetValue((int)invln_Tenure.Affordablerent);
            var tenureSocialrent = new OptionSetValue((int)invln_Tenure.Socialrent);

            var grantbenchmark1 = new invln_grantbenchmark()
            {
                Id = Guid.NewGuid(),
                invln_GovernmentOfficeRegion = eastofEnglandRegion,
                invln_tenure = tenureAffordablerent,
                invln_BenchmarkTable = new OptionSetValue((int)invln_BenchmarkTable.Table5RegionalBenchmarkGrantPerUnit),
                invln_benchmarkgpu = new Money(57000)
            };

            var grantbenchmark2 = new invln_grantbenchmark()
            {
                Id = Guid.NewGuid(),
                invln_GovernmentOfficeRegion = eastofEnglandRegion,
                invln_tenure = tenureSocialrent,
                invln_BenchmarkTable = new OptionSetValue((int)invln_BenchmarkTable.Table5RegionalBenchmarkGrantPerUnit),
                invln_benchmarkgpu = new Money(89000)
            };

            var grantbenchmark3 = new invln_grantbenchmark()
            {
                Id = Guid.NewGuid(),
                invln_GovernmentOfficeRegion = northEastRegion,
                invln_tenure = tenureSocialrent,
                invln_BenchmarkTable = new OptionSetValue((int)invln_BenchmarkTable.Table5RegionalBenchmarkGrantPerUnit),
                invln_benchmarkgpu = new Money(89000)
            };

            var site = new invln_Sites()
            {
                Id = Guid.NewGuid(),
                invln_sitename = "Test_Site1",
                invln_GovernmentOfficeRegion = eastofEnglandRegion
            };

            var ahpApplication = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                invln_Site = site.ToEntityReference(),
                invln_fundingrequired = new Money(100000),
                invln_noofhomes = 4,
                invln_expectedacquisitioncost = new Money(150000),
                invln_oncosts = new Money(1000),
                invln_workscosts = new Money(1000),
                invln_Tenure = tenureAffordablerent
            };

            var homeType1 = new invln_HomeType()
            {
                Id = Guid.NewGuid(),
                invln_application = ahpApplication.ToEntityReference(),
                invln_numberofhomeshometype = 85,
                invln_floorarea = 125
            };

            var homeType2 = new invln_HomeType()
            {
                Id = Guid.NewGuid(),
                invln_application = ahpApplication.ToEntityReference(),
                invln_numberofhomeshometype = 85,
                invln_floorarea = 125
            };

            var homeType3 = new invln_HomeType()
            {
                Id = Guid.NewGuid(),
                invln_numberofhomeshometype = 11,
                invln_floorarea = 15
            };

            Target = ahpApplication;

            fakedContext.Initialize(
                new List<Entity>()
                {
                    homeType1,
                    homeType2,
                    homeType3,
                    site,
                    ahpApplication,
                    grantbenchmark1,
                    grantbenchmark2,
                    grantbenchmark3
                }
            );

            Asset("Update", (int)StageEnum.PreOperation);

            // Act

            handler.DoWork();

            var applicationResult = fakedContext.CreateQuery<invln_scheme>()
                .Single(x => x.Id == ahpApplication.Id);

            var homesTypesResult = fakedContext.CreateQuery<invln_HomeType>().ToList();

            // Assert

            Assert.AreEqual(25000, applicationResult.invln_grantperunit.Value);
            Assert.AreEqual((decimal)65.79, applicationResult.invln_grantasapercentageoftotalschemecosts.Value, (decimal)0.01);
            Assert.AreEqual((decimal)43.86, applicationResult.invln_regionalbenchmarkagainstthegrantperunit.Value, (decimal)0.01);
            Assert.AreEqual(21250, applicationResult.invln_WorkssCostsm2.Value);
        }
    }
}
