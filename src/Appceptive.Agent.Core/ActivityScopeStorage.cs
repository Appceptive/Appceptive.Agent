using System;
using System.Runtime.Remoting.Messaging;

namespace Appceptive.Agent.Core
{
    public class ActivityScopeStorage
    {
        public const string CurrentScopeKey = "Appceptive.CurrentActivity";

        public static Func<ActivityScope> GetCurrentScope = () => (ActivityScope)CallContext.LogicalGetData(CurrentScopeKey);
        public static Action<ActivityScope> SetCurrentScope = scope => CallContext.LogicalSetData(CurrentScopeKey, scope);
    }
}