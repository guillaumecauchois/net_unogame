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
        static async Task RunClientAsync()
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
                        pipeline.AddLast(new StringEncoder(), new StringDecoder(), new ClientHandler());
                    }));

                IChannel bootstrapChannel = await bootstrap.ConnectAsync(new IPEndPoint(ClientSettings.Host, ClientSettings.Port));

                for (;;)
                {
                    string line = Console.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    try
                    {
                        await bootstrapChannel.WriteAndFlushAsync(line + "\r\n");
                    }
                    catch
                    {
                        // ignored
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

        //static void Main() => RunClientAsync().Wait();
        //(EventType type, Table table, Player player)
        static void Main()
        {
            var lol1 = new Card(CardColor.Blue, CardValue.Eight);
            var lol2 = new Card(CardColor.Green, CardValue.PassTurn);
            var lol3 = new Card(CardColor.Red, CardValue.Plus2);
            var table = new Table();
            
            var guigui = new Player(null);
            guigui.Hand.AddCard(lol1);
            guigui.Hand.AddCard(lol2);
            guigui.Hand.AddCard(lol3);
            
            var bito = new Event(EventType.YourTurn, guigui, table);
            /**
             * PIERRE - ATTENTION AU NOM DE CETTE CLASSE CAR System.EventHandler existe déjà.
             */
            //var touken = new EventHandler();
            //touken.HandleEvent(bito);
        }
    }
}