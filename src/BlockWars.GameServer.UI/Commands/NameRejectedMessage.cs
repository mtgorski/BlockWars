namespace BlockWars.Game.UI.Actors
{
    internal class NameRejectedMessage
    {
        public NameRejectedMessage(string connectionId, string name, string reason)
        {
            ConnectionId = connectionId;
            Name = name;
            Reason = reason;
        }

        public string ConnectionId { get; internal set; }
        public object Name { get; internal set; }
        public object Reason { get; internal set; }
    }
}