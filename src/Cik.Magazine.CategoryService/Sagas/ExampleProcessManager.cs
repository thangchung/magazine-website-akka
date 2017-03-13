using System;
using Akka.Actor;
using Status = Cik.Magazine.Shared.Messages.Category.Status;

namespace Cik.Magazine.CategoryService.Sagas
{
    public class Data
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
    }

    public class ExampleProcessManager : FSM<Status, Data>
    {
        public ExampleProcessManager()
         {
            var initData = new Data
            {
                Id = Guid.NewGuid(),
                Name = "Sample",
                Status = Status.Reviewing
            };

            StartWith(Status.Reviewing, initData);

            When(Status.Reviewing, @event =>
            {
                Console.WriteLine("Run Reviewing");
                if (@event.StateData.Status == Status.Reviewing)
                {
                    @event.StateData.Status = Status.Published;
                    return GoTo(Status.Published)
                        .Using(initData);
                }
                else
                {
                    Console.WriteLine("Already Reviewed.");
                }
                return null;
            }, TimeSpan.FromSeconds(5));

            When(Status.Published, @event =>
            {
                Console.WriteLine("Run Published");
                if (@event.StateData.Status == Status.Published)
                {
                    @event.StateData.Status = Status.Reviewing;
                    return GoTo(Status.Reviewing)
                        .Using(initData);
                }
                else
                {
                    Console.WriteLine("Change back to Review.");
                }
                return null;
            }, TimeSpan.FromSeconds(5));

             OnTransition((a, b) =>
             {
                 
             });
            
            Initialize();
            
        }

        protected override bool Receive(object message)
        {
            return base.Receive(message);
        }
    }
}