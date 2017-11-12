using Common;

namespace Client
{
    using System;
    using DotNetty.Transport.Channels;

    public class ClientHandler : SimpleChannelInboundHandler<string>
    {
        private ClientEventHandler MyEventHandler = new ClientEventHandler();
        
        protected override void ChannelRead0(IChannelHandlerContext contex, string msg)
        {
            try
            {
                var rEvent = SerializeHandler.DeserializeObject<Event>(msg);
                MyEventHandler.HandleEvent(rEvent);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext contex, Exception e)
        {
            Console.WriteLine(DateTime.Now.Millisecond);
            Console.WriteLine(e.StackTrace);
            contex.CloseAsync();
        }

        public ClientEventHandler GetEventHandler()
        {
            return MyEventHandler;
        }
    }
}