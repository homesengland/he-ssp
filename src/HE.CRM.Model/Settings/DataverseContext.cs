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

[assembly: Microsoft.Xrm.Sdk.Client.ProxyTypesAssemblyAttribute()]

namespace DataverseModel
{
	
	
	/// <summary>
	/// Represents a source of entities bound to a Dataverse service. It tracks and manages changes made to the retrieved entities.
	/// </summary>
	public partial class DataverseContext : Microsoft.Xrm.Sdk.Client.OrganizationServiceContext
	{
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public DataverseContext(Microsoft.Xrm.Sdk.IOrganizationService service) : 
				base(service)
		{
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.Account"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.Account> AccountSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.Account>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.Contact"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.Contact> ContactSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.Contact>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_contactwebrole"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_contactwebrole> invln_contactwebroleSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_contactwebrole>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_contract"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_contract> invln_contractSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_contract>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_Loanapplication"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_Loanapplication> invln_LoanapplicationSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_Loanapplication>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_portal"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_portal> invln_portalSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_portal>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_portalpermissionlevel"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_portalpermissionlevel> invln_portalpermissionlevelSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_portalpermissionlevel>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_SiteDetails"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_SiteDetails> invln_SiteDetailsSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_SiteDetails>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DataverseModel.invln_Webrole"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DataverseModel.invln_Webrole> invln_WebroleSet
		{
			get
			{
				return this.CreateQuery<DataverseModel.invln_Webrole>();
			}
		}
	}
}
#pragma warning restore CS1591
