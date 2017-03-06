using System;
using Autofac;
using Cik.Magazine.Shared.IoC;

namespace Cik.Magazine.Shared.Extensions
{
    public static class ContainerExtensions
    {
        public static IContainer GetContainer(this ContainerBuilder builder, Type[] types = null)
        {
            builder.RegisterModule<LoggingModule>();
            if (types != null)
                foreach (var type in types)
                    builder.RegisterType(type).AsSelf();
            var container = builder.Build();
            return container;
        }
    }
}