#pragma warning disable CS1591
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
	
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_setfrontdoorproject")]
	public partial class invln_setfrontdoorprojectRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_frontdoorprojectid = "invln_frontdoorprojectid";
			public const string invln_entityfieldsparameters = "invln_entityfieldsparameters";
			public const string invln_organisationid = "invln_organisationid";
			public const string invln_userid = "invln_userid";
		}
		
		public const string ActionLogicalName = "invln_setfrontdoorproject";
		
		public string invln_frontdoorprojectid
		{
			get
			{
				if (this.Parameters.Contains("invln_frontdoorprojectid"))
				{
					return ((string)(this.Parameters["invln_frontdoorprojectid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_frontdoorprojectid"] = value;
			}
		}
		
		public string invln_entityfieldsparameters
		{
			get
			{
				if (this.Parameters.Contains("invln_entityfieldsparameters"))
				{
					return ((string)(this.Parameters["invln_entityfieldsparameters"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_entityfieldsparameters"] = value;
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
		
		public string invln_userid
		{
			get
			{
				if (this.Parameters.Contains("invln_userid"))
				{
					return ((string)(this.Parameters["invln_userid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_userid"] = value;
			}
		}
		
		public invln_setfrontdoorprojectRequest()
		{
			this.RequestName = "invln_setfrontdoorproject";
			this.invln_entityfieldsparameters = default(string);
			this.invln_organisationid = default(string);
			this.invln_userid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_setfrontdoorproject")]
	public partial class invln_setfrontdoorprojectResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_frontdoorprojectid = "invln_frontdoorprojectid";
		}
		
		public const string ActionLogicalName = "invln_setfrontdoorproject";
		
		public invln_setfrontdoorprojectResponse()
		{
		}
		
		public string invln_frontdoorprojectid
		{
			get
			{
				if (this.Results.Contains("invln_frontdoorprojectid"))
				{
					return ((string)(this.Results["invln_frontdoorprojectid"]));
				}
				else
				{
					return default(string);
				}
			}
		}
	}
}
#pragma warning restore CS1591