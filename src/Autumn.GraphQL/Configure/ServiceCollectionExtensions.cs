﻿using GraphQL;
using GraphQL.Server;
using Microsoft.Extensions.DependencyInjection;
using Autumn.Debugging;

namespace Autumn.Configure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAndConfigureGraphQL(this IServiceCollection services)
        {
            services.AddScoped<IDependencyResolver>(
                x => new FuncDependencyResolver(x.GetRequiredService)
            );

            services
                .AddGraphQL(x => { x.ExposeExceptions = DebugHelper.IsDebug; })
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddUserContextBuilder(httpContext => httpContext.User)
                .AddDataLoader();
        }
    }
}