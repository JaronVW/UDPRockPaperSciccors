using UDPTicTacToe;

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
                game.setSendTimer();
                var answer = Console.ReadLine();
                if (answer == null) throw new Exception("Invalid input");
                game.Send(answer);
                game.Listener();
                Console.WriteLine(answer);
                Console.WriteLine(game.Response);
                Console.WriteLine(game.beats(answer, game.Response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}