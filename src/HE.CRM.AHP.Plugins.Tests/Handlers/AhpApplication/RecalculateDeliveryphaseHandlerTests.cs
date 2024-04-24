using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Common.Extensions;
using HE.CRM.AHP.Plugins.Handlers.AHPApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Tests.Plugins.Handlers;

namespace HE.CRM.AHP.Plugins.Tests.Handlers.AhpApplication
{
    [TestClass]
    public class RecalculateDeliveryphaseHandlerTests : CrmEntityHandlerTestBase<invln_scheme, RecalculateDeliveryphaseHandler, DataverseContext>
    {
        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();
        }

        [TestMethod]
        public void CanWork_Success_NumberOfHomesChange()
        {
            var applicatioId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = applicatioId,
                invln_noofhomes = 10,
            };

            PreImage = new invln_scheme
            {
                Id = applicatioId,
                invln_noofhomes = 5,
            };
            Asset("Update", (int)StageEnum.PreOperation);

            var result = handler.CanWork();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanWork_Success_FundingRequiredChange()
        {
            var applicatioId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = applicatioId,
                invln_fundingrequired = new Money(1000000),
            };

            PreImage = new invln_scheme
            {
                Id = applicatioId,
                invln_fundingrequired = new Money(700000),
            };

            Asset("Update", (int)StageEnum.PreOperation);

            var result = handler.CanWork();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanWork_Success_BothChange()
        {

            var applicatioId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = applicatioId,
                invln_noofhomes = 10,
                invln_fundingrequired = new Money(1000000),

            };
            PreImage = new invln_scheme
            {
                Id = applicatioId,
                invln_noofhomes = 5,
                invln_fundingrequired = new Money(700000),
            };

            Asset("Update", (int)StageEnum.PreOperation);

