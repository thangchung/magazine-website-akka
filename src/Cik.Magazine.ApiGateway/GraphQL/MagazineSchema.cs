using System;
using GraphQL.Types;

namespace Cik.Magazine.ApiGateway.GraphQL
{
    public class MagazineSchema : Schema
    {
        public MagazineSchema(Func<Type, GraphType> resolveType)
            : base(resolveType)
        {
            Query = (MagazineQuery) resolveType(typeof(MagazineQuery));
        }
    }
}