

using System;
using System.ComponentModel.DataAnnotations;

namespace BlockWars.GameState.Models
{
    public class League
    {
        public Guid LeagueId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public long Duration { get; set; }
    }
}
