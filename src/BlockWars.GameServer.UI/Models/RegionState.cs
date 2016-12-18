using System;

namespace BlockWars.Game.UI.Models
{
    public struct RegionState
    {
        public RegionState(Guid regionId, string name, long blockCount)
        {
            RegionId = regionId;
            Name = name;
            BlockCount = blockCount;
        }

        public RegionState(string name) : this(Guid.NewGuid(), name, 0)
        {
        }

        public Guid RegionId { get; }

        public string Name { get; }

        public long BlockCount { get; }

        public RegionState AddBlocks(long blockCount)
        {
            return new RegionState(RegionId, Name, BlockCount + blockCount);
        }
    }
}
