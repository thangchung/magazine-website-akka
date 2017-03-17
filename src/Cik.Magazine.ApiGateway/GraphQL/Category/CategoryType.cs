using Cik.Magazine.Shared.Queries;
using GraphQL.Types;

namespace Cik.Magazine.ApiGateway.GraphQL.Category
{
    public class CategoryType : ObjectGraphType<CategoryViewResponse>
    {
        public CategoryType()
        {
            Field(x => x.Id).Description("The Id of the Category.");
            Field(x => x.Name).Description("The Name of the Category.");
            Field<CategoryStatusEnum>("categoryStatus", "The Status of the Category.");
        }   
    }
}