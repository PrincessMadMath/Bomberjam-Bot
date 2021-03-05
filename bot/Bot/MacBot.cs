using MyBot.Bomberjam;

namespace MyBot.Bot
{
    public class MacBot
    {
        public BotAction PlayTurn(State state, Player myPlayer)
        {
            var targetX = state.Width / 2;
            var targetY = state.Height / 2;

            var (isCompleted, botAction, distance) = PathSolver.DoSomething(state, myPlayer, targetX, targetY);

            if (isCompleted)
            {
                return BotAction.Bomb;
            }
            else
            {
                return botAction;
            }
        }
    }
}