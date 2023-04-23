
namespace UDPRockPaperScissors;

public static class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            try
            {
                Console.Write("enter local socket: (format 127.0.0.1:1234) ");
                var local = Console.ReadLine() ?? "";

                Console.Write("enter remote socket: (format 127.0.0.1:1234) ");
                var remote = Console.ReadLine() ?? "";
                var game = new Game(local, remote);
                Console.Write("Make sure both players are connected then press enter to start the game");
                Console.ReadLine();
                Console.Write("You have ten seconds to enter your guess (rock, paper, scissors): ");
                
                var answer = Reader.ReadLine(10000);
                if (answer == null) throw new Exception("Invalid input");
                game.Send(answer);
                game.Listener();
                Console.WriteLine(answer);
                Console.WriteLine(game.Response);
                if (game.Response != null) Console.WriteLine(game.Beats(answer, game.Response));
                game.UnSet();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}