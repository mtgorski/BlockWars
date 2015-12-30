

using System.ComponentModel.DataAnnotations;

namespace BlockWars.GameState.Models
{
    public class Realm
    {
        public string RealmId { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Description { get; set; }
    }
}
