using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mediator
{
    public class Mediator : IMediator
    {
        private readonly MediatorResolver _mediatorResolver;
        
        public Mediator(Func<Type, object> resolve, Dictionary<Type, Type> details)
        {
            _mediatorResolver = new MediatorResolver(resolve, details);
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var handler = _mediatorResolver.TryFind(request.GetType());
            return await (Task<TResponse>)Invoke(handler, request);
        }

        private object Invoke<TResponse>(object handler, IRequest<TResponse> request)
        {
            return handler.GetType().GetMethod("HandleAsync").Invoke(handler, new []{request});
        }
    }
}