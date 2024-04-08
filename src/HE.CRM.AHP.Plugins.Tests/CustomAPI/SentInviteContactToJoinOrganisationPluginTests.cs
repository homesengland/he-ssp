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
    public class SentInviteContactToJoinOrganisationPluginTests
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

            var invitedContactId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var inviterContactId = Guid.NewGuid();
            var systemUserId = Guid.NewGuid();

            fakedContext.Initialize(new List<Entity>()
            {
                { new Contact() { Id = invitedContactId, OwnerId = systemUserId.ToEntityReference<SystemUser>(), FirstName = "Zdzisław", LastName = "Botak", EMailAddress1 = "bonie@OoO.pl" } },
                { new Contact() { Id = inviterContactId, FirstName = "Bogdan", LastName = "Bonie"} },
                { new Account() { Id = organizationId, Name = "ABW"} },
                { new invln_notificationsetting { Id = Guid.NewGuid(), invln_subject = "subject", invln_templateid = "tempalte id", invln_templatetypename = "COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION" } }
            });
            pluginContext.InputParameters = new ParameterCollection
            {
                { invln_sendinvitecontacttojoinorganisationRequest.Fields.invitedcontactid, invitedContactId.ToString()},
                { invln_sendinvitecontacttojoinorganisationRequest.Fields.invitercontactid, inviterContactId.ToString()},
                { invln_sendinvitecontacttojoinorganisationRequest.Fields.organisationid, organizationId.ToString()}
            };

            try
            {
                fakedContext.ExecutePluginWithConfigurations<SentInviteContactToJoinOrganisationPlugin>(pluginContext, "", "");
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
