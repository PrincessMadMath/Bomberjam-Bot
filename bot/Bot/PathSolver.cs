using System;
using System.Linq;
using MyBot.Bomberjam;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;

namespace MyBot.Bot
{
    public static class PathSolver
    {
        public static (bool IsCompleted, BotAction nextAction, float distance) DoSomething(State state, Player myPlayer, int x, int y)
        {
            var grid = CreateGrid(state, myPlayer);

            var pathFinder = new PathFinder();
            var path = pathFinder.FindPath(new GridPosition(myPlayer.X, myPlayer.Y), new GridPosition(x, y), grid);


            var nextMove = path.Edges.FirstOrDefault();

            if (nextMove == null)
            {
                return (true, BotAction.Stay, 0);
            }

            var distance = path.Distance.Meters;

            var dX = nextMove.End.Position.X - nextMove.Start.Position.X;
            if (dX > 0)
            {
                return (false, BotAction.Right, distance);
            }
            if (dX < 0)
            {
                return (false, BotAction.Left, distance);
            }

            var dY = nextMove.End.Position.Y - nextMove.Start.Position.Y;
            if (dY > 0)
            {
                return (false, BotAction.Down, distance);
            }
            if (dY < 0)
            {
                return (false, BotAction.Up, distance);
            }

            return (false, BotAction.Stay, distance);

        }

        public static Grid CreateGrid(State state, Player myPlayer)
        {
            var gridSize = new GridSize(columns: state.Width, rows: state.Height);
            var cellSize = new Size(Distance.FromMeters(1), Distance.FromMeters(1));
            var traversalVelocity = Velocity.FromKilometersPerHour(60);

            Grid grid = Grid.CreateGridWithLateralConnections(gridSize, cellSize, traversalVelocity);

            for (int x = 0; x < state.Width; x++)
            {
                for (int y = 0; y < state.Height; y++)
                {
                    var tile = state.GetTileAt(x, y);

                    if (x == myPlayer.X && y == myPlayer.Y)
                    {
                        continue;
                    }

                    if (tile.Type != TileKind.Empty
                        || tile.HasPlayer
                        || tile.HasBomb)
                    {
                        try
                        {
                            grid.DisconnectNode(new GridPosition(x, y));
                        }
                        catch (Exception e)
                        {
                            throw new Exception($"X: {x}, Y: {y}", e);
                        }
                    }
                }
            }

            return grid;
        }
    }
}