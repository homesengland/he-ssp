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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_deactivatefrontdoorproject")]
	public partial class invln_deactivatefrontdoorprojectRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_usehetables = "invln_usehetables";
			public const string invln_frontdoorprojectid = "invln_frontdoorprojectid";
		}
		
		public const string ActionLogicalName = "invln_deactivatefrontdoorproject";
		
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
		
		public invln_deactivatefrontdoorprojectRequest()
		{
			this.RequestName = "invln_deactivatefrontdoorproject";
			this.invln_frontdoorprojectid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_deactivatefrontdoorproject")]
	public partial class invln_deactivatefrontdoorprojectResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_projectdeactivated = "invln_projectdeactivated";
		}
		
		public const string ActionLogicalName = "invln_deactivatefrontdoorproject";
		
		public invln_deactivatefrontdoorprojectResponse()
		{
		}
		
		public string invln_projectdeactivated
		{
			get
			{
				if (this.Results.Contains("invln_projectdeactivated"))
				{
					return ((string)(this.Results["invln_projectdeactivated"]));
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
