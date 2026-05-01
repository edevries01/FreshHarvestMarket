/*
 * SessionExtensions.cs
 * FreshHarvestMarket
 *
 * This static class provides extension methods for ISession to simplify
 * storing and retrieving complex objects in session state.
 *
 * It allows objects to be serialized into JSON strings when stored,
 * and deserialized back into their original types when retrieved.
 *
 * This makes session storage more flexible by supporting complex data
 * types beyond simple strings.
 */

using System.Text.Json;

namespace FreshHarvestMarket.Extensions
{
    /// <summary>
    /// Extension for ISession recommended by our textbook. It allows us to more easily store and retrieve session data.
    /// It basically just wraps up serialization/deserialization for generic objects
    /// This lets us store more complex objects as strings in the session data
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Stores a generic item as session data
        /// </summary>
        /// <typeparam name="T">The object type to store</typeparam>
        /// <param name="session">This session</param>
        /// <param name="key">The key to store this object by</param>
        /// <param name="value">The object being stored</param>
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        /// <summary>
        /// Retrieves an object from session data
        /// </summary>
        /// <typeparam name="T">Type of object to retrieve</typeparam>
        /// <param name="session">This session</param>
        /// <param name="key">Key of the data to retrieve</param>
        /// <returns>Stored session data or default/null object if it does not exist</returns>
        public static T? GetObject<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            if (string.IsNullOrEmpty(json))
            {
                return default(T?);
            }
            else
            {
                return JsonSerializer.Deserialize<T>(json);
            }
        }
    }
}
