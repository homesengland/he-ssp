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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_deactivatefrontdoorsite")]
	public partial class invln_deactivatefrontdoorsiteRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_frontdoorsiteid = "invln_frontdoorsiteid";
			public const string invln_usehetables = "invln_usehetables";
		}
		
		public const string ActionLogicalName = "invln_deactivatefrontdoorsite";
		
		public string invln_frontdoorsiteid
		{
			get
			{
				if (this.Parameters.Contains("invln_frontdoorsiteid"))
				{
					return ((string)(this.Parameters["invln_frontdoorsiteid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_frontdoorsiteid"] = value;
			}
		}
		
		public string invln_usehetables
		{
			get
			{
				if (this.Parameters.Contains("invln_usehetables"))
				{
					return ((string)(this.Parameters["invln_usehetables"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_usehetables"] = value;
			}
		}
		
		public invln_deactivatefrontdoorsiteRequest()
		{
			this.RequestName = "invln_deactivatefrontdoorsite";
			this.invln_frontdoorsiteid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_deactivatefrontdoorsite")]
	public partial class invln_deactivatefrontdoorsiteResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_sitedeactivated = "invln_sitedeactivated";
		}
		
		public const string ActionLogicalName = "invln_deactivatefrontdoorsite";
		
		public invln_deactivatefrontdoorsiteResponse()
		{
		}
		
		public string invln_sitedeactivated
		{
			get
			{
				if (this.Results.Contains("invln_sitedeactivated"))
				{
					return ((string)(this.Results["invln_sitedeactivated"]));
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
