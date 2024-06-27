using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using FakeXrmEasy.Extensions;
using HE.Base.Common.Extensions;
using HE.CRM.AHP.Plugins.Handlers.AHPApplication;
using HE.CRM.AHP.Plugins.Tests.Helpers;
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
        [DataRow(invln_scheme_StatusCode.ApplicationSubmitted, invln_scheme_StatusCode.ReferredBackToApplicant)]
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
        [DataRow(invln_scheme_StatusCode.ReferredBackToApplicant)]
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
        public void DoWork_HappyPath1_ApplicationUpdated()
        {
            // Arrange

            var grantbenchmarkT5EastSharedownership = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Sharedownership, 46_000);

            var grantbenchmarkT5EastRenttobuy = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Renttobuy, 57_000);

            var grantbenchmarkT5EastSocialrent = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Socialrent, 89_000);

            var grantbenchmarkT5EastSpecialistrent = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Specialistrent, 89_000);

            var grantbenchmark5NorthEastExtracare = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.NorthEast, invln_Tenurechoice.Extracare, 89_000);

            var grantbenchmarkT1EastSharedownership = GrantBenchmarkHelper.CreateGrantBenchmarkTable1(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Sharedownership, 19.5m);
            var grantbenchmarkT2EastSharedownership = GrantBenchmarkHelper.CreateGrantBenchmarkTable2(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Sharedownership, 2_000);
            var grantbenchmarkT3EastSharedownership = GrantBenchmarkHelper.CreateGrantBenchmarkTable3(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Sharedownership, 48_048);
            var grantbenchmarkT4EastSharedownership = GrantBenchmarkHelper.CreateGrantBenchmarkTable4(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Sharedownership, 43_680);

            var site = new invln_Sites()
            {
                Id = Guid.NewGuid(),
                invln_sitename = "Test_Site1",
                invln_GovernmentOfficeRegion = new OptionSetValue((int)he_GovernmentOfficeRegion.EastofEngland)
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

            var homeType1 = HomeTypeHelper.CreateHomeType(ahpApplicationPreImage, 85, 125, invln_Typeofhousing.General, null, null);

            var homeType2 = HomeTypeHelper.CreateHomeType(ahpApplicationPreImage, 85, 125, invln_Typeofhousing.Housingforolderpeople, invln_typeofolderpeopleshousing.Housingforolderpeoplewithallspecialdesignfeatures, null);

            var homeType3 = HomeTypeHelper.CreateHomeType(null, 11, 15, invln_Typeofhousing.General, null, null);

            var ahpApplicationTarget = new invln_scheme()
            {
                Id = ahpApplicationPreImage.Id,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApplicationSubmitted)
            };

            var ahpApplicationPostImage = ahpApplicationPreImage.Merge(ahpApplicationTarget);

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
                    ahpApplicationPostImage,
                    grantbenchmarkT5EastSharedownership,
                    grantbenchmarkT5EastRenttobuy,
                    grantbenchmarkT5EastSocialrent,
                    grantbenchmarkT5EastSpecialistrent,
                    grantbenchmark5NorthEastExtracare,
                    grantbenchmarkT1EastSharedownership,
                    grantbenchmarkT2EastSharedownership,
                    grantbenchmarkT3EastSharedownership,
                    grantbenchmarkT4EastSharedownership
                }
            );
