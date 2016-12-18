using Akka.DI.Core;
using System;
using Akka.Actor;
using System.Collections.Concurrent;

namespace BlockWars.Game.UI.IoC
{
    public class ActorContainer : IDependencyResolver
    {
        private readonly IServiceProvider _aspNetContainer;
        private readonly ConcurrentDictionary<string, Type> _typeCache = new ConcurrentDictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
        private readonly ActorSystem _actorSystem;


        public ActorContainer(IServiceProvider aspNetContainer, ActorSystem actorSystem)
        {
            _aspNetContainer = aspNetContainer;
            _actorSystem = actorSystem;
        }

        public Props Create(Type actorType)
        {
            return _actorSystem.GetExtension<DIExt>().Props(actorType);
        }

        public Props Create<TActor>() where TActor : ActorBase
        {
            return _actorSystem.GetExtension<DIExt>().Props(typeof(TActor));
        }

        public Func<ActorBase> CreateActorFactory(Type actorType)
        {
            return () => (ActorBase)_aspNetContainer.GetService(actorType);
        }

        public Type GetType(string actorName)
        {
            _typeCache.TryAdd(actorName, actorName.GetTypeValue());
            return _typeCache[actorName];
        }

        public void Release(ActorBase actor)
        {
        }
    }
}
