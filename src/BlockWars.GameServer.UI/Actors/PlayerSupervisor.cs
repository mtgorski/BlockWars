using Akka.Actor;
using Akka.DI.Core;
using System.Collections.Generic;
using System;
using BlockWars.Game.UI.Queries;

namespace BlockWars.Game.UI.Actors
{
    public class PlayerSupervisor : ReceiveActor
    {
        private readonly HashSet<string> _names = new HashSet<string>();
        private readonly Dictionary<string, string> _connectionIdToName = new Dictionary<string, string>();

        public PlayerSupervisor()
        {
            Receive<UserConnectedMessage>(x =>
            {
                OnUserConnected(x);
                return true;
            });

            Receive<UserDisconnectedMessage>(x =>
            {
                OnUserDisconnected(x);
                return true;
            });

            Receive<ChangeNameCommand>(x =>
            {
                ChangeName(x);
                return true;
            });

            Receive<UserNameQuery>(x =>
            {
                TellUserName(x);
                return true;
            });
        }

        private void TellUserName(UserNameQuery x)
        {
            if(_connectionIdToName.ContainsKey(x.ConnectionId))
            {
                Sender.Tell(_connectionIdToName[x.ConnectionId]);
            }
            else
            {
                Sender.Tell(null);
            }
        }

        private void ChangeName(ChangeNameCommand command)
        {
            if (_connectionIdToName.ContainsKey(command.ConnectionId) && _connectionIdToName[command.ConnectionId] == command.Name)
                return;

            var validationResult = GetValidationResult(command);
            if (validationResult.Approved)
            {
                if(_connectionIdToName.ContainsKey(command.ConnectionId))
                {
                    var name = _connectionIdToName[command.ConnectionId];
                    _names.Remove(name);
                    _connectionIdToName[command.ConnectionId] = command.Name;
                }
                else
                {
                    _connectionIdToName.Add(command.ConnectionId, validationResult.Name);
                }
                _names.Add(validationResult.Name);                
                Context.Child("player" + command.ConnectionId).Tell(new ChangeNameCommand(command.ConnectionId, validationResult.Name));
            }
            else
                Context.Child("player" + command.ConnectionId).Tell(new NameRejectedMessage(command.ConnectionId, validationResult.Name, validationResult.Reason));
        }

        private NameValidationResult GetValidationResult(ChangeNameCommand command)
        {
            var name = command.Name.Trim();
            if(_names.Contains(name))
            {
                return new NameValidationResult(false, name, "Name is already in use.");
            }
            else if(name.Length > 20)
            {
                return new NameValidationResult(false, name, "Name is too long");
            }
            else if(name.Length < 1)
            {
                return new NameValidationResult(false, name, "Name cannot be empty.");
            }
            else
            {
                return new NameValidationResult(true, name);
            }
        }

        private void OnUserDisconnected(UserDisconnectedMessage message)
        {
            if(_connectionIdToName.ContainsKey(message.ConnectionId))
            {
                var name = _connectionIdToName[message.ConnectionId];
                _names.Remove(name);
                _connectionIdToName.Remove(message.ConnectionId);
            }
            Context.Child("player" + message.ConnectionId).Tell(PoisonPill.Instance);
        }

        private void OnUserConnected(UserConnectedMessage message)
        {
            var name = GetName();
            _names.Add(name);
            _connectionIdToName[message.ConnectionId] = name;
            var playerActor = Context.ActorOf(Context.System.DI().Props<PlayerActor>(), "player" + message.ConnectionId);
            Context.System.EventStream.Subscribe(playerActor, typeof(GameEndedMessage));
            playerActor.Tell(new ChangeNameCommand(message.ConnectionId, name));
        }

        private string GetName()
        {
            var baseName = "NotARobot";
            var counter = 1;
            while(_names.Contains(baseName + counter))
            {
                counter++;
            }

            return baseName + counter;
        }

        private class NameValidationResult
        {
            public bool Approved { get; }
            public string Name { get;  }
            public string Reason { get;  }

            public NameValidationResult(bool approved, string name, string reason)
            {
                this.Approved = approved;
                this.Name = name;
                this.Reason = reason;
            }

            public NameValidationResult(bool approved, string name)
            {
                Approved = approved;
                Name = name;
            }
        }
    }
}
