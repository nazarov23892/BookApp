using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApplication.Infrastructure
{
    public static class SessionExtensions
    {
        public static void WriteJson(this ISession session, string key, object value)
        {
            string cartJson = JsonConvert.SerializeObject(value: value);
            session.SetString(key: key,
                value: cartJson);
        }

        public static T ReadJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key: key);
            return sessionData == null
                ? default
                : JsonConvert.DeserializeObject<T>(sessionData) ?? default;
        }
    }
}
