namespace BlockWars.Game.UI.Actors
{
    public class UserDisconnectedMessage
    {
        public string ConnectionId { get; }

        public UserDisconnectedMessage(string connectionId)
        {
            ConnectionId = connectionId;
        }
    }
}