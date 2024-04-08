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
    public class ChangeExternalStatusOnInternalStatusChangeHandlerTests : CrmEntityHandlerTestBase<invln_scheme, ChangeExternalStatusOnInternalStatusChangeHandler, DataverseContext>
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

            PreImage = new invln_scheme
            {
                Id = Guid.NewGuid(),
            };

            Asset("Update", (int)StageEnum.PreOperation);

            var result = handler.CanWork();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanWork_False_1()
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
        public void DoWork_NoStatusChange_1()
        {
            var shemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = shemeId,
                StatusCode = null
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = shemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            PreImage = new invln_scheme
            {
                Id = Guid.NewGuid(),
            };

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == shemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_scheme_StatusCode.Draft, result.StatusCode.Value);
            Assert.IsNull(result.invln_ExternalStatus);
        }

        [TestMethod]
        public void DoWork_NoStatusChange_2()
        {
            var shemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = shemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            PreImage = new invln_scheme
            {
                Id = Guid.NewGuid(),
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = shemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == shemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_scheme_StatusCode.Draft, result.StatusCode.Value);
            Assert.IsNull(result.invln_ExternalStatus);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToDraft()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Inactive) // plugin nie gdy nie zmieni na draft? czy jest jaliś status przed draft?
            };

            fakedContext.Initialize(new List<Entity>()
            {
                {
                    new invln_scheme()
                    {
                        Id = schemeId,
                        StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
                    }
                }
            });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName)
                .Where(x => x.Id == schemeId)
                .Select(x => x.ToEntity<invln_scheme>())
                .FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.Draft, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToApplicationSubmitted()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApplicationSubmitted)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.ApplicationSubmitted, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToDeleted()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Deleted)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.Deleted, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToOnHold()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.OnHold)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.OnHold, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToWithdrawn()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Withdrawn)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.Withdrawn, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToUnderReview()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.UnderReviewPendingAssessment)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.UnderReview, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToUnderReviewInAssessment()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.UnderReviewInAssessment)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.UnderReview, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToUnderReviewGoingToSLT()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.UnderReviewGoingToSLT)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.UnderReview, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToUnderReviewInternallyApproved()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.UnderReviewInternallyApproved)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.UnderReview, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToInternallyApprovedSubjectToIPQ()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.InternallyApprovedSubjectToIPQ)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.UnderReview, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToInternallyApprovedSubjectToRegulatorSignOff()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.InternallyApprovedSubjectToRegulatorSignOff)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.UnderReview, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToInternallyApprovedSubjectToIPQAndRegulatorySignOff()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.InternallyApprovedSubjectToIPQAndRegulatorySignOff)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.UnderReview, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToInternallyRejected()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.InternallyRejected)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.UnderReview, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToRejected()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Rejected)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.Rejected, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToRequestedEditing()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.RequestedEditing)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.RequestedEditing, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToReferredBackToApplicant()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ReferredBackToApplicant)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.ReferredBackToApplicant, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToApprovedSubjecttoContract()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApprovedSubjecttoContract)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.ApprovedSubjectToContract, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToApprovedEngressmentIssued()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApprovedEngressmentIssued)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.ApprovedSubjectToContract, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToApprovedContractReceivedBackToHE()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApprovedContractReceivedBackToHE)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.ApprovedSubjectToContract, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToApprovedContractPassedComplianceChecks()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApprovedContractPassedComplianceChecks)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.ApprovedSubjectToContract, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToApprovedContractExecuted()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.ApprovedContractExecuted)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.ApprovedSubjectToContract, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatusToApproved()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Approved)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual((int)invln_ExternalStatusAHP.Approved, Target.invln_ExternalStatus.Value);
        }

        [TestMethod]
        public void DoWork_ChangeExternalStatus_NoChange()
        {
            var schemeId = Guid.NewGuid();

            Target = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue(100)
            };

            PreImage = new invln_scheme
            {
                Id = schemeId,
                StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
        {
            { new invln_scheme(){ Id = schemeId, StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft) } }
        });

            Asset("Update", (int)StageEnum.PreOperation);

            handler.DoWork();
            var result = fakedContext.CreateQuery(invln_scheme.EntityLogicalName).Where(x => x.Id == schemeId).Select(x => x.ToEntity<invln_scheme>()).FirstOrDefault();
            Assert.AreEqual(null, Target.invln_ExternalStatus);
        }
    }
}
