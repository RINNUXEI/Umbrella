using UnityEngine;

namespace Umbrella.Database
{
    /// <summary>
    /// Database settings ScriptableObject.
    /// </summary>
    [CreateAssetMenu]
    public class DatabaseSettings : ScriptableObject
    {
        [SerializeField] private string _appURL;
        [SerializeField] private string _defaultSheetName;

        /// <summary>
        /// The web app URL of your Google App Script.
        /// </summary>
        public string AppURL => _appURL;

        /// <summary>
        /// The default sheet name for storing and retrieving data.
        /// </summary>
        public string DefaultSheetName => _defaultSheetName;
    }
}
