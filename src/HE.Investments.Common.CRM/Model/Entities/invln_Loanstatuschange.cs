#pragma warning disable CS1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HE.Investments.Common.CRM.Model
{
	
	
	/// <summary>
	/// Reason for the status of the Loan status change
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum invln_Loanstatuschange_StatusCode
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 1,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 2,
	}
	
	/// <summary>
	/// Status of the Loan status change
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum invln_LoanstatuschangeState
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 1,
	}
	
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("invln_loanstatuschange")]
	public partial class invln_Loanstatuschange : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		/// <summary>
		/// Available fields, a the time of codegen, for the invln_loanstatuschange entity
		/// </summary>
		public partial class Fields
		{
			public const string CreatedBy = "createdby";
			public const string CreatedByName = "createdbyname";
			public const string CreatedByYomiName = "createdbyyominame";
			public const string CreatedOn = "createdon";
			public const string CreatedOnBehalfBy = "createdonbehalfby";
			public const string CreatedOnBehalfByName = "createdonbehalfbyname";
			public const string CreatedOnBehalfByYomiName = "createdonbehalfbyyominame";
			public const string ImportSequenceNumber = "importsequencenumber";
			public const string invln_Accepted = "invln_accepted";
			public const string invln_acceptedName = "invln_acceptedname";
			public const string invln_changedbycontactid = "invln_changedbycontactid";
			public const string invln_changedbycontactidName = "invln_changedbycontactidname";
			public const string invln_changedbycontactidYomiName = "invln_changedbycontactidyominame";
			public const string invln_changedbyuserid = "invln_changedbyuserid";
			public const string invln_changedbyuseridName = "invln_changedbyuseridname";
			public const string invln_changedbyuseridYomiName = "invln_changedbyuseridyominame";
			public const string invln_changefrom = "invln_changefrom";
			public const string invln_changefromName = "invln_changefromname";
			public const string invln_changesource = "invln_changesource";
			public const string invln_changesourceName = "invln_changesourcename";
			public const string invln_changeto = "invln_changeto";
			public const string invln_changetoName = "invln_changetoname";
			public const string invln_Comment = "invln_comment";
			public const string invln_contact_invln_loanstatuschange_changedbycontactid = "invln_contact_invln_loanstatuschange_changedbycontactid";
			public const string invln_Loanapplication = "invln_loanapplication";
			public const string invln_LoanapplicationName = "invln_loanapplicationname";
			public const string invln_loanstatuschange_Loanapplication = "invln_loanstatuschange_Loanapplication";
			public const string invln_LoanstatuschangeId = "invln_loanstatuschangeid";
			public const string Id = "invln_loanstatuschangeid";
			public const string invln_Name = "invln_name";
			public const string invln_systemuser_invln_loanstatuschange_changedbyuserid = "invln_systemuser_invln_loanstatuschange_changedbyuserid";
			public const string lk_invln_loanstatuschange_createdby = "lk_invln_loanstatuschange_createdby";
			public const string lk_invln_loanstatuschange_createdonbehalfby = "lk_invln_loanstatuschange_createdonbehalfby";
			public const string lk_invln_loanstatuschange_modifiedby = "lk_invln_loanstatuschange_modifiedby";
			public const string lk_invln_loanstatuschange_modifiedonbehalfby = "lk_invln_loanstatuschange_modifiedonbehalfby";
			public const string ModifiedBy = "modifiedby";
			public const string ModifiedByName = "modifiedbyname";
			public const string ModifiedByYomiName = "modifiedbyyominame";
			public const string ModifiedOn = "modifiedon";
			public const string ModifiedOnBehalfBy = "modifiedonbehalfby";
			public const string ModifiedOnBehalfByName = "modifiedonbehalfbyname";
			public const string ModifiedOnBehalfByYomiName = "modifiedonbehalfbyyominame";
			public const string OverriddenCreatedOn = "overriddencreatedon";
			public const string OwnerId = "ownerid";
			public const string OwnerIdName = "owneridname";
			public const string OwnerIdYomiName = "owneridyominame";
			public const string OwningBusinessUnit = "owningbusinessunit";
			public const string OwningBusinessUnitName = "owningbusinessunitname";
			public const string OwningTeam = "owningteam";
			public const string OwningUser = "owninguser";
			public const string StateCode = "statecode";
			public const string statecodeName = "statecodename";
			public const string StatusCode = "statuscode";
			public const string statuscodeName = "statuscodename";
			public const string team_invln_loanstatuschange = "team_invln_loanstatuschange";
			public const string TimeZoneRuleVersionNumber = "timezoneruleversionnumber";
			public const string user_invln_loanstatuschange = "user_invln_loanstatuschange";
			public const string UTCConversionTimeZoneCode = "utcconversiontimezonecode";
			public const string VersionNumber = "versionnumber";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public invln_Loanstatuschange() : 
				base(EntityLogicalName)
		{
		}
		
		public const string PrimaryIdAttribute = "invln_loanstatuschangeid";
		
		public const string PrimaryNameAttribute = "invln_name";
		
		public const string EntitySchemaName = "invln_Loanstatuschange";
		
		public const string EntityLogicalName = "invln_loanstatuschange";
		
		public const string EntityLogicalCollectionName = "invln_loanstatuschanges";
		
		public const string EntitySetName = "invln_loanstatuschanges";
		
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdbyname")]
		public string CreatedByName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("createdby"))
				{
					return this.FormattedValues["createdby"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdbyyominame")]
		public string CreatedByYomiName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("createdby"))
				{
					return this.FormattedValues["createdby"];
				}
				else
				{
					return default(string);
				}
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfbyname")]
		public string CreatedOnBehalfByName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("createdonbehalfby"))
				{
					return this.FormattedValues["createdonbehalfby"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfbyyominame")]
		public string CreatedOnBehalfByYomiName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("createdonbehalfby"))
				{
					return this.FormattedValues["createdonbehalfby"];
				}
				else
				{
					return default(string);
				}
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_acceptedname")]
		public string invln_acceptedName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("invln_accepted"))
				{
					return this.FormattedValues["invln_accepted"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changedbycontactid")]
		public Microsoft.Xrm.Sdk.EntityReference invln_changedbycontactid
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("invln_changedbycontactid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_changedbycontactid");
				this.SetAttributeValue("invln_changedbycontactid", value);
				this.OnPropertyChanged("invln_changedbycontactid");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changedbycontactidname")]
		public string invln_changedbycontactidName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("invln_changedbycontactid"))
				{
					return this.FormattedValues["invln_changedbycontactid"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changedbycontactidyominame")]
		public string invln_changedbycontactidYomiName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("invln_changedbycontactid"))
				{
					return this.FormattedValues["invln_changedbycontactid"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changedbyuserid")]
		public Microsoft.Xrm.Sdk.EntityReference invln_changedbyuserid
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("invln_changedbyuserid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_changedbyuserid");
				this.SetAttributeValue("invln_changedbyuserid", value);
				this.OnPropertyChanged("invln_changedbyuserid");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changedbyuseridname")]
		public string invln_changedbyuseridName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("invln_changedbyuserid"))
				{
					return this.FormattedValues["invln_changedbyuserid"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changedbyuseridyominame")]
		public string invln_changedbyuseridYomiName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("invln_changedbyuserid"))
				{
					return this.FormattedValues["invln_changedbyuserid"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changefrom")]
		public virtual Microsoft.Xrm.Sdk.OptionSetValue invln_changefrom
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("invln_changefrom");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_changefrom");
				this.SetAttributeValue("invln_changefrom", value);
				this.OnPropertyChanged("invln_changefrom");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changefromname")]
		public string invln_changefromName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("invln_changefrom"))
				{
					return this.FormattedValues["invln_changefrom"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changesource")]
		public virtual Microsoft.Xrm.Sdk.OptionSetValue invln_changesource
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("invln_changesource");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_changesource");
				this.SetAttributeValue("invln_changesource", value);
				this.OnPropertyChanged("invln_changesource");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changesourcename")]
		public string invln_changesourceName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("invln_changesource"))
				{
					return this.FormattedValues["invln_changesource"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changeto")]
		public virtual Microsoft.Xrm.Sdk.OptionSetValue invln_changeto
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("invln_changeto");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_changeto");
				this.SetAttributeValue("invln_changeto", value);
				this.OnPropertyChanged("invln_changeto");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changetoname")]
		public string invln_changetoName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("invln_changeto"))
				{
					return this.FormattedValues["invln_changeto"];
				}
				else
				{
					return default(string);
				}
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_loanapplication")]
		public Microsoft.Xrm.Sdk.EntityReference invln_Loanapplication
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("invln_loanapplication");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_Loanapplication");
				this.SetAttributeValue("invln_loanapplication", value);
				this.OnPropertyChanged("invln_Loanapplication");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_loanapplicationname")]
		public string invln_LoanapplicationName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("invln_loanapplication"))
				{
					return this.FormattedValues["invln_loanapplication"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		/// <summary>
		/// Unique identifier for entity instances
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_loanstatuschangeid")]
		public System.Nullable<System.Guid> invln_LoanstatuschangeId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("invln_loanstatuschangeid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_LoanstatuschangeId");
				this.SetAttributeValue("invln_loanstatuschangeid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("invln_LoanstatuschangeId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_loanstatuschangeid")]
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
				this.invln_LoanstatuschangeId = value;
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedbyname")]
		public string ModifiedByName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("modifiedby"))
				{
					return this.FormattedValues["modifiedby"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedbyyominame")]
		public string ModifiedByYomiName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("modifiedby"))
				{
					return this.FormattedValues["modifiedby"];
				}
				else
				{
					return default(string);
				}
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("modifiedonbehalfby"))
				{
					return this.FormattedValues["modifiedonbehalfby"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfbyyominame")]
		public string ModifiedOnBehalfByYomiName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("modifiedonbehalfby"))
				{
					return this.FormattedValues["modifiedonbehalfby"];
				}
				else
				{
					return default(string);
				}
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
		/// Name of the owner
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owneridname")]
		public string OwnerIdName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("ownerid"))
				{
					return this.FormattedValues["ownerid"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		/// <summary>
		/// Yomi name of the owner
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owneridyominame")]
		public string OwnerIdYomiName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("ownerid"))
				{
					return this.FormattedValues["ownerid"];
				}
				else
				{
					return default(string);
				}
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningbusinessunitname")]
		public string OwningBusinessUnitName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("owningbusinessunit"))
				{
					return this.FormattedValues["owningbusinessunit"];
				}
				else
				{
					return default(string);
				}
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
		/// Status of the Loan status change
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecodename")]
		public string statecodeName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("statecode"))
				{
					return this.FormattedValues["statecode"];
				}
				else
				{
					return default(string);
				}
			}
		}
		
		/// <summary>
		/// Reason for the status of the Loan status change
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscodename")]
		public string statuscodeName
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				if (this.FormattedValues.Contains("statuscode"))
				{
					return this.FormattedValues["statuscode"];
				}
				else
				{
					return default(string);
				}
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
		/// N:1 invln_contact_invln_loanstatuschange_changedbycontactid
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changedbycontactid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("invln_contact_invln_loanstatuschange_changedbycontactid")]
		public HE.Investments.Common.CRM.Model.Contact invln_contact_invln_loanstatuschange_changedbycontactid
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<HE.Investments.Common.CRM.Model.Contact>("invln_contact_invln_loanstatuschange_changedbycontactid", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_contact_invln_loanstatuschange_changedbycontactid");
				this.SetRelatedEntity<HE.Investments.Common.CRM.Model.Contact>("invln_contact_invln_loanstatuschange_changedbycontactid", null, value);
				this.OnPropertyChanged("invln_contact_invln_loanstatuschange_changedbycontactid");
			}
		}
		
		/// <summary>
		/// N:1 invln_loanstatuschange_Loanapplication
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_loanapplication")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("invln_loanstatuschange_Loanapplication")]
		public HE.Investments.Common.CRM.Model.invln_Loanapplication invln_loanstatuschange_Loanapplication
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<HE.Investments.Common.CRM.Model.invln_Loanapplication>("invln_loanstatuschange_Loanapplication", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_loanstatuschange_Loanapplication");
				this.SetRelatedEntity<HE.Investments.Common.CRM.Model.invln_Loanapplication>("invln_loanstatuschange_Loanapplication", null, value);
				this.OnPropertyChanged("invln_loanstatuschange_Loanapplication");
			}
		}
		
		/// <summary>
		/// N:1 invln_systemuser_invln_loanstatuschange_changedbyuserid
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invln_changedbyuserid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("invln_systemuser_invln_loanstatuschange_changedbyuserid")]
		public HE.Investments.Common.CRM.Model.SystemUser invln_systemuser_invln_loanstatuschange_changedbyuserid
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<HE.Investments.Common.CRM.Model.SystemUser>("invln_systemuser_invln_loanstatuschange_changedbyuserid", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("invln_systemuser_invln_loanstatuschange_changedbyuserid");
				this.SetRelatedEntity<HE.Investments.Common.CRM.Model.SystemUser>("invln_systemuser_invln_loanstatuschange_changedbyuserid", null, value);
				this.OnPropertyChanged("invln_systemuser_invln_loanstatuschange_changedbyuserid");
			}
		}
		
		/// <summary>
		/// N:1 lk_invln_loanstatuschange_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_invln_loanstatuschange_createdby")]
		public HE.Investments.Common.CRM.Model.SystemUser lk_invln_loanstatuschange_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<HE.Investments.Common.CRM.Model.SystemUser>("lk_invln_loanstatuschange_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_invln_loanstatuschange_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_invln_loanstatuschange_createdonbehalfby")]
		public HE.Investments.Common.CRM.Model.SystemUser lk_invln_loanstatuschange_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<HE.Investments.Common.CRM.Model.SystemUser>("lk_invln_loanstatuschange_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_invln_loanstatuschange_createdonbehalfby");
				this.SetRelatedEntity<HE.Investments.Common.CRM.Model.SystemUser>("lk_invln_loanstatuschange_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_invln_loanstatuschange_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_invln_loanstatuschange_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_invln_loanstatuschange_modifiedby")]
		public HE.Investments.Common.CRM.Model.SystemUser lk_invln_loanstatuschange_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<HE.Investments.Common.CRM.Model.SystemUser>("lk_invln_loanstatuschange_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_invln_loanstatuschange_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_invln_loanstatuschange_modifiedonbehalfby")]
		public HE.Investments.Common.CRM.Model.SystemUser lk_invln_loanstatuschange_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<HE.Investments.Common.CRM.Model.SystemUser>("lk_invln_loanstatuschange_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_invln_loanstatuschange_modifiedonbehalfby");
				this.SetRelatedEntity<HE.Investments.Common.CRM.Model.SystemUser>("lk_invln_loanstatuschange_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_invln_loanstatuschange_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 team_invln_loanstatuschange
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("team_invln_loanstatuschange")]
		public HE.Investments.Common.CRM.Model.Team team_invln_loanstatuschange
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<HE.Investments.Common.CRM.Model.Team>("team_invln_loanstatuschange", null);
			}
		}
		
		/// <summary>
		/// N:1 user_invln_loanstatuschange
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owninguser")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("user_invln_loanstatuschange")]
		public HE.Investments.Common.CRM.Model.SystemUser user_invln_loanstatuschange
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<HE.Investments.Common.CRM.Model.SystemUser>("user_invln_loanstatuschange", null);
			}
		}
		
		/// <summary>
		/// Constructor for populating via LINQ queries given a LINQ anonymous type
		/// <param name="anonymousType">LINQ anonymous type.</param>
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public invln_Loanstatuschange(object anonymousType) : 
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
                        Attributes["invln_loanstatuschangeid"] = base.Id;
                        break;
                    case "invln_loanstatuschangeid":
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
