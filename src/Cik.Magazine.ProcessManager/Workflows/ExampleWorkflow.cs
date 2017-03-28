using System;
using Akka.Actor;
using Status = Cik.Magazine.Shared.Messages.Category.Status;

namespace Cik.Magazine.ProcessManager.Workflows
{
    public class Data
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
    }

    public class ExampleWorkflow : FSM<Status, Data>
    {
        public ExampleWorkflow()
         {
            var initData = new Data
            {
                Id = Guid.NewGuid(),
                Name = "Sample",
                Status = Status.Draft
            };

            StartWith(Status.Draft, initData);

            When(Status.Draft, @event =>
            {
                Console.WriteLine("Run Draft");
                if (@event.StateData.Status == Status.Draft)
                {
                    @event.StateData.Status = Status.Published;
                    return GoTo(Status.Published)
                        .Using(initData);
                }
                Console.WriteLine("Already Drafted.");
                return null;
            }, TimeSpan.FromSeconds(5));

            When(Status.Published, @event =>
            {
                Console.WriteLine("Run Published");
                if (@event.StateData.Status == Status.Published)
                {
                    @event.StateData.Status = Status.Draft;
                    return GoTo(Status.Draft)
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
    }
}