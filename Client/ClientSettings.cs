namespace SecureChat.Client
{
    using System.Net;

    public class ClientSettings
    {
   
        public static IPAddress Host => IPAddress.Parse("127.0.0.1");

        public static int Port => 4242;

        public static int Size => 42;
    }
}