#pragma warning restore CS0618 // Type or member is obsolete

            Asset("Update", (int)StageEnum.PostOperation);

            // Act

            handler.DoWork();

            var applicationResult = fakedContext.CreateQuery<invln_scheme>()
                .Single(x => x.Id == ahpApplicationTarget.Id);

            // Assert

            Assert.AreEqual(25_000, applicationResult.invln_grantperunit.Value);
            Assert.AreEqual(27.03m, applicationResult.invln_grantasaoftotalschemecosts.Value, 0.01m);
            Assert.AreEqual(46_000, applicationResult.invln_RegionalBenchmarkGrantPerUnit.Value);
            Assert.AreEqual(54.35m, applicationResult.invln_regionalbenchmarkagainstthegrantperunit.Value, 0.01m);
            Assert.AreEqual(4.71m, applicationResult.invln_WorkssCostsm2.Value, 0.01m);

            Assert.AreEqual(138m, applicationResult.invln_grantasapercentageoftotalschemecosts.Value, 1m);
            Assert.AreEqual(0.23m, applicationResult.invln_worksm2asapercentageofareaavg.Value, 0.01m);
            Assert.AreEqual(null, applicationResult.invln_gpuaspercentageofareaaverage);
            Assert.AreEqual(58m, applicationResult.invln_supportedgpuaspercentageofareaaverage.Value, 1m);
        }

        [TestMethod]
        [Ignore]
        public void DoWork_HappyPath2_ApplicationUpdated()
        {
            // Arrange

            var grantbenchmarkT5EastSharedownership = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Affordablerent, 57_000);
            var grantbenchmarkT5EastRenttobuy = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Renttobuy, 57_000);
            var grantbenchmarkT5EastSocialrent = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Socialrent, 89_000);

            var grantbenchmark5NorthEastExtracare = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.NorthEast, invln_Tenurechoice.Extracare, 89_000);

            var grantbenchmarkT1EastSpecialistrent = GrantBenchmarkHelper.CreateGrantBenchmarkTable1(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Specialistrent, 26m);
            var grantbenchmarkT2EastSpecialistrent = GrantBenchmarkHelper.CreateGrantBenchmarkTable2(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Specialistrent, 2_800);
            var grantbenchmarkT3EastSpecialistrent = GrantBenchmarkHelper.CreateGrantBenchmarkTable3(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Specialistrent, 67_760);
            var grantbenchmarkT4EastSpecialistrent = GrantBenchmarkHelper.CreateGrantBenchmarkTable4(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Specialistrent, 61_600);
            var grantbenchmarkT5EastSpecialistrent = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Specialistrent, 89_000);

            var site = new invln_Sites()
            {
                Id = Guid.NewGuid(),
                invln_sitename = "Test_Site1",
                invln_GovernmentOfficeRegion = new OptionSetValue((int)he_GovernmentOfficeRegion.EastofEngland)
            };

            var ahpApplicationPreImage = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                invln_Site = site.ToEntityReference(),
                invln_Rural = true,
                invln_fundingrequired = new Money(100000),
                invln_noofhomes = 4,
                invln_expectedacquisitioncost = new Money(150000),
                invln_expectedoncosts = new Money(120_000),
                invln_expectedonworks = new Money(100_000),
                invln_Tenure = new OptionSetValue((int)invln_Tenure.Affordablerent),
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            var homeType1 = HomeTypeHelper.CreateHomeType(ahpApplicationPreImage, 85, 125, invln_Typeofhousing.General, null, null);

            var homeType2 = HomeTypeHelper.CreateHomeType(ahpApplicationPreImage, 85, 125, invln_Typeofhousing.Housingfordisabledandvulnerablepeople, null, invln_typeofhousingfordisabledvulnerable.Purposedesignedhousingfordisabledandvulnerablepeoplewithaccesstosupport);

            var homeType3 = HomeTypeHelper.CreateHomeType(null, 11, 15, invln_Typeofhousing.General, null, null);

            var ahpApplicationTarget = new invln_scheme()
            {
                Id = ahpApplicationPreImage.Id,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApplicationSubmitted)
            };

            var ahpApplicationPostImage = ahpApplicationPreImage.Merge(ahpApplicationTarget);

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
                    ahpApplicationPostImage,
                    grantbenchmarkT5EastSharedownership,
                    grantbenchmarkT5EastRenttobuy,
                    grantbenchmarkT5EastSocialrent,
                    grantbenchmark5NorthEastExtracare,
                    grantbenchmarkT1EastSpecialistrent,
                    grantbenchmarkT2EastSpecialistrent,
                    grantbenchmarkT3EastSpecialistrent,
                    grantbenchmarkT4EastSpecialistrent,
                    grantbenchmarkT5EastSpecialistrent,
                }
            );
