namespace BlockWars.Game.UI.Actors
{
    public class UserConnectedMessage
    {
        public string ConnectionId { get; }

        public UserConnectedMessage(string connectionId)
        {
            ConnectionId = connectionId;
        }
    }
}