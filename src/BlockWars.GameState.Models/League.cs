

using System;
using System.ComponentModel.DataAnnotations;

namespace BlockWars.GameState.Models
{
    public class League
    {
        public Guid LeagueId { get; set; }

        public DateTime ExpiresAt { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Description { get; set; }
    }
}
