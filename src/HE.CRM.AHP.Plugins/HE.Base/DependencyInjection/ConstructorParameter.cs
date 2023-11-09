using System.Reflection;

namespace HE.Base.DependencyInjection
{
    public abstract class ConstructorParameter : IConstructorParameter
    {
        public ConstructorParameter(object instance)
        {
            Instance = instance;
        }

        public object Instance { get; protected set; }

        public abstract bool IsValid(ParameterInfo parameter);
    }
}
