namespace Mediator.Sample
{
    public class OutputRequest : IRequest<bool>
    {
        public string Message { get; set; }
    }
}