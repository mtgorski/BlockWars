using Akka.Actor;
using Akka.TestKit.Xunit2;
using BlockWars.Game.UI.Actors;
using BlockWars.Game.UI.Commands;
using BlockWars.Game.UI.Models;
using BlockWars.Game.UI.ViewModels;
using Ploeh.SemanticComparison.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace BlockWars.Game.UI.Unit.Tests.Actors
{
    public class GameActorTests : TestKit
    {
        class GameActorFixture
        {
            private ActorSystem _sys;

            public Models.GameState Game { get; private set; }
            public List<RegionState> Regions { get; private set; }
            public Guid LeagueId { get { return Game.GameId; } }
            public string AValidRegionName
            {
                get
                {
                    return Regions.First().Name;
                }
            }

            public GameActorFixture(ActorSystem sys)
            {
                _sys = sys;
                Game = new Models.GameState(Guid.NewGuid(), "TestLeague", "LeagueForTesting", DateTime.UtcNow, 10000);
                Regions = new List<RegionState>
                {
                    new RegionState("TestRegion1"),
                    new RegionState("TestRegion2")
                };
            }

            public void UseShortDuration()
            {
                Game = new Models.GameState(Guid.NewGuid(), "TestLeague", "LeagueForTesting", DateTime.UtcNow, 10);
            }

            public IActorRef GetInitializedSut()
            {
                var sut = _sys.ActorOf(Props.Create(() => new GameActor()));

                var initCommand = new InitializeGameCommand(Game, Regions);
                sut.Tell(initCommand);
          
                return sut;
            }
        }


        [Fact]
        public void ShouldPublishItsState_WhenToldToCheckItAndNotExpired()
        {
            var fixture = new GameActorFixture(Sys);
            var sut = fixture.GetInitializedSut();
            Sys.EventStream.Subscribe(TestActor, typeof(GameViewModel));

            sut.Tell(new CheckStateCommand());

            var expected = new GameViewModel(0, fixture.Game, fixture.Regions, new List<PlayerBlockCount>())
                .AsSource().OfLikeness<GameViewModel>()
                .Without(x => x.Players)
                .Without(x => x.RemainingMilliseconds)
                .With(x => x.Regions)
                .EqualsWhen((x, y) => x.Regions.Count == y.Regions.Count && x.Regions.All(z => y.Regions.Single(a => a.RegionId == z.RegionId).Equals(z)));
            var result = ExpectMsg<GameViewModel>();
            expected.ShouldEqual(result);
        }

        [Fact]
        public void ShouldPublishAnIncrementedBlockCount_WhenToldToBuildABlock()
        {
            var fixture = new GameActorFixture(Sys);
            var sut = fixture.GetInitializedSut();
            Sys.EventStream.Subscribe(TestActor, typeof(GameViewModel));
            var regionToBuildIn = fixture.AValidRegionName;

            sut.Tell(new BuildBlockCommand(fixture.LeagueId, regionToBuildIn, "123"));
            var reply = ExpectMsg<BlockBuiltMessage>();
            var expectedReply = new BlockBuiltMessage("123", fixture.LeagueId).AsSource().OfLikeness<BlockBuiltMessage>();
            expectedReply.ShouldEqual(reply);

            sut.Tell(new CheckStateCommand());
            var result = ExpectMsg<GameViewModel>().Regions.Single(x => regionToBuildIn == x.Name).BlockCount;
            Assert.Equal(1, result);
        }

        class MutationActor : ReceiveActor
        {
            public MutationActor()
            {
                Receive<GameViewModel>(x =>
                {
                    x.Regions.Add(new RegionState());
                    return true;
                });
            }
        }


        [Fact]
        public void ShouldPublishAnImmutableState_vCollectionCantBeModified()
        {
            var fixture = new GameActorFixture(Sys);
            var sut = fixture.GetInitializedSut();
            var mutator = Sys.ActorOf(Props.Create(() => new MutationActor()));
            Sys.EventStream.Subscribe(mutator, typeof(GameViewModel));
            Sys.EventStream.Subscribe(TestActor, typeof(GameViewModel));

            sut.Tell(new CheckStateCommand());
            Thread.Sleep(100);
            var result = ExpectMsg<GameViewModel>();
            Assert.Equal(2, result.Regions.Count);
        }

        [Fact]
        public void ShouldPublishAnImmutableState_vShouldntPickUpNewChanges()
        {
            var fixture = new GameActorFixture(Sys);
            var sut = fixture.GetInitializedSut();
            Sys.EventStream.Subscribe(TestActor, typeof(GameViewModel));

            sut.Tell(new CheckStateCommand());
            var result = ExpectMsg<GameViewModel>().Regions.Single(x => x.Name == fixture.AValidRegionName).BlockCount;
            sut.Tell(new BuildBlockCommand(fixture.LeagueId, fixture.AValidRegionName, ""));

            Assert.Equal(0, result);
        }

        [Fact]
        public void ShouldPublishALeagueEndedMessageWhenGameOver()
        {
            var fixture = new GameActorFixture(Sys);
            fixture.UseShortDuration();
            var sut = fixture.GetInitializedSut();
            Sys.EventStream.Subscribe(TestActor, typeof(GameEndedMessage));

            Thread.Sleep((int)fixture.Game.Duration + 10);
            sut.Tell(new CheckStateCommand());

            var result = ExpectMsg<GameEndedMessage>();
            Assert.NotNull(result);
        }

        [Fact]
        public void ShouldKeepTrackOfFirstUserClick()
        {
            var fixture = new GameActorFixture(Sys);
            var sut = fixture.GetInitializedSut();
            Sys.EventStream.Subscribe(TestActor, typeof(GameViewModel));
            var userId = Guid.NewGuid().ToString();

            sut.Tell(new BuildBlockCommand(fixture.Game.GameId, fixture.AValidRegionName, userId));

            ExpectMsg<BlockBuiltMessage>();

            sut.Tell(new CheckStateCommand());
            var expected = new PlayerBlockCount(userId, 1);
            var actual = ExpectMsg<GameViewModel>().Players.Single(x => x.ConnectionId == userId);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldKeepTrackOfMultipleUserClicks()
        {
            var fixture = new GameActorFixture(Sys);
            var sut = fixture.GetInitializedSut();
            Sys.EventStream.Subscribe(TestActor, typeof(GameViewModel));
            var userId = Guid.NewGuid().ToString();

            for(int i = 0; i < 3; i++)
            {
                sut.Tell(new BuildBlockCommand(fixture.Game.GameId, fixture.AValidRegionName, userId));
                ExpectMsg<BlockBuiltMessage>();
            }

            sut.Tell(new CheckStateCommand());
            var expected = new PlayerBlockCount(userId, 3);
            var actual = ExpectMsg<GameViewModel>().Players.Single(x => x.ConnectionId == userId);
            Assert.Equal(expected, actual);
        }
    }
}
