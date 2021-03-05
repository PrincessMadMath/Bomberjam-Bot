using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MyBot.Bomberjam;
using MyBot.Bot;

namespace MyBot
{
    public class Program
    {
        private static readonly Random Rng = new Random();

        public static async Task Main()
        {
            // Standard output (Console.WriteLine) can ONLY BE USED to communicate with the bomberjam process
            // Use text files if you need to log something for debugging

            // 1) You must send an alphanumerical name up to 32 characters, prefixed by "0:"
            // No spaces or special characters are allowed
            Console.Out.WriteLine("0:MyName" + Rng.Next(1000, 9999));
            Console.Out.Flush();

            // 2) Receive your player ID from the standard input
            var myPlayerId = Console.In.ReadLine();

            var myBot = new MacBot();

            while (true)
            {
                // 3) Each tick, you'll receive the current game state serialized as JSON
                // From this moment, you have a limited time to send an action back to the bomberjam process through stdout
                var line = Console.In.ReadLine();
                var state = JsonSerializer.Deserialize<State>(line);

                try
                {
                    var myPlayer = state.GetMyPlayer(myPlayerId);

                    var action = myBot.PlayTurn(state, myPlayer);

                    Console.Out.WriteLine(state.Tick + ":" + Utils.ActionToString(action));
                    Console.Out.Flush();
                }
                catch(Exception e)
                {
                    await File.WriteAllTextAsync($"logs/Log_{myPlayerId}.txt", e.ToString());
                }
            }
        }
    }
}