using System;
using System.IO;

namespace Common
{
    public class SerializeHandlerException: Exception
    {
        public SerializeHandlerException()
            : base()
        {
        }

        public SerializeHandlerException(string message)
            : base(message)
        {
        }

        public SerializeHandlerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    
    public static class SerializeHandler
    {
        public static string SerializeObj<T>(T obj)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(ms, obj);
                    return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                }
            }
            catch (Exception e)
            {
                throw new SerializeHandlerException(e.Message);
            }
        }

        public static T DeserializeObject<T>(string serializedObj)
        {
            try
            {
                var arr = Convert.FromBase64String(serializedObj);
                using (var ms = new MemoryStream(arr))
                {
                    var obj = ProtoBuf.Serializer.Deserialize<T>(ms);
                    return obj;
                }
            }
            catch (Exception e)
            {
                throw new SerializeHandlerException(e.Message);
            }
        }
    }
}