using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using Autumn.Queries.Container;

namespace Autumn.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}