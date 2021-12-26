namespace NetCopy;

internal static class Program
{
   static void Main(string?[] args)
   {
      var config = new Config(args);

      if (config.AskForHelp || string.IsNullOrEmpty(config.FileName))
      {
         Console.WriteLine("NetCopy");
         Console.WriteLine("A simple peer to peer copy between machines.");
         Console.WriteLine("\tSend\tncp [-ip serverIP] [-p port] filename");
         Console.WriteLine("\tReceive\tncp [-p port] filename");
      }
      else if (config.IsServer)
      {
         var server = new Receiver(config.ServerIp, config.Port, config.FileName, config.Debug);
         server.Run();
      }
      else
      {
         var sender = new Sender(config.ServerIp, config.Port, config.FileName);
         sender.Run();
      }
   } 
}