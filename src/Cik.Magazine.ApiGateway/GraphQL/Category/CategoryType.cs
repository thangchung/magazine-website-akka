using GraphQL.Types;

namespace Cik.Magazine.ApiGateway.GraphQL.Category
{
    public class CategoryType : ObjectGraphType<CategoryGraph>
    {
        public CategoryType()
        {
            Name = "Category";
            Description = "The category type in the magazine system";

            Field(x => x.Id).Description("The Id of the Category.");
            Field(x => x.Name).Description("The Name of the Category.");
            Field(x => x.Status).Description("The Status of the Category.");
        }
    }
}