            var result = handler.CanWork();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanWork_Success_Fail()
        {
            Target = new invln_scheme
            {
                Id = Guid.NewGuid(),
            };
            Asset("Update", (int)StageEnum.PreOperation);

            var result = handler.CanWork();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DoWork_numberOfOhouse()
        {
            var applicatioId = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var programmeId = Guid.NewGuid();
            var milestone_1 = Guid.NewGuid();
            var milestone_2 = Guid.NewGuid();
            var milestone_3 = Guid.NewGuid();
            var deliveryPhaseId_1 = Guid.NewGuid();
            var deliveryPhaseId_2 = Guid.NewGuid();
            Target = new invln_scheme
            {
                Id = applicatioId,
                invln_noofhomes = 10,
            };

            PreImage = new invln_scheme
            {
                Id = applicatioId,
                invln_noofhomes = 5,
                invln_programmelookup = programmeId.ToEntityReference<invln_programme>(),
                invln_contactid = contactId.ToEntityReference<Contact>(),
                invln_fundingrequired = new Money(100000),
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact
                {
                    Id = contactId
                },
                new invln_programme()
                {
                    Id = programmeId
                },
                new invln_milestoneframeworkitem()
                {
                    Id= milestone_1,
                    invln_programmeId = programmeId.ToEntityReference<invln_programme>(),
                    invln_milestone = new OptionSetValue((int)invln_Milestone.Acquisition),
                    invln_percentagepaidonmilestone = 40
                },
                new invln_milestoneframeworkitem()
                {
                    Id= milestone_2,
                    invln_programmeId = programmeId.ToEntityReference<invln_programme>(),
                    invln_milestone = new OptionSetValue((int)invln_Milestone.SoS),
                    invln_percentagepaidonmilestone = 35
                },
                new invln_milestoneframeworkitem()
                {
                    Id= milestone_3,
                    invln_programmeId = programmeId.ToEntityReference<invln_programme>(),
                    invln_milestone = new OptionSetValue((int)invln_Milestone.PC),
                    invln_percentagepaidonmilestone = 25
                },
                new invln_DeliveryPhase()
                {
                    Id = deliveryPhaseId_1,
                    invln_Application = applicatioId.ToEntityReference<invln_scheme>(),
                    invln_NoofHomes = 4
                },
                new invln_DeliveryPhase()
                {
                    Id = deliveryPhaseId_2,
                    invln_Application = applicatioId.ToEntityReference<invln_scheme>(),
                    invln_NoofHomes = 6
                },
            });
            Asset("Update", (int)StageEnum.PreOperation);
            handler.DoWork();
            var deliveryPhase_1 = fakedContext.CreateQuery(invln_DeliveryPhase.EntityLogicalName)
                                    .Where(x => x.Id == deliveryPhaseId_1)
                                    .Select(x => x.ToEntity<invln_DeliveryPhase>())
                                    .FirstOrDefault();

            var deliveryPhase_2 = fakedContext.CreateQuery(invln_DeliveryPhase.EntityLogicalName)
                        .Where(x => x.Id == deliveryPhaseId_2)
                        .Select(x => x.ToEntity<invln_DeliveryPhase>())
                        .FirstOrDefault();

            Assert.AreEqual(16000, deliveryPhase_1.invln_AcquisitionValue.Value);
            Assert.AreEqual(14000, deliveryPhase_1.invln_StartOnSiteValue.Value);
            Assert.AreEqual(10000, deliveryPhase_1.invln_CompletionValue.Value);
            Assert.AreEqual(24000, deliveryPhase_2.invln_AcquisitionValue.Value);
            Assert.AreEqual(21000, deliveryPhase_2.invln_StartOnSiteValue.Value);
            Assert.AreEqual(15000, deliveryPhase_2.invln_CompletionValue.Value);

        }
        [TestMethod]
        public void DoWork_FundingRequiredChangee()
        {
            var applicatioId = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var programmeId = Guid.NewGuid();
            var milestone_1 = Guid.NewGuid();
            var milestone_2 = Guid.NewGuid();
            var milestone_3 = Guid.NewGuid();
            var deliveryPhaseId_1 = Guid.NewGuid();
            var deliveryPhaseId_2 = Guid.NewGuid();
            Target = new invln_scheme
            {
                Id = applicatioId,
                invln_fundingrequired = new Money(100000),
            };

            PreImage = new invln_scheme
            {
                Id = applicatioId,
                invln_programmelookup = programmeId.ToEntityReference<invln_programme>(),
                invln_contactid = contactId.ToEntityReference<Contact>(),
                invln_noofhomes = 10,
                invln_fundingrequired = new Money(70000),
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact
                {
                    Id = contactId
                },
                new invln_programme()
                {
                    Id = programmeId
                },
                new invln_milestoneframeworkitem()
                {
                    Id= milestone_1,
                    invln_programmeId = programmeId.ToEntityReference<invln_programme>(),
                    invln_milestone = new OptionSetValue((int)invln_Milestone.Acquisition),
                    invln_percentagepaidonmilestone = 40
                },
                new invln_milestoneframeworkitem()
                {
                    Id= milestone_2,
                    invln_programmeId = programmeId.ToEntityReference<invln_programme>(),
                    invln_milestone = new OptionSetValue((int)invln_Milestone.SoS),
                    invln_percentagepaidonmilestone = 35
                },
                new invln_milestoneframeworkitem()
                {
                    Id= milestone_3,
                    invln_programmeId = programmeId.ToEntityReference<invln_programme>(),
                    invln_milestone = new OptionSetValue((int)invln_Milestone.PC),
                    invln_percentagepaidonmilestone = 25
                },
                new invln_DeliveryPhase()
                {
                    Id = deliveryPhaseId_1,
                    invln_Application = applicatioId.ToEntityReference<invln_scheme>(),
                    invln_NoofHomes = 4
                },
                new invln_DeliveryPhase()
                {
                    Id = deliveryPhaseId_2,
                    invln_Application = applicatioId.ToEntityReference<invln_scheme>(),
                    invln_NoofHomes = 6
                },
            });
            Asset("Update", (int)StageEnum.PreOperation);
            handler.DoWork();
            var deliveryPhase_1 = fakedContext.CreateQuery(invln_DeliveryPhase.EntityLogicalName)
                                    .Where(x => x.Id == deliveryPhaseId_1)
                                    .Select(x => x.ToEntity<invln_DeliveryPhase>())
                                    .FirstOrDefault();

            var deliveryPhase_2 = fakedContext.CreateQuery(invln_DeliveryPhase.EntityLogicalName)
                        .Where(x => x.Id == deliveryPhaseId_2)
                        .Select(x => x.ToEntity<invln_DeliveryPhase>())
                        .FirstOrDefault();

