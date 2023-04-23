using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace UDPRockPaperScissors;

public class Game
{
    private readonly string _opponentIp;
    private readonly int _opponentPort;

    private readonly UdpClient _client;
    private IPEndPoint _localIpEndPoint;
    private IPEndPoint _opponentIpEndPoint;

    private const string SocketRegex = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}:([0-9]{1,5})$";

    public string? Response;


    public Game(string localSocket, string opponentSocket)
    {
        int localPort;
        string localIp;
        if (IsValidSocket(localSocket) && IsValidSocket(opponentSocket))
        {
            localIp = localSocket.Split(':')[0];
            _opponentIp = opponentSocket.Split(':')[0];

            localPort = int.Parse(localSocket.Split(':')[1]);
            _opponentPort = int.Parse(opponentSocket.Split(':')[1]);
        }
        else
        {
            throw new Exception("Invalid socket");
        }

        _client = new UdpClient(localPort);
        _localIpEndPoint = new IPEndPoint(IPAddress.Parse(localIp), localPort);
        _opponentIpEndPoint = new IPEndPoint(IPAddress.Parse(_opponentIp), _opponentPort);
    }

    private static bool IsValidSocket(string socket)
    {
        return socket.Split(':').Length == 2 && Regex.Match(socket, SocketRegex).Success;
    }

    public void Send(string message)
    {
        try
        {
            if (!ValidateInput(message)) throw new Exception("Invalid input");

            var data = Encoding.ASCII.GetBytes(message);
            _client.Send(data, data.Length, _opponentIp, _opponentPort);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public void Listener()
    {
        while (Response == null)
        {
            // receive bytes
            var receiveBytes = _client.Receive(ref _opponentIpEndPoint);
            // convert bytes to string
            var returnData = Encoding.ASCII.GetString(receiveBytes);
            // set string response 
            if (returnData.Length <= 0) continue;
            Response = returnData;
            break;
        }

        Response = Response.Length > 0 ? Response.ToLower() : "timeout";
    }

    private static bool ValidateInput(string input)
    {
        input = input.ToLower();
        return input switch
        {
            "rock" => true,
            "paper" => true,
            "scissors" => true,
            _ => false
        };
    }


    public string Beats(string me, string opponent)
    {
        if (!ValidateInput(me) || !ValidateInput(opponent)) throw new Exception("Invalid input");

        me = me.ToLower();
        opponent = opponent.ToLower();

        var myGuess = me switch
        {
            "rock" => 1,
            "paper" => 2,
            "scissors" => 3,
            _ => 0
        };

        var opponentGuess = opponent switch
        {
            "rock" => 1,
            "paper" => 2,
            "scissors" => 3,
            _ => 0
        };

        if (myGuess == opponentGuess)
            return "Draw";

        switch (myGuess)
        {
            case 1 when opponentGuess == 2:
            case 2 when opponentGuess == 3:
            case 3 when opponentGuess == 1:
                return "Lose";
            case 1 when opponentGuess == 3:
            case 2 when opponentGuess == 1:
                return "Win";
            default:
                return "Lose";
        }
    }

    public void UnSet()
    {
        _client.Close();
    }
}