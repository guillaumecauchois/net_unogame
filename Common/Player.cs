using System;
using DotNetty.Transport.Channels;
using ProtoBuf;

namespace Common
{
    [ProtoContract]
    public class Player
    {
        public Player()
        {
            Id = _idGenerator;
            Hand = new Hand();
            Context = null;
            _idGenerator++;
            Console.Write("\n[{0}] Join the game\n$> ", Id);
        }
        
        public Player(IChannelHandlerContext context)
        {
            Id = _idGenerator;
            Hand = new Hand();
            Context = context;
            _idGenerator++;
            Console.Write("\n[{0}] Join the game\n$> ", Id);
        }

        /* Serialized Prop */
        [ProtoMember(1)]
        private static int _idGenerator;
        [ProtoMember(2)]
        public int Id { get; set; }
        [ProtoMember(3)]
        public Hand Hand { get; set; }
        
        /* Non-Serialize Prop */
        public IChannelHandlerContext Context { get; set; }

        public override string ToString()
        {
            return $"{Id}\t *HandSize = {Hand.Cards.Count} ";
        }
    }
}