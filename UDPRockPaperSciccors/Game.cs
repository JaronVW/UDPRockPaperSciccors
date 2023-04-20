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
        if (isValidSocket(localSocket) && isValidSocket(opponentSocket))
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

    private bool isValidSocket(string socket)
    {
        return socket.Split(':').Length == 2;
    }

    public void Send(string message)
    {
        var data = Encoding.ASCII.GetBytes(message);
        _client.Send(data, data.Length, _opponentIP, _opponentPort);
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
    
    // public bool validateInput(string input)
    // {
    //     switch (input)
    //     {
    //         
    //     }
    //    
    // }

}