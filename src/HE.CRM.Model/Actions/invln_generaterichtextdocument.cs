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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_generaterichtextdocument")]
	public partial class invln_generaterichtextdocumentRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_richtext = "invln_richtext";
			public const string invln_entityname = "invln_entityname";
			public const string invln_entityid = "invln_entityid";
		}
		
		public const string ActionLogicalName = "invln_generaterichtextdocument";
		
		public string invln_richtext
		{
			get
			{
				if (this.Parameters.Contains("invln_richtext"))
				{
					return ((string)(this.Parameters["invln_richtext"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_richtext"] = value;
			}
		}
		
		public string invln_entityname
		{
			get
			{
				if (this.Parameters.Contains("invln_entityname"))
				{
					return ((string)(this.Parameters["invln_entityname"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_entityname"] = value;
			}
		}
		
		public string invln_entityid
		{
			get
			{
				if (this.Parameters.Contains("invln_entityid"))
				{
					return ((string)(this.Parameters["invln_entityid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_entityid"] = value;
			}
		}
		
		public invln_generaterichtextdocumentRequest()
		{
			this.RequestName = "invln_generaterichtextdocument";
			this.invln_richtext = default(string);
			this.invln_entityname = default(string);
			this.invln_entityid = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_generaterichtextdocument")]
	public partial class invln_generaterichtextdocumentResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_generatedrichtext = "invln_generatedrichtext";
		}
		
		public const string ActionLogicalName = "invln_generaterichtextdocument";
		
		public invln_generaterichtextdocumentResponse()
		{
		}
		
		public string invln_generatedrichtext
		{
			get
			{
				if (this.Results.Contains("invln_generatedrichtext"))
				{
					return ((string)(this.Results["invln_generatedrichtext"]));
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
