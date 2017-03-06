using Akka.Actor;
using Autofac;
using Autofac.Extras.Quartz;
using Cik.Magazine.Shared.Extensions;
using Quartz;
using Quartz.Spi;
using Serilog;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.Quartz;

namespace Cik.Magazine.EmailService
{
    internal class Program
    {
        private static ActorSystem _systemActor;
        private static IActorRef _emailActor;

        private static int Main(string[] args)
        {
            return (int) HostFactory.Run(x =>
            {
                // container
                var builder = new ContainerBuilder();
                builder.RegisterModule<QuartzAutofacFactoryModule>();
                builder.RegisterModule(new QuartzAutofacJobsModule(typeof(EmailJob).Assembly));
                var container = builder.GetContainer();

                // log
                x.UseSerilog();
                x.UseAutofacContainer(container);
                // x.Service(() => container.Resolve<EmailService>());

                // actor
                _systemActor = ActorSystem.Create("magazine-system");
                _emailActor = _systemActor.ActorOf(Props.Create(() => new EmailActor(container.Resolve<ILogger>())),
                    "email-actor");

                // Quartz
                x.UsingQuartzJobFactory(() => container.Resolve<IJobFactory>());
                x.ScheduleQuartzJobAsService(q =>
                {
                    q.WithJob(() => JobBuilder.Create<EmailJob>().Build());
                    q.AddTrigger(
                        () =>
                            TriggerBuilder.Create()
                                .WithSimpleSchedule(b => b.WithIntervalInSeconds(1).RepeatForever())
                                .Build());
                });

                x.UseAssemblyInfoForServiceInfo();
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.EnableServiceRecovery(r => r.RestartService(1));

                x.SetServiceName("EmailService");
                x.SetDisplayName("Email Service");
                x.SetDescription("Magazine Website - Email Service.");
            });
        }

        internal class EmailJob : IJob
        {
            private static int _counter = 1;
            private readonly ILogger _log;

            public EmailJob(ILogger log)
            {
                _log = log;
            }

            public void Execute(IJobExecutionContext context)
            {
                _emailActor.Tell(new SendEmail($"Send an email {_counter} times."));
                _counter++;
            }
        }

        internal class EmailActor : ReceiveActor
        {
            private readonly ILogger _log;

            public EmailActor(ILogger log)
            {
                _log = log;
                Receive<SendEmail>(x => { _log.Information(x.Content); });
            }
        }

        internal class SendEmail
        {
            public SendEmail(string content)
            {
                Content = content;
            }

            public string Content { get; }
        }
    }
}