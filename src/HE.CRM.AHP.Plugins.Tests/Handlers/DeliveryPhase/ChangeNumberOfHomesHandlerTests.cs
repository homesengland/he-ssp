using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.CRM.AHP.Plugins.Handlers.AHPApplication;
using HE.CRM.AHP.Plugins.Handlers.DeliveryPhase;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Tests.Plugins.Handlers;

namespace HE.CRM.AHP.Plugins.Tests.Handlers.DeliveryPhase
{
    [TestClass]
    public class ChangeNumberOfHomesHandlerTests : CrmEntityHandlerTestBase<invln_DeliveryPhase, ChangeNumberOfHomes, DataverseContext>
    {
        private static readonly int NumberOfHouseOnDeliveryphase = 47;
        private static readonly int NumberOfHouseOnApplication = 47;
        private static readonly Money Fundingrequired = new Money(333333);
        private static readonly Guid DeliveryPhaseId = Guid.NewGuid();
        private static readonly Guid ApplicationId = Guid.NewGuid();
        private static readonly Guid ProgrammeId = Guid.NewGuid();
        private static readonly Guid MilestonesAcquisitionId = Guid.NewGuid();
        private static readonly Guid MilestonesSoSId = Guid.NewGuid();
        private static readonly Guid MilestonesPCId = Guid.NewGuid();
        private static readonly Guid AccountId = Guid.NewGuid();

        private static readonly invln_programme _programme = new invln_programme()
        {
            Id = ProgrammeId
        };

        private static readonly invln_milestoneframeworkitem MilestonesAcquisition = new invln_milestoneframeworkitem()
        {
            Id = MilestonesAcquisitionId,
            invln_percentagepaidonmilestone = 40,
            invln_programmeId = _programme.ToEntityReference(),
            invln_milestone = new OptionSetValue((int)invln_Milestone.Acquisition),
        };

        private static readonly invln_milestoneframeworkitem MilestonesSoS = new invln_milestoneframeworkitem()
        {
            Id = MilestonesSoSId,
            invln_percentagepaidonmilestone = 35,
            invln_programmeId = _programme.ToEntityReference(),
            invln_milestone = new OptionSetValue((int)invln_Milestone.SoS),
        };

        private static readonly invln_milestoneframeworkitem MilestonesPC = new invln_milestoneframeworkitem()
        {
            Id = MilestonesPCId,
            invln_percentagepaidonmilestone = 25,
            invln_programmeId = _programme.ToEntityReference(),
            invln_milestone = new OptionSetValue((int)invln_Milestone.PC),
        };

        private static readonly Account Partner = new Account()
        {
            Id = AccountId,
            invln_UnregisteredBody = false,
        };

        private static readonly Account URBpartner = new Account()
        {
            Id = AccountId,
            invln_UnregisteredBody = true,
        };

        private static readonly invln_scheme Application = new invln_scheme()
        {
            Id = ApplicationId,
            invln_noofhomes = 47,
            invln_fundingrequired = Fundingrequired,
            invln_programmelookup = _programme.ToEntityReference(),
            invln_organisationid = Partner.ToEntityReference(),
        };

        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();
        }

