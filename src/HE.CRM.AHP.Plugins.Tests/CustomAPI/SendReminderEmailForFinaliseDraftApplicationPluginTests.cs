using DataverseModel;
using FakeItEasy;
using FakeXrmEasy;
using HE.CRM.AHP.Plugins.Plugins.CustomApi;
using HE.Base.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HE.CRM.AHP.Plugins.Tests.CustomApi
{
    [TestClass]
    public class SendReminderEmailForFinaliseDraftApplicationPluginTests
    {
        private XrmFakedContext fakedContext;

        private XrmFakedPluginExecutionContext pluginContext;

        [TestInitialize]
        public void Initialize()
        {
            fakedContext = new XrmFakedContext();
            pluginContext = fakedContext.GetDefaultPluginContext();
        }

        [TestMethod]
        public void Test_1()
        {
            fakedContext.AddExecutionMock<invln_sendgovnotifyemailRequest>(SendMessageMock);
            var appId = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var systemUserID = Guid.NewGuid();

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact()
                {
                    Id = contactId,
                    FirstName = "Zdzisław",
                    LastName = "Botak",
                    EMailAddress1 = "bonie@OoO.pl"
                },
                new invln_scheme()
                {
                    Id = appId,
                    invln_contactid = contactId.ToEntityReference<Contact>(),
                    OwnerId = systemUserID.ToEntityReference<SystemUser>()
                },
                new invln_notificationsetting
                {
                    Id = Guid.NewGuid(),
                    invln_subject = "subject",
                    invln_templateid = "template id",
                    invln_templatetypename = "AHP_EXTERNAL_REMINDER_TO_FINALIZE_DRAFT_APPLICATION"
                }
            });

            pluginContext.InputParameters = new ParameterCollection
            {
                { invln_sendremindertofinalisadraftapplicationRequest.Fields.invln_applicationid,
                                                                            appId.ToString()}
            };
            try
            {
                fakedContext.ExecutePluginWithConfigurations<SendReminderEmailForFinaliseDraftApplicationPlugin>(pluginContext, "", "");
            }
            catch (Exception e)
            {
                Assert.AreEqual("Message Send", e.Message);
            }
        }

        private OrganizationResponse SendMessageMock(OrganizationRequest req)
        {
            throw new Exception("Message Send");
        }
    }
}
