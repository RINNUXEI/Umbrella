using System;
using UnityEngine;

namespace Umbrella.GSSDataService
{
    /// <summary>
    /// Helper functions.
    /// </summary>
    public static class LocalSaveDataHelper
    {
        private const string DefaultUserIdKey = "UserId";
        private const string DefaultUserNameKey = "UserName";

        /// <summary>
        /// Get a unique user id.
        /// If not found, create one and store it in PlayerPrefs.
        /// </summary>
        /// <param name="idKey">Key of the user id, if not specified "UserId" will be used.</param>
        /// <returns>The unique user id.</returns>
        public static string GetUserID(string idKey = "")
        {
            if (string.IsNullOrEmpty(idKey)) idKey = DefaultUserIdKey;
            var id = PlayerPrefs.GetString(idKey);
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
                PlayerPrefs.SetString(idKey, id);
            }
            return id;
        }

        /// <summary>
        /// Set user name.
        /// </summary>
        /// <param name="name">User name.</param>
        /// <param name="nameKey">Key of the user name, if not specified "UserName" will be used.</param>
        public static void SetUserName(string name, string nameKey = "")
        {
            if (string.IsNullOrEmpty(nameKey)) nameKey = DefaultUserNameKey;
            PlayerPrefs.SetString(nameKey, name);
        }

        /// <summary>
        /// Get user name.
        /// </summary>
        /// <param name="nameKey">Key of the user name, if not specified "UserName" will be used.</param>
        /// <returns>User name.</returns>
        public static string GetUserName(string nameKey = "")
        {
            if (string.IsNullOrEmpty(nameKey)) nameKey = DefaultUserNameKey;
            return PlayerPrefs.GetString(nameKey);
        }
    }
}
