using Topshelf;

namespace Cik.Magazine.CategoryService
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return (int)HostFactory.Run(x =>
            {
                x.SetServiceName("Category");
                x.SetDisplayName("Category Service");
                x.SetDescription("Magazine Website - Category Service.");

                x.UseAssemblyInfoForServiceInfo();
                x.RunAsLocalSystem();
                x.StartAutomatically();
                //x.UseNLog();
                x.Service<CategoryService>();
                x.EnableServiceRecovery(r => r.RestartService(1));
            });
        }
    }
}
