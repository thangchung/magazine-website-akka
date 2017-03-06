using Autofac;
using Serilog;

namespace Cik.Magazine.Shared.IoC
{
    public class LoggingModule : Module 
    {
        protected override void Load(ContainerBuilder builder)
        {
            var config = new LoggerConfiguration().MinimumLevel.Verbose()
                .WriteTo.Trace()
                .WriteTo.Console();

            var logger = config.CreateLogger();
            Log.Logger = logger;

            builder.RegisterInstance(logger).As<ILogger>();
        }
    }
}