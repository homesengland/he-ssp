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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_searchorganizationbynameandcompanyhousename")]
	public partial class invln_searchorganizationbynameandcompanyhousenameRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_organizationname = "invln_organizationname";
			public const string invln_companyhousenumber = "invln_companyhousenumber";
		}
		
		public const string ActionLogicalName = "invln_searchorganizationbynameandcompanyhousename";
		
		public string invln_organizationname
		{
			get
			{
				if (this.Parameters.Contains("invln_organizationname"))
				{
					return ((string)(this.Parameters["invln_organizationname"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_organizationname"] = value;
			}
		}
		
		public string invln_companyhousenumber
		{
			get
			{
				if (this.Parameters.Contains("invln_companyhousenumber"))
				{
					return ((string)(this.Parameters["invln_companyhousenumber"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_companyhousenumber"] = value;
			}
		}
		
		public invln_searchorganizationbynameandcompanyhousenameRequest()
		{
			this.RequestName = "invln_searchorganizationbynameandcompanyhousename";
			this.invln_organizationname = default(string);
			this.invln_companyhousenumber = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_searchorganizationbynameandcompanyhousename")]
	public partial class invln_searchorganizationbynameandcompanyhousenameResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_organization = "invln_organization";
		}
		
		public const string ActionLogicalName = "invln_searchorganizationbynameandcompanyhousename";
		
		public invln_searchorganizationbynameandcompanyhousenameResponse()
		{
		}
		
		public string invln_organization
		{
			get
			{
				if (this.Results.Contains("invln_organization"))
				{
					return ((string)(this.Results["invln_organization"]));
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