            Assert.AreEqual(16000, deliveryPhase_1.invln_AcquisitionValue.Value);
            Assert.AreEqual(14000, deliveryPhase_1.invln_StartOnSiteValue.Value);
            Assert.AreEqual(10000, deliveryPhase_1.invln_CompletionValue.Value);
            Assert.AreEqual(24000, deliveryPhase_2.invln_AcquisitionValue.Value);
            Assert.AreEqual(21000, deliveryPhase_2.invln_StartOnSiteValue.Value);
            Assert.AreEqual(15000, deliveryPhase_2.invln_CompletionValue.Value);

        }
        [TestMethod]
        public void DoWork_Both()
        {
            var applicatioId = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var programmeId = Guid.NewGuid();
            var milestone_1 = Guid.NewGuid();
            var milestone_2 = Guid.NewGuid();
            var milestone_3 = Guid.NewGuid();
            var deliveryPhaseId_1 = Guid.NewGuid();
            var deliveryPhaseId_2 = Guid.NewGuid();
            Target = new invln_scheme
            {
                Id = applicatioId,
                invln_fundingrequired = new Money(100000),
                invln_noofhomes = 10,
            };

            PreImage = new invln_scheme
            {
                Id = applicatioId,
                invln_programmelookup = programmeId.ToEntityReference<invln_programme>(),
                invln_contactid = contactId.ToEntityReference<Contact>(),
                invln_noofhomes = 5,
                invln_fundingrequired = new Money(70000),
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact
                {
                    Id = contactId
                },
                new invln_programme()
                {
                    Id = programmeId
                },
                new invln_milestoneframeworkitem()
                {
                    Id= milestone_1,
                    invln_programmeId = programmeId.ToEntityReference<invln_programme>(),
                    invln_milestone = new OptionSetValue((int)invln_Milestone.Acquisition),
                    invln_percentagepaidonmilestone = 40
                },
                new invln_milestoneframeworkitem()
                {
                    Id= milestone_2,
                    invln_programmeId = programmeId.ToEntityReference<invln_programme>(),
                    invln_milestone = new OptionSetValue((int)invln_Milestone.SoS),
                    invln_percentagepaidonmilestone = 35
                },
                new invln_milestoneframeworkitem()
                {
                    Id= milestone_3,
                    invln_programmeId = programmeId.ToEntityReference<invln_programme>(),
                    invln_milestone = new OptionSetValue((int)invln_Milestone.PC),
                    invln_percentagepaidonmilestone = 25
                },
                new invln_DeliveryPhase()
                {
                    Id = deliveryPhaseId_1,
                    invln_Application = applicatioId.ToEntityReference<invln_scheme>(),
                    invln_NoofHomes = 4
                },
                new invln_DeliveryPhase()
                {
                    Id = deliveryPhaseId_2,
                    invln_Application = applicatioId.ToEntityReference<invln_scheme>(),
                    invln_NoofHomes = 6
                },
            });
            Asset("Update", (int)StageEnum.PreOperation);
            handler.DoWork();
            var deliveryPhase_1 = fakedContext.CreateQuery(invln_DeliveryPhase.EntityLogicalName)
                                    .Where(x => x.Id == deliveryPhaseId_1)
                                    .Select(x => x.ToEntity<invln_DeliveryPhase>())
                                    .FirstOrDefault();

            var deliveryPhase_2 = fakedContext.CreateQuery(invln_DeliveryPhase.EntityLogicalName)
                        .Where(x => x.Id == deliveryPhaseId_2)
                        .Select(x => x.ToEntity<invln_DeliveryPhase>())
                        .FirstOrDefault();

            Assert.AreEqual(16000, deliveryPhase_1.invln_AcquisitionValue.Value);
            Assert.AreEqual(14000, deliveryPhase_1.invln_StartOnSiteValue.Value);
            Assert.AreEqual(10000, deliveryPhase_1.invln_CompletionValue.Value);
            Assert.AreEqual(24000, deliveryPhase_2.invln_AcquisitionValue.Value);
            Assert.AreEqual(21000, deliveryPhase_2.invln_StartOnSiteValue.Value);
            Assert.AreEqual(15000, deliveryPhase_2.invln_CompletionValue.Value);

        }
    };
}
