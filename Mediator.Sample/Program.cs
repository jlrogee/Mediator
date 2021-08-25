using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddMediator(ServiceLifetime.Transient, typeof(Program)).BuildServiceProvider();

            var mediator = serviceProvider.GetRequiredService<IMediator>();
            
            var request = new OutputRequest
            {
                Message = "Hello world"
            };
            await mediator.SendAsync(request);

            Console.ReadKey();
        }
    }
}