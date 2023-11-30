#pragma warning disable CS1591
// Code Generated by DLaB.ModelBuilderExtensions
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataverseModel
{
	
	
	/// <summary>
	/// Status of the SharePoint document location.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum sharepointdocumentlocationState
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 1,
	}
	
	/// <summary>
	/// Reason for the status of the SharePoint document location.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum SharePointDocumentLocation_StatusCode
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 1,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 2,
	}
	
	/// <summary>
	/// Document libraries or folders on a SharePoint server from where documents can be managed in Microsoft Dynamics 365.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("sharepointdocumentlocation")]
	public partial class SharePointDocumentLocation : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		/// <summary>
		/// Available fields, a the time of codegen, for the sharepointdocumentlocation entity
		/// </summary>
		public static partial class Fields
		{
			public const string AbsoluteURL = "absoluteurl";
			public const string Account_SharepointDocumentLocation = "Account_SharepointDocumentLocation";
			public const string CreatedBy = "createdby";
			public const string CreatedByName = "createdbyname";
			public const string CreatedOn = "createdon";
			public const string CreatedOnBehalfBy = "createdonbehalfby";
			public const string CreatedOnBehalfByName = "createdonbehalfbyname";
			public const string Description = "description";
			public const string ExchangeRate = "exchangerate";
			public const string ImportSequenceNumber = "importsequencenumber";
			public const string invln_conditions_SharePointDocumentLocations = "invln_conditions_SharePointDocumentLocations";
			public const string invln_loanapplication_SharePointDocumentLocations = "invln_loanapplication_SharePointDocumentLocations";
			public const string invln_scheme_SharePointDocumentLocations = "invln_scheme_SharePointDocumentLocations";
			public const string lk_sharepointdocumentlocationbase_createdby = "lk_sharepointdocumentlocationbase_createdby";
			public const string lk_sharepointdocumentlocationbase_createdonbehalfby = "lk_sharepointdocumentlocationbase_createdonbehalfby";
			public const string lk_sharepointdocumentlocationbase_modifiedby = "lk_sharepointdocumentlocationbase_modifiedby";
			public const string lk_sharepointdocumentlocationbase_modifiedonbehalfby = "lk_sharepointdocumentlocationbase_modifiedonbehalfby";
			public const string LocationType = "locationtype";
			public const string ModifiedBy = "modifiedby";
			public const string ModifiedByName = "modifiedbyname";
			public const string ModifiedOn = "modifiedon";
			public const string ModifiedOnBehalfBy = "modifiedonbehalfby";
			public const string ModifiedOnBehalfByName = "modifiedonbehalfbyname";
			public const string msft_DataState = "msft_datastate";
			public const string Name = "name";
			public const string OverriddenCreatedOn = "overriddencreatedon";
			public const string OwnerId = "ownerid";
			public const string OwnerIdName = "owneridname";
			public const string OwningBusinessUnit = "owningbusinessunit";
			public const string OwningBusinessUnitName = "owningbusinessunitname";
			public const string OwningTeam = "owningteam";
			public const string OwningTeamName = "owningteamname";
			public const string OwningUser = "owninguser";
			public const string OwningUserName = "owningusername";
			public const string ParentSiteOrLocation = "parentsiteorlocation";
			public const string ParentSiteOrLocationName = "parentsiteorlocationname";
			public const string Referencedsharepointdocumentlocation_parent_sharepointdocumentlocation = "Referencedsharepointdocumentlocation_parent_sharepointdocumentlocation";
			public const string RegardingObjectId = "regardingobjectid";
			public const string RegardingObjectIdName = "regardingobjectidname";
			public const string RelativeUrl = "relativeurl";
			public const string ServiceType = "servicetype";
			public const string Referencingsharepointdocumentlocation_parent_sharepointdocumentlocation = "sharepointdocumentlocation_parent_sharepointdocumentlocation";
			public const string SharePointDocumentLocationId = "sharepointdocumentlocationid";
			public const string Id = "sharepointdocumentlocationid";
			public const string SiteCollectionId = "sitecollectionid";
			public const string StateCode = "statecode";
			public const string StatusCode = "statuscode";
			public const string team_sharepointdocumentlocation = "team_sharepointdocumentlocation";
			public const string TimeZoneRuleVersionNumber = "timezoneruleversionnumber";
			public const string TransactionCurrencyId = "transactioncurrencyid";
			public const string TransactionCurrencyIdName = "transactioncurrencyidname";
			public const string user_sharepointdocumentlocation = "user_sharepointdocumentlocation";
			public const string UserId = "userid";
			public const string UTCConversionTimeZoneCode = "utcconversiontimezonecode";
			public const string VersionNumber = "versionnumber";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public SharePointDocumentLocation() : 
				base(EntityLogicalName)
		{
		}
		
		public const string PrimaryIdAttribute = "sharepointdocumentlocationid";
		
		public const string PrimaryNameAttribute = "name";
		
		public const string EntitySchemaName = "SharePointDocumentLocation";
		
		public const string EntityLogicalName = "sharepointdocumentlocation";
		
		public const string EntityLogicalCollectionName = "sharePointdocumentlocations";
		
		public const string EntitySetName = "sharepointdocumentlocations";
		
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		
		public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
		
		[System.Diagnostics.DebuggerNonUserCode()]
		private void OnPropertyChanged(string propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
		
		[System.Diagnostics.DebuggerNonUserCode()]
		private void OnPropertyChanging(string propertyName)
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
			}
		}
		
		/// <summary>
		/// Absolute URL of the SharePoint document location.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("absoluteurl")]
		public string AbsoluteURL
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("absoluteurl");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("AbsoluteURL");
				this.SetAttributeValue("absoluteurl", value);
				this.OnPropertyChanged("AbsoluteURL");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who created the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		public Microsoft.Xrm.Sdk.EntityReference CreatedBy
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
			}
		}
		
		/// <summary>
		/// Date and time when the SharePoint document location record was created.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
		public System.Nullable<System.DateTime> CreatedOn
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
			}
		}
		
		/// <summary>
		/// Unique identifier of the delegate user who created the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("CreatedOnBehalfBy");
				this.SetAttributeValue("createdonbehalfby", value);
				this.OnPropertyChanged("CreatedOnBehalfBy");
			}
		}
		
		/// <summary>
		/// Description of the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
		public string Description
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("description");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Description");
				this.SetAttributeValue("description", value);
				this.OnPropertyChanged("Description");
			}
		}
		
		/// <summary>
		/// Exchange rate between the currency associated with the SharePoint document location record and the base currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("exchangerate")]
		public System.Nullable<decimal> ExchangeRate
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("exchangerate");
			}
		}
		
		/// <summary>
		/// Sequence number of the import that created the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("importsequencenumber")]
		public System.Nullable<int> ImportSequenceNumber
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("importsequencenumber");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ImportSequenceNumber");
				this.SetAttributeValue("importsequencenumber", value);
				this.OnPropertyChanged("ImportSequenceNumber");
			}
		}
		
		/// <summary>
		/// Location type of the SharePoint document location.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("locationtype")]
		public virtual Microsoft.Xrm.Sdk.OptionSetValue LocationType
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("locationtype");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("LocationType");
				this.SetAttributeValue("locationtype", value);
				this.OnPropertyChanged("LocationType");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who last modified the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
			}
		}
		
		/// <summary>
		/// Date and time when the SharePoint document location record was last modified.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
		public System.Nullable<System.DateTime> ModifiedOn
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
			}
		}
		
		/// <summary>
		/// Unique identifier of the delegate user who modified the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ModifiedOnBehalfBy");
				this.SetAttributeValue("modifiedonbehalfby", value);
				this.OnPropertyChanged("ModifiedOnBehalfBy");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("msft_datastate")]
		public virtual Microsoft.Xrm.Sdk.OptionSetValue msft_DataState
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("msft_datastate");
			}
		}
		
		/// <summary>
		/// Name of the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
		public string Name
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("name");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Name");
				this.SetAttributeValue("name", value);
				this.OnPropertyChanged("Name");
			}
		}
		
		/// <summary>
		/// Date and time that the record was migrated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overriddencreatedon")]
		public System.Nullable<System.DateTime> OverriddenCreatedOn
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("overriddencreatedon");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("OverriddenCreatedOn");
				this.SetAttributeValue("overriddencreatedon", value);
				this.OnPropertyChanged("OverriddenCreatedOn");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user or team who owns the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ownerid")]
		public Microsoft.Xrm.Sdk.EntityReference OwnerId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("ownerid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("OwnerId");
				this.SetAttributeValue("ownerid", value);
				this.OnPropertyChanged("OwnerId");
			}
		}
		
		/// <summary>
		/// Unique identifier of the business unit that owns the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningbusinessunit")]
		public Microsoft.Xrm.Sdk.EntityReference OwningBusinessUnit
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owningbusinessunit");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("OwningBusinessUnit");
				this.SetAttributeValue("owningbusinessunit", value);
				this.OnPropertyChanged("OwningBusinessUnit");
			}
		}
		
		/// <summary>
		/// Unique identifier of the team who owns the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		public Microsoft.Xrm.Sdk.EntityReference OwningTeam
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owningteam");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who owns the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owninguser")]
		public Microsoft.Xrm.Sdk.EntityReference OwningUser
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owninguser");
			}
		}
		
		/// <summary>
		/// Unique identifier of the parent site or location.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("parentsiteorlocation")]
		public Microsoft.Xrm.Sdk.EntityReference ParentSiteOrLocation
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("parentsiteorlocation");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ParentSiteOrLocation");
				this.SetAttributeValue("parentsiteorlocation", value);
				this.OnPropertyChanged("ParentSiteOrLocation");
			}
		}
		
		/// <summary>
		/// Unique identifier of the object with which the SharePoint document location record is associated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("regardingobjectid")]
		public Microsoft.Xrm.Sdk.EntityReference RegardingObjectId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("regardingobjectid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("RegardingObjectId");
				this.SetAttributeValue("regardingobjectid", value);
				this.OnPropertyChanged("RegardingObjectId");
			}
		}
		
		/// <summary>
		/// Relative URL of the SharePoint document location.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("relativeurl")]
		public string RelativeUrl
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("relativeurl");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("RelativeUrl");
				this.SetAttributeValue("relativeurl", value);
				this.OnPropertyChanged("RelativeUrl");
			}
		}
		
		/// <summary>
		/// Shows the service type of the SharePoint site.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("servicetype")]
		public virtual Microsoft.Xrm.Sdk.OptionSetValue ServiceType
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("servicetype");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ServiceType");
				this.SetAttributeValue("servicetype", value);
				this.OnPropertyChanged("ServiceType");
			}
		}
		
		/// <summary>
		/// Unique identifier of the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sharepointdocumentlocationid")]
		public System.Nullable<System.Guid> SharePointDocumentLocationId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("sharepointdocumentlocationid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("SharePointDocumentLocationId");
				this.SetAttributeValue("sharepointdocumentlocationid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("SharePointDocumentLocationId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sharepointdocumentlocationid")]
		public override System.Guid Id
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return base.Id;
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.SharePointDocumentLocationId = value;
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sitecollectionid")]
		public System.Nullable<System.Guid> SiteCollectionId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("sitecollectionid");
			}
		}
		
		/// <summary>
		/// Status of the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public virtual Microsoft.Xrm.Sdk.OptionSetValue StateCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("StateCode");
				this.SetAttributeValue("statecode", value);
				this.OnPropertyChanged("StateCode");
			}
		}
		
		/// <summary>
		/// Reason for the status of the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public virtual Microsoft.Xrm.Sdk.OptionSetValue StatusCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statuscode");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("StatusCode");
				this.SetAttributeValue("statuscode", value);
				this.OnPropertyChanged("StatusCode");
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("timezoneruleversionnumber")]
		public System.Nullable<int> TimeZoneRuleVersionNumber
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("timezoneruleversionnumber");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("TimeZoneRuleVersionNumber");
				this.SetAttributeValue("timezoneruleversionnumber", value);
				this.OnPropertyChanged("TimeZoneRuleVersionNumber");
			}
		}
		
		/// <summary>
		/// Unique identifier of the currency associated with the SharePoint document location record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("transactioncurrencyid")]
		public Microsoft.Xrm.Sdk.EntityReference TransactionCurrencyId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("transactioncurrencyid");
			}
		}
		
		/// <summary>
		/// Choose the user who owns the SharePoint document location.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("userid")]
		public System.Nullable<System.Guid> UserId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("userid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("UserId");
				this.SetAttributeValue("userid", value);
				this.OnPropertyChanged("UserId");
			}
		}
		
		/// <summary>
		/// Time zone code that was in use when the record was created.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("utcconversiontimezonecode")]
		public System.Nullable<int> UTCConversionTimeZoneCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("utcconversiontimezonecode");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("UTCConversionTimeZoneCode");
				this.SetAttributeValue("utcconversiontimezonecode", value);
				this.OnPropertyChanged("UTCConversionTimeZoneCode");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
		public System.Nullable<long> VersionNumber
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
			}
		}
		
		/// <summary>
		/// 1:N sharepointdocumentlocation_parent_sharepointdocumentlocation
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sharepointdocumentlocation_parent_sharepointdocumentlocation", Microsoft.Xrm.Sdk.EntityRole.Referenced)]
		public System.Collections.Generic.IEnumerable<DataverseModel.SharePointDocumentLocation> Referencedsharepointdocumentlocation_parent_sharepointdocumentlocation
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<DataverseModel.SharePointDocumentLocation>("sharepointdocumentlocation_parent_sharepointdocumentlocation", Microsoft.Xrm.Sdk.EntityRole.Referenced);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Referencedsharepointdocumentlocation_parent_sharepointdocumentlocation");
				this.SetRelatedEntities<DataverseModel.SharePointDocumentLocation>("sharepointdocumentlocation_parent_sharepointdocumentlocation", Microsoft.Xrm.Sdk.EntityRole.Referenced, value);
				this.OnPropertyChanged("Referencedsharepointdocumentlocation_parent_sharepointdocumentlocation");
			}
		}
		
		/// <summary>
		/// N:1 Account_SharepointDocumentLocation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("regardingobjectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Account_SharepointDocumentLocation")]
		public DataverseModel.Account Account_SharepointDocumentLocation
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.Account>("Account_SharepointDocumentLocation", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Account_SharepointDocumentLocation");
				this.SetRelatedEntity<DataverseModel.Account>("Account_SharepointDocumentLocation", null, value);
				this.OnPropertyChanged("Account_SharepointDocumentLocation");
			}
		}
		
		/// <summary>
		/// N:1 invln_conditions_SharePointDocumentLocations
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("regardingobjectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("invln_conditions_SharePointDocumentLocations")]
		public DataverseModel.invln_Conditions invln_conditions_SharePointDocumentLocations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.invln_Conditions>("invln_conditions_SharePointDocumentLocations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_conditions_SharePointDocumentLocations");
				this.SetRelatedEntity<DataverseModel.invln_Conditions>("invln_conditions_SharePointDocumentLocations", null, value);
				this.OnPropertyChanged("invln_conditions_SharePointDocumentLocations");
			}
		}
		
		/// <summary>
		/// N:1 invln_loanapplication_SharePointDocumentLocations
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("regardingobjectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("invln_loanapplication_SharePointDocumentLocations")]
		public DataverseModel.invln_Loanapplication invln_loanapplication_SharePointDocumentLocations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.invln_Loanapplication>("invln_loanapplication_SharePointDocumentLocations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_loanapplication_SharePointDocumentLocations");
				this.SetRelatedEntity<DataverseModel.invln_Loanapplication>("invln_loanapplication_SharePointDocumentLocations", null, value);
				this.OnPropertyChanged("invln_loanapplication_SharePointDocumentLocations");
			}
		}
		
		/// <summary>
		/// N:1 invln_scheme_SharePointDocumentLocations
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("regardingobjectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("invln_scheme_SharePointDocumentLocations")]
		public DataverseModel.invln_scheme invln_scheme_SharePointDocumentLocations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.invln_scheme>("invln_scheme_SharePointDocumentLocations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_scheme_SharePointDocumentLocations");
				this.SetRelatedEntity<DataverseModel.invln_scheme>("invln_scheme_SharePointDocumentLocations", null, value);
				this.OnPropertyChanged("invln_scheme_SharePointDocumentLocations");
			}
		}
		
		/// <summary>
		/// N:1 lk_sharepointdocumentlocationbase_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_sharepointdocumentlocationbase_createdby")]
		public DataverseModel.SystemUser lk_sharepointdocumentlocationbase_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.SystemUser>("lk_sharepointdocumentlocationbase_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_sharepointdocumentlocationbase_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_sharepointdocumentlocationbase_createdonbehalfby")]
		public DataverseModel.SystemUser lk_sharepointdocumentlocationbase_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.SystemUser>("lk_sharepointdocumentlocationbase_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_sharepointdocumentlocationbase_createdonbehalfby");
				this.SetRelatedEntity<DataverseModel.SystemUser>("lk_sharepointdocumentlocationbase_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_sharepointdocumentlocationbase_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_sharepointdocumentlocationbase_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_sharepointdocumentlocationbase_modifiedby")]
		public DataverseModel.SystemUser lk_sharepointdocumentlocationbase_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.SystemUser>("lk_sharepointdocumentlocationbase_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_sharepointdocumentlocationbase_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_sharepointdocumentlocationbase_modifiedonbehalfby")]
		public DataverseModel.SystemUser lk_sharepointdocumentlocationbase_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.SystemUser>("lk_sharepointdocumentlocationbase_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_sharepointdocumentlocationbase_modifiedonbehalfby");
				this.SetRelatedEntity<DataverseModel.SystemUser>("lk_sharepointdocumentlocationbase_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_sharepointdocumentlocationbase_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 sharepointdocumentlocation_parent_sharepointdocumentlocation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("parentsiteorlocation")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sharepointdocumentlocation_parent_sharepointdocumentlocation", Microsoft.Xrm.Sdk.EntityRole.Referencing)]
		public DataverseModel.SharePointDocumentLocation Referencingsharepointdocumentlocation_parent_sharepointdocumentlocation
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.SharePointDocumentLocation>("sharepointdocumentlocation_parent_sharepointdocumentlocation", Microsoft.Xrm.Sdk.EntityRole.Referencing);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Referencingsharepointdocumentlocation_parent_sharepointdocumentlocation");
				this.SetRelatedEntity<DataverseModel.SharePointDocumentLocation>("sharepointdocumentlocation_parent_sharepointdocumentlocation", Microsoft.Xrm.Sdk.EntityRole.Referencing, value);
				this.OnPropertyChanged("Referencingsharepointdocumentlocation_parent_sharepointdocumentlocation");
			}
		}
		
		/// <summary>
		/// N:1 team_sharepointdocumentlocation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("team_sharepointdocumentlocation")]
		public DataverseModel.Team team_sharepointdocumentlocation
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.Team>("team_sharepointdocumentlocation", null);
			}
		}
		
		/// <summary>
		/// N:1 user_sharepointdocumentlocation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owninguser")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("user_sharepointdocumentlocation")]
		public DataverseModel.SystemUser user_sharepointdocumentlocation
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.SystemUser>("user_sharepointdocumentlocation", null);
			}
		}
		
		/// <summary>
		/// Constructor for populating via LINQ queries given a LINQ anonymous type
		/// <param name="anonymousType">LINQ anonymous type.</param>
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public SharePointDocumentLocation(object anonymousType) : 
				this()
		{
            foreach (var p in anonymousType.GetType().GetProperties())
            {
                var value = p.GetValue(anonymousType, null);
                var name = p.Name.ToLower();
            
                if (name.EndsWith("enum") && value.GetType().BaseType == typeof(System.Enum))
                {
                    value = new Microsoft.Xrm.Sdk.OptionSetValue((int) value);
                    name = name.Remove(name.Length - "enum".Length);
                }
            
                switch (name)
                {
                    case "id":
                        base.Id = (System.Guid)value;
                        Attributes["sharepointdocumentlocationid"] = base.Id;
                        break;
                    case "sharepointdocumentlocationid":
                        var id = (System.Nullable<System.Guid>) value;
                        if(id == null){ continue; }
                        base.Id = id.Value;
                        Attributes[name] = base.Id;
                        break;
                    case "formattedvalues":
                        // Add Support for FormattedValues
                        FormattedValues.AddRange((Microsoft.Xrm.Sdk.FormattedValueCollection)value);
                        break;
                    default:
                        Attributes[name] = value;
                        break;
                }
            }
		}
	}
}
#pragma warning restore CS1591
