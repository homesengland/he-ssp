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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getmultiplefrontdoorprojectssite")]
	public partial class invln_getmultiplefrontdoorprojectssiteRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_usehetables = "invln_usehetables";
			public const string invln_fieldstoretrieve = "invln_fieldstoretrieve";
			public const string invln_pagingrequest = "invln_pagingrequest";
			public const string invln_frontdoorprojectid = "invln_frontdoorprojectid";
		}
		
		public const string ActionLogicalName = "invln_getmultiplefrontdoorprojectssite";
		
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
		
		public string invln_pagingrequest
		{
			get
			{
				if (this.Parameters.Contains("invln_pagingrequest"))
				{
					return ((string)(this.Parameters["invln_pagingrequest"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_pagingrequest"] = value;
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
		
		public invln_getmultiplefrontdoorprojectssiteRequest()
		{
			this.RequestName = "invln_getmultiplefrontdoorprojectssite";
			this.invln_pagingrequest = default(string);
			this.invln_frontdoorprojectid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getmultiplefrontdoorprojectssite")]
	public partial class invln_getmultiplefrontdoorprojectssiteResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_frontdoorprojectsites = "invln_frontdoorprojectsites";
		}
		
		public const string ActionLogicalName = "invln_getmultiplefrontdoorprojectssite";
		
		public invln_getmultiplefrontdoorprojectssiteResponse()
		{
		}
		
		public string invln_frontdoorprojectsites
		{
			get
			{
				if (this.Results.Contains("invln_frontdoorprojectsites"))
				{
					return ((string)(this.Results["invln_frontdoorprojectsites"]));
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
