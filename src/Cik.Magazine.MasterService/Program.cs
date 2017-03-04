using Topshelf;

namespace Cik.Magazine.MasterService
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return (int)HostFactory.Run(x =>
            {
                // x.Service<MasterService>();

                x.SetServiceName("Master");
                x.SetDisplayName("Master Service");
                x.SetDescription("Magazine Website - Master Service.");

                x.UseAssemblyInfoForServiceInfo();
                x.RunAsLocalSystem();
                x.StartAutomatically();
                // x.UseNLog();

                x.EnableServiceRecovery(r => r.RestartService(1));
            });
        }
    }
}