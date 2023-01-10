using Business.Config;
using DataAccess.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Business.Utils

{
    public static  class SerializationUtil
    {
        public static byte[] ToByteArray(this object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                return null;
            }

            return Encoding.Default.GetBytes(JsonConvert.SerializeObject(objectToSerialize));
        }

        public static T FromByteArray<T>(this byte[] arrayToDeserialize) where T : class
        {
            if (arrayToDeserialize is null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(Encoding.Default.GetString(arrayToDeserialize));
        }

        public static Dictionary<string, FeatureStatistic> FromString(string json)
        {
            Dictionary<string, FeatureStatistic> result = new Dictionary<string, FeatureStatistic>();
            result = JsonConvert.DeserializeObject<Dictionary<string, FeatureStatistic>>(json);
            return result;
        }
        public static string ToString(this object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }
    }
}
