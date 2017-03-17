using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Cik.Magazine.ApiGateway.GraphQL.Category;
using Cik.Magazine.Shared;
using Cik.Magazine.Shared.Queries;
using GraphQL.Types;

namespace Cik.Magazine.ApiGateway.GraphQL
{
    public class MagazineQuery : ObjectGraphType
    {
        public MagazineQuery(IActorRefFactory actorSystem)
        {
            Name = "Query";

            var categoryQuery = actorSystem.ActorSelection($"/user/{SystemData.CategoryQueryActor.Name}-group");
            FieldAsync<ListGraphType<CategoryType>>("categories", resolve: async context =>
            {
                var result = await categoryQuery.Ask<List<CategoryViewResponse>>(new ListCategoryViewRequest());
                return result.Select(x => new CategoryGraph
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Status = x.Status.ToString()
                });
            });
        }
    }
}