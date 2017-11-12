using Common;

namespace Server
{
    using System;
    using DotNetty.Transport.Channels;
    
    public class ServerHandler : SimpleChannelInboundHandler<string>
    {
        private GameCore                _gameCore;

        public ServerHandler(GameCore gameCore)
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

        public override void HandlerRemoved(IChannelHandlerContext context)
        {
            try
            {
                var playerHasGone = _gameCore.Table.GetPlayerByContext(context);
                _gameCore.Table.RemovePlayer(playerHasGone);

                

            }
            catch (Exception e)
            {
                Console.Error.Write("[ERR] {0}\n$>", e.Message);
            }
        }

        protected override void ChannelRead0(IChannelHandlerContext contex, string msg)
        {
            _gameCore.HandleTurnResponse(contex, msg);
        }

        public override void ChannelReadComplete(IChannelHandlerContext ctx) => ctx.Flush();

        public override void ExceptionCaught(IChannelHandlerContext ctx, Exception e)
        {
            // Console.Error.WriteLine(e.StackTrace);
            ctx.CloseAsync();
        }

        public override bool IsSharable => true;
    }
}