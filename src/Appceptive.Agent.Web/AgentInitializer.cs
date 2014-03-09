using System.Web;
using Appceptive.Agent.Core;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Appceptive.Agent.Web
{
    public class AgentInitializer
    {
        public static void Start()
        {
            ActivityScopeStorage.GetCurrentScope = () =>  (ActivityScope) HttpContext.Current.Items[ActivityScopeStorage.CurrentScopeKey];
            ActivityScopeStorage.SetCurrentScope = scope => HttpContext.Current.Items[ActivityScopeStorage.CurrentScopeKey] = scope;

            DynamicModuleUtility.RegisterModule(typeof(AgentHttpModule));
        }
    }
}