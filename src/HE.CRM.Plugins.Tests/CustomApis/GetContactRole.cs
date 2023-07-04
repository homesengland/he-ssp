//using DataverseModel;
//using FakeItEasy;
//using FakeXrmEasy;
//using HE.CRM.Plugins.Plugins.CustomApi;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.Xrm.Sdk;
//using Microsoft.Xrm.Sdk.Messages;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace HE.CRM.Plugins.Tests.CustomApis
//{
//    [TestClass]
//    public class GetContactRole
//    {
//        private XrmFakedContext fakedContext;
//        private XrmFakedPluginExecutionContext pluginContext;


//        [TestInitialize]
//        public void Initialize()
//        {
//            fakedContext = new XrmFakedContext();
//            pluginContext = fakedContext.GetDefaultPluginContext();
//        }

//        [TestMethod]
//        public void GetContactRole_NoRoleForContact_ShouldAssingDefaultRoleAndReturnItsName()
//        {
//            Contact contact = new Contact()
//            {
//                Id = Guid.NewGuid(),
//                EMailAddress1 = "test@test.pl",
//            };

//            invln_portal portal = new invln_portal()
//            {
//                Id = Guid.NewGuid(),
//            };

//            invln_Webrole defaultRole = new invln_Webrole()
//            {
//                Id = Guid.NewGuid(),
//                invln_Portalid = portal.ToEntityReference(),
//                invln_Name = "Default role",
//                invln_Isdefaultrole = true,
//            };

//            fakedContext.Initialize(new List<Entity> { contact, defaultRole, portal });

//            Exception exception = null;
//            try
//            {
//                var request = new invln_getcontactroleRequest();
//                pluginContext.InputParameters = new ParameterCollection
//                {
//                    {nameof(request.invln_ssid), contact.Id.ToString() },
//                    {nameof(request.invln_portalid), defaultRole.invln_Portalid.Id.ToString() },
//                    {nameof(request.invln_email), contact.EMailAddress1 },
//                };

//                fakedContext.ExecutePluginWithConfigurations<GetContactRolePlugin>(pluginContext, "", "");
//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//            }

//            Assert.IsNull(exception);
//            Assert.AreEqual(pluginContext.OutputParameters.Values.ElementAt(0), defaultRole.invln_Name);
//            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_contactwebrole>.Ignored)).MustHaveHappened();

//        }

//        [TestMethod]
//        public void GetContactRole_ContactHasRole_ShouldReturnItsName()
//        {
//            Contact contact = new Contact()
//            {
//                Id = Guid.NewGuid(),
//                EMailAddress1 = "test@test.pl",
//            };

//            invln_portal portal = new invln_portal()
//            {
//                Id = Guid.NewGuid(),
//            };

//            invln_Webrole role = new invln_Webrole()
//            {
//                Id = Guid.NewGuid(),
//                invln_Portalid = portal.ToEntityReference(),
//                invln_Name = "role name",
//            };

//            var relationship = new invln_contactwebrole()
//            {
//                Id = Guid.NewGuid(),
//                invln_Contactid = contact.ToEntityReference(),
//                invln_Webroleid = role.ToEntityReference(),
//            };

//            fakedContext.Initialize(new List<Entity> { contact, role, relationship, portal });

//            Exception exception = null;
//            try
//            {
//                var request = new invln_getcontactroleRequest();
//                pluginContext.InputParameters = new ParameterCollection
//                {
//                    {nameof(request.invln_ssid), contact.Id.ToString() },
//                    {nameof(request.invln_portalid), role.invln_Portalid.Id.ToString() },
//                    {nameof(request.invln_email), contact.EMailAddress1 },
//                };

//                fakedContext.ExecutePluginWithConfigurations<GetContactRolePlugin>(pluginContext, "", "");
//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//            }

//            Assert.IsNull(exception);
//            Assert.AreEqual(pluginContext.OutputParameters.Values.ElementAt(0), role.invln_Name);

//        }

//        [TestMethod]
//        public void GetContactRole_ContactDoesNotExistAndDefaultRoleExists_ShouldCreateContactAndAssignDefaultRole()
//        {
//            invln_portal portal = new invln_portal()
//            {
//                Id = Guid.NewGuid(),
//            };

//            invln_Webrole role = new invln_Webrole()
//            {
//                Id = Guid.NewGuid(),
//                invln_Portalid = portal.ToEntityReference(),
//                invln_Name = "Default role",
//                invln_Isdefaultrole = true,
//            };

//            fakedContext.Initialize(new List<Entity> {  role, portal });

//            Exception exception = null;
//            try
//            {
//                var request = new invln_getcontactroleRequest();
//                pluginContext.InputParameters = new ParameterCollection
//                {
//                    {nameof(request.invln_ssid), Guid.NewGuid().ToString() },
//                    {nameof(request.invln_portalid), role.invln_Portalid.Id.ToString() },
//                    {nameof(request.invln_email), "test@test.pl" },
//                };

//                fakedContext.ExecutePluginWithConfigurations<GetContactRolePlugin>(pluginContext, "", "");
//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//            }

//            Assert.IsNull(exception);
//            Assert.AreEqual(pluginContext.OutputParameters.Values.ElementAt(0), role.invln_Name);
//            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<Contact>.Ignored)).MustHaveHappened();
//            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_contactwebrole>.Ignored)).MustHaveHappened();

//        }

//        [TestMethod]
//        public void GetContactRole_TwoPortalsExistsAndContactHasRoleForOne_ShouldAssignNewRoleToSecondPortal()
//        {
//            Contact contact = new Contact()
//            {
//                Id = Guid.NewGuid(),
//                EMailAddress1 = "test@test.pl",
//            };

//            invln_portal portal1 = new invln_portal()
//            {
//                Id = Guid.NewGuid(),
//            };
//            invln_portal portal2 = new invln_portal()
//            {
//                Id = Guid.NewGuid(),
//            };

//            invln_Webrole role = new invln_Webrole()
//            {
//                Id = Guid.NewGuid(),
//                invln_Portalid = portal1.ToEntityReference(),
//                invln_Name = "role name",
//            };
//            invln_Webrole defaultRole = new invln_Webrole()
//            {
//                Id = Guid.NewGuid(),
//                invln_Name = "Default role",
//                invln_Portalid = portal2.ToEntityReference(),
//                invln_Isdefaultrole = true,
//            };

//            var relationship = new invln_contactwebrole()
//            {
//                Id = Guid.NewGuid(),
//                invln_Contactid = contact.ToEntityReference(),
//                invln_Webroleid = role.ToEntityReference(),
//            };

//            fakedContext.Initialize(new List<Entity> { contact, role, relationship, defaultRole, portal1 });

//            Exception exception = null;
//            try
//            {
//                var request = new invln_getcontactroleRequest();
//                pluginContext.InputParameters = new ParameterCollection
//                {
//                    {nameof(request.invln_ssid), contact.Id.ToString() },
//                    {nameof(request.invln_portalid), portal2.Id.ToString() },
//                    {nameof(request.invln_email), contact.EMailAddress1 },
//                };

//                fakedContext.ExecutePluginWithConfigurations<GetContactRolePlugin>(pluginContext, "", "");
//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//            }

//            Assert.IsNull(exception);
//            Assert.AreEqual(pluginContext.OutputParameters.Values.ElementAt(0), defaultRole.invln_Name);
//            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_contactwebrole>.Ignored)).MustHaveHappened();
//        }

//    }
//}
