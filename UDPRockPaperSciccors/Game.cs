using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace UDPTicTacToe;

public class Game
{
    private string _localSocket;
    private string _opponentSocket;

    private string _localIp;
    private string _opponentIp;

    private int _localPort;
    private int _opponentPort;

    private UdpClient _client;
    private IPEndPoint _localIpEndPoint;
    private IPEndPoint _opponentIpEndPoint;

    private const string SocketRegex = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}:([0-9]{1,5})$";

    public string? Response;


    public Game(string localSocket, string opponentSocket)
    {
        if (IsValidSocket(localSocket) && IsValidSocket(opponentSocket))
        {
            _localSocket = localSocket;
            _opponentSocket = opponentSocket;
            _localIp = _localSocket.Split(':')[0];
            _opponentIp = _opponentSocket.Split(':')[0];

            _localPort = int.Parse(_localSocket.Split(':')[1]);
            _opponentPort = int.Parse(_opponentSocket.Split(':')[1]);
        }
        else
        {
            throw new Exception("Invalid socket");
        }

        _client = new UdpClient(_localPort);
        _localIpEndPoint = new IPEndPoint(IPAddress.Parse(_localIp), _localPort);
        _opponentIpEndPoint = new IPEndPoint(IPAddress.Parse(_opponentIp), _opponentPort);
    }

    private bool IsValidSocket(string socket)
    {
        return socket.Split(':').Length == 2 && Regex.Match(socket, SocketRegex).Success;
    }

    public void setSendTimer()
    {
        _client.Client.SendTimeout = 10000;
    }

    public void Send(string message)
    {
        if (!ValidateInput(message)) throw new Exception("Invalid input");

        var data = Encoding.ASCII.GetBytes(message);
        _client.Send(data, data.Length, _opponentIp, _opponentPort);
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
            if (returnData.Length > 0)
            {
                Response = returnData;
                break;
            }
        }

        Response = Response.Length > 0 ? Response.ToLower() : "timeout";
    }

    private bool ValidateInput(string input)
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

    public string beats(string first, string second)
    {
        int firstInt = first switch
        {
            "rock" => 1,
            "paper" => 2,
            "scissors" => 3,
            _ => 0
        };

        int secondInt = second switch
        {
            "rock" => 1,
            "paper" => 2,
            "scissors" => 3,
            _ => 0
        };

        if ((firstInt) % 3 + 1 == secondInt)
            return "Win";
        else if ((firstInt) % 3 + 1 == secondInt)
            return "Lose";
        else
            return "Draw";
    }
}