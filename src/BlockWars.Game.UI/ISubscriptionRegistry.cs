using Akka.Actor;

namespace BlockWars.Game.UI
{
    public interface ISubscriptionRegistry
    {
        void Subscribe<TMessage>(IActorRef actor);
        IActorRef[] GetSubscribers<TMessage>();
    }

}
