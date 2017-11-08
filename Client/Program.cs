using System;
using Common;

namespace SecureChat.Client
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
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

            X509Certificate2 cert = null;
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
                        pipeline.AddLast(new StringEncoder(), new StringDecoder(), new SecureChatClientHandler());
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
            
            var guigui = new Player("guigui");
            guigui.GetHand().AddCard(lol1);
            guigui.GetHand().AddCard(lol2);
            guigui.GetHand().AddCard(lol3);
            
            var bito = new Event(EventType.YourTurn, table, guigui);
            var touken = new EventHandler();
            touken.HandleEvent(bito);
        }
    }
}