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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getsiteapplicationsallocations")]
	public partial class invln_getsiteapplicationsallocationsRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_siteid = "invln_siteid";
			public const string invln_userid = "invln_userid";
			public const string invln_accountid = "invln_accountid";
		}
		
		public const string ActionLogicalName = "invln_getsiteapplicationsallocations";
		
		public System.Guid invln_siteid
		{
			get
			{
				if (this.Parameters.Contains("invln_siteid"))
				{
					return ((System.Guid)(this.Parameters["invln_siteid"]));
				}
				else
				{
					return default(System.Guid);
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
		
		public System.Guid invln_accountid
		{
			get
			{
				if (this.Parameters.Contains("invln_accountid"))
				{
					return ((System.Guid)(this.Parameters["invln_accountid"]));
				}
				else
				{
					return default(System.Guid);
				}
			}
			set
			{
				this.Parameters["invln_accountid"] = value;
			}
		}
		
		public invln_getsiteapplicationsallocationsRequest()
		{
			this.RequestName = "invln_getsiteapplicationsallocations";
			this.invln_siteid = default(System.Guid);
			this.invln_userid = default(string);
			this.invln_accountid = default(System.Guid);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getsiteapplicationsallocations")]
	public partial class invln_getsiteapplicationsallocationsResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_siteapplicationallocation = "invln_siteapplicationallocation";
		}
		
		public const string ActionLogicalName = "invln_getsiteapplicationsallocations";
		
		public invln_getsiteapplicationsallocationsResponse()
		{
		}
		
		public string invln_siteapplicationallocation
		{
			get
			{
				if (this.Results.Contains("invln_siteapplicationallocation"))
				{
					return ((string)(this.Results["invln_siteapplicationallocation"]));
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
