using UnityEngine;

namespace Umbrella.Ranking
{
    /// <summary>
    /// Ranking sort order.
    /// </summary>
    public enum OrderBy
    {
        // Ascending
        ASC,
        // Descending
        DESC
    }

    /// <summary>
    /// Ranking request setting.
    /// </summary>
    [System.Serializable]
    public class RankingRequestSetting
    {
        /// <summary>
        /// The name of this ranking, also defines the name of the sheet used to store ranking data.
        /// </summary>
        public string RankingName;

        /// <summary>
        /// How many ranking data should be retrieved.
        /// </summary>
        public int RankingNumber;

        /// <summary>
        /// The sort order (ascending or descending) of this ranking.
        /// </summary>
        public OrderBy OrderBy;
    }

    /// <summary>
    /// Ranking settings ScriptableObject.
    /// </summary>
    [CreateAssetMenu]
    public class RankingSettings : ScriptableObject
    {
        /// <summary>
        /// The web app URL of your Google App Script.
        /// </summary>
        public string AppURL;

        /// <summary>
        /// Array of ranking request settings.
        /// </summary>
        public RankingRequestSetting[] RankingRequestSettings;
    }
}
