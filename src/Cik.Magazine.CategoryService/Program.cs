using Autofac;
using Cik.Magazine.Shared.Extensions;
using Topshelf;
using Topshelf.Autofac;

namespace Cik.Magazine.CategoryService
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return (int) HostFactory.Run(x =>
            {
                var container = new ContainerBuilder().GetContainer(new[] {typeof(CategoryService)});

                x.UseSerilog();
                x.UseAutofacContainer(container);
                x.Service(() => container.Resolve<CategoryService>());

                x.UseAssemblyInfoForServiceInfo();
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.EnableServiceRecovery(r => r.RestartService(1));

                x.SetServiceName("Category");
                x.SetDisplayName("Category Service");
                x.SetDescription("Magazine Website - Category Service.");
            });
        }
    }
}