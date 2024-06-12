using System;
using System.Collections.Generic;
using DataverseModel;
using FakeXrmEasy.Extensions;
using HE.CRM.AHP.Plugins.Handlers.AHPApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Tests.Plugins.Handlers;

namespace HE.CRM.AHP.Plugins.Tests.Handlers.AhpApplication
{
    [TestClass]
    public class PopulateFieldsForApplicationReportHandlerTests : CrmEntityHandlerTestBase<invln_scheme, PopulateFieldsForApplicationReportHandler, DataverseContext>
    {
        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();
        }

        [TestMethod]
        public void DoWork_AllHomeTypesWithRtsoExemptOnFalse_PopulateRtsoExamptionToFalse()
        {
            // Arrange

            var ahpApplicationPreImage = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            var ahpApplicationTarget = ahpApplicationPreImage.Clone<invln_scheme>();
            ahpApplicationTarget.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApplicationSubmitted);

            var homeType1 = new invln_HomeType() { Id = Guid.NewGuid(), invln_rtsoexempt = false, invln_application = ahpApplicationPreImage.ToEntityReference() };
            var homeType2 = new invln_HomeType() { Id = Guid.NewGuid(), invln_rtsoexempt = false, invln_application = ahpApplicationPreImage.ToEntityReference() };
            var homeType3 = new invln_HomeType() { Id = Guid.NewGuid(), invln_rtsoexempt = false, invln_application = ahpApplicationPreImage.ToEntityReference() };
            var homeType4 = new invln_HomeType() { Id = Guid.NewGuid(), invln_rtsoexempt = true };


            PreImage = ahpApplicationPreImage;
            Target = ahpApplicationTarget;

#pragma warning disable CS0618 // Type or member is obsolete
            fakedContext.Initialize(new List<Entity>()
            {
                ahpApplicationTarget,
                homeType1,
                homeType2,
                homeType3,
                homeType4
            });
#pragma warning restore CS0618 // Type or member is obsolete

            Asset("Update", (int)StageEnum.PreOperation);

            // Act

            handler.DoWork();

            // Assert

            Assert.IsFalse(Target.invln_RtSOExamption);
        }

        [TestMethod]
        public void DoWork_SomeHomeTypesWithRtsoExemptOnTrue_PopulateRtsoExamptionToTrue()
        {
            // Arrange

            var ahpApplicationPreImage = new invln_scheme()
            {
                Id = Guid.NewGuid(),
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            var ahpApplicationTarget = ahpApplicationPreImage.Clone<invln_scheme>();
            ahpApplicationTarget.StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApplicationSubmitted);

            var homeType1 = new invln_HomeType() { Id = Guid.NewGuid(), invln_rtsoexempt = true, invln_application = ahpApplicationPreImage.ToEntityReference() };
            var homeType2 = new invln_HomeType() { Id = Guid.NewGuid(), invln_rtsoexempt = true, invln_application = ahpApplicationPreImage.ToEntityReference() };
            var homeType3 = new invln_HomeType() { Id = Guid.NewGuid(), invln_rtsoexempt = false, invln_application = ahpApplicationPreImage.ToEntityReference() };
            var homeType4 = new invln_HomeType() { Id = Guid.NewGuid(), invln_rtsoexempt = true };


            PreImage = ahpApplicationPreImage;
            Target = ahpApplicationTarget;

#pragma warning disable CS0618 // Type or member is obsolete
            fakedContext.Initialize(new List<Entity>()
            {
                ahpApplicationTarget,
                homeType1,
                homeType2,
                homeType3,
                homeType4
            });
#pragma warning restore CS0618 // Type or member is obsolete

            Asset("Update", (int)StageEnum.PreOperation);

            // Act

            handler.DoWork();

            // Assert

            Assert.IsTrue(Target.invln_RtSOExamption);
        }
    }
}
