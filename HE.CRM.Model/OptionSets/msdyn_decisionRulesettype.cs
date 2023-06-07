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
	
	
	/// <summary>
	/// Defines type of the rule set
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum msdyn_decisionRulesettype
	{
		
		/// <summary>
		/// Embellishing the work-item using dataverse components
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		DataverseComponents = 192350002,
		
		/// <summary>
		/// Embellishing the work-item using conditional expressions on different entities
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Declarative = 192350000,
		
		/// <summary>
		/// Embellishing the work-item using ML model
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		MLmodelbased = 192350001,
	}
}
#pragma warning restore CS1591
