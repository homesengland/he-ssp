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
	/// Defines the type for ML model
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum msdyn_MLmodeltype
	{
		
		/// <summary>
		/// Routing using Effort ML model
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Effortbased = 192350002,
		
		/// <summary>
		/// Routing using Sentiment ML model
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Sentimentbased = 192350001,
		
		/// <summary>
		/// Skill identification using ML model
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Skillbased = 192350000,
	}
}
#pragma warning restore CS1591
