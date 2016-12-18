using System;

namespace BlockWars.Game.UI.Actors
{
    public struct PlayerBlockCount
    {
        public string ConnectionId { get; }
        public int BlockCount { get; }

        public PlayerBlockCount(string connectionId, int blockCount)
        {
            ConnectionId = connectionId;
            BlockCount = blockCount;
        }

        public PlayerBlockCount AddClicks(int count)
        {
            return new PlayerBlockCount(ConnectionId, BlockCount + count);
        }
    }
}