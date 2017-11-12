using System.Runtime.Remoting.Contexts;
using Common;
using SecureChat.Client;

namespace Client
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using DotNetty.Codecs;
    using DotNetty.Transport.Bootstrapping;
    using DotNetty.Transport.Channels;
    using DotNetty.Transport.Channels.Sockets;
    
    
    class Program
    {
        
        public static ClientGameHandler GameHandler = new ClientGameHandler();
        public static ClientHandler handler = new ClientHandler();
        
        static async Task RunClientAsync(string ip, int port)
        {
  
            var group = new MultithreadEventLoopGroup();

            string targetHost = null;
  
            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;

                        pipeline.AddLast(new DelimiterBasedFrameDecoder(8192, Delimiters.LineDelimiter()));
                        pipeline.AddLast(new StringEncoder(), new StringDecoder(), handler);
                    }));

                IChannel bootstrapChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(ip), port));

                for (;;)
                {
                    Console.Write("$> ");
                    var line = Console.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    try
                    {
                        var response = GameHandler.HandleClientCmd(handler.GetEventHandler(), line);
                        if (response != null)
                        {   
                            var serObj = SerializeHandler.SerializeObj(response);
                            await bootstrapChannel.WriteAndFlushAsync(serObj + "\r\n");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                    if (string.Equals(line, "bye", StringComparison.OrdinalIgnoreCase))
                    {
                        await bootstrapChannel.CloseAsync();
                        break;
                    }
                }

                await bootstrapChannel.CloseAsync();
            }
            finally
            {
                group.ShutdownGracefullyAsync().Wait(1000);
            }
        }

        static int Main(string[] args)
        {
            int port;
            string ip;

            if (args.Length != 2)
            {
                Console.Error.WriteLine("ERROR: You need provide a port for the connection.");
                return (1);
            }
            try
            {
                port = int.Parse(args[1]);
                ip = args[0];
            }
            catch (FormatException)
            {
                Console.Error.WriteLine("ERROR: Invalid port provided.");
                return (1);
            }
            Console.WriteLine("|******| UNO - SERVER - C# .NET Project |*****|");
            Console.WriteLine("Contributors: Guillaume CAUCHOIS & Pierre STASZAK");
            try
            {
                RunClientAsync(ip, port).Wait();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return (0);
        }
    }
}