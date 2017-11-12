using System;
using System.Security.Claims;
using DotNetty.Transport.Channels;
using ProtoBuf;

namespace Common
{
    [ProtoContract]
    public class Player
    {
        public Player()
        {
            Id = 0;
            Hand = new Hand();
            Context = null;
            HasDraw = false;
            HasUno = false;
        }
        
        public Player(IChannelHandlerContext context)
        {
            Id = _idGenerator;
            _idGenerator++;
            Hand = new Hand();
            Context = context;
            HasDraw = false;
            HasUno = false;
        }

        /* Serialized Prop */
        [ProtoMember(1)]
        private static int _idGenerator;
        [ProtoMember(2)]
        public int Id { get; set; }
        [ProtoMember(3)]
        public Hand Hand { get; set; }
        [ProtoMember(4)]
        public bool HasDraw { get; set; }
        [ProtoMember(5)]
        public bool HasUno { get; set; }
        
        /* Non-Serialize Prop */
        public IChannelHandlerContext Context { get; set; }

        public override string ToString()
        {
            return $"{Id}";
        }
        
        public string ToStringDetails()
        {
            return $"{Id}\t HandSize = {Hand.Cards.Count} HasDraw = {HasDraw} HasUno = {HasUno}";
        }
        
        public bool SendError(string msg, Table table = null)
        {
            var e = new Event(EventType.Error, this, table) {ErrorMsg = msg};
            var serObj = SerializeHandler.SerializeObj(e);
            Context.WriteAndFlushAsync(serObj + "\r\n");
            return (true);
        }
    }
}