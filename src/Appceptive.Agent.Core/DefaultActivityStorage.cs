using System.Runtime.Remoting.Messaging;

namespace Appceptive.Agent.Core
{
    public class DefaultActivityStorage : IActivityStorage
    {
        private const string CurrentActivityKey = "Appceptive.CurrentActivity";

        public Activity CurrentActivity
        {
            get { return (Activity) CallContext.LogicalGetData(CurrentActivityKey); }
            set { CallContext.LogicalSetData(CurrentActivityKey, value); }
        }
    }
}