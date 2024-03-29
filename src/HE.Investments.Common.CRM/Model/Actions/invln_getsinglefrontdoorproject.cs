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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getsinglefrontdoorproject")]
	public partial class invln_getsinglefrontdoorprojectRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_includeinactive = "invln_includeinactive";
			public const string invln_fieldstoretrieve = "invln_fieldstoretrieve";
			public const string invln_frontdoorprojectid = "invln_frontdoorprojectid";
			public const string invln_organisationid = "invln_organisationid";
			public const string invln_userid = "invln_userid";
			public const string invln_usehetables = "invln_usehetables";
		}
		
		public const string ActionLogicalName = "invln_getsinglefrontdoorproject";
		
		public string invln_includeinactive
		{
			get
			{
				if (this.Parameters.Contains("invln_includeinactive"))
				{
					return ((string)(this.Parameters["invln_includeinactive"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_includeinactive"] = value;
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
		
		public invln_getsinglefrontdoorprojectRequest()
		{
			this.RequestName = "invln_getsinglefrontdoorproject";
			this.invln_frontdoorprojectid = default(string);
			this.invln_organisationid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getsinglefrontdoorproject")]
	public partial class invln_getsinglefrontdoorprojectResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_retrievedfrontdoorprojectfields = "invln_retrievedfrontdoorprojectfields";
		}
		
		public const string ActionLogicalName = "invln_getsinglefrontdoorproject";
		
		public invln_getsinglefrontdoorprojectResponse()
		{
		}
		
		public string invln_retrievedfrontdoorprojectfields
		{
			get
			{
				if (this.Results.Contains("invln_retrievedfrontdoorprojectfields"))
				{
					return ((string)(this.Results["invln_retrievedfrontdoorprojectfields"]));
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
