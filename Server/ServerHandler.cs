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
        private GameCore                _gameCore;

        public ServerHandler(GameCore gameCore) : base()
        {
            _gameCore = gameCore;
        }

        public override void HandlerAdded(IChannelHandlerContext context)
        {
            base.HandlerAdded(context);
            var p = new Player(context);
            try
            {
                _gameCore.Table.AddPlayer(p);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /* public override void HandlerRemoved(IChannelHandlerContext context)
        {
            base.HandlerRemoved(context);
            try
            {
                _gameCore.Table.RemovePlayer();
            }
            catch (Exception e)
            {
                Console.WriteLine();
            }
        } */

        protected override void ChannelRead0(IChannelHandlerContext contex, string msg)
        {
            _gameCore.HandleTurnResponse(contex, msg);
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