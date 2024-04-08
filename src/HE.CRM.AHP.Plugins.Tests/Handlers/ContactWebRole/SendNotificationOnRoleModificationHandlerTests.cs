using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using DataverseModel;
using FakeXrmEasy;
using HE.Base.Common.Extensions;
using HE.CRM.AHP.Plugins.Handlers.ContactWebrole;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using PwC.Base.Tests.Plugins.Handlers;

namespace HE.CRM.AHP.Plugins.Tests.Handlers.ContactWebRole
{
    [TestClass]
    public class SendNotificationOnRoleModificationHandlerTests : CrmEntityHandlerTestBase<invln_contactwebrole, SendNotificationOnRoleModificationHandler, DataverseContext>
    {
        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();
        }

        [TestMethod]
        public void CanWork_Success_Update()
        {
            Target = new invln_contactwebrole
            {
                Id = Guid.NewGuid(),
            };

            PreImage = new invln_contactwebrole
            {
                Id = Guid.NewGuid(),
            };

            Asset("Update", (int)StageEnum.PostOperation);

            var result = handler.CanWork();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanWork_Success_Create()
        {
            Target = new invln_contactwebrole
            {
                Id = Guid.NewGuid(),
            };

            PreImage = new invln_contactwebrole
            {
                Id = Guid.NewGuid(),
            };

            Asset("Create", (int)StageEnum.PostOperation);

            var result = handler.CanWork();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanWork_Success_Delete()
        {
            Target = new invln_contactwebrole
            {
                Id = Guid.NewGuid(),
            };

            Asset("Delete", (int)StageEnum.PreOperation);

            var result = handler.CanWork();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoWork_Success_Create()
        {
            fakedContext.AddExecutionMock<invln_sendgovnotifyemailRequest>(CalculateRollupFieldRequestMock);

            var contactWebRoleId = Guid.NewGuid();
            var webRoleId = Guid.NewGuid();
            var portalPermisionLevelId = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var notificationSetingsId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();

            fakedContext.Initialize(new List<Entity>()
            {
                new invln_contactwebrole() { Id = contactWebRoleId, invln_Contactid = contactId.ToEntityReference<Contact>(), invln_Webroleid = webRoleId.ToEntityReference<invln_Webrole>()},
                new Contact() { Id = contactId, OwnerId = ownerId.ToEntityReference<SystemUser>() , FirstName = "Eugenius", LastName = "Sosna", EMailAddress1 = "gieneksosna@OoO.pl"},
                new invln_notificationsetting() { Id = notificationSetingsId, invln_subject = "AHP", invln_templateid = notificationSetingsId.ToString() ,invln_templatetypename = "COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS" },
            });

            Target = new invln_contactwebrole()
            {
                Id = contactWebRoleId,
            };

            Asset("Crate", (int)StageEnum.PostOperation);

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
        public void DoWork_Success_Update()
        {
            fakedContext.AddExecutionMock<invln_sendgovnotifyemailRequest>(CalculateRollupFieldRequestMock);

            var contactWebRoleId = Guid.NewGuid();
            var webRoleId = Guid.NewGuid();
            var portalPermisionLevelId = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var notificationSetingsId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();

            fakedContext.Initialize(new List<Entity>()
            {
                new invln_contactwebrole() { Id = contactWebRoleId, invln_Contactid = contactId.ToEntityReference<Contact>(), invln_Webroleid = webRoleId.ToEntityReference<invln_Webrole>()},
                new Contact() { Id = contactId, OwnerId = ownerId.ToEntityReference<SystemUser>() , FirstName = "Eugenius", LastName = "Sosna", EMailAddress1 = "gieneksosna@OoO.pl"},
                new invln_notificationsetting() { Id = notificationSetingsId, invln_subject = "AHP", invln_templateid = notificationSetingsId.ToString() ,invln_templatetypename = "COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS" },
            });

            Target = new invln_contactwebrole()
            {
                Id = contactWebRoleId,
            };

            Asset("Update", (int)StageEnum.PostOperation);

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
        public void DoWork_Success_Delete()
        {
            //must be done in diffirent way
        }

        private OrganizationResponse CalculateRollupFieldRequestMock(OrganizationRequest req)
        {
            throw new Exception("Message Send");
        }
    }
}
