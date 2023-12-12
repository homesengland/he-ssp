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
	/// Status of the AHP Status Change
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum invln_ahpStatusChangeState
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 1,
	}
	
	/// <summary>
	/// Reason for the status of the AHP Status Change
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum invln_AHPStatusChange_StatusCode
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 1,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 2,
	}
	
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("invln_ahpstatuschange")]
	public partial class invln_AHPStatusChange : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		/// <summary>
		/// Available fields, a the time of codegen, for the invln_ahpstatuschange entity
		/// </summary>
		public static partial class Fields
		{
			public const string CreatedBy = "createdby";
			public const string CreatedByName = "createdbyname";
			public const string CreatedOn = "createdon";
			public const string CreatedOnBehalfBy = "createdonbehalfby";
			public const string CreatedOnBehalfByName = "createdonbehalfbyname";
			public const string ImportSequenceNumber = "importsequencenumber";
			public const string invln_Accepted = "invln_accepted";
			public const string invln_AHPApplication = "invln_ahpapplication";
			public const string invln_AHPApplicationName = "invln_ahpapplicationname";
			public const string invln_ahpstatuschange_AHPApplication_invln_sche = "invln_ahpstatuschange_AHPApplication_invln_sche";
			public const string invln_AHPStatusChangeId = "invln_ahpstatuschangeid";
			public const string Id = "invln_ahpstatuschangeid";
			public const string invln_Changefrom = "invln_changefrom";
			public const string invln_ChangeSource = "invln_changesource";
			public const string invln_Changeto = "invln_changeto";
			public const string invln_Comment = "invln_comment";
			public const string invln_Name = "invln_name";
			public const string lk_invln_ahpstatuschange_createdby = "lk_invln_ahpstatuschange_createdby";
			public const string lk_invln_ahpstatuschange_createdonbehalfby = "lk_invln_ahpstatuschange_createdonbehalfby";
			public const string lk_invln_ahpstatuschange_modifiedby = "lk_invln_ahpstatuschange_modifiedby";
			public const string lk_invln_ahpstatuschange_modifiedonbehalfby = "lk_invln_ahpstatuschange_modifiedonbehalfby";
			public const string ModifiedBy = "modifiedby";
			public const string ModifiedByName = "modifiedbyname";
			public const string ModifiedOn = "modifiedon";
			public const string ModifiedOnBehalfBy = "modifiedonbehalfby";
			public const string ModifiedOnBehalfByName = "modifiedonbehalfbyname";
			public const string OverriddenCreatedOn = "overriddencreatedon";
			public const string OwnerId = "ownerid";
			public const string OwnerIdName = "owneridname";
			public const string OwningBusinessUnit = "owningbusinessunit";
			public const string OwningBusinessUnitName = "owningbusinessunitname";
			public const string OwningTeam = "owningteam";
			public const string OwningTeamName = "owningteamname";
			public const string OwningUser = "owninguser";
			public const string OwningUserName = "owningusername";
			public const string StateCode = "statecode";
			public const string StatusCode = "statuscode";
			public const string team_invln_ahpstatuschange = "team_invln_ahpstatuschange";
			public const string TimeZoneRuleVersionNumber = "timezoneruleversionnumber";
			public const string user_invln_ahpstatuschange = "user_invln_ahpstatuschange";
			public const string UTCConversionTimeZoneCode = "utcconversiontimezonecode";
			public const string VersionNumber = "versionnumber";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public invln_AHPStatusChange() : 
				base(EntityLogicalName)
		{
		}
		
		public const string PrimaryIdAttribute = "invln_ahpstatuschangeid";
		
		public const string PrimaryNameAttribute = "invln_name";
		
		public const string EntitySchemaName = "invln_AHPStatusChange";
		
		public const string EntityLogicalName = "invln_ahpstatuschange";
		
		public const string EntityLogicalCollectionName = "invln_ahpstatuschanges";
		
		public const string EntitySetName = "invln_ahpstatuschanges";
		
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
		/// Unique identifier of the user who created the record.
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
		/// Date and time when the record was created.
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
		/// Unique identifier of the delegate user who created the record.
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
		/// Sequence number of the import that created this record.
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_accepted")]
		public System.Nullable<bool> invln_Accepted
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("invln_accepted");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_Accepted");
				this.SetAttributeValue("invln_accepted", value);
				this.OnPropertyChanged("invln_Accepted");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_ahpapplication")]
		public Microsoft.Xrm.Sdk.EntityReference invln_AHPApplication
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("invln_ahpapplication");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_AHPApplication");
				this.SetAttributeValue("invln_ahpapplication", value);
				this.OnPropertyChanged("invln_AHPApplication");
			}
		}
		
		/// <summary>
		/// Unique identifier for entity instances
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_ahpstatuschangeid")]
		public System.Nullable<System.Guid> invln_AHPStatusChangeId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("invln_ahpstatuschangeid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_AHPStatusChangeId");
				this.SetAttributeValue("invln_ahpstatuschangeid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("invln_AHPStatusChangeId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_ahpstatuschangeid")]
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
				this.invln_AHPStatusChangeId = value;
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changefrom")]
		public virtual Microsoft.Xrm.Sdk.OptionSetValue invln_Changefrom
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("invln_changefrom");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_Changefrom");
				this.SetAttributeValue("invln_changefrom", value);
				this.OnPropertyChanged("invln_Changefrom");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changesource")]
		public virtual Microsoft.Xrm.Sdk.OptionSetValue invln_ChangeSource
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("invln_changesource");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_ChangeSource");
				this.SetAttributeValue("invln_changesource", value);
				this.OnPropertyChanged("invln_ChangeSource");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changeto")]
		public virtual Microsoft.Xrm.Sdk.OptionSetValue invln_Changeto
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("invln_changeto");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_Changeto");
				this.SetAttributeValue("invln_changeto", value);
				this.OnPropertyChanged("invln_Changeto");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_comment")]
		public string invln_Comment
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("invln_comment");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_Comment");
				this.SetAttributeValue("invln_comment", value);
				this.OnPropertyChanged("invln_Comment");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_name")]
		public string invln_Name
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("invln_name");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_Name");
				this.SetAttributeValue("invln_name", value);
				this.OnPropertyChanged("invln_Name");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who modified the record.
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
		/// Date and time when the record was modified.
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
		/// Unique identifier of the delegate user who modified the record.
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
		/// Owner Id
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
		/// Unique identifier for the business unit that owns the record
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
		/// Unique identifier for the team that owns the record.
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
		/// Unique identifier for the user that owns the record.
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
		/// Status of the AHP Status Change
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
		/// Reason for the status of the AHP Status Change
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
		
		/// <summary>
		/// Version Number
		/// </summary>
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
		/// N:1 invln_ahpstatuschange_AHPApplication_invln_sche
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_ahpapplication")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("invln_ahpstatuschange_AHPApplication_invln_sche")]
		public DataverseModel.invln_scheme invln_ahpstatuschange_AHPApplication_invln_sche
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.invln_scheme>("invln_ahpstatuschange_AHPApplication_invln_sche", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_ahpstatuschange_AHPApplication_invln_sche");
				this.SetRelatedEntity<DataverseModel.invln_scheme>("invln_ahpstatuschange_AHPApplication_invln_sche", null, value);
				this.OnPropertyChanged("invln_ahpstatuschange_AHPApplication_invln_sche");
			}
		}
		
		/// <summary>
		/// N:1 lk_invln_ahpstatuschange_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_invln_ahpstatuschange_createdby")]
		public DataverseModel.SystemUser lk_invln_ahpstatuschange_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.SystemUser>("lk_invln_ahpstatuschange_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_invln_ahpstatuschange_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_invln_ahpstatuschange_createdonbehalfby")]
		public DataverseModel.SystemUser lk_invln_ahpstatuschange_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.SystemUser>("lk_invln_ahpstatuschange_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_invln_ahpstatuschange_createdonbehalfby");
				this.SetRelatedEntity<DataverseModel.SystemUser>("lk_invln_ahpstatuschange_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_invln_ahpstatuschange_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_invln_ahpstatuschange_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_invln_ahpstatuschange_modifiedby")]
		public DataverseModel.SystemUser lk_invln_ahpstatuschange_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.SystemUser>("lk_invln_ahpstatuschange_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_invln_ahpstatuschange_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_invln_ahpstatuschange_modifiedonbehalfby")]
		public DataverseModel.SystemUser lk_invln_ahpstatuschange_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.SystemUser>("lk_invln_ahpstatuschange_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_invln_ahpstatuschange_modifiedonbehalfby");
				this.SetRelatedEntity<DataverseModel.SystemUser>("lk_invln_ahpstatuschange_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_invln_ahpstatuschange_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 team_invln_ahpstatuschange
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("team_invln_ahpstatuschange")]
		public DataverseModel.Team team_invln_ahpstatuschange
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.Team>("team_invln_ahpstatuschange", null);
			}
		}
		
		/// <summary>
		/// N:1 user_invln_ahpstatuschange
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owninguser")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("user_invln_ahpstatuschange")]
		public DataverseModel.SystemUser user_invln_ahpstatuschange
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<DataverseModel.SystemUser>("user_invln_ahpstatuschange", null);
			}
		}
		
		/// <summary>
		/// Constructor for populating via LINQ queries given a LINQ anonymous type
		/// <param name="anonymousType">LINQ anonymous type.</param>
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public invln_AHPStatusChange(object anonymousType) : 
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
                        Attributes["invln_ahpstatuschangeid"] = base.Id;
                        break;
                    case "invln_ahpstatuschangeid":
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
