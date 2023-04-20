using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPTicTacToe;

public class Game
{
    private string _localSocket;
    private string _opponentSocket;

    private string _localIP;
    private string _opponentIP;

    private int _localPort;
    private int _opponentPort;

    private UdpClient _client;
    private IPEndPoint _localIPEndPoint;
    private IPEndPoint _opponentIPEndPoint;


    public Game(string localSocket, string opponentSocket)
    {
        if (IsValidSocket(localSocket) && IsValidSocket(opponentSocket))
        {
            _localSocket = localSocket;
            _opponentSocket = opponentSocket;
            _localIP = _localSocket.Split(':')[0];
            _opponentIP = _opponentSocket.Split(':')[0];

            _localPort = int.Parse(_localSocket.Split(':')[1]);
            _opponentPort = int.Parse(_opponentSocket.Split(':')[1]);
        }
        else
        {
            throw new Exception("Invalid socket");
        }

        _client = new UdpClient(_localPort);
        _localIPEndPoint = new IPEndPoint(IPAddress.Parse(_localIP), _localPort);
        _opponentIPEndPoint = new IPEndPoint(IPAddress.Parse(_opponentIP), _opponentPort);
        Listener();
    }

    private bool IsValidSocket(string socket)
    {
        return socket.Split(':').Length == 2;
    }

    public void Send(string message)
    {
        var data = Encoding.ASCII.GetBytes(message);
        _client.Send(data, data.Length, _opponentIP, _opponentPort);
    }

    public string getGuesWithinTenSeconds()
    {
        Console.Write("Enter your guess within ten seconds rock,paper,scissors (enter quit to stop) : ");

        var guess = "";
        var startTime = DateTime.Now;
        while (string.IsNullOrEmpty(guess) && (DateTime.Now - startTime).TotalSeconds < 10)
        {
            guess = Console.ReadLine();
        }
        if (ValidateInput(guess!))
        {
            return guess!;
        }
        Console.WriteLine("Invalid input");
        return getGuesWithinTenSeconds();
    }


    private void Listener()
    {
        while (true)
        {
            while (true)
            {
                // receive bytes
                var receiveBytes = _client.Receive(ref _opponentIPEndPoint);

                // convert bytes to string
                var returnData = Encoding.ASCII.GetString(receiveBytes);

                // print string
                Console.WriteLine(returnData);
            }
        }
    }

    private bool ValidateInput(string input)
    {
        input = input.ToLower();
        return input switch
        {
            "rock" => true,
            "paper" => true,
            "scissors" => true,
            "quit" => true,
            _ => false
        };
    }
}