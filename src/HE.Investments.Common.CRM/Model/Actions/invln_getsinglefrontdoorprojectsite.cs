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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getsinglefrontdoorprojectsite")]
	public partial class invln_getsinglefrontdoorprojectsiteRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_frontdoorprojectid = "invln_frontdoorprojectid";
			public const string invln_fieldstoretrieve = "invln_fieldstoretrieve";
			public const string invln_frontdoorprojectsiteid = "invln_frontdoorprojectsiteid";
		}
		
		public const string ActionLogicalName = "invln_getsinglefrontdoorprojectsite";
		
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
		
		public string invln_fieldstoretrieve
		{
			get
			{
				if (this.Parameters.Contains("invln_fieldstoretrieve"))
				{
					return ((string)(this.Parameters["invln_fieldstoretrieve"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_fieldstoretrieve"] = value;
			}
		}
		
		public string invln_frontdoorprojectsiteid
		{
			get
			{
				if (this.Parameters.Contains("invln_frontdoorprojectsiteid"))
				{
					return ((string)(this.Parameters["invln_frontdoorprojectsiteid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_frontdoorprojectsiteid"] = value;
			}
		}
		
		public invln_getsinglefrontdoorprojectsiteRequest()
		{
			this.RequestName = "invln_getsinglefrontdoorprojectsite";
			this.invln_frontdoorprojectid = default(string);
			this.invln_frontdoorprojectsiteid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getsinglefrontdoorprojectsite")]
	public partial class invln_getsinglefrontdoorprojectsiteResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_frontdoorprojectsite = "invln_frontdoorprojectsite";
		}
		
		public const string ActionLogicalName = "invln_getsinglefrontdoorprojectsite";
		
		public invln_getsinglefrontdoorprojectsiteResponse()
		{
		}
		
		public string invln_frontdoorprojectsite
		{
			get
			{
				if (this.Results.Contains("invln_frontdoorprojectsite"))
				{
					return ((string)(this.Results["invln_frontdoorprojectsite"]));
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
