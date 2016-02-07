using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using System.Collections.Concurrent;

namespace BlockWars.Game.UI
{
    public class SubscriptionRegistry : ISubscriptionRegistry
    {
        private readonly ConcurrentDictionary<Type, ConcurrentBag<IActorRef>> _registry = new ConcurrentDictionary<Type, ConcurrentBag<IActorRef>>();

        public IActorRef[] GetSubscribers<TMessage>()
        {
            ConcurrentBag<IActorRef> subscribers;
            if (_registry.TryGetValue(typeof(TMessage), out subscribers))
            {
                return subscribers.ToArray();
            }

            return new IActorRef[0];
        }

        public void Subscribe<TMessage>(IActorRef actor)
        {
            var current = _registry.GetOrAdd(typeof(TMessage), new ConcurrentBag<IActorRef>());
            current.Add(actor);
        }

    }
}
