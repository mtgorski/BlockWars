namespace BlockWars.Game.UI.Queries
{
    public class UserNameQuery
    {

        public UserNameQuery(string connectionId)
        {
            ConnectionId = connectionId;
        }

        public string ConnectionId { get; }
    }
}