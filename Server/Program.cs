namespace Server
{
    using System;
    using System.Threading.Tasks;
    using DotNetty.Codecs;
    using DotNetty.Handlers.Logging;
    using DotNetty.Transport.Bootstrapping;
    using DotNetty.Transport.Channels;
    using DotNetty.Transport.Channels.Sockets;

    class Program
    {
        private static GameCore GameCore = new GameCore();
        
        static async Task RunServerAsync(int port)
        {
            var bossGroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();

            var stringEncoder = new StringEncoder();
            var stringDecoder = new StringDecoder();
            var serverHandler = new ServerHandler(GameCore);
            
            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.SoBacklog, 100)
                    .Handler(new LoggingHandler(LogLevel.INFO))
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;

                        pipeline.AddLast(new DelimiterBasedFrameDecoder(8192, Delimiters.LineDelimiter()));
                        pipeline.AddLast(stringEncoder, stringDecoder, serverHandler);
                    }));

                var bootstrapChannel = await bootstrap.BindAsync(port);

                GameCore.RunContainers();
                await bootstrapChannel.CloseAsync();
            }
            finally
            {
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
            }
        }
        
        public static int Main(string[] args)
        {
            int port;

            if (args.Length != 1)
            {
                Console.Error.WriteLine("ERROR: You need provide a port for the connection.");
                return (1);
            }
            try
            {
                port = int.Parse(args[0]);
            }
            catch (FormatException)
            {
                Console.Error.WriteLine("ERROR: Invalid port provided.");
                return (1);
            }
            Console.WriteLine("|******| UNO - SERVER - C# .NET Project |*****|");
            Console.WriteLine("Contributors: Guillaume CAUCHOIS & Pierre STASZAK");
            
            RunServerAsync(port).Wait();
            return (0);
        }
    }
}