#pragma warning restore CS0618 // Type or member is obsolete

            Asset("Update", (int)StageEnum.PostOperation);

            // Act

            handler.DoWork();

            var applicationResult = fakedContext.CreateQuery<invln_scheme>()
                .Single(x => x.Id == ahpApplicationTarget.Id);

            // Assert

            Assert.AreEqual(25_000, applicationResult.invln_grantperunit.Value);
            Assert.AreEqual(27.02m, applicationResult.invln_grantasaoftotalschemecosts.Value, 0.01m);
            Assert.AreEqual(89_000, applicationResult.invln_RegionalBenchmarkGrantPerUnit.Value);
            Assert.AreEqual(28.08m, applicationResult.invln_regionalbenchmarkagainstthegrantperunit.Value, 0.01m);
            Assert.AreEqual(4.70m, applicationResult.invln_WorkssCostsm2.Value, 0.01m);

            Assert.AreEqual(103m, applicationResult.invln_grantasapercentageoftotalschemecosts.Value, 1m);
            Assert.AreEqual(0.16m, applicationResult.invln_worksm2asapercentageofareaavg.Value, 0.01m);
            Assert.IsNotNull(applicationResult.invln_gpuaspercentageofareaaverage);
            Assert.AreEqual(36m, applicationResult.invln_gpuaspercentageofareaaverage.Value, 1m);
            Assert.IsNotNull(applicationResult.invln_supportedgpuaspercentageofareaaverage);
            Assert.AreEqual(40m, applicationResult.invln_supportedgpuaspercentageofareaaverage.Value, 1m);
        }

        [TestMethod]
        public void DoWork_DeliverabilityScoring_ApplicationUpdated()
        {
            // Arrange

            var grantbenchmarkT5EastSharedownership = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Sharedownership, 46_000);

            var grantbenchmarkT5EastRenttobuy = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Renttobuy, 57_000);

            var grantbenchmarkT5EastSocialrent = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Socialrent, 89_000);

            var grantbenchmarkT5EastSpecialistrent = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Specialistrent, 89_000);

            var grantbenchmark5NorthEastExtracare = GrantBenchmarkHelper.CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion.NorthEast, invln_Tenurechoice.Extracare, 89_000);

            var grantbenchmarkT1EastSharedownership = GrantBenchmarkHelper.CreateGrantBenchmarkTable1(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Sharedownership, 19.5m);
            var grantbenchmarkT2EastSharedownership = GrantBenchmarkHelper.CreateGrantBenchmarkTable2(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Sharedownership, 2_000);
            var grantbenchmarkT3EastSharedownership = GrantBenchmarkHelper.CreateGrantBenchmarkTable3(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Sharedownership, 48_048);
            var grantbenchmarkT4EastSharedownership = GrantBenchmarkHelper.CreateGrantBenchmarkTable4(he_GovernmentOfficeRegion.EastofEngland, invln_Tenurechoice.Sharedownership, 43_680);

            var site = new invln_Sites()
            {
                Id = Guid.NewGuid(),
                invln_sitename = "Test_Site1",
                invln_GovernmentOfficeRegion = new OptionSetValue((int)he_GovernmentOfficeRegion.EastofEngland)
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

            var homeType1 = HomeTypeHelper.CreateHomeType(ahpApplicationPreImage, 85, 125, invln_Typeofhousing.General, null, null);

            var deliveryPhase0 = HelperDeliveryPhase.CreateDeliveryPhase(ahpApplicationPreImage, null, new DateTime(2023, 4, 30));
            var deliveryPhase1 = HelperDeliveryPhase.CreateDeliveryPhase(ahpApplicationPreImage, new DateTime(2024, 1, 1), new DateTime(2024, 4, 1));
            var deliveryPhase2 = HelperDeliveryPhase.CreateDeliveryPhase(ahpApplicationPreImage, new DateTime(2024, 4, 2), new DateTime(2025, 1, 1));

            var deliveryPhaseA = HelperDeliveryPhase.CreateDeliveryPhase(null, new DateTime(2024, 4, 2), new DateTime(2025, 4, 1));

            var ahpApplicationTarget = new invln_scheme()
            {
                Id = ahpApplicationPreImage.Id,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApplicationSubmitted)
            };

            var ahpApplicationPostImage = ahpApplicationPreImage.Merge(ahpApplicationTarget);

            PreImage = ahpApplicationPreImage;
            Target = ahpApplicationTarget;
            PostImage = ahpApplicationPostImage;


#pragma warning disable CS0618 // Type or member is obsolete
            fakedContext.Initialize(
                new List<Entity>()
                {
                    homeType1,
                    site,
                    ahpApplicationPostImage,
                    grantbenchmarkT5EastSharedownership,
                    grantbenchmarkT5EastRenttobuy,
                    grantbenchmarkT5EastSocialrent,
                    grantbenchmarkT5EastSpecialistrent,
                    grantbenchmark5NorthEastExtracare,
                    grantbenchmarkT1EastSharedownership,
                    grantbenchmarkT2EastSharedownership,
                    grantbenchmarkT3EastSharedownership,
                    grantbenchmarkT4EastSharedownership,
                    deliveryPhase0,
                    deliveryPhase1,
                    deliveryPhase2,
                    deliveryPhaseA
                }
            );
#pragma warning restore CS0618 // Type or member is obsolete

            Asset("Update", (int)StageEnum.PostOperation);

            // Act

            handler.DoWork();

            var applicationResult = fakedContext.CreateQuery<invln_scheme>()
                .Single(x => x.Id == ahpApplicationTarget.Id);

            // Assert

            Assert.AreEqual(8, applicationResult.invln_SoSScore);
            Assert.AreEqual(6, applicationResult.invln_CompScore);
        }
    }
}
