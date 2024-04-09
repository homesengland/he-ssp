using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Common.Extensions;
using HE.CRM.AHP.Plugins.Handlers.AHPApplication;
using HE.CRM.AHP.Plugins.Handlers.AhpStatusChange;
using HE.CRM.AHP.Plugins.Handlers.ContactWebrole;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Tests.Plugins.Handlers;

namespace HE.CRM.AHP.Plugins.Tests.Handlers.ContactWebRole
{
    [TestClass]
    public class SendNotificationOnAdminRoleGrantedHandlerTests : CrmEntityHandlerTestBase<invln_contactwebrole, SendNotificationOnAdminRoleGrantedHandler, DataverseContext>
    {
        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();
        }

        [TestMethod]
        public void CanWork_Success()
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
        public void Dowork_()
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
                new SystemUser()
                {
                    Id = ownerId
                },
                new invln_contactwebrole()
                {
                    Id = contactWebRoleId,
                    invln_Contactid = contactId.ToEntityReference<Contact>(),
                    invln_Webroleid = webRoleId.ToEntityReference<invln_Webrole>()
                },
                new invln_Webrole()
                {
                    Id = webRoleId,
                    invln_Portalpermissionlevelid = portalPermisionLevelId.ToEntityReference<invln_portalpermissionlevel>()
                },
                new invln_portalpermissionlevel()
                {
                    Id = portalPermisionLevelId,
                    invln_Permission = new OptionSetValue((int)invln_Permission.Admin)
                },
                new Contact()
                {
                    Id = contactId,
                    OwnerId = ownerId.ToEntityReference<SystemUser>() ,
                    FirstName = "Eugenius",
                    LastName = "Sosna",
                    EMailAddress1 = "gieneksosna@OoO.pl"
                },
                new invln_notificationsetting()
                {
                    Id = notificationSetingsId,
                    invln_subject = "AHP",
                    invln_templateid = notificationSetingsId.ToString(),
                    invln_templatetypename = "COMMON_GRANT_ORGANISATION_ADMIN_PERMISSIONS"
                },
            });

            Target = new invln_contactwebrole
            {
                Id = contactWebRoleId,
            };
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
