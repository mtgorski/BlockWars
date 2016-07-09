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
    public class LeagueActorTests : TestKit
    {
        class LeagueActorFixture
        {
            private ActorSystem _sys;

            public LeagueState League { get; private set; }
            public List<RegionState> Regions { get; private set; }
            public Guid LeagueId { get { return League.LeagueId; } }
            public string AValidRegionName
            {
                get
                {
                    return Regions.First().Name;
                }
            }

            public LeagueActorFixture(ActorSystem sys)
            {
                _sys = sys;
                League = new LeagueState(Guid.NewGuid(), "TestLeague", "LeagueForTesting", DateTime.UtcNow, 10000);
                Regions = new List<RegionState>
                {
                    new RegionState("TestRegion1"),
                    new RegionState("TestRegion2")
                };
            }

            public void UseShortDuration()
            {
                League = new LeagueState(Guid.NewGuid(), "TestLeague", "LeagueForTesting", DateTime.UtcNow, 10);
            }

            public IActorRef GetInitializedSut()
            {
                var sut = _sys.ActorOf(Props.Create(() => new LeagueActor()));

                var initCommand = new InitializeLeagueCommand(League);
                sut.Tell(initCommand);
                Regions.ForEach(x => sut.Tell(new AddRegionCommand(League.LeagueId, x)));

                return sut;
            }
        }


        [Fact]
        public void ShouldPublishItsState_WhenToldToCheckItAndNotExpired()
        {
            var fixture = new LeagueActorFixture(Sys);
            var sut = fixture.GetInitializedSut();
            Sys.EventStream.Subscribe(TestActor, typeof(LeagueViewModel));

            sut.Tell(new CheckStateCommand());

            var expected = new LeagueViewModel(0, fixture.League, fixture.Regions)
                .AsSource().OfLikeness<LeagueViewModel>()
                .Without(x => x.RemainingMilliseconds)
                .With(x => x.Regions)
                .EqualsWhen((x, y) => x.Regions.Count == y.Regions.Count && x.Regions.All(z => y.Regions.Single(a => a.RegionId == z.RegionId).Equals(z)));
            var result = ExpectMsg<LeagueViewModel>();
            expected.ShouldEqual(result);
        }

        [Fact]
        public void ShouldPublishAnIncrementedBlockCount_WhenToldToBuildABlock()
        {
            var fixture = new LeagueActorFixture(Sys);
            var sut = fixture.GetInitializedSut();
            Sys.EventStream.Subscribe(TestActor, typeof(LeagueViewModel));
            var regionToBuildIn = fixture.AValidRegionName;

            sut.Tell(new BuildBlockCommand(fixture.LeagueId, regionToBuildIn));

            sut.Tell(new CheckStateCommand());
            var result = ExpectMsg<LeagueViewModel>().Regions.Single(x => regionToBuildIn == x.Name).BlockCount;
            Assert.Equal(1, result);
        }

        class MutationActor : ReceiveActor
        {
            public MutationActor()
            {
                Receive<LeagueViewModel>(x =>
                {
                    x.Regions.Add(new RegionState());
                    return true;
                });
            }
        }


        [Fact]
        public void ShouldPublishAnImmutableState_vCollectionCantBeModified()
        {
            var fixture = new LeagueActorFixture(Sys);
            var sut = fixture.GetInitializedSut();
            var mutator = Sys.ActorOf(Props.Create(() => new MutationActor()));
            Sys.EventStream.Subscribe(mutator, typeof(LeagueViewModel));
            Sys.EventStream.Subscribe(TestActor, typeof(LeagueViewModel));

            sut.Tell(new CheckStateCommand());
            Thread.Sleep(100);
            var result = ExpectMsg<LeagueViewModel>();
            Assert.Equal(2, result.Regions.Count);
        }

        [Fact]
        public void ShouldPublishAnImmutableState_vShouldntPickUpNewChanges()
        {
            var fixture = new LeagueActorFixture(Sys);
            var sut = fixture.GetInitializedSut();
            Sys.EventStream.Subscribe(TestActor, typeof(LeagueViewModel));

            sut.Tell(new CheckStateCommand());
            var result = ExpectMsg<LeagueViewModel>().Regions.Single(x => x.Name == fixture.AValidRegionName).BlockCount;
            sut.Tell(new BuildBlockCommand(fixture.LeagueId, fixture.AValidRegionName));

            Assert.Equal(0, result);
        }

        [Fact]
        public void ShouldPublishALeagueEndedMessageWhenGameOver()
        {
            var fixture = new LeagueActorFixture(Sys);
            fixture.UseShortDuration();
            var sut = fixture.GetInitializedSut();
            Sys.EventStream.Subscribe(TestActor, typeof(LeagueEndedMessage));

            Thread.Sleep((int)fixture.League.Duration + 10);
            sut.Tell(new CheckStateCommand());

            var result = ExpectMsg<LeagueEndedMessage>();
            Assert.NotNull(result);
        }
    }
}
