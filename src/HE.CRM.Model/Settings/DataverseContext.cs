#pragma warning disable CS1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: Microsoft.Xrm.Sdk.Client.ProxyTypesAssemblyAttribute()]

namespace DataverseModel
{
	
	
	/// <summary>
	/// Represents a source of entities bound to a Dataverse service. It tracks and manages changes made to the retrieved entities.
	/// </summary>
	public partial class DataverseContext : Microsoft.Xrm.Sdk.Client.OrganizationServiceContext
	{
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public DataverseContext(Microsoft.Xrm.Sdk.IOrganizationService service) : 
				base(service)
		{
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.Account"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.Account> AccountSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.Account>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.Contact"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.Contact> ContactSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.Contact>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.EnvironmentVariableDefinition"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.EnvironmentVariableDefinition> EnvironmentVariableDefinitionSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.EnvironmentVariableDefinition>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.EnvironmentVariableValue"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.EnvironmentVariableValue> EnvironmentVariableValueSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.EnvironmentVariableValue>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_AHGLocalAuthorities"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_AHGLocalAuthorities> invln_AHGLocalAuthoritiesSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_AHGLocalAuthorities>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_ahpcontract"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_ahpcontract> invln_ahpcontractSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_ahpcontract>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_ahpproject"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_ahpproject> invln_ahpprojectSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_ahpproject>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_AHPStatusChange"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_AHPStatusChange> invln_AHPStatusChangeSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_AHPStatusChange>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_BorrowerPreviousSchemes"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_BorrowerPreviousSchemes> invln_BorrowerPreviousSchemesSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_BorrowerPreviousSchemes>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_Conditions"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_Conditions> invln_ConditionsSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_Conditions>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_Consortium"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_Consortium> invln_ConsortiumSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_Consortium>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_ConsortiumMember"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_ConsortiumMember> invln_ConsortiumMemberSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_ConsortiumMember>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_contact_invln_loanapplication"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_contact_invln_loanapplication> invln_contact_invln_loanapplicationSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_contact_invln_loanapplication>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_contactwebrole"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_contactwebrole> invln_contactwebroleSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_contactwebrole>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_contract"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_contract> invln_contractSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_contract>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_DeliveryPhase"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_DeliveryPhase> invln_DeliveryPhaseSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_DeliveryPhase>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_Emailnotification"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_Emailnotification> invln_EmailnotificationSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_Emailnotification>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_EmailTemplate"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_EmailTemplate> invln_EmailTemplateSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_EmailTemplate>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_Externalcomms"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_Externalcomms> invln_ExternalcommsSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_Externalcomms>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_FinancialCovenants"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_FinancialCovenants> invln_FinancialCovenantsSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_FinancialCovenants>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_FrontDoorProjectPOC"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_FrontDoorProjectPOC> invln_FrontDoorProjectPOCSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_FrontDoorProjectPOC>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_FrontDoorProjectSitePOC"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_FrontDoorProjectSitePOC> invln_FrontDoorProjectSitePOCSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_FrontDoorProjectSitePOC>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_govnotifyemail"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_govnotifyemail> invln_govnotifyemailSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_govnotifyemail>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_grantbenchmark"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_grantbenchmark> invln_grantbenchmarkSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_grantbenchmark>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_homesindeliveryphase"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_homesindeliveryphase> invln_homesindeliveryphaseSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_homesindeliveryphase>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_HomeType"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_HomeType> invln_HomeTypeSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_HomeType>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_Housetype"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_Housetype> invln_HousetypeSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_Housetype>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_ISP"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_ISP> invln_ISPSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_ISP>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_KeyRisks"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_KeyRisks> invln_KeyRisksSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_KeyRisks>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_Loanapplication"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_Loanapplication> invln_LoanapplicationSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_Loanapplication>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_Loanstatuschange"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_Loanstatuschange> invln_LoanstatuschangeSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_Loanstatuschange>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_localauthority"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_localauthority> invln_localauthoritySet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_localauthority>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_milestoneframeworkitem"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_milestoneframeworkitem> invln_milestoneframeworkitemSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_milestoneframeworkitem>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_Milestones"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_Milestones> invln_MilestonesSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_Milestones>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_ndss"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_ndss> invln_ndssSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_ndss>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_notificationsetting"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_notificationsetting> invln_notificationsettingSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_notificationsetting>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_organisationchangerequest"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_organisationchangerequest> invln_organisationchangerequestSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_organisationchangerequest>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_plot"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_plot> invln_plotSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_plot>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_portal"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_portal> invln_portalSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_portal>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_portalpermissionlevel"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_portalpermissionlevel> invln_portalpermissionlevelSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_portalpermissionlevel>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_Precomplete"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_Precomplete> invln_PrecompleteSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_Precomplete>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_programme"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_programme> invln_programmeSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_programme>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_ProjectSpecificCondition"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_ProjectSpecificCondition> invln_ProjectSpecificConditionSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_ProjectSpecificCondition>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_reviewapproval"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_reviewapproval> invln_reviewapprovalSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_reviewapproval>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_scheme"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_scheme> invln_schemeSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_scheme>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_SecretVariable"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_SecretVariable> invln_SecretVariableSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_SecretVariable>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_SiteDetails"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_SiteDetails> invln_SiteDetailsSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_SiteDetails>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_Sites"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_Sites> invln_SitesSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_Sites>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_StandardCondition"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_StandardCondition> invln_StandardConditionSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_StandardCondition>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_VfT"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_VfT> invln_VfTSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_VfT>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_Webrole"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_Webrole> invln_WebroleSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_Webrole>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.SharePointDocumentLocation"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.SharePointDocumentLocation> SharePointDocumentLocationSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.SharePointDocumentLocation>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.SharePointSite"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.SharePointSite> SharePointSiteSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.SharePointSite>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.SystemUser"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.SystemUser> SystemUserSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.SystemUser>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.Team"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.Team> TeamSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.Team>();
			}
		}
	}
}
#pragma warning restore CS1591
