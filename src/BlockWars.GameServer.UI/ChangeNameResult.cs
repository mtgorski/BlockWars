namespace BlockWars.Game.UI.Actors
{
    internal class ChangeNameResult
    {
        private bool approved;
        private object message;
        private string name;

        public ChangeNameResult(bool approved, string name, object message)
        {
            this.approved = approved;
            this.name = name;
            this.message = message;
        }
    }
}