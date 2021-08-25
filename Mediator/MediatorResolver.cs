using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Mediator
{
    public class MediatorResolver
    {
        private readonly Func<Type, object> _resolver;
        private readonly ConcurrentDictionary<Type, Type> _handlers;

        public MediatorResolver(Func<Type, object> resolver, IDictionary<Type, Type> handlers)
        {
            _resolver = resolver;
            _handlers = new (handlers);
        }

        public object TryFind(Type type)
        {
            if (!_handlers.ContainsKey(type))
                throw new Exception("No handler to handle");

            _handlers.TryGetValue(type, out var handlerType);
            var handler = _resolver(handlerType);
            return handler;
        }
    }
}