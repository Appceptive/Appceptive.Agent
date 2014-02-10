using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using Appceptive.Agent.Web;

[assembly: AssemblyTitle("Appceptive.Agent.Web")]
[assembly: AssemblyDescription("Appceptive Agent Web Module")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Appceptive")]
[assembly: AssemblyProduct("Appceptive.Agent.Web")]
[assembly: AssemblyCopyright("Copyright ©  2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("660d44c1-8633-4a5b-b9d1-211c40381fef")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: PreApplicationStartMethod(typeof(AgentInitializer), "Start")]