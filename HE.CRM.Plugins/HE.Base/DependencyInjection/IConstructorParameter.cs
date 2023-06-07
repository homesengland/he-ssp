using System.Reflection;

namespace HE.Base.DependencyInjection
{
    public interface IConstructorParameter
    {
        /// <summary>
        /// Gets resolved instance
        /// </summary>
        object Instance { get; }

        /// <summary>
        /// Checks if input parameter meets constructor parameter requirements
        /// </summary>
        /// <param name="parameter">Parameter to check</param>
        /// <returns>True </returns>
        bool IsValid(ParameterInfo parameter);
    }
}
