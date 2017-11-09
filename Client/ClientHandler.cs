using Common;

namespace Client
{
    using System;
    using DotNetty.Transport.Channels;

    public class ClientHandler : SimpleChannelInboundHandler<string>
    {   
        protected override void ChannelRead0(IChannelHandlerContext contex, string msg)
        {
            /*Console.WriteLine("on reçoit un truc !!!!");
            try
            {
                var rEvent = SerializeHandler.DeserializeObject<Event>(msg);
                MyEventHandler.Event = rEvent;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }*/
            contex.WriteAndFlushAsync("bien reçu mamène");
            Console.WriteLine(msg);
        }

        public override void ExceptionCaught(IChannelHandlerContext contex, Exception e)
        {
            Console.WriteLine(DateTime.Now.Millisecond);
            Console.WriteLine(e.StackTrace);
            contex.CloseAsync();
        }
    }
}