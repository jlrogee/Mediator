using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mediator
{
    public static class MediatorInjection
    {
        public static IServiceCollection AddMediator(
            this IServiceCollection serviceCollection,
            ServiceLifetime serviceLifetime,
            params Type[] types)
        {
            var handlerInfo = new Dictionary<Type, Type>();
            
            foreach (var type in types)
            {
                var assembly = type.Assembly;
                var requests = GetClassesForInterface(assembly, typeof(IRequest<>));
                var handlers = GetClassesForInterface(assembly, typeof(IHandler<,>));
                
                requests.ForEach(x =>
                {
                    handlerInfo[x] = handlers.SingleOrDefault(h => x == h.GetInterface("IHandler`2")!.GetGenericArguments()[0]);
                });
                
                serviceCollection.TryAdd(handlers.Select(x => new ServiceDescriptor(x, x, serviceLifetime)));
            }

            serviceCollection.AddSingleton<IMediator>(x => new Mediator(x.GetRequiredService, handlerInfo));
            
            return serviceCollection;
        }

        private static List<Type> GetClassesForInterface(Assembly assembly, Type type)
        {
            return assembly.ExportedTypes
                .Where(t =>
                {
                    var genericInterfaces = t.GetInterfaces().Where(x => x.IsGenericType);
                    var hasImplemented = genericInterfaces.Any(x => x.GetGenericTypeDefinition() == type);
                    return !t.IsInterface && !t.IsAbstract && hasImplemented;
                }).ToList();
        }
    }
}