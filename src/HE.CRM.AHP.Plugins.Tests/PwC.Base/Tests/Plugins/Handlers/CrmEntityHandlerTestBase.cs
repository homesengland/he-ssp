using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using HE.Base.Plugins.Common;
using HE.Base.Plugins.Handlers;

namespace PwC.Base.Tests.Plugins.Handlers
{
    /// <summary>
    /// Base entity handler test abstraact class. Contains methods and properties for fast plugins handlers unit test development
    /// Every entity handler test should inherit from this class.
    /// </summary>
    /// <typeparam name="TEntity">The type of the Crm entity.</typeparam>
    /// <typeparam name="THandler">The type of the crm plugin handler. Must implement ICrmHandler interface</typeparam>
    /// <typeparam name="TContext">The type of the context inherited from OrganizationServiceCotnext.</typeparam>
    /// <seealso cref="PwC.Plugins.Tests.Base.Handlers.CrmHandlerTestBase{THandler, TContext}" />
    public abstract class CrmEntityHandlerTestBase<TEntity, THandler, TContext> : CrmHandlerTestBase<THandler, TContext>
        where TEntity : Entity, new()
        where THandler : ICrmWorkHandler
        where TContext : OrganizationServiceContext
    {
        protected TEntity Target { get; set; }

        protected TEntity PreImage { get; set; }

        protected TEntity PostImage { get; set; }

        /// <summary>
        /// Assets all required elements to ACT in the test, initializes handler object. Should be invoked just before ACT (invoked on handler) in the test.
        /// </summary>
        /// <param name="messageName">Execution pipeline message name. Can be passed from messagesnameshelper class</param>
        /// <param name="messageStage">Execution pipeline message stage. Can be passed from stages enum</param>
        /// <param name="cache">Handler cache. It's passed across handlers within specified plugin</param>
        public override void Asset(string messageName, int messageStage, HandlerCache cache = null)
        {
            if (Target != null)
            {
                fakedPluginContext.InputParameters.Add(nameof(Target), Target);
            }

            if (PreImage != null)
            {
                fakedPluginContext.PreEntityImages.Add(nameof(PreImage), PreImage);
            }

            if (PostImage != null)
            {
                fakedPluginContext.PostEntityImages.Add(nameof(PostImage), PostImage);
            }

            base.Asset(messageName, messageStage, cache);
        }
    }
}
