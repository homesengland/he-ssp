using Microsoft.AspNetCore.Mvc.Filters;

namespace HE.Investments.Common.WWW.Routing;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class WorkflowStateAttribute : ActionFilterAttribute
{
    public WorkflowStateAttribute(object state)
    {
        State = state;
    }

    public object State { get; set; }

    public TState StateAs<TState>() => (TState)State;
}
