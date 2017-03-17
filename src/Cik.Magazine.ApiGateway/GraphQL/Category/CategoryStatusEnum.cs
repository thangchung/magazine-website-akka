using GraphQL.Types;

namespace Cik.Magazine.ApiGateway.GraphQL.Category
{
    public class CategoryStatusEnum : EnumerationGraphType
    {
        public CategoryStatusEnum()
        {
            Name = "Status";
            Description = "One of the Category Status.";
            AddValue("Reviewing", "Reviewing.", 1);
            AddValue("Published", "Published.", 2);
        }
    }
}