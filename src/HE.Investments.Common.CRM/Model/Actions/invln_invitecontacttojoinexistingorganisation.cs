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
	
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_invitecontacttojoinexistingorganisation")]
	public partial class invln_invitecontacttojoinexistingorganisationRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_invitedcontactid = "invln_invitedcontactid";
			public const string invln_invitercontactid = "invln_invitercontactid";
			public const string invln_organisationid = "invln_organisationid";
		}
		
		public const string ActionLogicalName = "invln_invitecontacttojoinexistingorganisation";
		
		public string invln_invitedcontactid
		{
			get
			{
				if (this.Parameters.Contains("invln_invitedcontactid"))
				{
					return ((string)(this.Parameters["invln_invitedcontactid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_invitedcontactid"] = value;
			}
		}
		
		public string invln_invitercontactid
		{
			get
			{
				if (this.Parameters.Contains("invln_invitercontactid"))
				{
					return ((string)(this.Parameters["invln_invitercontactid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_invitercontactid"] = value;
			}
		}
		
		public string invln_organisationid
		{
			get
			{
				if (this.Parameters.Contains("invln_organisationid"))
				{
					return ((string)(this.Parameters["invln_organisationid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_organisationid"] = value;
			}
		}
		
		public invln_invitecontacttojoinexistingorganisationRequest()
		{
			this.RequestName = "invln_invitecontacttojoinexistingorganisation";
			this.invln_invitedcontactid = default(string);
			this.invln_invitercontactid = default(string);
			this.invln_organisationid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_invitecontacttojoinexistingorganisation")]
	public partial class invln_invitecontacttojoinexistingorganisationResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public const string ActionLogicalName = "invln_invitecontacttojoinexistingorganisation";
		
		public invln_invitecontacttojoinexistingorganisationResponse()
		{
		}
	}
}
#pragma warning restore CS1591
