using System.Web;
using Appceptive.Agent.Core;

namespace Appceptive.Agent.Web
{
    public class WebActivityStorage : IActivityStorage
    {
        private const string CurrentActivityKey = "Appceptive.CurrentActivity";

        public Activity CurrentActivity
        {
            get { return (Activity) HttpContext.Current.Items[CurrentActivityKey]; }
            set { HttpContext.Current.Items[CurrentActivityKey] = value; }
        }
    }
}