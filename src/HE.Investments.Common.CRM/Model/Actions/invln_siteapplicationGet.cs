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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_siteapplicationGet")]
	public partial class invln_siteapplicationGetRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_consortiumid = "invln_consortiumid";
			public const string invln_organizationid = "invln_organizationid";
			public const string invln_siteid = "invln_siteid";
			public const string invln_userid = "invln_userid";
		}
		
		public const string ActionLogicalName = "invln_siteapplicationGet";
		
		public string invln_consortiumid
		{
			get
			{
				if (this.Parameters.Contains("invln_consortiumid"))
				{
					return ((string)(this.Parameters["invln_consortiumid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_consortiumid"] = value;
			}
		}
		
		public string invln_organizationid
		{
			get
			{
				if (this.Parameters.Contains("invln_organizationid"))
				{
					return ((string)(this.Parameters["invln_organizationid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_organizationid"] = value;
			}
		}
		
		public string invln_siteid
		{
			get
			{
				if (this.Parameters.Contains("invln_siteid"))
				{
					return ((string)(this.Parameters["invln_siteid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_siteid"] = value;
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
		
		public invln_siteapplicationGetRequest()
		{
			this.RequestName = "invln_siteapplicationGet";
			this.invln_consortiumid = default(string);
			this.invln_organizationid = default(string);
			this.invln_siteid = default(string);
			this.invln_userid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_siteapplicationGet")]
	public partial class invln_siteapplicationGetResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_siteapplication = "invln_siteapplication";
		}
		
		public const string ActionLogicalName = "invln_siteapplicationGet";
		
		public invln_siteapplicationGetResponse()
		{
		}
		
		public string invln_siteapplication
		{
			get
			{
				if (this.Results.Contains("invln_siteapplication"))
				{
					return ((string)(this.Results["invln_siteapplication"]));
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
