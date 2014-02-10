using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Appceptive.Agent.Web
{
    public class AgentInitializer
    {
        public static void Start()
        {
            Core.Appceptive.SetActivityStorage(new WebActivityStorage());
            DynamicModuleUtility.RegisterModule(typeof(AgentHttpModule));
        }
    }
}