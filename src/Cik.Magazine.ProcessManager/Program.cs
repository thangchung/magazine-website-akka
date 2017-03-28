using Autofac;
using Cik.Magazine.Shared.Extensions;
using Topshelf;
using Topshelf.Autofac;

namespace Cik.Magazine.ProcessManager
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return (int)HostFactory.Run(x =>
            {
                var container = new ContainerBuilder().GetContainer(new[] { typeof(ProcessManagerService) });

                x.UseSerilog();
                x.UseAutofacContainer(container);
                x.Service(() => container.Resolve<ProcessManagerService>());

                x.UseAssemblyInfoForServiceInfo();
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.EnableServiceRecovery(r => r.RestartService(1));

                x.SetServiceName("ProcessManager");
                x.SetDisplayName("Process Manager Service");
                x.SetDescription("Magazine Website - Process Manager Service.");
            });
        }
    }
}
