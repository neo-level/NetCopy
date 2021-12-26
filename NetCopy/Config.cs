using System.Net;

namespace NetCopy;

/// <summary>
/// Parses the cli arguments.
/// </summary>
internal class Config
{
    public string? FileName { get; }
    public int Port { get; } = 9021;
    public bool IsServer { get; } = true;
    public bool Debug { get; }
    public IPAddress ServerIp { get; }
    public bool AskForHelp { get; }

    public Config(string?[] args)
    {
        ServerIp = IPAddress.Any;
        if (args.Length == 0) return;

        int index = 0;
        while (index < args.Length)
        {
            if (IsMatch(args[index], "-ip", "/ip", "ip"))
            {
                index++;
                ServerIp = IPAddress.Parse(args[index] ?? "No valid IP was given.");
                IsServer = false;
            }
            else if (IsMatch(args[index], "-p", "/p", "port"))
            {
                index++;
                Port = int.Parse(args[index] ?? "No valid Port was given.");
            }
            else if (IsMatch(args[index], "-h", "/h", "/help", "help"))
            {
                AskForHelp = true;
                return;
            }
            else if (IsMatch(args[index], "-d", "/d"))
            {
                Debug = true;
            }
            else
            {
                FileName = args[index];
            }

            index++;
        }
    }

    private bool IsMatch(string? leftHand, params string[] rightHand)
    {
        var match = rightHand.FirstOrDefault(s => s == leftHand?.ToLower());
        return !string.IsNullOrEmpty(match);
    }
}