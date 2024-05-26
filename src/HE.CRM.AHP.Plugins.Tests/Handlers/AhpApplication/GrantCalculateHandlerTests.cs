using System;
using System.Collections.Generic;
using System.Linq;
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

        [DataTestMethod]
        [DataRow(invln_scheme_StatusCode.Draft, invln_scheme_StatusCode.ApplicationSubmitted)]
        public void CanWork_ChangeApplicationStatus_Work(invln_scheme_StatusCode preStatus, invln_scheme_StatusCode postStatus)
        {
            var ahpApplicationPreImage = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                StatusCode = new OptionSetValue((int)preStatus)
            };

            var ahpApplicationTarget = ahpApplicationPreImage.Clone<invln_scheme>();
            ahpApplicationTarget.StatusCode = new OptionSetValue((int)postStatus);

            PreImage = ahpApplicationPreImage;
            Target = ahpApplicationTarget;

            fakedContext.Initialize(ahpApplicationTarget);

            Asset("Update", (int)StageEnum.PostOperation);

            // Act

            var canWork = handler.CanWork();

            // Assert

            Assert.AreEqual(true, canWork);
        }

        [DataTestMethod]
        [DataRow(invln_scheme_StatusCode.ApplicationSubmitted, invln_scheme_StatusCode.Approved)]
        [DataRow(invln_scheme_StatusCode.ApplicationSubmitted, invln_scheme_StatusCode.Deleted)]
        [DataRow(invln_scheme_StatusCode.ApplicationSubmitted, invln_scheme_StatusCode.Rejected)]
        public void CanWork_ChangeApplicationStatus_SkipWork(invln_scheme_StatusCode preStatus, invln_scheme_StatusCode postStatus)
        {
            var ahpApplicationPreImage = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                StatusCode = new OptionSetValue((int)preStatus)
            };

            var ahpApplicationTarget = ahpApplicationPreImage.Clone<invln_scheme>();
            ahpApplicationTarget.StatusCode = new OptionSetValue((int)postStatus);

            PreImage = ahpApplicationPreImage;
            Target = ahpApplicationTarget;

            fakedContext.Initialize(ahpApplicationTarget);

            Asset("Update", (int)StageEnum.PostOperation);

            // Act

            var canWork = handler.CanWork();

            // Assert

            Assert.AreEqual(false, canWork);
        }

        [TestMethod]
        [DataRow(invln_scheme_StatusCode.ApplicationSubmitted)]
        [DataRow(invln_scheme_StatusCode.Approved)]
        [DataRow(invln_scheme_StatusCode.OnHold)]
        public void CanWork_UpdateFundingRequiredForDifferentStatusesOnSubmittedApplication_Work(invln_scheme_StatusCode status)
        {
            var ahpApplicationPreImage = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                invln_fundingrequired = new Money(10000),
                StatusCode = new OptionSetValue((int)status)
            };

            var ahpApplicationTarget = ahpApplicationPreImage.Clone<invln_scheme>();
            ahpApplicationTarget.invln_fundingrequired = new Money(12000);
            var ahpApplicationPostImage = new invln_scheme()
            {
                Id = ahpApplicationTarget.Id,
                StatusCode = ahpApplicationTarget.StatusCode
            };

            PreImage = ahpApplicationPreImage;
            Target = ahpApplicationTarget;
            PostImage = ahpApplicationPostImage;

            fakedContext.Initialize(ahpApplicationTarget);

            Asset("Update", (int)StageEnum.PostOperation);

            // Act

            var canWork = handler.CanWork();

            // Assert

            Assert.AreEqual(true, canWork);
        }

        [TestMethod]
        [DataRow(invln_scheme_StatusCode.Inactive)]
        [DataRow(invln_scheme_StatusCode.Draft)]
        [DataRow(invln_scheme_StatusCode.Deleted)]
        public void CanWork_UpdateFundingRequiredForDifferentStatusesOnNotSubmittedApplication_SkipWork(invln_scheme_StatusCode status)
        {
            var ahpApplicationPreImage = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                invln_fundingrequired = new Money(10000),
                StatusCode = new OptionSetValue((int)status)
            };

            var ahpApplicationTarget = ahpApplicationPreImage.Clone<invln_scheme>();
            ahpApplicationTarget.invln_fundingrequired = new Money(12000);
            var ahpApplicationPostImage = new invln_scheme()
            {
                Id = ahpApplicationTarget.Id,
                StatusCode = ahpApplicationTarget.StatusCode
            };

            PreImage = ahpApplicationPreImage;
            Target = ahpApplicationTarget;
            PostImage = ahpApplicationPostImage;

            fakedContext.Initialize(ahpApplicationTarget);

            Asset("Update", (int)StageEnum.PostOperation);

            // Act

            var canWork = handler.CanWork();

            // Assert

            Assert.AreEqual(false, canWork);
        }

        [TestMethod]
        public void DoWork_AllCorrectData_UpdateGrantCalculationOnApplication()
        {
            // Arrange

            var eastofEnglandRegion = new OptionSetValue((int)he_GovernmentOfficeRegion.EastofEngland);
            var northEastRegion = new OptionSetValue((int)he_GovernmentOfficeRegion.NorthEast);

            var grantbenchmark1 = new invln_grantbenchmark()
            {
                Id = Guid.NewGuid(),
                invln_GovernmentOfficeRegion = eastofEnglandRegion,
                invln_tenure = new OptionSetValue((int)invln_Tenurechoice.Sharedownership),
                invln_BenchmarkTable = new OptionSetValue((int)invln_BenchmarkTable.Table5RegionalBenchmarkGrantPerUnit),
                invln_benchmarkgpu = new Money(46000)
            };

            var grantbenchmark2 = new invln_grantbenchmark()
            {
                Id = Guid.NewGuid(),
                invln_GovernmentOfficeRegion = eastofEnglandRegion,
                invln_tenure = new OptionSetValue((int)invln_Tenurechoice.Renttobuy),
                invln_BenchmarkTable = new OptionSetValue((int)invln_BenchmarkTable.Table5RegionalBenchmarkGrantPerUnit),
                invln_benchmarkgpu = new Money(57000)
            };

            var grantbenchmark3 = new invln_grantbenchmark()
            {
                Id = Guid.NewGuid(),
                invln_GovernmentOfficeRegion = eastofEnglandRegion,
                invln_tenure = new OptionSetValue((int)invln_Tenurechoice.Socialrent),
                invln_BenchmarkTable = new OptionSetValue((int)invln_BenchmarkTable.Table5RegionalBenchmarkGrantPerUnit),
                invln_benchmarkgpu = new Money(89000)
            };

            var grantbenchmark4 = new invln_grantbenchmark()
            {
                Id = Guid.NewGuid(),
                invln_GovernmentOfficeRegion = eastofEnglandRegion,
                invln_tenure = new OptionSetValue((int)invln_Tenurechoice.Specialistrent),
                invln_BenchmarkTable = new OptionSetValue((int)invln_BenchmarkTable.Table5RegionalBenchmarkGrantPerUnit),
                invln_benchmarkgpu = new Money(89000)
            };

            var grantbenchmark5 = new invln_grantbenchmark()
            {
                Id = Guid.NewGuid(),
                invln_GovernmentOfficeRegion = northEastRegion,
                invln_tenure = new OptionSetValue((int)invln_Tenurechoice.Extracare),
                invln_BenchmarkTable = new OptionSetValue((int)invln_BenchmarkTable.Table5RegionalBenchmarkGrantPerUnit),
                invln_benchmarkgpu = new Money(89000)
            };

            var site = new invln_Sites()
            {
                Id = Guid.NewGuid(),
                invln_sitename = "Test_Site1",
                invln_GovernmentOfficeRegion = eastofEnglandRegion
            };

            var ahpApplicationPreImage = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                invln_Site = site.ToEntityReference(),
                invln_fundingrequired = new Money(100000),
                invln_noofhomes = 4,
                invln_expectedacquisitioncost = new Money(150000),
                invln_expectedoncosts = new Money(120_000),
                invln_expectedonworks = new Money(100_000),
                invln_Tenure = new OptionSetValue((int)invln_Tenure.Sharedownership),
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            var homeType1 = new invln_HomeType()
            {
                Id = Guid.NewGuid(),
                invln_application = ahpApplicationPreImage.ToEntityReference(),
                invln_numberofhomeshometype = 85,
                invln_floorarea = 125,
                invln_typeofhousing = new OptionSetValue((int)invln_Typeofhousing.General)
            };

            var homeType2 = new invln_HomeType()
            {
                Id = Guid.NewGuid(),
                invln_application = ahpApplicationPreImage.ToEntityReference(),
                invln_numberofhomeshometype = 85,
                invln_floorarea = 125,
                invln_typeofhousing = new OptionSetValue((int)invln_Typeofhousing.Housingforolderpeople)
            };

            var homeType3 = new invln_HomeType()
            {
                Id = Guid.NewGuid(),
                invln_numberofhomeshometype = 11,
                invln_floorarea = 15,
                invln_typeofhousing = new OptionSetValue((int)invln_Typeofhousing.General)
            };

            var ahpApplicationPostImage = new invln_scheme()
            {
                Id = ahpApplicationPreImage.Id,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApplicationSubmitted)
            };

            var ahpApplicationTarget = ahpApplicationPreImage.Clone<invln_scheme>();
            ahpApplicationTarget.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApplicationSubmitted);

            PreImage = ahpApplicationPreImage;
            Target = ahpApplicationTarget;
            PostImage = ahpApplicationPostImage;

