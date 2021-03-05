using System;
using System.Linq;

namespace MyBot.Bomberjam
{
    public class Tile
    {
        public TileKind Type { get; set; }

        public bool HasPlayer { get; set; }

        public bool HasBomb { get; set; }

        public bool HasBonus { get; set; }
    }

    public enum TileKind
    {
        OutOfBound,
        Empty,
        Wall,
        Block,
        Explosion,
    }

    public enum BotAction
    {
        Up,
        Down,
        Left,
        Right,
        Stay,
        Bomb
    }

    public static class Utils
    {
        public static Tile GetTileAt(this State state, int x, int y)
        {
            return state.IsOutOfBound(x, y)
                ? new Tile()
                {
                    Type = TileKind.OutOfBound
                }
                : state.ParseTile(x, y);
        }

        public static Player GetMyPlayer(this State state, string playerId)
        {
            return state.Players[playerId];
        }

        private static bool IsOutOfBound(this State state, int x, int y)
        {
            return x < 0 || y < 0 || x >= state.Width || y >= state.Height;
        }

        private static Tile ParseTile(this State state, int x, int y)
        {
            var tile = new Tile();

            var tileChar = state.Tiles[state.CoordToTileIndex(x, y)];
            switch (tileChar)
            {
                case '.':
                    tile.Type = TileKind.Empty;
                    break;
                case '*':
                    tile.Type = TileKind.Explosion;
                    break;
                case '+':
                    tile.Type = TileKind.Block;
                    break;
                case '#':
                    tile.Type = TileKind.Wall;
                    break;
            }

            tile.HasPlayer = state.Players.Any(p => p.Value.X == x && p.Value.Y == y);
            tile.HasBomb = state.Bombs.Any(p => p.Value.X == x && p.Value.Y == y);
            tile.HasBonus = state.Bonuses.Any(p => p.Value.X == x && p.Value.Y == y);

            return tile;
        }

        private static int CoordToTileIndex(this State state, int x, int y)
        {
            return y * state.Width + x;
        }



        public static string ActionToString(BotAction botAction)
        {
            switch (botAction)
            {
                case BotAction.Up:
                    return "up";
                case BotAction.Down:
                    return "down";
                case BotAction.Left:
                    return "left";
                case BotAction.Right:
                    return "right";
                case BotAction.Stay:
                    return "stay";
                case BotAction.Bomb:
                    return "bomb";
                default:
                    throw new ArgumentOutOfRangeException(nameof(botAction), botAction, null);
            }
        }
    }
}