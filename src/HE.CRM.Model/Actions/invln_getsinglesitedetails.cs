#pragma warning disable CS1591
// Code Generated by DLaB.ModelBuilderExtensions
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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_getsinglesitedetails")]
	public partial class invln_getsinglesitedetailsRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_sitedetailsid = "invln_sitedetailsid";
			public const string invln_fieldstoretrieve = "invln_fieldstoretrieve";
		}
		
		public const string ActionLogicalName = "invln_getsinglesitedetails";
		
		public string invln_sitedetailsid
		{
			get
			{
				if (this.Parameters.Contains("invln_sitedetailsid"))
				{
					return ((string)(this.Parameters["invln_sitedetailsid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_sitedetailsid"] = value;
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
		
		public invln_getsinglesitedetailsRequest()
		{
			this.RequestName = "invln_getsinglesitedetails";
			this.invln_sitedetailsid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_getsinglesitedetails")]
	public partial class invln_getsinglesitedetailsResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_sitedetail = "invln_sitedetail";
		}
		
		public const string ActionLogicalName = "invln_getsinglesitedetails";
		
		public invln_getsinglesitedetailsResponse()
		{
		}
		
		public string invln_sitedetail
		{
			get
			{
				if (this.Results.Contains("invln_sitedetail"))
				{
					return ((string)(this.Results["invln_sitedetail"]));
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
