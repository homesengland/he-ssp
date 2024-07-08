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
        private static int _numberOfHouseOnDeliveryphase = 10;
        private static int _numberOfHouseOnApplication = 100;
        private static Money fundingrequired = new Money(100000);
        private static readonly Guid _deliveryPhaseId = Guid.NewGuid();
        private static readonly Guid _applicationId = Guid.NewGuid();
        private static readonly Guid _programmeId = Guid.NewGuid();
        private static readonly Guid _milestonesAcquisitionId = Guid.NewGuid();
        private static readonly Guid _milestonesSoSId = Guid.NewGuid();
        private static readonly Guid _milestonesPCId = Guid.NewGuid();
        private static readonly Guid _AccountId = Guid.NewGuid();

        private static invln_programme _programme = new invln_programme()
        {
            Id = _programmeId
        };

        private static invln_milestoneframeworkitem _milestonesAcquisition = new invln_milestoneframeworkitem()
        {
            Id = _milestonesAcquisitionId,
            invln_percentagepaidonmilestone = 40,
            invln_programmeId = _programme.ToEntityReference(),
            invln_milestone = new OptionSetValue((int)invln_Milestone.Acquisition),
        };

        private static invln_milestoneframeworkitem _milestonesSoS = new invln_milestoneframeworkitem()
        {
            Id = _milestonesSoSId,
            invln_percentagepaidonmilestone = 35,
            invln_programmeId = _programme.ToEntityReference(),
            invln_milestone = new OptionSetValue((int)invln_Milestone.SoS),
        };

        private static invln_milestoneframeworkitem _milestonesPC = new invln_milestoneframeworkitem()
        {
            Id = _milestonesPCId,
            invln_percentagepaidonmilestone = 25,
            invln_programmeId = _programme.ToEntityReference(),
            invln_milestone = new OptionSetValue((int)invln_Milestone.PC),
        };

        private static Account _partner = new Account()
        {
            Id = _AccountId,
            invln_UnregisteredBody = false,
        };

        private static Account _URBpartner = new Account()
        {
            Id = _AccountId,
            invln_UnregisteredBody = true,
        };

        private static invln_scheme _application = new invln_scheme()
        {
            Id = _applicationId,
            invln_noofhomes = 100,
            invln_fundingrequired = fundingrequired,
            invln_programmelookup = _programme.ToEntityReference(),
            invln_organisationid = _partner.ToEntityReference(),
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
                Id = _deliveryPhaseId,
                invln_NoofHomes = 10
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
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
                Id = _deliveryPhaseId,
                invln_buildactivitytype = new Microsoft.Xrm.Sdk.OptionSetValue((int)invln_NewBuildActivityType.AcquisitionandWorks)
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
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
                Id = _deliveryPhaseId,
                invln_CompletionPercentageValue = 5
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
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
                Id = _deliveryPhaseId,
                invln_StartOnSitePercentageValue = 20
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
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
                Id = _deliveryPhaseId,
                invln_AcquisitionPercentageValue = 15
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
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
                Id = _deliveryPhaseId,
                StatusCode = new Microsoft.Xrm.Sdk.OptionSetValue((int)invln_DeliveryPhase_StatusCode.PendingAdjustment)
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
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
                Id = _deliveryPhaseId,
                invln_nbrh = true
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
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
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new Microsoft.Xrm.Sdk.OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab)
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new Microsoft.Xrm.Sdk.OptionSetValue((int)invln_RehabActivityType.WorksOnly)
            };

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());
        }

        [TestMethod]
        public void DoWork_NumberOFHomesChange()
        {
            int newNumnerOfHomes = 5;

            Target = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_NoofHomes = newNumnerOfHomes
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = _application.ToEntityReference(),
                invln_NoofHomes = _numberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                _milestonesAcquisition,
                _milestonesPC,
                _milestonesSoS,
                _application,
                _partner,
                new invln_DeliveryPhase() {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = _application.ToEntityReference(),
                invln_NoofHomes = _numberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(fundingrequired.Value / _numberOfHouseOnApplication * newNumnerOfHomes * 0.4m, Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(fundingrequired.Value / _numberOfHouseOnApplication * newNumnerOfHomes * 0.35m, Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(fundingrequired.Value / _numberOfHouseOnApplication * newNumnerOfHomes * 0.25m, Target.invln_CompletionValue.Value);
        }

        [TestMethod]
        public void DoWork_NumberOFHomesChange_urb()
        {
            int newNumnerOfHomes = 5;

            Target = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_NoofHomes = newNumnerOfHomes
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = _application.ToEntityReference(),
                invln_NoofHomes = _numberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                _milestonesAcquisition,
                _milestonesPC,
                _milestonesSoS,
                _application,
                _URBpartner,
                new invln_DeliveryPhase() {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = _application.ToEntityReference(),
                invln_NoofHomes = _numberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(fundingrequired.Value / _numberOfHouseOnApplication * newNumnerOfHomes * 1m, Target.invln_CompletionValue.Value);
        }

        [TestMethod]
        public void DoWork_ResetMilestons()
        {
            int newNumnerOfHomes = 10;

            Target = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.PendingAdjustment),
                invln_Application = _application.ToEntityReference(),
                invln_NoofHomes = _numberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                _milestonesAcquisition,
                _milestonesPC,
                _milestonesSoS,
                _application,
                _partner,
                new invln_DeliveryPhase() {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = _application.ToEntityReference(),
                invln_NoofHomes = _numberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 40,
                invln_StartOnSitePercentageValue = 35,
                invln_CompletionPercentageValue = 25,
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(0.4m * 100, Target.invln_AcquisitionPercentageValue);
            Assert.AreEqual(0.35m * 100, Target.invln_StartOnSitePercentageValue);
            Assert.AreEqual(0.25m * 100, Target.invln_CompletionPercentageValue);
            Assert.AreEqual(fundingrequired.Value / _numberOfHouseOnApplication * newNumnerOfHomes * 0.4m, Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(fundingrequired.Value / _numberOfHouseOnApplication * newNumnerOfHomes * 0.35m, Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(fundingrequired.Value / _numberOfHouseOnApplication * newNumnerOfHomes * 0.25m, Target.invln_CompletionValue.Value);
        }

        [TestMethod]
        public void DoWork_ChangeActivityTypeToAcquisitionandWorksrehab()
        {
            int newNumnerOfHomes = 10;

            Target = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.ExistingSatisfactory),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.PendingAdjustment),
                invln_Application = _application.ToEntityReference(),
                invln_NoofHomes = _numberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 60,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                _milestonesAcquisition,
                _milestonesPC,
                _milestonesSoS,
                _application,
                _partner,
                new invln_DeliveryPhase() {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = _application.ToEntityReference(),
                invln_NoofHomes = _numberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 60,
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);
            Assert.IsTrue(handler.CanWork());

            handler.DoWork();
            Assert.AreEqual(0.2m * 100, Target.invln_AcquisitionPercentageValue);
            Assert.AreEqual(0.2m * 100, Target.invln_StartOnSitePercentageValue);
            Assert.AreEqual(0.6m * 100, Target.invln_CompletionPercentageValue);
            Assert.AreEqual(fundingrequired.Value / _numberOfHouseOnApplication * newNumnerOfHomes * 0.2m, Target.invln_AcquisitionValue.Value);
            Assert.AreEqual(fundingrequired.Value / _numberOfHouseOnApplication * newNumnerOfHomes * 0.2m, Target.invln_StartOnSiteValue.Value);
            Assert.AreEqual(fundingrequired.Value / _numberOfHouseOnApplication * newNumnerOfHomes * 0.6m, Target.invln_CompletionValue.Value);
        }

        [TestMethod]
        public void DoWork_ChangeActivityTypeToExistingSatisfactory()
        {
            int newNumnerOfHomes = 10;
            Target = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.ExistingSatisfactory),
            };

            PreImage = new invln_DeliveryPhase()
            {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.PendingAdjustment),
                invln_Application = _application.ToEntityReference(),
                invln_NoofHomes = _numberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 20,

            };

            fakedContext.Initialize(new List<Entity>()
            {
                _programme,
                _milestonesAcquisition,
                _milestonesPC,
                _milestonesSoS,
                _application,
                _partner,
                new invln_DeliveryPhase() {
                Id = _deliveryPhaseId,
                invln_rehabactivitytype = new OptionSetValue((int)invln_RehabActivityType.AcquisitionandWorksrehab),
                StatusCode = new OptionSetValue((int)invln_DeliveryPhase_StatusCode.RejectedAdjustment),
                invln_Application = _application.ToEntityReference(),
                invln_NoofHomes = _numberOfHouseOnDeliveryphase,
                invln_buildactivitytype = new OptionSetValue((int)invln_NewBuildActivityType.WorksOnly),
                invln_nbrh = true,
                invln_AcquisitionPercentageValue = 20,
                invln_StartOnSitePercentageValue = 20,
                invln_CompletionPercentageValue = 60,
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
            Assert.AreEqual(fundingrequired.Value / _numberOfHouseOnApplication * newNumnerOfHomes, Target.invln_CompletionValue.Value);
        }
    }
}
