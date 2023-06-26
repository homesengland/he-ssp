using DataverseModel;
using FakeItEasy;
using FakeXrmEasy;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Repositories.Implementations;
using HE.CRM.Plugins.Plugins.CustomApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace HE.CRM.Plugins.Tests.CustomApis
{
    [TestClass]
    public class GetContactRole
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
        public void GetContactRole_NoRoleForContact_ShouldAssingDefaultRoleAndReturnItsName()
        {
            Contact contact = new Contact()
            {
                Id = Guid.NewGuid(),
                EMailAddress1 = "test@test.pl",
            };

            invln_Webrole defaultRole = new invln_Webrole()
            {
                Id = Guid.NewGuid(),
                invln_Portalname = new OptionSetValue(858110001),
                invln_Name = "Default role",
            };

            fakedContext.Initialize(new List<Entity> { contact, defaultRole });
            fakedContext.AddRelationship("invln_Contact_Webrole", new XrmFakedRelationship("contactid", "invln_webroleid", Contact.EntityLogicalName, invln_Webrole.EntityLogicalName));

            Exception exception = null;
            try
            {
                var request = new invln_getcontactroleRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_ssid), contact.Id.ToString() },
                    {nameof(request.invln_portalid), defaultRole.invln_Portalname.Value.ToString() },
                    {nameof(request.invln_email), contact.EMailAddress1 },
                };

                fakedContext.ExecutePluginWithConfigurations<GetContactRolePlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception);
            Assert.AreEqual(pluginContext.OutputParameters.Values.ElementAt(0), defaultRole.invln_Name);
            A.CallTo(() => fakedContext.GetOrganizationService().Execute(A<AssociateRequest>.Ignored)).MustHaveHappened();

        }

        [TestMethod]
        public void GetContactRole_ContactHasRole_ShouldReturnItsName()
        {
            Contact contact = new Contact()
            {
                Id = Guid.NewGuid(),
                EMailAddress1 = "test@test.pl",
            };

            invln_Webrole role = new invln_Webrole()
            {
                Id = Guid.NewGuid(),
                invln_Portalname = new OptionSetValue(858110001),
                invln_Name = "role name",
            };

            var relationship = new Entity()
            {
                Id = Guid.NewGuid(),
                LogicalName = "invln_contact_webrole",
                Attributes = {
                    {"contactid", contact.Id },
                    {"invln_webroleid", role.Id}
                },
            };

            fakedContext.AddRelationship("invln_Contact_Webrole", new XrmFakedRelationship("contactid", "invln_webroleid", Contact.EntityLogicalName, invln_Webrole.EntityLogicalName));

            fakedContext.Initialize(new List<Entity> { contact, role, relationship });

            Exception exception = null;
            try
            {
                var request = new invln_getcontactroleRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_ssid), contact.Id.ToString() },
                    {nameof(request.invln_portalid), role.invln_Portalname.Value.ToString() },
                    {nameof(request.invln_email), contact.EMailAddress1 },
                };

                fakedContext.ExecutePluginWithConfigurations<GetContactRolePlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception);
            Assert.AreEqual(pluginContext.OutputParameters.Values.ElementAt(0), role.invln_Name);

        }

        [TestMethod]
        public void GetContactRole_ContactDoesNotExistAndDefaultRoleExists_ShouldCreateContactAndAssignDefaultRole()
        {
            invln_Webrole role = new invln_Webrole()
            {
                Id = Guid.NewGuid(),
                invln_Portalname = new OptionSetValue(858110001),
                invln_Name = "Default role",
            };

            fakedContext.AddRelationship("invln_Contact_Webrole", new XrmFakedRelationship("contactid", "invln_webroleid", Contact.EntityLogicalName, invln_Webrole.EntityLogicalName));

            fakedContext.Initialize(new List<Entity> {  role });

            Exception exception = null;
            try
            {
                var request = new invln_getcontactroleRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_ssid), Guid.NewGuid().ToString() },
                    {nameof(request.invln_portalid), role.invln_Portalname.Value.ToString() },
                    {nameof(request.invln_email), "test@test.pl" },
                };

                fakedContext.ExecutePluginWithConfigurations<GetContactRolePlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception);
            Assert.AreEqual(pluginContext.OutputParameters.Values.ElementAt(0), role.invln_Name);
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<Contact>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakedContext.GetOrganizationService().Execute(A<AssociateRequest>.Ignored)).MustHaveHappened();

        }

        [TestMethod]
        public void GetContactRole_TwoPortalsExistsAndContactHasRoleForOne_ShouldAssignNewRoleToSecondPortal()
        {
            Contact contact = new Contact()
            {
                Id = Guid.NewGuid(),
                EMailAddress1 = "test@test.pl",
            };

            invln_Webrole role = new invln_Webrole()
            {
                Id = Guid.NewGuid(),
                invln_Portalname = new OptionSetValue(858110001),
                invln_Name = "role name",
            };
            invln_Webrole defaultRole = new invln_Webrole()
            {
                Id = Guid.NewGuid(),
                invln_Name = "Default role",
            };

            var relationship = new Entity()
            {
                Id = Guid.NewGuid(),
                LogicalName = "invln_contact_webrole",
                Attributes = {
                    {"contactid", contact.Id },
                    {"invln_webroleid", role.Id}
                },
            };

            fakedContext.AddRelationship("invln_Contact_Webrole", new XrmFakedRelationship("contactid", "invln_webroleid", Contact.EntityLogicalName, invln_Webrole.EntityLogicalName));

            fakedContext.Initialize(new List<Entity> { contact, role, relationship, defaultRole });

            Exception exception = null;
            try
            {
                var request = new invln_getcontactroleRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_ssid), contact.Id.ToString() },
                    {nameof(request.invln_portalid), new OptionSetValue(858110002).Value.ToString() },
                    {nameof(request.invln_email), contact.EMailAddress1 },
                };

                fakedContext.ExecutePluginWithConfigurations<GetContactRolePlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception);
            Assert.AreEqual(pluginContext.OutputParameters.Values.ElementAt(0), defaultRole.invln_Name);
            A.CallTo(() => fakedContext.GetOrganizationService().Execute(A<AssociateRequest>.Ignored)).MustHaveHappened();
        }

    }
}
