namespace BlockWars.GameState.Api.HttpUtility
{
    public class NotFoundValue
    {
        public string Message { get; }

        public NotFoundValue(string message)
        {
            Message = message;
        }
    }
}
