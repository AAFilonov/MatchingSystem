using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;

namespace MatchingSystem.UI.Services
{
    #nullable enable
    public static class Session
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize<T>(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return (value == null ? default(T) : JsonSerializer.Deserialize<T>(value))!;
        }

        public static string GetStr(this ISession session, string key)
        {
            return session.GetString(key);
        }
    }
}
