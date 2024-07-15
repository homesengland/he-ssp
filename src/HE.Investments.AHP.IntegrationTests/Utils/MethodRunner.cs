using System.Reflection;

namespace HE.Investments.AHP.IntegrationTests.Utils;

public class MethodRunner
{
    public static MethodRunner New() => new();

    public MethodRunner RunAllPublicMethods(Type type)
    {
        var instance = Activator.CreateInstance(type);
        var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Where(m => m.DeclaringType == type)
            .Where(m => m.GetParameters().Length == 0);

        foreach (var method in methods)
        {
            try
            {
                method.Invoke(instance, null);
            }
            catch (TargetInvocationException ex)
            {
                Console.WriteLine($"Exception invoking method {method.Name}: {ex.InnerException}");
            }
        }

        return this;
    }
}
