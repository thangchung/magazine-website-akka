using Cik.Magazine.Shared.Messages.Category;
using GraphQL.Types;

namespace Cik.Magazine.ApiGateway.GraphQL.Category
{
    public class CategoryStatusEnum : EnumerationGraphType<Status>
    {
        public CategoryStatusEnum()
        {
            Name = "CategoryStatus";
            Description = "One of the Category Status.";
            AddValue("Reviewing", "Reviewing.", 1);
            AddValue("Published", "Published.", 2);
        }    
    }
}