        [TestMethod]
        public void CanWork_NumberOfHomesChange()
        {
            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_NoofHomes = 10
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_NoofHomes = 5
            };

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());
        }

        [TestMethod]
        public void CanWork_BuildActivityChange()
        {
            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_buildactivitytype = new Microsoft.Xrm.Sdk.OptionSetValue((int)invln_NewBuildActivityType.AcquisitionandWorks)
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_buildactivitytype = new Microsoft.Xrm.Sdk.OptionSetValue((int)invln_NewBuildActivityType.WorksOnly)
            };

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());
        }

        [TestMethod]
        public void CanWork_CompletionPercentageValue()
        {
            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_CompletionPercentageValue = 5
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_CompletionPercentageValue = 15
            };

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());
        }

        [TestMethod]
        public void CanWork_StartOnSitePercentageValueChange()
        {
            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_StartOnSitePercentageValue = 20
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_StartOnSitePercentageValue = 25
            };

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());
        }

        [TestMethod]
        public void CanWork_AcquisitionPercentageValueChange()
        {
            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_AcquisitionPercentageValue = 15
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_AcquisitionPercentageValue = 45
            };

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());
        }

        [TestMethod]
        public void CanWork_StatusCodeChange()
        {
            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                StatusCode = new Microsoft.Xrm.Sdk.OptionSetValue((int)invln_DeliveryPhase_StatusCode.PendingAdjustment)
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                StatusCode = new Microsoft.Xrm.Sdk.OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment)
            };

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());
        }

        [TestMethod]
        public void CanWork_nbrhChange()
        {
            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_nbrh = true
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_nbrh = false
            };

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());
        }

        [TestMethod]
        public void CanWork_rehabactivitytypeChange()
        {
            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new Microsoft.Xrm.Sdk.OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab)
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new Microsoft.Xrm.Sdk.OptionSetValue((int)invln_RehabActivityType.WorksOnly)
            };

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());
        }

        [TestMethod]
        public void DoWork_NumberOFHomesChange()
        {
            var newNumnerOfHomes = 47;

            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_NoofHomes = newNumnerOfHomes
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 30,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                MilestonesAcquisition,
                MilestonesPC,
                MilestonesSoS,
                Application,
                Partner,
                new invln_DeliveryPhase() {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                invln_sumofcalculatedfounds = new Money(100),
                invln_CompletionValue = new Money(100)
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.4m, MidpointRounding.AwayFromZero), Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.35m, MidpointRounding.AwayFromZero), Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.25m, MidpointRounding.AwayFromZero), Target.invln_CompletionValue.Value);
        }

        [TestMethod]
        public void DoWork_NumberOFHomesChange_urb()
        {
            var newNumnerOfHomes = 47;

            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_NoofHomes = newNumnerOfHomes
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 30,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                MilestonesAcquisition,
                MilestonesPC,
                MilestonesSoS,
                Application,
                URBpartner,
                new invln_DeliveryPhase() {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                invln_sumofcalculatedfounds = new Money(100),
                invln_CompletionValue = new Money(333333),
                }
            })
            ;

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 1m, Target.invln_CompletionValue.Value);
        }

        [TestMethod]
        public void DoWork_ResetMilestons()
        {
            var newNumnerOfHomes = 47;

            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.PendingAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                MilestonesAcquisition,
                MilestonesPC,
                MilestonesSoS,
                Application,
                Partner,
                new invln_DeliveryPhase() {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                invln_sumofcalculatedfounds = new Money(100),
                invln_CompletionValue = new Money(100)
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(0.4m * 100, Target.invln_AcquisitionPercentageValue);
            Assert.AreEqual(0.35m * 100, Target.invln_StartOnSitePercentageValue);
            Assert.AreEqual(0.25m * 100, Target.invln_CompletionPercentageValue);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.4m, MidpointRounding.AwayFromZero), Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.35m, MidpointRounding.AwayFromZero), Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.25m, MidpointRounding.AwayFromZero), Target.invln_CompletionValue.Value);
        }

        [TestMethod]
        public void DoWork_ChangeActivityTypeToAcquisitionandWorksrehab()
        {
            var newNumnerOfHomes = 47;

            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.ExistingSatisfactory),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.PendingAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 0,
                invln_StartOnSitePercentageValue = 0,
                invln_CompletionPercentageValue = 100,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                MilestonesAcquisition,
                MilestonesPC,
                MilestonesSoS,
                Application,
                Partner,
                new invln_DeliveryPhase() {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 0,
                invln_StartOnSitePercentageValue = 0,
                invln_CompletionPercentageValue = 100,
                invln_sumofcalculatedfounds = new Money(100),
                invln_CompletionValue = new Money(100)
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(0.4m * 100, Target.invln_AcquisitionPercentageValue);
            Assert.AreEqual(0.35m * 100, Target.invln_StartOnSitePercentageValue);
            Assert.AreEqual(0.25m * 100, Target.invln_CompletionPercentageValue);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.4m, MidpointRounding.AwayFromZero), Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.35m, MidpointRounding.AwayFromZero), Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.25m, MidpointRounding.AwayFromZero), Target.invln_CompletionValue.Value);
        }

        [TestMethod]
        public void DoWork_ChangeActivityTypeToExistingSatisfactory()
        {
            var newNumnerOfHomes = 47;
            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.ExistingSatisfactory),
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.PendingAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 60,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                MilestonesAcquisition,
                MilestonesPC,
                MilestonesSoS,
                Application,
                Partner,
                new invln_DeliveryPhase() {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 60,
                invln_sumofcalculatedfounds = new Money(100),
                invln_CompletionValue = new Money(100)
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(0, Target.invln_AcquisitionPercentageValue);
            Assert.AreEqual(0, Target.invln_StartOnSitePercentageValue);
            Assert.AreEqual(1m * 100, Target.invln_CompletionPercentageValue);
            Assert.AreEqual(0, Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(0, Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes, Target.invln_CompletionValue.Value);
        }

        [TestMethod]
        public void DoWork_Percentage_5_5_90()
        {

            var newNumnerOfHomes = 47;
            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                invln_AcquisitionPercentageValue = 5,
                invln_StartOnSitePercentageValue = 5,
                invln_CompletionPercentageValue = 90,
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.PendingAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 60,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                MilestonesAcquisition,
                MilestonesPC,
                MilestonesSoS,
                Application,
                Partner,
                new invln_DeliveryPhase() {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 60,
                invln_sumofcalculatedfounds = new Money(100),
                invln_CompletionValue = new Money(100)
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(0.05m * 100, Target.invln_AcquisitionPercentageValue);
            Assert.AreEqual(0.05m * 100, Target.invln_StartOnSitePercentageValue);
            Assert.AreEqual(0.90m * 100, Target.invln_CompletionPercentageValue);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.05m, MidpointRounding.AwayFromZero), Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.05m, MidpointRounding.AwayFromZero), Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.9m, MidpointRounding.AwayFromZero), Target.invln_CompletionValue.Value, 1);
        }

        [TestMethod]
        public void DoWork_Pervcentage_0_5_95()
        {
            var newNumnerOfHomes = 47;
            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                invln_AcquisitionPercentageValue = 0,
                invln_StartOnSitePercentageValue = 5,
                invln_CompletionPercentageValue = 95,
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.PendingAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 60,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                MilestonesAcquisition,
                MilestonesPC,
                MilestonesSoS,
                Application,
                Partner,
                new invln_DeliveryPhase() {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 60,
                invln_sumofcalculatedfounds = new Money(100),
                invln_CompletionValue = new Money(100)
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(0, Target.invln_AcquisitionPercentageValue);
            Assert.AreEqual(0.05m * 100, Target.invln_StartOnSitePercentageValue);
            Assert.AreEqual(0.95m * 100, Target.invln_CompletionPercentageValue);
            Assert.AreEqual(0, Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.05m, MidpointRounding.AwayFromZero), Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.95m, MidpointRounding.AwayFromZero), Target.invln_CompletionValue.Value);
        }

        [TestMethod]
        public void DoWork_Percentage_5_0_95()
        {
            var newNumnerOfHomes = 47;
            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                invln_AcquisitionPercentageValue = 5,
                invln_StartOnSitePercentageValue = 0,
                invln_CompletionPercentageValue = 95,
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.PendingAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 60,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                MilestonesAcquisition,
                MilestonesPC,
                MilestonesSoS,
                Application,
                Partner,
                new invln_DeliveryPhase() {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 60,
                invln_sumofcalculatedfounds = new Money(100),
                invln_CompletionValue = new Money(100)
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(0.05m * 100, Target.invln_AcquisitionPercentageValue);
            Assert.AreEqual(0, Target.invln_StartOnSitePercentageValue);
            Assert.AreEqual(0.95m * 100, Target.invln_CompletionPercentageValue);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.05m, MidpointRounding.AwayFromZero), Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(0, Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes * 0.95m, MidpointRounding.AwayFromZero), Target.invln_CompletionValue.Value);
        }

        [TestMethod]
        public void DoWork_Percentage_0_0_100()
        {
            var newNumnerOfHomes = 47;
            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                invln_AcquisitionPercentageValue = 0,
                invln_StartOnSitePercentageValue = 0,
                invln_CompletionPercentageValue = 100,
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.PendingAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 20,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                MilestonesAcquisition,
                MilestonesPC,
                MilestonesSoS,
                Application,
                Partner,
                new invln_DeliveryPhase() {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = NumberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 60,
                invln_sumofcalculatedfounds = new Money(100),
                invln_CompletionValue = new Money(100)
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(0, Target.invln_AcquisitionPercentageValue);
            Assert.AreEqual(0, Target.invln_StartOnSitePercentageValue);
            Assert.AreEqual(1m * 100, Target.invln_CompletionPercentageValue);
            Assert.AreEqual(0, Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(0, Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * newNumnerOfHomes, MidpointRounding.AwayFromZero), Target.invln_CompletionValue.Value);
        }

        [TestMethod]
        public void DoWork_ManyDeliverypahses()
        {
            var df2 = Guid.NewGuid();
            var df3 = Guid.NewGuid();

            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_NoofHomes = 10
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.Default),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 30,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                MilestonesAcquisition,
                MilestonesPC,
                MilestonesSoS,
                Application,
                Partner,
                new invln_DeliveryPhase() {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 9,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                invln_sumofcalculatedfounds = new Money(70922),
                invln_CompletionValue = new Money(70922),
                invln_completionmilestoneclaimdate = DateTime.Now.AddDays(10),
                },

                new invln_DeliveryPhase() {
                Id = df2,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 15,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                invln_sumofcalculatedfounds = new Money(106383),
                invln_CompletionValue = new Money(26596),
                invln_completionmilestoneclaimdate = DateTime.Now.AddDays(20),
                },
                new invln_DeliveryPhase() {
                Id = df3,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 22,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                invln_sumofcalculatedfounds = new Money(156027),
                invln_CompletionValue = new Money(39007),
                invln_completionmilestoneclaimdate = DateTime.Now.AddDays(30),
                }
            });
            ;

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(Math.Round(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * 10, 0, MidpointRounding.AwayFromZero) * 0.4m, 0, MidpointRounding.AwayFromZero), Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(Math.Round(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * 10, 0, MidpointRounding.AwayFromZero) * 0.35m, 0, MidpointRounding.AwayFromZero), Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(Math.Round(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * 10, 0, MidpointRounding.AwayFromZero) * 0.25m, 0, MidpointRounding.AwayFromZero), Target.invln_CompletionValue.Value, 1);
            Assert.AreEqual(70922, Target.invln_AcquisitionValue.Value + Target.invln_StartOnSiteValue.Value + Target.invln_CompletionValue.Value);
            var sum = fakedContext.CreateQuery<invln_DeliveryPhase>().Where(x => x.invln_Application.Id == ApplicationId).Sum(x => x.invln_sumofcalculatedfounds.Value);
            Assert.AreEqual(333333, sum);
            var df = fakedContext.CreateQuery<invln_DeliveryPhase>().Where(x => x.Id == df3).FirstOrDefault();
            Assert.AreEqual(156028, df.invln_sumofcalculatedfounds.Value);

        }

        [TestMethod]
        public void DoWork_ManyDeliverypahses_5_5_90()
        {
            Guid df2 = Guid.NewGuid();
            Guid df3 = Guid.NewGuid();

            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_AcquisitionPercentageValue = 5,
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.Default),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 10,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 5,
                invln_CompletionPercentageValue = 90,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                MilestonesAcquisition,
                MilestonesPC,
                MilestonesSoS,
                Application,
                Partner,
                new invln_DeliveryPhase() {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 10,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 5,
                invln_CompletionPercentageValue = 90,
                invln_sumofcalculatedfounds = new Money(70922),
                invln_CompletionValue = new Money(70922),
                invln_completionmilestoneclaimdate = DateTime.Now.AddDays(10),
                },

                new invln_DeliveryPhase() {
                Id = df2,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 15,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                invln_sumofcalculatedfounds = new Money(106383),
                invln_CompletionValue = new Money(26596),
                invln_completionmilestoneclaimdate = DateTime.Now.AddDays(20),
                },
                new invln_DeliveryPhase() {
                Id = df3,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 22,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                invln_sumofcalculatedfounds = new Money(156027),
                invln_CompletionValue = new Money(39007),
                invln_completionmilestoneclaimdate = DateTime.Now.AddDays(30),
                }
            });
            ;

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(Math.Round(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * 10, 0, MidpointRounding.AwayFromZero) * 0.05m, 0, MidpointRounding.AwayFromZero), Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(Math.Round(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * 10, 0, MidpointRounding.AwayFromZero) * 0.05m, 0, MidpointRounding.AwayFromZero), Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(Math.Round(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * 10, 0, MidpointRounding.AwayFromZero) * 0.9m, 0, MidpointRounding.AwayFromZero), Target.invln_CompletionValue.Value, 1);
            Assert.AreEqual(70922, Target.invln_AcquisitionValue.Value + Target.invln_StartOnSiteValue.Value + Target.invln_CompletionValue.Value);
            var sum = fakedContext.CreateQuery<invln_DeliveryPhase>().Where(x => x.invln_Application.Id == ApplicationId).Sum(x => x.invln_sumofcalculatedfounds.Value);
            Assert.AreEqual(333333, sum);
            var df = fakedContext.CreateQuery<invln_DeliveryPhase>().Where(x => x.Id == df3).FirstOrDefault();
            Assert.AreEqual(156028, df.invln_sumofcalculatedfounds.Value);

        }

        [TestMethod]
        public void DoWork_ManyDeliverypahses_0_0_100()
        {
            var df2 = Guid.NewGuid();
            var df3 = Guid.NewGuid();

            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_AcquisitionPercentageValue = 0,
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.Default),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 10,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 0,
                invln_CompletionPercentageValue = 100,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                MilestonesAcquisition,
                MilestonesPC,
                MilestonesSoS,
                Application,
                Partner,
                new invln_DeliveryPhase() {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 10,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 5,
                invln_CompletionPercentageValue = 90,
                invln_sumofcalculatedfounds = new Money(70922),
                invln_CompletionValue = new Money(70922),
                invln_completionmilestoneclaimdate = DateTime.Now.AddDays(10),
                },

                new invln_DeliveryPhase() {
                Id = df2,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 15,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                invln_sumofcalculatedfounds = new Money(106383),
                invln_CompletionValue = new Money(26596),
                invln_completionmilestoneclaimdate = DateTime.Now.AddDays(20),
                },
                new invln_DeliveryPhase() {
                Id = df3,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 22,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                invln_sumofcalculatedfounds = new Money(156027),
                invln_CompletionValue = new Money(39007),
                invln_completionmilestoneclaimdate = DateTime.Now.AddDays(30),
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(Math.Round(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * 10, 0, MidpointRounding.AwayFromZero) * 0m, 0, MidpointRounding.AwayFromZero), Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(Math.Round(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * 10, 0, MidpointRounding.AwayFromZero) * 0m, 0, MidpointRounding.AwayFromZero), Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(Math.Round(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * 10, 0, MidpointRounding.AwayFromZero) * 1m, 0, MidpointRounding.AwayFromZero), Target.invln_CompletionValue.Value, 1);
            Assert.AreEqual(70922, Target.invln_AcquisitionValue.Value + Target.invln_StartOnSiteValue.Value + Target.invln_CompletionValue.Value);
            var sum = fakedContext.CreateQuery<invln_DeliveryPhase>().Where(x => x.invln_Application.Id == ApplicationId).Sum(x => x.invln_sumofcalculatedfounds.Value);
            Assert.AreEqual(333333, sum);
            var df = fakedContext.CreateQuery<invln_DeliveryPhase>().Where(x => x.Id == df3).FirstOrDefault();
            Assert.AreEqual(156028, df.invln_sumofcalculatedfounds.Value);
        }

        [TestMethod]
        public void DoWork_ManyDeliverypahses_100_0_0()
        {
            var df2 = Guid.NewGuid();
            var df3 = Guid.NewGuid();

            Target = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_AcquisitionPercentageValue = 100,
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.Default),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 10,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.AcquisitionandWorks),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 0,
                invln_CompletionPercentageValue = 0,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                MilestonesAcquisition,
                MilestonesPC,
                MilestonesSoS,
                Application,
                Partner,
                new invln_DeliveryPhase() {
                Id = DeliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 10,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 5,
                invln_CompletionPercentageValue = 90,
                invln_sumofcalculatedfounds = new Money(70922),
                invln_CompletionValue = new Money(70922),
                invln_completionmilestoneclaimdate = DateTime.Now.AddDays(10),
                },

                new invln_DeliveryPhase() {
                Id = df2,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 15,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                invln_sumofcalculatedfounds = new Money(106383),
                invln_CompletionValue = new Money(26596),
                invln_completionmilestoneclaimdate = DateTime.Now.AddDays(20),
                },
                new invln_DeliveryPhase() {
                Id = df3,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = Application.ToEntityReference(),
                invln_NoofHomes = 22,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                invln_sumofcalculatedfounds = new Money(156027),
                invln_CompletionValue = new Money(39007),
                invln_completionmilestoneclaimdate = DateTime.Now.AddDays(30),
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(Math.Round(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * 10, 0, MidpointRounding.AwayFromZero) * 1m, 0, MidpointRounding.AwayFromZero), Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(Math.Round(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * 10, 0, MidpointRounding.AwayFromZero) * 0m, 0, MidpointRounding.AwayFromZero), Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(Math.Round(Math.Round(Fundingrequired.Value / NumberOfHouseOnApplication * 10, 0, MidpointRounding.AwayFromZero) * 0m, 0, MidpointRounding.AwayFromZero), Target.invln_CompletionValue.Value, 1);
            Assert.AreEqual(70922, Target.invln_AcquisitionValue.Value + Target.invln_StartOnSiteValue.Value + Target.invln_CompletionValue.Value);
            var sum = fakedContext.CreateQuery<invln_DeliveryPhase>().Where(x => x.invln_Application.Id == ApplicationId).Sum(x => x.invln_sumofcalculatedfounds.Value);
            Assert.AreEqual(333333, sum);
            var df = fakedContext.CreateQuery<invln_DeliveryPhase>().Where(x => x.Id == df3).FirstOrDefault();
            Assert.AreEqual(156028, df.invln_sumofcalculatedfounds.Value);
        }
    }

}
