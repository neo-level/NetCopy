using System.Net;
using System.Net.Sockets;

namespace NetCopy;

/// <summary>
/// Receives the data from the remote client.
/// </summary>
internal class Receiver
{
    private readonly int _port;
    private readonly bool _debug;
    private readonly string? _fileName;
    private readonly IPAddress _serverIp;

    public Receiver(IPAddress serverIp, int port, string? fileName, bool debug)
    {
        _port = port;
        _debug = debug;
        _fileName = fileName;
        _serverIp = serverIp;
    }

    public void Run()
    {
        Console.WriteLine("Listening for connection");
        var contents = ReceiveContents();

        if (contents.Length == 0) return;

        if (_fileName != null) File.WriteAllBytes(_fileName, contents);
    }

    private byte[] ReceiveContents()
    {
        var listener = new TcpListener(_serverIp, _port);
        listener.Start();

        var socket = listener.AcceptSocket();
        var buffer = new byte[1024];
        var received = socket.Receive(buffer);
        received -= 4;

        int index = 0;
        int length = BitConverter.ToInt32(buffer, index);
        Console.WriteLine($"Size of the received file is: {length} byte(s)");

        if (_debug) Console.WriteLine($"Received {received} amount of bytes");


        int sourceIndex = 4;
        int destinationIndex = 0;
        int bufferOffset = received;
        var superBuffer = new byte[length];
        Array.Copy(buffer, sourceIndex, superBuffer, destinationIndex, bufferOffset);

        int bytesRemaining = length - received;
        while (bytesRemaining > 0)
        {
            index = 0;
            Array.Clear(buffer, index, buffer.Length);

            received = socket.Receive(buffer);
            bytesRemaining -= received;
            if (_debug) Console.WriteLine($"Received {received} amount of bytes, {bytesRemaining} remaining.");

            sourceIndex = 0;
            Array.Copy(buffer, sourceIndex, superBuffer, bufferOffset, received);
            bufferOffset += received;
        }

        socket.Close();
        socket.Dispose();
        listener.Stop();

        return superBuffer;
    }
}