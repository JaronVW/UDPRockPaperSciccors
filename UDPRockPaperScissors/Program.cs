namespace UDPRockPaperScissors;

public static class Program
{
    public static void Main(string[] args)
    {
        var exit = "";
        while (exit != "exit")
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
                string? answer = null;
                try
                {
                    answer = Reader.ReadLine(10000);
                }
                catch (TimeoutException)
                {
                    game.Response = "timeout";
                }

                if (answer == null) throw new Exception("Invalid input");
                game.Send(answer);
                game.Listener();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("My guess " + answer);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Opponent guess " + game.Response);
                Console.ResetColor();
                var outcome = "";
                if (game.Response != null) outcome = Game.Beats(answer, game.Response);
                switch (outcome)
                {
                    case "Win":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You win!");
                        Console.ResetColor();
                        break;
                    case "Lose":
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You lose!");
                        Console.ResetColor();
                        break;
                    case "Draw":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Draw!");
                        Console.ResetColor();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You win?, we don't know honestly");
                        Console.ResetColor();
                        break;
                }

                if (game.Response == "timeout") Console.WriteLine("Your opponent chickened out, you win!");
                game.UnSet();
                Console.WriteLine("Press enter to play again, type exit to exit");
                exit = Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}