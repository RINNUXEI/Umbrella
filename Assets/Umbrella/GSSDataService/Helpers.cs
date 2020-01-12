using System;
using UnityEngine;

namespace Umbrella.GSSDataService
{
    /// <summary>
    /// Helper functions.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Get a unique user id.
        /// If not found, create one and store it in PlayerPrefs.
        /// </summary>
        /// <param name="idKey">Key of the user id, if not specified "UserId" will be used.</param>
        /// <returns>The unique user id.</returns>
        public static string GetUserID(string idKey = "")
        {
            if (string.IsNullOrEmpty(idKey)) idKey = "UserId";
            var id = PlayerPrefs.GetString(idKey);
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
                PlayerPrefs.SetString(idKey, id);
            }
            return id;
        }
    }
}
