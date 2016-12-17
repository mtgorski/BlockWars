namespace BlockWars.Game.UI
{
    public class ChangeNameCommand
    {
        public string ConnectionId { get; }
        public string Name { get; }

        public ChangeNameCommand(string connectionId, string name)
        {
            ConnectionId = connectionId;
            Name = name;
        }
    }
}