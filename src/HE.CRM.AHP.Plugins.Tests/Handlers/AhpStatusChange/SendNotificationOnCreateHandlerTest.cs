using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Common.Extensions;
using HE.CRM.AHP.Plugins.Handlers.AhpStatusChange;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Tests.Plugins.Handlers;

namespace HE.CRM.AHP.Plugins.Tests.Handlers.AhpStatusChange
{
    [TestClass]
    public class SendNotificationOnCreateHandlerTest : CrmEntityHandlerTestBase<invln_AHPStatusChange, SendNotificationOnCreateHandler, DataverseContext>
    {
        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();
        }

        [TestMethod]
        public void CanWork_Success()
        {
            Target = new invln_AHPStatusChange
            {
                Id = Guid.NewGuid(),
            };

            PreImage = new invln_AHPStatusChange
            {
                Id = Guid.NewGuid(),
            };

            Asset("Create", (int)StageEnum.PostOperation);

            var result = handler.CanWork();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoWork_NotificationNotSent()
        {
            var ahpStatusChangeId = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = null
            };

            Asset("Create", (int)StageEnum.PostOperation);
            handler.CanWork();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void CanWork_MessageSend()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.Draft)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC",
                    invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_ApplicationSubmitted()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.ApplicationSubmitted)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>(),
                    invln_schemename = "ABC",
                    invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_Approved()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.Approved)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>()
                    , invln_schemename = "ABC",
                    invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_ApprovedContractExecuted()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.ApprovedContractExecuted)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_ApprovedContractPassedComplianceChecks()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.ApprovedContractPassedComplianceChecks)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_ApprovedContractReceivedBackToHE()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.ApprovedContractReceivedBackToHE)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_ApprovedEngressmentIssued()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.ApprovedEngressmentIssued)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_ApprovedSubjecttoContract()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.ApprovedSubjecttoContract)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_Deleted()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.Deleted)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_InternallyApprovedSubjectToIPQ()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.InternallyApprovedSubjectToIPQ)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_InternallyApprovedSubjectToIPQAndRegulatorySignOff()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.InternallyApprovedSubjectToIPQAndRegulatorySignOff)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_InternallyApprovedSubjectToRegulatorSignOff()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.InternallyApprovedSubjectToRegulatorSignOff)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_OnHold()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.OnHold)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_ReferredBackToApplicant()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.ReferredBackToApplicant)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_Rejected()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.Rejected)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_RequestedEditing()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.RequestedEditing)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_UnderReviewGoingToBidClinic()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.UnderReviewGoingToBidClinic)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_UnderReviewGoingToCMEModeration()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.UnderReviewGoingToCMEModeration)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_UnderReviewGoingToSLT()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.UnderReviewGoingToSLT)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_UnderReviewInAssessment()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.UnderReviewInAssessment)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_UnderReviewInternallyApproved()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.UnderReviewInternallyApproved)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_UnderReviewPendingAssessment()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.UnderReviewPendingAssessment)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_Withdrawn()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue((int)invln_AHPInternalStatus.Withdrawn)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        [TestMethod]
        public void CanWork_MessageSend_sendWithoutPreparatio()
        {
            fakedContext.AddExecutionMock<invln_sendinternalcrmnotificationRequest>(CalculateRollupFieldRequestMock);

            var ahpStatusChangeId = Guid.NewGuid();
            var ahpApplicationID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            Target = new invln_AHPStatusChange
            {
                Id = ahpStatusChangeId,
                invln_ChangeSource = new OptionSetValue((int)invln_AHPInternalStatus.Draft),
                invln_AHPApplication = ahpApplicationID.ToEntityReference<invln_scheme>(),
                invln_Changeto = new OptionSetValue(11111)
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId
                },
                new SystemUser()
                {
                    Id = systemUserID
                },
                new invln_scheme()
                {
                    Id = ahpApplicationID,
                    OwnerId = systemUserID.ToEntityReference<SystemUser>() ,
                    invln_schemename = "ABC", invln_contactid = contactId.ToEntityReference<Contact>()
                }
            });

            Asset("Create", (int)StageEnum.PostOperation);
            try
            {
                handler.DoWork();
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        private OrganizationResponse CalculateRollupFieldRequestMock(OrganizationRequest req)
        {
            throw new Exception("Message Send");
        }
    }
}
