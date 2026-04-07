namespace SSS.Domain.Entities.AI
{
    public class MessageContext
    {
        public List<long> ModuleIds { get; set; } = new();
        public List<long> TaskIds { get; set; } = new();
    }
}
