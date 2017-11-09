using System.IO;
using Common;
using ProtoBuf;

namespace Server
{
    using System;
    using DotNetty.Transport.Channels;
    using DotNetty.Transport.Channels.Groups;
    
    public class ServerHandler : SimpleChannelInboundHandler<string>
    {
        static volatile IChannelGroup group;
        private GameCore GameCore;

        public ServerHandler(GameCore gameCore) : base()
        {
            GameCore = gameCore;
        }
        
        public override void ChannelActive(IChannelHandlerContext contex)
        {
            var g = group;
            
            if (g == null)
            {
                lock (this)
                {
                    if (group == null)
                    {
                        g = group = new DefaultChannelGroup(contex.Executor);
                    }
                }
            }
            var p = new Player(contex);
            g.Add(contex.Channel);
            try
            {
                GameCore.Table.AddPlayer(p);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private class EveryOneBut : IChannelMatcher
        {
            readonly IChannelId id;

            public EveryOneBut(IChannelId id)
            {
                this.id = id;
            }

            public bool Matches(IChannel channel) => !channel.Id.Equals(this.id);
        }

        protected override void ChannelRead0(IChannelHandlerContext contex, string msg)
        {
            //   group.WriteAndFlushAsync(broadcast, new EveryOneBut(contex.Channel.Id));
            // contex.WriteAndFlushAsync(response);
        }

        public override void ChannelReadComplete(IChannelHandlerContext ctx) => ctx.Flush();

        public override void ExceptionCaught(IChannelHandlerContext ctx, Exception e)
        {
            Console.Error.WriteLine("{0}", e.StackTrace);
            ctx.CloseAsync();
        }

        public override bool IsSharable => true;
    }
}