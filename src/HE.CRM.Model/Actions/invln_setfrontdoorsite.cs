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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_setfrontdoorsite")]
	public partial class invln_setfrontdoorsiteRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_entityfieldsparameters = "invln_entityfieldsparameters";
			public const string invln_frontdoorprojectid = "invln_frontdoorprojectid";
			public const string invln_frontdoorsiteid = "invln_frontdoorsiteid";
		}
		
		public const string ActionLogicalName = "invln_setfrontdoorsite";
		
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
		
		public invln_setfrontdoorsiteRequest()
		{
			this.RequestName = "invln_setfrontdoorsite";
			this.invln_entityfieldsparameters = default(string);
			this.invln_frontdoorprojectid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_setfrontdoorsite")]
	public partial class invln_setfrontdoorsiteResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_frontdoorsiteid = "invln_frontdoorsiteid";
		}
		
		public const string ActionLogicalName = "invln_setfrontdoorsite";
		
		public invln_setfrontdoorsiteResponse()
		{
		}
		
		public string invln_frontdoorsiteid
		{
			get
			{
				if (this.Results.Contains("invln_frontdoorsiteid"))
				{
					return ((string)(this.Results["invln_frontdoorsiteid"]));
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
