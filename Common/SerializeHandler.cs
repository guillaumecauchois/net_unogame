using System;
using System.IO;

namespace Common
{
    public static class SerializeHandler
    {
        public static string SerializeObj<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, obj);
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
        }

        public static T DeserializeObject<T>(string serializedObj)
        {
            var arr = Convert.FromBase64String(serializedObj);
            using (var ms = new MemoryStream(arr))
            {
                var obj = ProtoBuf.Serializer.Deserialize<T>(ms);
                return obj;
            }
        }
    }
}