#pragma warning disable CS0618 // Type or member is obsolete
            fakedContext.Initialize(
                new List<Entity>()
                {
                    homeType1,
                    homeType2,
                    homeType3,
                    site,
                    ahpApplicationTarget,
                    grantbenchmark1,
                    grantbenchmark2,
                    grantbenchmark3,
                    grantbenchmark4,
                    grantbenchmark5
                }
            );
#pragma warning restore CS0618 // Type or member is obsolete

            Asset("Update", (int)StageEnum.PostOperation);

            // Act

            handler.DoWork();

            var applicationResult = fakedContext.CreateQuery<invln_scheme>()
                .Single(x => x.Id == ahpApplicationTarget.Id);

            var homesTypesResult = fakedContext.CreateQuery<invln_HomeType>().ToList();

            // Assert

            Assert.AreEqual(25_000, applicationResult.invln_grantperunit.Value);
            Assert.AreEqual((decimal)27.03, applicationResult.invln_grantasaoftotalschemecosts.Value, (decimal)0.01);
            Assert.AreEqual(46_000, applicationResult.invln_RegionalBenchmarkGrantPerUnit.Value);
            Assert.AreEqual((decimal)54.35, applicationResult.invln_regionalbenchmarkagainstthegrantperunit.Value, (decimal)0.01);
            Assert.AreEqual((decimal)4.71, applicationResult.invln_WorkssCostsm2.Value, (decimal)0.01);
        }
    }
}
