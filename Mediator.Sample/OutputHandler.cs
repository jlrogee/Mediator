using System;
using System.Threading.Tasks;

namespace Mediator.Sample
{
    public class OutputHandler : IHandler<OutputRequest, bool>
    {
        public Task<bool> HandleAsync(OutputRequest request)
        {
            Console.WriteLine(request.Message);
            return Task.FromResult(true);
        }
    }
}