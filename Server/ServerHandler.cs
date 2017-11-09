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

        public override void HandlerAdded(IChannelHandlerContext context)
        {
            base.HandlerAdded(context);
            var p = new Player(context);
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
            GameCore.HandleTurnResponse(contex, msg);
        }

        public override void ChannelReadComplete(IChannelHandlerContext ctx) => ctx.Flush();

        public override void ExceptionCaught(IChannelHandlerContext ctx, Exception e)
        {
            Console.Error.WriteLine(e.StackTrace);
            ctx.CloseAsync();
        }

        public override bool IsSharable => true;
    }
}