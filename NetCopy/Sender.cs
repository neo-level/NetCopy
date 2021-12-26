using System.Net;
using System.Net.Sockets;

namespace NetCopy;

/// <summary>
/// Sends the file to the remote machine.
/// </summary>
internal class Sender
{
    private readonly int _port;
    private readonly string? _fileName;
    private readonly IPAddress _serverIp;

    public Sender(IPAddress serverIp, int port, string? fileName)
    {
        _port = port;
        _serverIp = serverIp;
        _fileName = fileName;
    }

    public void Run()
    {
        var contents = File.ReadAllBytes(_fileName ?? "No content could be read.");
        int offset = 0;
        int length = contents.Length;
        var socket = Create(_serverIp, _port);

        if (socket == null) return;

        socket.Send(BitConverter.GetBytes(contents.Length));

        while (length > 0)
        {
            var sentBytes = socket.Send(contents, offset, length, SocketFlags.None);
            length -= sentBytes;
            offset += sentBytes;
            Console.WriteLine($"Sent {sentBytes} byte(s)");
            socket.Send(contents, offset, length, SocketFlags.None);
        }

        Console.WriteLine("Finished!");
        socket.Close();
        socket.Dispose();
    }

    private Socket? Create(IPAddress ip, int port)
    {
        try
        {
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ip, port);
            return socket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;
    }
}